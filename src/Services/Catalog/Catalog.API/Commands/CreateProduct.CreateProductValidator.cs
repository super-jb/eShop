using Ardalis.GuardClauses;
using Catalog.API.Repositories;
using FluentValidation;
using System.Threading.Tasks;

namespace Catalog.API.Commands
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IProductRepository _repository;

        public CreateProductValidator(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));

            RuleFor(x => x.Product).NotNull();

            When(x => x.Product != null, () =>
            {
                RuleFor(x => x.Product.Id)
                    .Must((entity, value, c) => !IsIdEmpty(value))
                    .WithMessage("Product Id can't be empty");

                RuleFor(x => x.Product.Price)
                     .Must((entity, value, c) => IsPriceValid(value))
                     .WithMessage("Product Price must be > $0");

                RuleFor(x => x.Product.Id)
                    .MustAsync((entity, value, c) => IsIdUnique(value))
                    .WithMessage("Product already exists with this Id");
            });
        }

        private bool IsIdEmpty(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return false;
            }

            return true;
        }

        private bool IsPriceValid(decimal price)
        {
            return price > 0;
        }

        private async Task<bool> IsIdUnique(string id)
        {
            if ((await _repository.GetProduct(id)) != null)
            {
                return false;
            }

            return true;
        }
    }
}
