using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface IPreparationService
{
    Task PrepareSalesOrder(SalesOrder salesOrder);
}
