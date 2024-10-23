using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class PreparationService : IPreparationService
{
    private readonly IStockTransportOrderService _stockTransportOrderService;
    private readonly IStorage _storage;
    private readonly ILogger<PreparationService> _logger;

    public PreparationService(IStockTransportOrderService stockTransportOrderService, IStorage storage,
        ILogger<PreparationService> logger)
    {
        _storage = storage;
        _stockTransportOrderService = stockTransportOrderService;
        _logger = logger;
    }

    public void PrepareSalesOrder(SalesOrder salesOrder)
    {
        var _localDistributionCenter =
            _storage.LocalDistributionCenters.FirstOrDefault(ldc => ldc.Name == salesOrder.LocalDistributionCenterName);
        var requiredQuantity = 0;

        if (_localDistributionCenter.FinishedGoodsStockQuantity >= salesOrder.Quantity)
        {
            _localDistributionCenter.FinishedGoodsStockQuantity = _localDistributionCenter.FinishedGoodsStockQuantity - salesOrder.Quantity;
            _logger.LogInformation(_localDistributionCenter.Name + "'s new stock quantity: " + _localDistributionCenter.FinishedGoodsStockQuantity);
            return;
        }
        else if (_localDistributionCenter.FinishedGoodsStockQuantity < salesOrder.Quantity)
        {
            var shortfall = salesOrder.Quantity - _localDistributionCenter.FinishedGoodsStockQuantity;
            _localDistributionCenter.FinishedGoodsStockQuantity = 0;

            if (_localDistributionCenter.SafetyStockQuantity >= shortfall)
            {
                _localDistributionCenter.SafetyStockQuantity -= shortfall;
                requiredQuantity = shortfall;
            }
            else
            {
                requiredQuantity = _localDistributionCenter.SafetyStockThreshold;
                _localDistributionCenter.SafetyStockQuantity = 0;
            }

            if (_localDistributionCenter.SafetyStockQuantity == 0)
            {
                CreateSto(salesOrder, _localDistributionCenter, requiredQuantity);
            }
            else if (_localDistributionCenter.SafetyStockQuantity < _localDistributionCenter.SafetyStockThreshold)
            {
                CreateSto(salesOrder, _localDistributionCenter, (_localDistributionCenter.SafetyStockThreshold - _localDistributionCenter.SafetyStockQuantity));
            }

            _logger.LogWarning(_localDistributionCenter.Name + "'s new safety stock quantity: " + _localDistributionCenter.SafetyStockQuantity);
            return;
        }

        else if (salesOrder.Quantity > _localDistributionCenter.FinishedGoodsStockQuantity
        && salesOrder.Quantity <= _localDistributionCenter.FinishedGoodsStockQuantity + _localDistributionCenter.SafetyStockQuantity)

        {
            int requiredFromSafetyStock = salesOrder.Quantity - _localDistributionCenter.FinishedGoodsStockQuantity;
            _localDistributionCenter.FinishedGoodsStockQuantity = 0;
            _localDistributionCenter.SafetyStockQuantity = _localDistributionCenter.SafetyStockQuantity - requiredFromSafetyStock;

            if (_localDistributionCenter.SafetyStockQuantity == 0
                || _localDistributionCenter.SafetyStockQuantity < _localDistributionCenter.SafetyStockThreshold)
            {
                CreateSto(salesOrder, _localDistributionCenter, requiredFromSafetyStock);
            }
            _logger.LogInformation(_localDistributionCenter.Name + "'s new stock quantity: " + _localDistributionCenter.FinishedGoodsStockQuantity);
            _logger.LogWarning(_localDistributionCenter.Name + "'s new safety stock quantity: " + _localDistributionCenter.SafetyStockQuantity);
        }
    }

    private void CreateSto(SalesOrder salesOrder, LocalDistributionCenter _localDistributionCenter,
        int requiredQuantity)
    {
        _stockTransportOrderService.CreateStockTransportOrder(new StockTransportOrder(
            salesOrder.FinishedGoodsName,
            requiredQuantity,
            _storage.RegionalDistributionCenter.Name,
            _localDistributionCenter.Name));

        _logger.LogInformation($"new STO created: " + salesOrder.FinishedGoodsName + "Quantity: " + requiredQuantity
                               + ", from : " + _storage.RegionalDistributionCenter.Name + " to " +
                               _localDistributionCenter.Name);
    }
}