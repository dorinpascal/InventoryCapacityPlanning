using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IStorage _storage;
        private readonly ILogger<SalesOrderService> _logger;

        public SalesOrderService(IStorage storage, ILogger<SalesOrderService> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        public List<SalesOrder> GetSalesOrders()
        {
            return _storage.SalesOrders;
        }

        public void CreateSalesOrder(SalesOrder salesOrder)
        {
            var _localDistributionCenter = _storage.LocalDistributionCenters.FirstOrDefault(ldc => ldc.Name == salesOrder.LocalDistributionCenterName);

            if (_localDistributionCenter == null)
            {
                _logger.LogError("invalid local distribution center name");
                throw new Exception("invalid local distribution center name");
            }
            else
            {
                _storage.SalesOrders.Add(salesOrder);
                _logger.LogInformation($"new order created: " + salesOrder.FinishedGoodsName + " : " + salesOrder.Quantity + " -LDC: " + salesOrder.LocalDistributionCenterName);
            }
        }

    }
}
