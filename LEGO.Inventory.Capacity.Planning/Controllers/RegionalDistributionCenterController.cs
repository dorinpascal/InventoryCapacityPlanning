using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers;

public class RegionalDistributionCenterController(ILogger<RegionalDistributionCenterController> _logger, IRegionalDistributionCenterService _regionalDistributionCenterService, IGoodsReceiptService _goodsReceiptService) : Controller
{
    [HttpPost("handleStockTransportOrder")]
    public async Task<IActionResult> HandleStockTransportOrder([FromBody] Guid stockTransportOrderId)
    {
        try
        {
            var quantityLeft = await _regionalDistributionCenterService.TryPickSTOAsync(stockTransportOrderId);
            if(quantityLeft == 0)
            {
                throw new ArgumentException("Insufficient stock");
            }
            _logger.LogInformation($"Order --" + stockTransportOrderId.ToString() + "-- is being picked");
            _logger.LogInformation($"Quantity left: {quantityLeft}");
            await _goodsReceiptService.Create(new GoodsReceipt(stockTransportOrderId));

            _logger.LogInformation($"Stock transport order --{stockTransportOrderId}" + $"-- Has been finished");

            return new CreatedResult();
        }
        catch (Exception ex)
        {
            return ControllerResponseMessageHelper.HandleException(ex, _logger);
        }
    }
}
