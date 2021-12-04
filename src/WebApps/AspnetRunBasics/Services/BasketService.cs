using Ardalis.GuardClauses;
using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _client;

    public BasketService(HttpClient client)
    {
        _client = Guard.Against.Null(client, nameof(client));
    }

    public async Task<BasketModel> GetBasket(string userName)
    {
        HttpResponseMessage response = await _client.GetAsync($"/Basket/{userName}");

        return await response.ReadContentAs<BasketModel>();
    }

    public async Task<BasketModel> UpdateBasket(BasketModel model)
    {
        HttpResponseMessage response = await _client.PostAsJson($"/Basket", model);

        return response.IsSuccessStatusCode
            ? await response.ReadContentAs<BasketModel>()
            : throw new Exception("Something went wrong when calling api.");
    }

    public async Task CheckoutBasket(BasketCheckoutModel model)
    {
        HttpResponseMessage response = await _client.PostAsJson($"/Basket/Checkout", model);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling api.");
        }
    }
}
