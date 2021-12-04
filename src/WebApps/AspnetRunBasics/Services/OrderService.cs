using Ardalis.GuardClauses;
using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _client;

    public OrderService(HttpClient client)
    {
        _client = Guard.Against.Null(client, nameof(client));
    }

    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
    {
        HttpResponseMessage response = await _client.GetAsync($"/Order/{userName}");
        
        return await response.ReadContentAs<List<OrderResponseModel>>();
    }
}
