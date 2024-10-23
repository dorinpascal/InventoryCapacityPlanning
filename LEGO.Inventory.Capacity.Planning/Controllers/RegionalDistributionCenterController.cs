using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Helpers;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LEGO.Inventory.Capacity.Planning.Controllers
{
    public class RegionalDistributionCenterController(ILogger<RegionalDistributionCenterController> _logger, IRegionalDistributionCenterService _regionalDistributionCenterService, IGoodsReceiptService _goodsReceiptService) : Controller
    {
        [HttpPost("handleStockTransportOrder")]
        public IActionResult HandleStockTransportOrder([FromBody] Guid stockTransportOrderId)
        {
            var quantityLeft = 0;
            try
            {
                quantityLeft = _regionalDistributionCenterService.TryPickSTO(stockTransportOrderId);
                _logger.LogInformation($"Order --" + stockTransportOrderId.ToString() + "-- is being picked");
                _logger.LogInformation($"Quantity left: {quantityLeft}");
                _goodsReceiptService.CreateGoodsReceipt(new GoodsReceipt(stockTransportOrderId));

                _logger.LogInformation($"Stock transport order --{stockTransportOrderId}" + $"-- Has been finished");

                return Ok();
            }
            catch (Exception ex)
            {
                if (quantityLeft == 0)
                {
                    return BadRequest($"Insufficient stock");
                }

                _logger.LogError("Error");
                return ControllerResponseMessageHelper.HandleException(ex, _logger);
            }
        }
    }
}
