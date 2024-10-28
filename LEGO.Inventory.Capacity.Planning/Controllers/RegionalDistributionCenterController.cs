using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

[ApiController]
[Route("stock-transport-orders")]
public class RegionalDistributionCenterController(ILogger<RegionalDistributionCenterController> _logger, IRegionalDistributionCenterService _regionalDistributionCenterService, IGoodsReceiptService _goodsReceiptService, IMapper _mapper) : Controller
{
    [HttpPost("pick")]
    public async Task<IActionResult> HandleStockTransportOrder([FromQuery] Guid stockTransportOrderId)
    {
        try
        {
            var quantityLeft = await _regionalDistributionCenterService.TryPickSTOAsync(stockTransportOrderId);
            if (quantityLeft == 0)
            {
                throw new ArgumentException("Insufficient stock");
            }
            _logger.LogInformation("Order {StockTransportOrderId} is being picked", stockTransportOrderId);
            _logger.LogInformation("Quantity left: {QuantityLeft}", quantityLeft);

            var goodsReceipt = await _goodsReceiptService.Create(new GoodsReceipt { StockTransportOrderId = stockTransportOrderId });
            _logger.LogInformation("Stock transport order {StockTransportOrderId} has been completed", stockTransportOrderId);

            var dto = _mapper.Map<GoodsReceipt>(goodsReceipt);
            return new OkObjectResult(dto);
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
