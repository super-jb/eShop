﻿using Ardalis.GuardClauses;
using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _client;

    public CatalogService(HttpClient client)
    {
        _client = Guard.Against.Null(client, nameof(client));
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        HttpResponseMessage response = await _client.GetAsync("/Catalog");

        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<CatalogModel> GetCatalog(string id)
    {
        HttpResponseMessage response = await _client.GetAsync($"/Catalog/{id}");

        return await response.ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        HttpResponseMessage response = await _client.GetAsync($"/Catalog/GetProductByCategory/{category}");
        
        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<CatalogModel> CreateCatalog(CatalogModel model)
    {
        HttpResponseMessage response = await _client.PostAsJson($"/Catalog", model);
        
        return response.IsSuccessStatusCode
            ? await response.ReadContentAs<CatalogModel>()
            : throw new Exception("Something went wrong when calling api.");
    }
}
