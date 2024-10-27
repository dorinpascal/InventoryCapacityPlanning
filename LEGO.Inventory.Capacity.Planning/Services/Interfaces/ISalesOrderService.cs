using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface ISalesOrderService
{
    /// <summary>
    /// Creates a new sales order and adds it to the system, returning the created sales order.
    /// </summary>
    /// <param name="salesOrder">The sales order to be created.</param>
    /// <returns>A <see cref="Task{SalesOrder}"/> representing the asynchronous operation, with the created <see cref="SalesOrder"/> object as the result.</returns>
    /// <exception cref="ArgumentException">Thrown if the local distribution center name is invalid.</exception>
    Task<SalesOrder> Create(SalesOrder salesOrder);

    /// <summary>
    /// Retrieves all sales orders from the system.
    /// </summary>
    /// <returns>A <see cref="Task{List{SalesOrder}}"/> representing the asynchronous operation, with a list of all existing <see cref="SalesOrder"/> objects as the result.</returns>
    Task<List<SalesOrder>> GetAll();
}
