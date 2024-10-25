using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class GoodsReceiptStorage : IGoodsReceiptStorage
{
    private static readonly List<GoodsReceipt> _goodsReceipts = [];

    public Task AddAsync(GoodsReceipt goodsReceipt)
    {
        _goodsReceipts.Add(goodsReceipt);
        return Task.CompletedTask;
    }

    public Task<List<GoodsReceipt>> GetAllAsync()
    {
        return Task.FromResult(_goodsReceipts);
    }
}
