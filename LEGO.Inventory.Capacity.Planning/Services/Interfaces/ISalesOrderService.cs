using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface ISalesOrderService
{
    /// <summary>
    /// Creates a new sales order and adds it to the system.
    /// </summary>
    /// <param name="salesOrder">The sales order to be created.</param>
    /// <exception cref="ArgumentException">Thrown if the local distribution center name is invalid.</exception>
    Task Create(SalesOrder salesOrder);

    /// <summary>
    /// Retrieves all sales orders from the system.
    /// </summary>
    /// <returns>A list of all existing sales orders.</returns>
    Task<List<SalesOrder>> GetAll();
}
