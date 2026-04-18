using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Project.API.Controllers.V1;
using Project.Core.Entities.Business;
using Project.Core.Interfaces.IServices;

namespace Project.UnitTest.API
{
    public class ProductControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private Mock<ILogger<ProductController>> _loggerMock;
        private IMemoryCache _memoryCache;
        private ProductController _productController;

        [SetUp]
        public void Setup()
        {
            _productServiceMock = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _productController = new ProductController(_loggerMock.Object, _productServiceMock.Object, _memoryCache);
        }

        [Test]
        public async Task Get_ReturnsViewWithListOfProducts()
        {
            // Arrange
            var products = new List<ProductViewModel>
            {
                new ProductViewModel { Id = 1, Code = "P001", Name = "Product A", Price = 9.99, IsActive = true },
                new ProductViewModel { Id = 2, Code = "P002", Name = "Product B", Price = 19.99, IsActive = true }
            };

            _productServiceMock.Setup(service => service.GetAll(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(products);

            // Act
            var result = await _productController.Get(CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);

            var model = (IEnumerable<ProductViewModel>)okObjectResult.Value;
            Assert.NotNull(model);
            Assert.That(model.Count(), Is.EqualTo(products.Count));
        }

        [Test]
        public async Task GetPrice_ReturnsOkWithProductPrice()
        {
            // Arrange
            const double expectedPrice = 99.95;
            _productServiceMock
                .Setup(service => service.PriceCheck(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPrice);

            // Act
            var result = await _productController.GetPrice(1, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as ResponseViewModel<double>;
            Assert.NotNull(model);
            Assert.IsTrue(model.Success);
            Assert.That(model.Data, Is.EqualTo(expectedPrice));
        }
    }
}
