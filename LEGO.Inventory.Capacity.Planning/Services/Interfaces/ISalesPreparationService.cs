using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface ISalesPreparationService
{
    /// <summary>
    /// Prepares the specified sales order by ensuring the local distribution center (LDC) has sufficient stock to fulfill the order.
    /// </summary>
    /// <param name="salesOrder">The sales order to be prepared.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method checks the stock availability at the LDC for the requested sales order quantity. 
    /// If there is insufficient finished goods stock, it utilizes safety stock if available. 
    /// If further stock is needed, a new stock transport order (STO) is created to replenish the LDC.
    /// </remarks>
    Task Prepare(SalesOrder salesOrder);
}
