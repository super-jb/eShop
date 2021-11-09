using Catalog.API.Data.Entities;
using MediatR;

namespace Catalog.API.Commands
{
    public record UpdateProductCommand(string Id, Product Product) : IRequest<UpdateProductResponse>;
}
