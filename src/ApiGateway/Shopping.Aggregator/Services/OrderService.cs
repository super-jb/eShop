using Ardalis.GuardClauses;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _client;

    public OrderService(HttpClient client)
    {
        _client = Guard.Against.Null(client, nameof(client));
    }

    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
    {
        HttpResponseMessage? response = await _client.GetAsync($"/api/v1/Order/{userName}");
        return await response.ReadContentAs<List<OrderResponseModel>>();
    }
}
