namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface IRegionalDistributionCenterService
{
    Task<int> TryPickSTOAsync(Guid stockTransportOrderId);
}
