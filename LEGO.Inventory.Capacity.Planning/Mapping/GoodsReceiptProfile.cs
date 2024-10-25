using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Dtos.GoodsReceipt;

namespace LEGO.Inventory.Capacity.Planning.Mapping;

public class GoodsReceiptProfile : Profile
{
    public GoodsReceiptProfile()
    {
        CreateMap<GoodsReceipt, GoodsReceiptDto>()
                            .ForCtorParam(nameof(GoodsReceiptDto.StockTransportOrderId), opt => opt.MapFrom(src => src.StockTransportOrderId.ToString()));
    }
}
