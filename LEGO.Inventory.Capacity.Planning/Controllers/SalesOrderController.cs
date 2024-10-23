using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

[ApiController]
[Route("sales-order")]
public class SalesOrderController(ILogger<SalesOrderController> _logger, ISalesOrderService _salesOrderService, IPreparationService _preparationService) : ControllerBase
{
    [HttpPost()]
    public IActionResult Create([FromBody] SalesOrder salesOrder)
    {
        try
        {
            _salesOrderService.CreateSalesOrder(salesOrder);
            _preparationService.PrepareSalesOrder(salesOrder);

            return new CreatedResult();
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
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
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
