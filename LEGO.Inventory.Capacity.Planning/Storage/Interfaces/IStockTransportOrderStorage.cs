using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

public interface IStockTransportOrderStorage
{
    Task<List<StockTransportOrder>> GetAllAsync();
    Task<StockTransportOrder> AddAsync(StockTransportOrder stockTransportOrder);
    Task<StockTransportOrder?> GetByIdAsync(Guid id);
    Task<StockTransportOrder> UpdateAsync(StockTransportOrder sto);
}
