using MediatR;

namespace Catalog.API.Queries
{
    public record GetProductByCategoryQuery(string Category) : IRequest<GetProductByCategoryResponse>;
}
