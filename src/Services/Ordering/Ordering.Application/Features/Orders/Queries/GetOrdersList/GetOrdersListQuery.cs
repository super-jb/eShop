using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQuery : IRequest<IList<GetOrdersListQueryResponse>>
{
    public string UserName;
}
