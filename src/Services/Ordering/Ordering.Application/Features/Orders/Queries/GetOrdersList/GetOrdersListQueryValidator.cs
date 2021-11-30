using FluentValidation;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryValidator : AbstractValidator<GetOrdersListQuery>
{
    public GetOrdersListQueryValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage($"{nameof(GetOrdersListQuery.UserName)} must be provided");
    }
}
