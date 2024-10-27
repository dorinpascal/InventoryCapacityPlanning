using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class GoodsReceiptStorage : IGoodsReceiptStorage
{
    private static readonly List<GoodsReceipt> _goodsReceipts = [];

    public Task<GoodsReceipt> AddAsync(GoodsReceipt goodsReceipt)
    {
        _goodsReceipts.Add(goodsReceipt);
        return Task.FromResult(goodsReceipt);
    }

    public Task<List<GoodsReceipt>> GetAllAsync()
    {
        return Task.FromResult(_goodsReceipts);
    }
}
