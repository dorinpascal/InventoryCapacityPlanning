using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface IStockTransportOrderService
{
    Task<List<StockTransportOrder>> GetByLDC(string localDistributionCenterName);
    Task Create(StockTransportOrder stockTransportOrder);
}
