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

    [HttpPost("pick")]
    public async Task<IActionResult> PickStockTransportOrder([FromQuery] Guid id)
    {
        try
        {
            var stOrder = await _stockTransportOrderService.PickStockTransportOrder(id);
            _logger.LogInformation("Stock Transport Order with ID {StockTransportOrderId} picked successfully.", id);
            var dto = _mapper.Map<StockTransportOrderDto>(stOrder);
            return new OkObjectResult(dto);
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);

        }
    }
}
