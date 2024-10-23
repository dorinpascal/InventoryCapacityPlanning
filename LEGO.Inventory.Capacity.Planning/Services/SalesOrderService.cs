using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services
{
    public class SalesOrderService(IStorage _storage, ILogger<SalesOrderService> _logger) : ISalesOrderService
    {
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
