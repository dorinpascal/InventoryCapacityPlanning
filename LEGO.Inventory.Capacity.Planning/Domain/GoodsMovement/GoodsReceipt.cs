namespace LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;

public class GoodsReceipt
{
    public GoodsReceipt(Guid stockTransportOrderId)
    {
        StockTransportOrderId = stockTransportOrderId;
    }

    public Guid StockTransportOrderId { get; }
}