using Ardalis.GuardClauses;
using Catalog.API.Data.Entities;
using Catalog.API.Repositories;
using FluentValidation;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Commands
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IProductRepository _repository;

        public UpdateProductValidator(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));

            RuleFor(x => x.Product).NotNull();

            RuleFor(x => x.Id)
                .MustAsync((entity, value, c) => DoesProductExist(value))
                .WithMessage("Product Id can't be empty");
        }

        private async Task<bool> DoesProductExist(string id)
        {
            if ((await _repository.GetProduct(id.ToString())) != null)
            {
                return false;
            }

            return true;
        }
    }
}

