using Catalog.API.Commands;
using Catalog.API.Data.Entities;
using Catalog.API.Repositories;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.UnitTests
{
    [TestClass]
    public class CreateProductValidatorTests
    {
        private CreateProductValidator _validator;
        private Mock<IProductRepository> _mockProductRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockProductRepository = new Mock<IProductRepository>(MockBehavior.Strict);
            _validator = new CreateProductValidator(_mockProductRepository.Object);
        }

        [TestMethod]
        public async Task ValidRequestShouldNotHaveValidationErrors()
        {
            // arrange
            var product = GenerateProduct();
            _mockProductRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(null as Product);

            // act
            ValidationResult result = await _validator.ValidateAsync(new CreateProductCommand(product));

            // assert
            result.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public async Task NullProductShouldHaveErrorMessage()
        {
            // act
            ValidationResult result = await _validator.ValidateAsync(new CreateProductCommand(null));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("'Product' must not be empty.");
        }

        [TestMethod]
        public async Task InvalidIdShouldHaveErrorMessage()
        {
            // arrange
            var product = GenerateProduct();
            product.Id = product.Id[0..^1]; // remove the last character
            _mockProductRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(null as Product);

            // act
            ValidationResult result = await _validator.ValidateAsync(new CreateProductCommand(product));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("Product Id must be valid 24 char hexadecimal");
        }

        [DataRow(0)]
        [DataRow(-3.5)]
        [DataTestMethod]
        public async Task InvalidPriceShouldHaveErrorMessage(double badPrice)
        {
            // arrange
            var product = GenerateProduct();
            _mockProductRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(null as Product);
            product.Price = (decimal)badPrice; // decimal isn't a primitive number, can't be used as a DataRow param

            // act
            ValidationResult result = await _validator.ValidateAsync(new CreateProductCommand(product));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("Product Price must be > $0");
        }

        [TestMethod]
        public async Task ExistingIdShouldHaveErrorMessage()
        {
            // arrange
            var product = GenerateProduct();
            _mockProductRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(product);

            // act
            ValidationResult result = await _validator.ValidateAsync(new CreateProductCommand(product));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("Product already exists with this Id");
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
