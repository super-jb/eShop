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
    public class DeleteProductValidatorTests
    {
        private DeleteProductValidator _validator;
        private Mock<IProductRepository> _mockProductRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockProductRepository = new Mock<IProductRepository>(MockBehavior.Strict);
            _validator = new DeleteProductValidator(_mockProductRepository.Object);
        }

        [TestMethod]
        public async Task NonExistingProductShouldHaveErrorMessage()
        {
            // arrange
            string id = ObjectId.GenerateNewId().ToString();
            _mockProductRepository.Setup(x => x.GetProduct(id)).ReturnsAsync(new Product());

            // act
            ValidationResult result = await _validator.ValidateAsync(new DeleteProductCommand(id));

            // assert
            result.Errors.Single().ErrorMessage.Should().Be("Product doesn't exist");
        }
    }
}
