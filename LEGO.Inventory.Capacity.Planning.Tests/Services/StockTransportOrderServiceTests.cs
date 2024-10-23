using System;
using System.Linq;
using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using Xunit;

namespace LEGO.Inventory.Capacity.Planning.Tests.Services
{
    public class StockTransportOrderServiceTests
    {
        private readonly IRegionalDistributionCenterStorage _storage;
        private readonly IStockTransportOrderService _stockTransportOrderService;
        public StockTransportOrderServiceTests() 
        {
            _storage = new Storage.RegionalDistributionCenterStorage();
            _stockTransportOrderService = new StockTransportOrderService(_storage);

        }
        [Fact]
        public void GetStockTransportOrdersByLDC_ShouldReturnMatchingOrders()
        {
            // Arrange

            var ldcName = "Central Warehouse Europe";
            var sto1 = new StockTransportOrder("Lego - Harry Potter", 10, "LEGO European Distribution Center", ldcName);
            var sto2 = new StockTransportOrder("Lego - Star Wars", 15, "LEGO European Distribution Center", ldcName);
            var sto3 = new StockTransportOrder("Lego - Ninjago", 5, "LEGO European Distribution Center", "Other LDC");

            _storage.StockTransportOrders.AddRange(new[] { sto1, sto2, sto3 });

            // Act
            var result = _stockTransportOrderService.GetByLDC(ldcName);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(sto1, result);
            Assert.Contains(sto2, result);
            Assert.DoesNotContain(sto3, result);
        }

        [Fact]
        public void CreateStockTransportOrder_ShouldAddOrderToStorage()
        {
            // Arrange

            var sto = new StockTransportOrder("Lego - Star Wars", 10, "LEGO European Distribution Center", "Western Warehouse Europe");

            // Act
            _stockTransportOrderService.Create(sto);

            // Assert
            Assert.Single(_storage.StockTransportOrders);
            Assert.Contains(sto, _storage.StockTransportOrders);
        }

    }
}
