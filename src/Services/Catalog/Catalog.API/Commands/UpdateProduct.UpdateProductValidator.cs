using FluentValidation;
using MongoDB.Bson;
using System;

namespace Catalog.API.Commands
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Product).NotNull();

            When(x => x.Product != null, () =>
            {
                RuleFor(x => x.Product.Id)
                    .Must((entity, value, c) => IsIdValid(value))
                    .WithMessage("Product Id must be valid 24 char hexadecimal");

                RuleFor(x => x.Product.Id)
                    .Equal(x => x.Id, StringComparer.OrdinalIgnoreCase)
                    .WithMessage("Request Id and Product Id must match");

                RuleFor(x => x.Product.Price)
                     .Must((entity, value, c) => IsPriceValid(value))
                     .WithMessage("Product Price must be > $0");
            });
        }

        private bool IsIdValid(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId o))
            {
                return true;
            }

            return false;
        }

        private bool IsPriceValid(decimal price)
        {
            return price > 0;
        }
    }
}

