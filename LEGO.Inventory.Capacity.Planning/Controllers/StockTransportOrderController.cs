using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

[ApiController]
[Route("stock-transport-orders")]
public class StockTransportOrderController : ControllerBase
{
    private readonly IStockTransportOrderService _stockTransportOrderService;
    public StockTransportOrderController(IStockTransportOrderService stockTransportOrderService)
    {
        _stockTransportOrderService = stockTransportOrderService;
    }
    [HttpGet("getAllByLDCName")]
    public IActionResult GetAll([FromQuery] string nameLDC)
    {
        try
        {
            return Ok(_stockTransportOrderService.GetStockTransportOrdersByLDC(nameLDC));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}
