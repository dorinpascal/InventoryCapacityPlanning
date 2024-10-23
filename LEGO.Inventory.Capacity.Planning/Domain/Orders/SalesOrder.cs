namespace LEGO.Inventory.Capacity.Planning.Domain.Orders;

public class SalesOrder
{
    public SalesOrder(
        string finishedGoodsName,
        int quantity,
        string localDistributionCenterName)
    {
        FinishedGoodsName = finishedGoodsName;
        Quantity = quantity;
        LocalDistributionCenterName = localDistributionCenterName;
    }

    public string FinishedGoodsName { get; }
    public int Quantity { get; }
    public string LocalDistributionCenterName { get; }
}
