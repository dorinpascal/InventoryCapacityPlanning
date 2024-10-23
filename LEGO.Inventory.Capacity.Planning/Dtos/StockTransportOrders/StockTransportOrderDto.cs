using LEGO.Inventory.Capacity.Planning.Domain;

namespace LEGO.Inventory.Capacity.Planning.Dtos.StockTransportOrders;

public record StockTransportOrderDto(Guid Id, string Stock, int Quantity, string RegionalDistributionCenter, string LocalDistributionCetner, StockTransportOrderStatus Status );
