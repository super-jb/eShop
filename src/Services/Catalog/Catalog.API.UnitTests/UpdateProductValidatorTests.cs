using Catalog.API.Commands;
using Catalog.API.Data.Entities;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.UnitTests
{
    [TestClass]
    public class UpdateProductValidatorTests
    {
        private UpdateProductValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new UpdateProductValidator();
        }

        [TestMethod]
        public async Task ValidRequestShouldNotHaveValidationErrors()
        {
            // arrange
            var product = GenerateProduct();

            // act
            ValidationResult result = await _validator.ValidateAsync(new UpdateProductCommand(product.Id, product));

            // assert
            result.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public async Task NullProductShouldHaveErrorMessage()
        {
            // arrange
            var product = GenerateProduct();

            // act
            ValidationResult result = await _validator.ValidateAsync(new UpdateProductCommand(product.Id, null));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("'Product' must not be empty.");
        }

        [TestMethod]
        public async Task InvalidProductIdShouldHaveErrorMessage()
        {
            // arrange
            var product = GenerateProduct();
            product.Id = product.Id[0..^1]; // remove the last character
            string id = ObjectId.GenerateNewId().ToString();

            // act
            ValidationResult result = await _validator.ValidateAsync(new UpdateProductCommand(id, product));

            // assert
            result.Errors.Any(x => x.ErrorMessage == "Product Id must be valid 24 char hexadecimal").Should().BeTrue();
        }

        [TestMethod]
        public async Task InvalidIdShouldHaveErrorMessage()
        {
            // arrange
            var product = GenerateProduct();
            string id = ObjectId.GenerateNewId().ToString(); // different Id than Product.Id

            // act
            ValidationResult result = await _validator.ValidateAsync(new UpdateProductCommand(id, product));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("Request Id and Product Id must match");
        }

        [DataRow(0)]
        [DataRow(-3.5)]
        [DataTestMethod]
        public async Task InvalidPriceShouldHaveErrorMessage(double badPrice)
        {
            // arrange
            var product = GenerateProduct();
            product.Price = (decimal)badPrice; // decimal isn't a primitive number, can't be used as a DataRow param

            // act
            ValidationResult result = await _validator.ValidateAsync(new UpdateProductCommand(product.Id, product));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("Product Price must be > $0");
        }

        private static Product GenerateProduct()
        {
            return new Product
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Garmin Fenix 500x",
                Category = "Fitness",
                Description = "Cool watch",
                ImageFile = null,
                Summary = "This is the latest Garmin fitness watch",
                Price = 500
            };
        }
    }
}
