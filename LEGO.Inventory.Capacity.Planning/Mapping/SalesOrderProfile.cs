using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Dtos.SalesOrder;

namespace LEGO.Inventory.Capacity.Planning.Mapping;

public class SalesOrderProfile : Profile
{
    public SalesOrderProfile()
    {
        CreateMap<SalesOrder, SalesOrderDto>()
            .ForCtorParam(nameof(SalesOrderDto.Stock), opt => opt.MapFrom(src => src.FinishedGoodsName))
            .ForCtorParam(nameof(SalesOrderDto.LocalDistributionCenter), opt => opt.MapFrom(src => src.LocalDistributionCenterName));


        CreateMap<SalesOrderRequestDto, SalesOrder>()
           .ConstructUsing(src => new SalesOrder(src.FinishedGoodsName, src.Quantity, src.LocalDistributionCenterName));
    }
}
