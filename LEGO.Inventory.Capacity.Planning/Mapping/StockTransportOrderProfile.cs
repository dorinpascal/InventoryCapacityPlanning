using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Dtos.StockTransportOrders;

namespace LEGO.Inventory.Capacity.Planning.Mapping;

public class StockTransportOrderProfile : Profile
{
    public StockTransportOrderProfile()
    {
        CreateMap<StockTransportOrder, StockTransportOrderDto>()
            .ForCtorParam(nameof(StockTransportOrderDto.Stock), opt => opt.MapFrom(src => src.FinishedGoodsName))
            .ForCtorParam(nameof(StockTransportOrderDto.LocalDistributionCenter), opt => opt.MapFrom(src => src.LocalDistributionCenterName))
            .ForCtorParam(nameof(StockTransportOrderDto.RegionalDistributionCenter), opt => opt.MapFrom(src => src.RegionalDistributionCenterName))
            .ForCtorParam(nameof(StockTransportOrderDto.Status), opt => opt.MapFrom(src => src.Status == StockTransportOrderStatus.Picked ? "picked" : "open"));
    }
}
