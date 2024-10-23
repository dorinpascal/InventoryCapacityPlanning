using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface IGoodsReceiptService
{
    Task Create(GoodsReceipt goodsReceipt);
    Task<List<GoodsReceipt>> GetAll();
}
