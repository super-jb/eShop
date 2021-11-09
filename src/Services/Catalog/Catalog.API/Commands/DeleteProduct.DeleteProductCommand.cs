using MediatR;

namespace Catalog.API.Commands
{
    public record DeleteProductCommand(string Id) : IRequest<DeleteProductResponse>;
}
