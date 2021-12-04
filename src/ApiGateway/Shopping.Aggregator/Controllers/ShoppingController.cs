using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
    {
        _catalogService = Guard.Against.Null(catalogService, nameof(catalogService));
        _basketService = Guard.Against.Null(basketService, nameof(basketService));
        _orderService = Guard.Against.Null(orderService, nameof(orderService));
    }

    /// <summary>
    /// * get basket with username
    /// * iterate basket items and consume products with basket item productId member
    /// * map product related members into basketitem dto with extended columns
    /// * consume ordering microservices in order to retrieve order list
    /// * return root ShoppngModel dto class which including all responses
    /// </summary>
    [HttpGet("{userName}", Name = "GetShopping")]
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
    {
        BasketModel basket = await _basketService.GetBasket(userName);

        foreach (BasketItemExtendedModel item in basket.Items)
        {
            CatalogModel product = await _catalogService.GetCatalogById(item.ProductId);

            // set additional product fields onto basket item
            item.ProductName = product.Name;
            item.Category = product.Category;
            item.Summary = product.Summary;
            item.Description = product.Description;
            item.ImageFile = product.ImageFile;
        }

        IEnumerable<OrderResponseModel> orders = await _orderService.GetOrdersByUserName(userName);

        ShoppingModel shoppingModel = new()
        {
            UserName = userName,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Ok(shoppingModel);
    }
}
