using Ardalis.GuardClauses;
using Catalog.API.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Commands
{
    public class DeleteProduct : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
    {
        private readonly IProductRepository _repository;

        public DeleteProduct(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _repository.DeleteProduct(request.Id.ToString());

            return deleted 
                ? new DeleteProductResponse { Deleted = deleted, StatusCode = HttpStatusCode.NoContent } 
                : new DeleteProductResponse { StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = new List<string> { "An error occurred deleting this Product" } };
        }
    }
}
