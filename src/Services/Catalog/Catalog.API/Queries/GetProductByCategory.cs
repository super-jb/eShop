using Ardalis.GuardClauses;
using Catalog.API.Data.Entities;
using Catalog.API.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Queries
{
    public class GetProductByCategory : IRequestHandler<GetProductByCategoryQuery, GetProductByCategoryResponse>
    {
        private readonly IProductRepository _repository;

        public GetProductByCategory(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<GetProductByCategoryResponse> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await _repository.GetProductByCategory(request.Category);

            return new GetProductByCategoryResponse { Products = products };
        }
    }
}
