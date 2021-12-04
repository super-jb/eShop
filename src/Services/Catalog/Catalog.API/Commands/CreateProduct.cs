using Ardalis.GuardClauses;
using Catalog.API.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Commands;

public class CreateProduct : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _repository;

    public CreateProduct(IProductRepository repository)
    {
        _repository = Guard.Against.Null(repository, nameof(repository));
    }

    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await _repository.CreateProduct(request.Product);

        return new CreateProductResponse { Product = request.Product };
    }
}
