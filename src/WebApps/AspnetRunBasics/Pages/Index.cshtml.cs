using Ardalis.GuardClauses;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspnetRunBasics.Pages;

public class IndexModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;

    public IndexModel(ICatalogService catalogService, IBasketService basketService)
    {
        _catalogService = Guard.Against.Null(catalogService, nameof(catalogService));
        _basketService = Guard.Against.Null(basketService, nameof(basketService));
    }

    public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        ProductList = await _catalogService.GetCatalog();
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string productId)
    {
        CatalogModel product = await _catalogService.GetCatalog(productId);

        const string userName = "jb";
        BasketModel basket = await _basketService.GetBasket(userName);

        basket.Items.Add(new()
        {
            ProductId = productId,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = 1,
            Color = "Black"
        });

        BasketModel basketUpdated = await _basketService.UpdateBasket(basket);

        return RedirectToPage("Cart");
    }
}
