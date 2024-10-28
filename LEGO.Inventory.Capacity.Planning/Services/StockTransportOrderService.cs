using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class StockTransportOrderService(IStockTransportOrderStorage _stockTransportOrderStorage) : IStockTransportOrderService
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

    public async Task<StockTransportOrder> Create(StockTransportOrder stockTransportOrder)
    {
        return await _stockTransportOrderStorage.AddAsync(stockTransportOrder);
    }
}
