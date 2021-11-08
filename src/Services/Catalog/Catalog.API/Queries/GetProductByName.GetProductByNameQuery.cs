using MediatR;

namespace Catalog.API.Queries
{
    public record GetProductByNameQuery(string Name) : IRequest<GetProductByNameResponse>;
}
