using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Domain;

public class StockTransportOrder
{
    public StockTransportOrder(string finishedGoodsName, int quantity, string regionalDistributionCenterName, string localDistributionCenterName)
    {
        Id = Guid.NewGuid();
        FinishedGoodsName = finishedGoodsName;
        Quantity = quantity;
        RegionalDistributionCenterName = regionalDistributionCenterName;
        LocalDistributionCenterName = localDistributionCenterName;
    }

    public StockTransportOrder(Guid id, string finishedGoodsName, int quantity, string regionalDistributionCenterName, string localDistributionCenterName, StockTransportOrderStatus status)
    {
        Id = id;
        FinishedGoodsName = finishedGoodsName;
        Quantity = quantity;
        RegionalDistributionCenterName = regionalDistributionCenterName;
        LocalDistributionCenterName = localDistributionCenterName;
        Status = status;
    }

    public Guid Id { get; }
    public string FinishedGoodsName { get; }
    public int Quantity { get; }
    public string RegionalDistributionCenterName { get; }
    public string LocalDistributionCenterName { get; }
    public StockTransportOrderStatus Status { get; private set; }

    public void UpdateStatus(StockTransportOrderStatus status) => Status = status;
}
