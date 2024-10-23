using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

[ApiController]
[Route("sales-order")]
public class SalesOrderController : ControllerBase
{
    private readonly ISalesOrderService _salesOrderService;
    private readonly IPreparationService _preparationService;
    public SalesOrderController(ISalesOrderService salesOrderService, IPreparationService preparationService)
    {
        _salesOrderService = salesOrderService;
        _preparationService = preparationService;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] SalesOrder salesOrder)
    {
        try
        {
            _salesOrderService.CreateSalesOrder(salesOrder);
            _preparationService.PrepareSalesOrder(salesOrder);
            
            return Ok("created");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
    [HttpGet()]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(_salesOrderService.GetSalesOrders());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
