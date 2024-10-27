using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface IStockTransportOrderService
{
    /// <summary>
    /// Retrieves stock transport orders for a specified local distribution center (LDC) and optional status.
    /// </summary>
    /// <param name="localDistributionCenterName">The name of the local distribution center to filter stock transport orders by.</param>
    /// <param name="status">Optional. The status of the stock transport orders to filter by (e.g., "Open", "Picked").</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a list of stock transport orders that match the criteria.</returns>
    Task<List<StockTransportOrder>> GetByLDC(string localDistributionCenterName, string? status = null);

    /// <summary>
    /// Creates a new stock transport order and adds it to the storage.
    /// </summary>
    /// <param name="stockTransportOrder">The stock transport order to be created and added to the storage.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Create(StockTransportOrder stockTransportOrder);

    /// <summary>
    /// Picks a stock transport order by its unique identifier, updating its status to indicate it has been picked.
    /// </summary>
    /// <param name="id">The unique identifier of the stock transport order to pick.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the stock transport order with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the stock transport order cannot be picked due to insufficient stock or incorrect status.</exception>
    Task PickStockTransportOrder(Guid id);
}
