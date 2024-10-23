namespace LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;

public class RegionalDistributionCenter
{
    public RegionalDistributionCenter(
        string name,
        string finishedGoodsName,
        int finishedGoodsStockQuantity)
    {
        Name = name;
        FinishedGoodsName = finishedGoodsName;
        FinishedGoodsStockQuantity = finishedGoodsStockQuantity;
    }

    public string Name { get; }
    public string FinishedGoodsName { get; }
    public int FinishedGoodsStockQuantity { get; private set; }
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
        {
            throw new ArgumentException("Quantity cannot be negative.");
        }

        FinishedGoodsStockQuantity = newQuantity;
    }
}
