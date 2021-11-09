using MediatR;

namespace Catalog.API.Queries
{
    public record GetProductByIdQuery(string Id) : IRequest<GetProductByIdResponse>;
}
