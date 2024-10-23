namespace LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;

public class GoodsReceipt
{
    public Guid StockTransportOrderId { get; }

    public GoodsReceipt(Guid stockTransportOrderId) => StockTransportOrderId = stockTransportOrderId;
}