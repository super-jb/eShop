using Ardalis.GuardClauses;
using Catalog.API.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Commands
{
    public class UpdateProduct : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
    {
        private readonly IProductRepository _repository;

        public UpdateProduct(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateProduct(request.Product);

            return result ? new UpdateProductResponse { Product = request.Product } : null;
        }
    }
}
