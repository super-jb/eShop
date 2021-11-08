using Ardalis.GuardClauses;
using Catalog.API.Repositories;
using FluentValidation;
using System.Threading.Tasks;

namespace Catalog.API.Commands
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;

        public DeleteProductValidator(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));

            RuleFor(x => x.Id)
                .MustAsync((entity, value, c) => ProductDoesNotExist(value))
                .WithMessage("Product doesn't exist");
        }

        private async Task<bool> ProductDoesNotExist(string id)
        {
            if (await _repository.GetProduct(id) != null)
            {
                return false;
            }

            return true;
        }
    }
}

