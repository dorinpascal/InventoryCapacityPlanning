using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Storage
{
    public interface IStorage
    {
        List<LocalDistributionCenter> LocalDistributionCenters { get; }
        RegionalDistributionCenter RegionalDistributionCenter { get; }
        List<StockTransportOrder> StockTransportOrders { get; set; }
        List<SalesOrder> SalesOrders { get; set; }
        List<GoodsReceipt> GoodsReceipts { get; set; }
    }
}
