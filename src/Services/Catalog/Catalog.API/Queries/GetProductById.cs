using Ardalis.GuardClauses;
using Catalog.API.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Queries
{
    public class GetProductById : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
    {
        private readonly IProductRepository _repository;

        public GetProductById(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
        }


        public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProduct(request.Id.ToString());

            return new GetProductByIdResponse { Product = product };
        }
    }
}
