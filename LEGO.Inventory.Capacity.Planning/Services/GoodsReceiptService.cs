using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services
{
    public class GoodsReceiptService : IGoodsReceiptService
    {
        private readonly IStorage _storage;
        private readonly ILogger<GoodsReceipt> _logger;


        public GoodsReceiptService(IStorage storage, ILogger<GoodsReceipt> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        public List<GoodsReceipt> GetGoodsReceiptList()
        {
            return _storage.GoodsReceipts;
        }

        void IGoodsReceiptService.CreateGoodsReceipt(GoodsReceipt goodsReceipt)
        {
            _storage.GoodsReceipts.Add(goodsReceipt);

            var stockTransportOrder =
                _storage.StockTransportOrders.FirstOrDefault(sto => sto.Id == goodsReceipt.StockTransportOrderId) ??
                throw new Exception("Missing stock transport order");

            if (stockTransportOrder.Status == StockTransportOrderStatus.Picked)
            {
                var localDistributionCenter =
                    _storage.LocalDistributionCenters.First(x =>
                        x.Name == stockTransportOrder.LocalDistributionCenterName);
                localDistributionCenter.SafetyStockQuantity = localDistributionCenter.SafetyStockThreshold;
                _logger.LogInformation(localDistributionCenter.Name + "'s safety stock has been updated to " +
                                       localDistributionCenter.SafetyStockQuantity);
            }
        }
    }
}