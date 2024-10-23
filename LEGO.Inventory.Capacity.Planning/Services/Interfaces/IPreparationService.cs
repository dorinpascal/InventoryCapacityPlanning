using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces
{
    public interface IPreparationService
    {
        void PrepareSalesOrder(SalesOrder salesOrder);
    }
}
