using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, IList<GetOrdersListQueryResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = Guard.Against.Null(orderRepository, nameof(orderRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<IList<GetOrdersListQueryResponse>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Order> orders = await _orderRepository.GetOrdersByUserName(request.UserName);

        return _mapper.Map<IList<GetOrdersListQueryResponse>>(orders);
    }
}
