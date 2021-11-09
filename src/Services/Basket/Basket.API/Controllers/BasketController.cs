using Ardalis.GuardClauses;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        //private readonly ILogger<BasketController> _logger;
        private IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
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
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "Delete Basket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeelteBasket([FromRoute] string userName)
        {
            await _repository.DeleteBasket(userName);
            return new NoContentResult();
        }
    }
}