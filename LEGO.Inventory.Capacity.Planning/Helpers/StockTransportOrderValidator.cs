using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Helpers;

public static class StockTransportOrderValidator
{
    public static void ValidateStatus(string? status)
    {
        if (!string.IsNullOrEmpty(status) &&
            !Enum.TryParse(typeof(StockTransportOrderStatus), status, true, out _))
        {
            throw new ArgumentException($"Invalid status value: {status}. Only 'open' and 'picked' are allowed.");
        }
    }
}
