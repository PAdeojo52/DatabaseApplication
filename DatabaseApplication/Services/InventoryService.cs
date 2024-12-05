using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DatabaseApplication.Data;
using DatabaseApplication.Interfaces;

namespace DatabaseApplication.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly QueryService _queryService;

        public InventoryService(QueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<long> GetTotalStockInAsync()
        {
            return await _queryService.GetTotalStockInAsync();
        }

        public async Task<long> GetTotalStockCheckedOutAsync()
        {
            return await _queryService.GetTotalStockCheckedOutAsync();
        }

        public async Task<Dictionary<string, long>> GetStockByCategoryAsync()
        {
            return await _queryService.GetStockByCategoryAsync();
        }

        public long GetTotalStockIn()
        {
            throw new NotImplementedException();
        }

        public long GetTotalStockCheckedOut()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, long> GetStockByCategory()
        {
            throw new NotImplementedException();
        }
    }
}