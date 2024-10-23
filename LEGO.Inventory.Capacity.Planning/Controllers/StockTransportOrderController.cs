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
    [HttpGet("getAllByLDCName")]
    public async Task<IActionResult> GetAll([FromQuery] string nameLDC)
    {
        try
        {
            var stockTransportOrders = await _stockTransportOrderService.GetByLDC(nameLDC);
            var dto = _mapper.Map<List<StockTransportOrderDto>>(stockTransportOrders);
            return new OkObjectResult(dto);
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
