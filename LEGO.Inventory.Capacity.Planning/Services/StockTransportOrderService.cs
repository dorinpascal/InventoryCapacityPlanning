using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services
{
    public class StockTransportOrderService : IStockTransportOrderService
    {
        private readonly IStorage _storage;

        public StockTransportOrderService(IStorage storage)
        {
            _storage = storage;
        }

        List<StockTransportOrder> IStockTransportOrderService.GetStockTransportOrdersByLDC(string localDistributionCenterName)
        {
            return _storage.StockTransportOrders.Where(sto => sto.LocalDistributionCenterName == localDistributionCenterName).ToList();
        }

        public void CreateStockTransportOrder(StockTransportOrder stockTransportOrder)
        {
            _storage.StockTransportOrders.Add(stockTransportOrder);
        }
    }
}
