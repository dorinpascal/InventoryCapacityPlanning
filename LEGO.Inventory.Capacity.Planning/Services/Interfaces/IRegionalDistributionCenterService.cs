namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces
{
    public interface IRegionalDistributionCenterService
    {
        int TryPickSTO(Guid stockTransportOrderId);
    }
}
