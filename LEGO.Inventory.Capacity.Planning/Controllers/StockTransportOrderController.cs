using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

[ApiController]
[Route("stock-transport-orders")]
public class StockTransportOrderController(ILogger<StockTransportOrderController> _logger, IStockTransportOrderService _stockTransportOrderService) : ControllerBase
{
    [HttpGet("getAllByLDCName")]
    public IActionResult GetAll([FromQuery] string nameLDC)
    {
        try
        {
            return Ok(_stockTransportOrderService.GetStockTransportOrdersByLDC(nameLDC));
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
