using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Dtos.GoodsReceipt;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace LEGO.Inventory.Capacity.Planning.Controllers;

public class GoodsReceiptController(ILogger<GoodsReceiptController> _logger, IGoodsReceiptService _goodsReceiptService, IMapper _mapper) : Controller
{
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var goodsReceipts = await _goodsReceiptService.GetAll();
            var dto = _mapper.Map<GoodsReceiptDto>(goodsReceipts);
            return new OkObjectResult(dto);
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
