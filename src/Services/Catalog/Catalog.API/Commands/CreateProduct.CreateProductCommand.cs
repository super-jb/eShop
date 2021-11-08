using Catalog.API.Data.Entities;
using MediatR;

namespace Catalog.API.Commands
{
    public record CreateProductCommand(Product Product): IRequest<CreateProductResponse>;
}
