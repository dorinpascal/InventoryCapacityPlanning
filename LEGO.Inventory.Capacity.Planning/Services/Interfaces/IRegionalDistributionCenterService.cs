namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface IRegionalDistributionCenterService
{
    /// <summary>
    /// Attempts to pick the specified stock transport order (STO) by reducing the stock quantity at the regional distribution center (RDC).
    /// </summary>
    /// <param name="stockTransportOrderId">The unique identifier of the stock transport order to be picked.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the quantity left in stock after picking the STO.
    /// </returns>
    /// <remarks>
    /// This method attempts to fulfill a stock transport order by reducing the stock at the regional distribution center. 
    /// If successful, it updates the status of the STO to 'Picked' and adjusts the stock quantity.
    /// </remarks>
    Task<int> TryPickSTOAsync(Guid stockTransportOrderId);
}
