using Ardalis.GuardClauses;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
            _discountGrpcService = Guard.Against.Null(discountGrpcService, nameof(discountGrpcService));
            _publishEndpoint = Guard.Against.Null(publishEndpoint, nameof(publishEndpoint));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        [HttpGet("{userName}", Name = "Get Basket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket([FromRoute] string userName)
        {
            ShoppingCart basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost(Name = "Upsert Basket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // Communicate with Discount.Grpc and calculate lastest prices of products into sc
            foreach (ShoppingCartItem item in basket.Items)
            {
                CouponModel coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "Delete Basket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeelteBasket([FromRoute] string userName)
        {
            await _repository.DeleteBasket(userName);
            return new NoContentResult();
        }

        [Route("[action]")] // checkout
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price
            ShoppingCart basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            BasketCheckoutEvent eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            await _publishEndpoint.Publish(eventMessage);

            // remove the basket
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}
