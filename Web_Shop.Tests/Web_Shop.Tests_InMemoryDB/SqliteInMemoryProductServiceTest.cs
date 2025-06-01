using Castle.Core.Logging;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.CustomQueries;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Mappings.PropertiesMappings;
using Web_Shop.Application.Services;
using Web_Shop.Persistence.Repositories;
using Web_Shop.Persistence.Repositories.Interfaces;
using Web_Shop.Persistence.UOW;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.Tests.Common.Sieve;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Tests_InMemoryDB
{
    public class SqliteInMemoryProductServiceTest : IDisposable
    {
        private readonly SqliteDatabaseFixture _databaseFixture;

        private readonly Mock<ILogger<Product>> _loggerMock;

        private readonly SieveProcessor _processor;
        private readonly SieveOptionsAccessor _optionsAccessor;

        public SqliteInMemoryProductServiceTest()
        {
            _databaseFixture = new SqliteDatabaseFixture();

            _loggerMock = new Mock<ILogger<Product>>();

            _optionsAccessor = new SieveOptionsAccessor();

            _processor = new ApplicationSieveProcessor(_optionsAccessor,
                new SieveCustomSortMethods(),
                new SieveCustomFilterMethods());
        }
        public void Dispose()
        {
            _databaseFixture.Dispose();
        }

        [Fact]
        public async Task ProductService_CreateNewProductAsync_ReturnsTrue()
        {
            {
                using var context = _databaseFixture.CreateContext();

                var unitOfWork = new UnitOfWork(context);

                var productService = new ProductService(_loggerMock.Object, _processor, _optionsAccessor, unitOfWork);

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
                Assert.Equal("AAA_BBB_ABC01", verifyResult.entity!.Sku);
            }
        }

        [Fact]
        public async Task ProductService_SearchAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var unitOfWork = new UnitOfWork(context);

            var productService = new ProductService(_loggerMock.Object, _processor, _optionsAccessor, unitOfWork);

            var model = new SieveModel
            {
                Filters = "Name@=Dron"
            };

            var searchResult = await productService.SearchAsync(model, resultEntity => DomainToDtoMapper.MapGetSingleProductDTO(resultEntity));

            Assert.True(searchResult.IsSuccess);
            Assert.Equal(1, searchResult.entityList!.TotalItemCount);
        }
    }
}
