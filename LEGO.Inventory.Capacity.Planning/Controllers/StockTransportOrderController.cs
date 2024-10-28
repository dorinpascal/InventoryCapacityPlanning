using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Dtos.StockTransportOrders;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

[ApiController]
[Route("stock-transport-orders")]
public class StockTransportOrderController(ILogger<StockTransportOrderController> _logger, IStockTransportOrderService _stockTransportOrderService, IMapper _mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string ldcName, [FromQuery] string? status = null)
    {
        try
        {
            var stockTransportOrders = await _stockTransportOrderService.GetByLDC(ldcName, status);
            var dto = _mapper.Map<List<StockTransportOrderDto>>(stockTransportOrders);
            return new OkObjectResult(dto);
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
