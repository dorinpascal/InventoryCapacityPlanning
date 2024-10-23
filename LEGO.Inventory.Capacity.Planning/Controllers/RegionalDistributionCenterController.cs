using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace LEGO.Inventory.Capacity.Planning.Controllers
{
    public class RegionalDistributionCenterController : Controller
    {
        private readonly IRegionalDistributionCenterService _regionalDistributionCenterService;
        private readonly IGoodsReceiptService _goodsReceiptService;
        private readonly ILogger<RegionalDistributionCenterController> _logger;
        public RegionalDistributionCenterController(IRegionalDistributionCenterService regionalDistributionCenterService, IGoodsReceiptService goodsReceiptService, ILogger<RegionalDistributionCenterController> logger)
        {
            _regionalDistributionCenterService = regionalDistributionCenterService;
            _goodsReceiptService = goodsReceiptService;
            _logger = logger;
        }
        [HttpPost("handleStockTransportOrder")]
        public IActionResult HandleStockTransportOrder([FromBody] Guid stockTransportOrderId)
        {
            var quantityLeft = 0;
            try 
            { 
                quantityLeft = _regionalDistributionCenterService.TryPickSTO(stockTransportOrderId);
                _logger.LogInformation($"Order --"+stockTransportOrderId.ToString()+ "-- is being picked");
                _logger.LogInformation($"Quantity left: {quantityLeft}");
            } 
            catch (Exception)
            {
                if (quantityLeft == 0)
                {
                    return BadRequest($"Insufficient stock");
                }

                _logger.LogError("Error");
                return BadRequest($"wrong transport order id");
            }

            _goodsReceiptService.CreateGoodsReceipt(new GoodsReceipt(stockTransportOrderId));
            
            _logger.LogInformation($"Stock transport order --{stockTransportOrderId}"+ $"-- Has been finished");
            
            return Ok();
        }
    }
}
