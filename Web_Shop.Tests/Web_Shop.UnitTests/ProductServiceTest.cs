using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using Sieve.Services;
using System.Runtime.CompilerServices;
using Web_Shop.Application.CustomQueries;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Mappings.PropertiesMappings;
using Web_Shop.Application.Services;
using Web_Shop.Persistence.Repositories.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.Tests.Common.Sieve;
using WWSI_Shop.Persistence.MySQL.Model;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop.UnitTests
{
    public class ProductServiceTest
    {
        private readonly Mock<ILogger<Product>> _loggerMock;

        private readonly Mock<ApplicationSieveProcessor> _processorMock;
        private readonly Mock<SieveOptionsAccessor> _optionsAccessorMock;

        public ProductServiceTest()
        {
            _loggerMock = new Mock<ILogger<Product>>();

            _optionsAccessorMock = new Mock<SieveOptionsAccessor>();

            _processorMock = new Mock<ApplicationSieveProcessor>(_optionsAccessorMock.Object,
                new Mock<SieveCustomSortMethods>().Object,
                new Mock<SieveCustomFilterMethods>().Object);
        }

        [Theory]
        [InlineData(false)]
        public async Task ProductService_CreateNewProductAsync_ReturnsTrue(bool skuExists)
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.SkuExistsAsync(It.IsAny<string>())).Returns((string sku) => Task.FromResult(skuExists));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(0));

            var productService = new ProductService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var addUpdateProductDTO = new AddUpdateProductDTO()
            {
                Name = "TestName",
                Description = "TestDescription",
                Price = 5,
                Sku = "AAA_BBB_ABC01"
            };

            var verifyResult = await productService.CreateNewProductAsync(addUpdateProductDTO);

            Assert.True(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, verifyResult.StatusCode);
        }

        [Theory]
        [InlineData(true)]
        public async Task ProductService_CreateNewProductAsync_ReturnsFalse(bool skuExists)
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.SkuExistsAsync(It.IsAny<string>())).Returns((string sku) => Task.FromResult(skuExists));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(0));

            var productService = new ProductService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var addUpdateProductDTO = new AddUpdateProductDTO()
            {
                Name = "TestName",
                Description = "TestDescription",
                Price = 5,
                Sku = "AAA_BBB_ABC01"
            };

            var verifyResult = await productService.CreateNewProductAsync(addUpdateProductDTO);

            Assert.False(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, verifyResult.StatusCode);
        }

    }
}