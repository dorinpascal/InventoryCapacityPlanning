using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces;

public interface IGoodsReceiptService
{
    /// <summary>
    /// Creates a new goods receipt entry.
    /// </summary>
    /// <param name="goodsReceipt">The goods receipt to be created, containing details about the completed stock transport order.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method adds a new entry for a goods receipt, which typically involves updating
    /// stock levels and marking associated stock transport orders as received.
    /// </remarks>
    Task Create(GoodsReceipt goodsReceipt);

    /// <summary>
    /// Retrieves all goods receipt entries.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a list of <see cref="GoodsReceipt"/> entries.</returns>
    /// <remarks>
    /// This method provides a comprehensive list of all goods receipts stored in the system,
    /// allowing clients to review completed stock transport orders and associated inventory updates.
    /// </remarks>
    Task<List<GoodsReceipt>> GetAll();
}
