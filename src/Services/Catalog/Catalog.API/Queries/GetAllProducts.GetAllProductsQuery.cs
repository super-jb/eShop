using MediatR;

namespace Catalog.API.Queries
{
    public record GetAllProductsQuery : IRequest<GetAllProductsResponse>;
}
