using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces
{
    public interface IGoodsReceiptService
    {
        void CreateGoodsReceipt(GoodsReceipt goodsReceipt);
        List<GoodsReceipt> GetGoodsReceiptList();
    }
}
