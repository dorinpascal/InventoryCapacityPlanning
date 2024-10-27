using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class StockTransportOrderStorage : IStockTransportOrderStorage
{
    private static readonly List<StockTransportOrder> _stockTransportOrders = [];

    public Task<StockTransportOrder> AddAsync(StockTransportOrder stockTransportOrder)
    {
        _stockTransportOrders.Add(stockTransportOrder);
        return Task.FromResult(stockTransportOrder);
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

    public Task<StockTransportOrder> UpdateAsync(StockTransportOrder sto)
    {
        var existingStoIndex = _stockTransportOrders.FindIndex(order => order.Id == sto.Id);
        if (existingStoIndex == -1) throw new ArgumentException("Invalid order id");
        
        // Replace the existing STO in the list with the updated STO using the new constructor
        var stockTransportOrder = new StockTransportOrder(sto.Id, sto.FinishedGoodsName, sto.Quantity, sto.RegionalDistributionCenterName, sto.LocalDistributionCenterName, sto.Status
        );
        _stockTransportOrders[existingStoIndex] = stockTransportOrder;
        return Task.FromResult(stockTransportOrder);
    }
}
