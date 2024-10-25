using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

public interface IStockTransportOrderStorage
{
    Task<List<StockTransportOrder>> GetAllAsync();
    Task AddAsync(StockTransportOrder stockTransportOrder);
    Task<StockTransportOrder?> GetByIdAsync(Guid id);
    Task UpdateAsync(StockTransportOrder sto);
}
