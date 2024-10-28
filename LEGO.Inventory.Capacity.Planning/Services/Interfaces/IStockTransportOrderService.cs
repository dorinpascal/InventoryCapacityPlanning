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
    /// Creates a new stock transport order and adds it to the storage, returning the created order.
    /// </summary>
    /// <param name="stockTransportOrder">The stock transport order to be created and added to the storage.</param>
    /// <returns>A <see cref="Task{StockTransportOrder}"/> representing the asynchronous operation, with the created <see cref="StockTransportOrder"/> object as the result.</returns>
    Task<StockTransportOrder> Create(StockTransportOrder stockTransportOrder);

}
