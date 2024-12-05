using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseTestingSolution.DBInitializations;
using DatabaseApplication.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert = Xunit.Assert;

namespace DatabaseTestingSolution.Serivce_Test
{
    public class QueryServiceTests : IClassFixture<DatabaseTestFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly QueryService _queryService;

        public QueryServiceTests(DatabaseTestFixture fixture)
        {
            _context = fixture.Context;
            _queryService = new QueryService(_context);
        }

        [Fact]
        public async Task GetTotalStockInAsync_ShouldReturnCorrectSum()
        {
            // Arrange
            await SeedDatabaseAsync();

            // Act
            var totalStockIn = await _queryService.GetTotalStockInAsync();

            // Assert
            Assert.Equal(100, totalStockIn); // Total stock: 50 + 30 + 20 = 100
        }

        [Fact]
        public async Task GetTotalStockCheckedOutAsync_ShouldReturnCorrectSum()
        {
            // Arrange
            await SeedDatabaseAsync();

            // Act
            var totalStockCheckedOut = await _queryService.GetTotalStockCheckedOutAsync();

            // Assert
            Assert.Equal(50, totalStockCheckedOut); // Example stockout: 15 + 25 + 10 = 50
        }

        [Fact]
        public async Task GetStockByCategoryAsync_ShouldReturnCorrectStockGroupedByCategory()
        {
            // Arrange
            await SeedDatabaseAsync();

            // Act
            var stockByCategory = await _queryService.GetStockByCategoryAsync();

            // Assert
            Assert.Equal(2, stockByCategory.Count); // Two categories: Electronics, Clothing
            Assert.Equal(70, stockByCategory["Electronics"]); // 50 + 20 = 70
            Assert.Equal(30, stockByCategory["Clothing"]); // 30
        }

        [Fact]
        public async Task GetTotalStockInAsync_ShouldReturnDefaultOnException()
        {
            // Arrange: Simulate a database failure
            _context.Dispose();

            // Act
            var totalStockIn = await _queryService.GetTotalStockInAsync();

            // Assert
            Assert.Equal(1, totalStockIn); // Default value on exception
        }

        [Fact]
        public async Task GetStockByCategoryAsync_ShouldReturnDefaultOnException()
        {
            // Arrange: Simulate a database failure
            _context.Dispose();

            // Act
            var stockByCategory = await _queryService.GetStockByCategoryAsync();

            // Assert
            Assert.Single(stockByCategory);
            Assert.Equal(0, stockByCategory["Unknown"]); // Default value on exception
        }

        private async Task SeedDatabaseAsync()
        {
            // Clear existing data
            _context.Items.RemoveRange(_context.Items);
            await _context.SaveChangesAsync();

            // Seed test data
            _context.Items.AddRange(
                new Item { Category = "Electronics", Stock = 50, StockOut = 15 },
                new Item { Category = "Clothing", Stock = 30, StockOut = 25 },
                new Item { Category = "Electronics", Stock = 20, StockOut = 10 }
            );
            await _context.SaveChangesAsync();
        }
    }
}
