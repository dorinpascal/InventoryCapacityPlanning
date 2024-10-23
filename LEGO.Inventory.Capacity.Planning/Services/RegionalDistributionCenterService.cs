using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services
{
    public class RegionalDistributionCenterService : IRegionalDistributionCenterService
    {
        private readonly IStorage _storage;
        private readonly ILogger<RegionalDistributionCenterService> _logger;
        
        public RegionalDistributionCenterService(IStorage storage, ILogger<RegionalDistributionCenterService> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        public int TryPickSTO(Guid stockTransportOrderId)
        {
            var stockTransportOrder = _storage.StockTransportOrders.FirstOrDefault(sto => sto.Id == stockTransportOrderId) ?? throw new Exception("Missing stock transport order");
            if (stockTransportOrder.Quantity > _storage.RegionalDistributionCenter.FinishedGoodsStockQuantity)
            {
                _logger.LogError($@"Couldn't pick stock transport order {stockTransportOrder.Id}. Insufficient stock for product {_storage.RegionalDistributionCenter.FinishedGoodsName}.
Ordered stock: {stockTransportOrder.Quantity}, current stock: {_storage.RegionalDistributionCenter.FinishedGoodsStockQuantity}");
                throw new Exception(
    $@"Couldn't pick stock transport order {stockTransportOrder.Id}. Insufficient stock for product {_storage.RegionalDistributionCenter.FinishedGoodsName}.
Ordered stock: {stockTransportOrder.Quantity}, current stock: {_storage.RegionalDistributionCenter.FinishedGoodsStockQuantity}");
                
            }

            stockTransportOrder.UpdateStatus(StockTransportOrderStatus.Picked);

            _storage.RegionalDistributionCenter.UpdateQuantity(_storage.RegionalDistributionCenter.FinishedGoodsStockQuantity - stockTransportOrder.Quantity);
            

            return stockTransportOrder.Quantity;
        }
    }
}
