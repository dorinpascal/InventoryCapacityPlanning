using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Storage
{
    public class Storage : IStorage
    {
        public List<LocalDistributionCenter> LocalDistributionCenters { get; set; } = new List<LocalDistributionCenter>() {
                new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 50, 20, 20),
                new LocalDistributionCenter("Eastern Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 60, 20, 20),
                new LocalDistributionCenter("Western Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 70, 20, 20),
            };

        public RegionalDistributionCenter RegionalDistributionCenter { get; } = new RegionalDistributionCenter("LEGO European Distribution Center", "Lego - Harry Potter", 10);

        public List<StockTransportOrder> StockTransportOrders { get; set; } = new List<StockTransportOrder>();

        public List<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();

        public List<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();
    }
}
