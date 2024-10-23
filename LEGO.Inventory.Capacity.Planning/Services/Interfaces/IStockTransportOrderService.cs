using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces
{
    public interface IStockTransportOrderService
    {
        List<StockTransportOrder> GetStockTransportOrdersByLDC(string localDistributionCenterName);
        void CreateStockTransportOrder(StockTransportOrder stockTransportOrder);
    }
}
