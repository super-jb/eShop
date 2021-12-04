﻿using Ardalis.GuardClauses;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetRunBasics;

public class ProductModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;

    public ProductModel(ICatalogService catalogService, IBasketService basketService)
    {
        _catalogService = Guard.Against.Null(catalogService, nameof(catalogService));
        _basketService = Guard.Against.Null(basketService, nameof(basketService));
    }

    public IEnumerable<string> CategoryList { get; set; } = new List<string>();

    public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


    [BindProperty(SupportsGet = true)]
    public string SelectedCategory { get; set; }

    public async Task<IActionResult> OnGetAsync(string categoryName)
    {
        IEnumerable<CatalogModel> productList = await _catalogService.GetCatalog();
        CategoryList = productList.Select(p => p.Category).Distinct();

        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            ProductList = productList.Where(p => p.Category == categoryName);
            SelectedCategory = categoryName;
        }
        else
        {
            ProductList = productList;
        }

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
