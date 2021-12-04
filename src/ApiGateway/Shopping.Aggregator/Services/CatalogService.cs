using Ardalis.GuardClauses;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _client;

    public CatalogService(HttpClient client)
    {
        _client = Guard.Against.Null(client, nameof(client));
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogs()
    {
        // TODO: Consume OpenApi client .NET Core, instead of hard-coding RequestUri ("/api/v1/Catalog")
        // https://stackoverflow.com/questions/67235046/consume-openapi-client-net-core-with-interface/70185324#70185324
        HttpResponseMessage response = await _client.GetAsync("/api/v1/Catalog");
        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<CatalogModel> GetCatalogById(string id)
    {
        HttpResponseMessage response = await _client.GetAsync($"/api/v1/Catalog/{id}");
        return await response.ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        HttpResponseMessage response = await _client.GetAsync($"/api/v1/Catalog/category/{category}");
        return await response.ReadContentAs<IEnumerable<CatalogModel>>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByName(string name)
    {
        HttpResponseMessage response = await _client.GetAsync($"/api/v1/Catalog/name/{name}");
        return await response.ReadContentAs<IEnumerable<CatalogModel>>();
    }
}
