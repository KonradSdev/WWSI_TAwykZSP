using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Persistence.Repositories;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Tests_InMemoryDB
{
    public class SqliteInMemoryProductRepositoryTest : IDisposable
    {
        private readonly SqliteDatabaseFixture _databaseFixture;
        public SqliteInMemoryProductRepositoryTest()
        {
            _databaseFixture = new SqliteDatabaseFixture();
        }
        public void Dispose()
        {
            _databaseFixture.Dispose();
        }

        [Fact]
        public async Task ProductRepository_SkuExistsAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var skuExists = await productRepository.SkuExistsAsync("AKC_LAP_TOR02");

            Assert.True(skuExists);
        }

        [Fact]
        public async Task ProductRepository_SkuExistsAsync_ReturnsFalse()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var skuExists = await productRepository.SkuExistsAsync("GGG_FFF_HHH01");

            Assert.False(skuExists);
        }

        [Fact]
        public async Task ProductRepository_NameExistsAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var nameExists = await productRepository.NameExistsAsync("Torba na laptopa 15.6 cala");

            Assert.True(nameExists);
        }

        [Fact]
        public async Task ProductRepository_NameExistsAsync_ReturnsFalse()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var nameExists = await productRepository.NameExistsAsync("Dron wojskowy niebieski");

            Assert.False(nameExists);
        }

        [Fact]
        public async Task ProductRepository_IsNameEditAllowedAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var nameEditAllowed = await productRepository.IsNameEditAllowedAsync("Torba na laptopa 15.6 cala", 1);

            Assert.True(nameEditAllowed);
        }

        [Fact]
        public async Task ProductRepository_IsNameEditAllowedAsync_ReturnsFalse()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var nameEditAllowed = await productRepository.IsNameEditAllowedAsync("Dron wojskowy", 1);

            Assert.False(nameEditAllowed);
        }

        [Fact]
        public async Task ProductRepository_GetByNameAsync_Exists()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var product = await productRepository.GetByNameAsync("Torba na laptopa 15.6 cala");

            Assert.Equal("Torba na laptopa 15.6 cala", product.Name);
            Assert.Equal("Stylowa i wytrzymała torba z wyściełaną przegrodą na laptopa, idealna do pracy i podróży.", product.Description);
        }

        [Fact]
        public async Task ProductRepository_GetByNameAsync_NotExist()
        {
            using var context = _databaseFixture.CreateContext();

            var productRepository = new ProductRepository(context);

            var product = await productRepository.GetByNameAsync("Dron wojskowy niebieski");

            Assert.Null(product);
        }
    }
}
