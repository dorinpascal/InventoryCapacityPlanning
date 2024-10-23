using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Dtos;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

[ApiController]
[Route("sales-order")]
public class SalesOrderController(ILogger<SalesOrderController> _logger, ISalesOrderService _salesOrderService, IPreparationService _preparationService, IMapper _mapper) : ControllerBase
{
    [HttpPost()]
    public IActionResult Create([FromBody] SalesOrderRequestDto salesOrder)
    {
        try
        {
            var newSalesOrder = _mapper.Map<SalesOrder>(salesOrder);
            _salesOrderService.CreateSalesOrder(newSalesOrder);
            _preparationService.PrepareSalesOrder(newSalesOrder);
            _logger.LogInformation($"Sales order created successfully for {salesOrder.FinishedGoodsName}");

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
            var salesOrders = _salesOrderService.GetSalesOrders();
            var dto = _mapper.Map<List<SalesOrderDto>>(salesOrders);
            return new OkObjectResult(dto);
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
