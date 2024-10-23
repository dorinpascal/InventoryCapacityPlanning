using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;

namespace LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

public interface IGoodsReceiptStorage
{
    Task AddAsync(GoodsReceipt goodsReceipt);
    Task<List<GoodsReceipt>> GetAllAsync();
}
