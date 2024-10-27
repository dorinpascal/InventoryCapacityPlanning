namespace LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;

public class LocalDistributionCenter
{
    public LocalDistributionCenter(
        string name,
        string regionalDistributionCenterName,
        string finishedGoodsName,
        int finishedGoodsStockQuantity,
        int safetyStockQuantity,
        int safetyStockThreshold)
    {
        Name = name;
        RegionalDistributionCenterName = regionalDistributionCenterName;
        FinishedGoodsName = finishedGoodsName;
        FinishedGoodsStockQuantity = finishedGoodsStockQuantity;
        SafetyStockQuantity = safetyStockQuantity;
        SafetyStockThreshold = safetyStockThreshold;
    }

    public string Name { get; }
    public string RegionalDistributionCenterName { get; }
    public string FinishedGoodsName { get; }
    public int FinishedGoodsStockQuantity { get; set; }
    public int SafetyStockQuantity { get; set; }
    public int SafetyStockThreshold { get; }
}
