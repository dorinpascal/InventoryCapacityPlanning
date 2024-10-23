using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class StockTransportOrderService(IStockTransportOrderStorage _storage) : IStockTransportOrderService
{
    public async Task<List<StockTransportOrder>> GetByLDC(string localDistributionCenterName)
    {
        var stockTransportOrder = await _storage.GetAllAsync();
        return stockTransportOrder.Where(sto => sto.LocalDistributionCenterName == localDistributionCenterName).ToList();
    }

    public async Task Create(StockTransportOrder stockTransportOrder)
    {
        await _storage.AddAsync(stockTransportOrder);
    }
}
