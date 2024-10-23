using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces
{
    public interface IPreparationService
    {
        void PrepareSalesOrder(SalesOrder salesOrder);
    }
}
