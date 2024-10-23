using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces
{
    public interface IRegionalDistributionCenterService
    {
        int TryPickSTO(Guid stockTransportOrderId);
    }
}
