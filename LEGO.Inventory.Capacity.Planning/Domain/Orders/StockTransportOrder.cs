namespace LEGO.Inventory.Capacity.Planning.Domain;

public enum StockTransportOrderStatus
{
    Open,
    Picked
}


public class StockTransportOrder
{
    public StockTransportOrder(
        string finishedGoodsName,
        int quantity,
        string regionalDistributionCenterName,
        string localDistributionCenterName)
    {
        Id = Guid.NewGuid();
        FinishedGoodsName = finishedGoodsName;
        Quantity = quantity;
        RegionalDistributionCenterName = regionalDistributionCenterName;
        LocalDistributionCenterName = localDistributionCenterName;
    }

    public Guid Id { get; }
    public string FinishedGoodsName { get; }
    public int Quantity { get; }
    public string RegionalDistributionCenterName { get; }
    public string LocalDistributionCenterName { get; }
    public StockTransportOrderStatus Status { get; private set; }

    public void UpdateStatus(StockTransportOrderStatus status) => Status = status;
}