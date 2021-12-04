using Ardalis.GuardClauses;
using Catalog.API.Data.Entities;
using Catalog.API.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Queries;

public class GetProductByName : IRequestHandler<GetProductByNameQuery, GetProductByNameResponse>
{
    private readonly IProductRepository _repository;

    public GetProductByName(IProductRepository repository)
    {
        _repository = Guard.Against.Null(repository, nameof(repository));
    }

    public async Task<GetProductByNameResponse> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = await _repository.GetProductByName(request.Name);

        return new GetProductByNameResponse { Products = products };
    }
}
