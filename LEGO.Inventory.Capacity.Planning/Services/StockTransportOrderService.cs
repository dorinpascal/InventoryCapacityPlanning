using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class StockTransportOrderService(IStockTransportOrderStorage _stockTransportOrderStorage, IRegionalDistributionCenterStorage _regionalDistributionCenterStorage) : IStockTransportOrderService
{
    public async Task<List<StockTransportOrder>> GetByLDC(string localDistributionCenterName, string? status = null)
    {
        StockTransportOrderValidator.ValidateStatus(status);
        var stockTransportOrders = await _stockTransportOrderStorage.GetAllAsync();
        var query = stockTransportOrders.Where(sto => sto.LocalDistributionCenterName == localDistributionCenterName);
        if (!string.IsNullOrEmpty(status))
        {
            Enum.TryParse(typeof(StockTransportOrderStatus), status, true, out var parsedStatus);
            query = query.Where(sto => sto.Status == (StockTransportOrderStatus)parsedStatus!);
        }
        return query.ToList();
    }

    public async Task Create(StockTransportOrder stockTransportOrder)
    {
        await _stockTransportOrderStorage.AddAsync(stockTransportOrder);
    }

    public async Task PickStockTransportOrder(Guid id)
    {
        var sto = await _stockTransportOrderStorage.GetByIdAsync(id) ?? throw new ArgumentException($"Stock transport order with ID {id} not found.");

        // Ensure the STO is open before picking
        if (sto.Status is not StockTransportOrderStatus.Open)
        {
            throw new InvalidOperationException("Stock transport order must be open to be picked.");
        }
        var rdc = await _regionalDistributionCenterStorage.GetAllAsync();
        if (sto.Quantity > rdc.FinishedGoodsStockQuantity)
        {
            throw new InvalidOperationException("Insufficient stock at the RDC to fulfill the stock transport order.");
        }
        // Update the status of the STO to picked
        sto.UpdateStatus(StockTransportOrderStatus.Picked);

        // Reduce the stock quantity in the RDC
        rdc.UpdateQuantity(rdc.FinishedGoodsStockQuantity - sto.Quantity);

        // Persist the changes in the storage
        await _stockTransportOrderStorage.UpdateAsync(sto);
    }
}
