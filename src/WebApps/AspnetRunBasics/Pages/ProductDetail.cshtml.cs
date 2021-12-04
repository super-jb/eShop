using Ardalis.GuardClauses;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AspnetRunBasics;

public class ProductDetailModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;

    public ProductDetailModel(ICatalogService catalogService, IBasketService basketService)
    {
        _catalogService = Guard.Against.Null(catalogService, nameof(catalogService));
        _basketService = Guard.Against.Null(basketService, nameof(basketService));
    }

    public CatalogModel Product { get; set; }

    [BindProperty]
    public string Color { get; set; }

    [BindProperty]
    public int Quantity { get; set; }

    public async Task<IActionResult> OnGetAsync(string productId)
    {
        if (productId == null)
        {
            return NotFound();
        }

        Product = await _catalogService.GetCatalog(productId);
        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string productId)
    {
        CatalogModel product = await _catalogService.GetCatalog(productId);

        const string userName = "swn";
        BasketModel basket = await _basketService.GetBasket(userName);

        basket.Items.Add(new()
        {
            ProductId = productId,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = Quantity,
            Color = Color
        });

        BasketModel basketUpdated = await _basketService.UpdateBasket(basket);

        return RedirectToPage("Cart");
    }
}
