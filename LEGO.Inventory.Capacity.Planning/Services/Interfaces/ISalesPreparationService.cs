using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface ISalesPreparationService
{
    Task Prepare(SalesOrder salesOrder);
}
