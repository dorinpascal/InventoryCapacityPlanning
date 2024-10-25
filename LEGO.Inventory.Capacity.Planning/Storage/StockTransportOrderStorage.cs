using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class StockTransportOrderStorage : IStockTransportOrderStorage
{
    private static readonly List<StockTransportOrder> _stockTransportOrders = [];

    public Task AddAsync(StockTransportOrder stockTransportOrder)
    {
        _stockTransportOrders.Add(stockTransportOrder);
        return Task.CompletedTask;
    }

    public Task<List<StockTransportOrder>> GetAllAsync()
    {
        return Task.FromResult(_stockTransportOrders.ToList());
    }

    public Task<StockTransportOrder?> GetByIdAsync(Guid id)
    {
        var sto = _stockTransportOrders.Find(order => order.Id == id);
        return Task.FromResult(sto);
    }

    public Task UpdateAsync(StockTransportOrder sto)
    {
        var existingStoIndex = _stockTransportOrders.FindIndex(order => order.Id == sto.Id);
        if (existingStoIndex != -1)
        {
            // Replace the existing STO in the list with the updated STO using the new constructor
            _stockTransportOrders[existingStoIndex] = new StockTransportOrder(sto.Id, sto.FinishedGoodsName, sto.Quantity, sto.RegionalDistributionCenterName, sto.LocalDistributionCenterName, sto.Status
            );
        }
        return Task.CompletedTask;
    }
}
