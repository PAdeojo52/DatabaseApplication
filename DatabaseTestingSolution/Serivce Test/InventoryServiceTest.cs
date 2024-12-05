using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DatabaseApplication.Data;
using DatabaseApplication.Services;
using DatabaseApplication.EntityModels;
using DatabaseTestingSolution.DBInitializations;
using NUnit.Framework;

namespace DatabaseTestingSolution
{
    public class InventoryServiceTest : IClassFixture<DatabaseTestFixture>
    {

        private readonly ApplicationDbContext _context;
        private readonly InventoryService _inventoryService;
        private readonly QueryService _queryService;


        public InventoryServiceTest(DatabaseTestFixture fixture)
        {
            _context = fixture.Context; // Get the real database context from the fixture
            _inventoryService = new InventoryService(_queryService);
        }


        [Fact, CancelAfter(10000)]
        public void GetTotalStockIn()
        {
            // Act
            var totalStockIn = _inventoryService.GetTotalStockIn();

            Console.WriteLine(totalStockIn);

            // Assert
            Xunit.Assert.True(totalStockIn > 0); // Example assertion to verify the total stock is greater than 0
        }

        [Fact, CancelAfter(10000)]
        public void GetStockByCategory()
        {
            // Act
            var stockByCategory = _inventoryService.GetStockByCategory();
            Console.WriteLine(stockByCategory);

            // Assert
            Xunit.Assert.NotEmpty(stockByCategory); // Ensure the dictionary is not empty
            Xunit.Assert.True(stockByCategory.Values.All(stock => stock >= 0)); // Validate stock values are non-negative
        }




    }
}