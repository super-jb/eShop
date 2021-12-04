using Ardalis.GuardClauses;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AspnetRunBasics;

public class CheckOutModel : PageModel
{
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public CheckOutModel(IBasketService basketService, IOrderService orderService)
    {
        _basketService = Guard.Against.Null(basketService, nameof(basketService));
        _orderService = Guard.Against.Null(orderService, nameof(orderService));
    }

    [BindProperty]
    public BasketCheckoutModel Order { get; set; }

    public BasketModel Cart { get; set; } = new BasketModel();

    public async Task<IActionResult> OnGetAsync()
    {
        const string userName = "jb";
        Cart = await _basketService.GetBasket(userName);

        return Page();
    }

    public async Task<IActionResult> OnPostCheckOutAsync()
    {
        const string userName = "jb";
        Cart = await _basketService.GetBasket(userName);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Order.UserName = userName;
        Order.TotalPrice = Cart.TotalPrice;

        await _basketService.CheckoutBasket(Order);

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}
