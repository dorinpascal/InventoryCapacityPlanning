using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Dtos.StockTransportOrders;

public record StockTransportOrderDto(Guid Id, string Stock, int Quantity, string RegionalDistributionCenter, string LocalDistributionCenter, string Status);
