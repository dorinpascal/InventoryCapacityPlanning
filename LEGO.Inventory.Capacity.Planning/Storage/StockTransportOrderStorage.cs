using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class StockTransportOrderStorage : IStockTransportOrderStorage
{
    private readonly List<StockTransportOrder> _stockTransportOrders = [];

    public Task AddAsync(StockTransportOrder stockTransportOrder)
    {
        _stockTransportOrders.Add(stockTransportOrder);
        return Task.CompletedTask;
    }

    public Task<List<StockTransportOrder>> GetAllAsync()
    {
        return Task.FromResult(_stockTransportOrders.ToList());
    }
}
