﻿using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using LEGO.Inventory.Capacity.Planning.Tests.Helper;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Services;

public class GoodsReceiptServiceTests
{
    private readonly Mock<IGoodsReceiptStorage> _mockGoodsReceiptStorage;
    private readonly Mock<IStockTransportOrderStorage> _mockTransportOrderStorage;
    private readonly Mock<ILocalDistributionCenterStorage> _mockDistributionCenterStorage;
    private readonly Mock<ILogger<GoodsReceiptService>> _mockLogger;
    private readonly GoodsReceiptService _service;

    public GoodsReceiptServiceTests()
    {
        _mockGoodsReceiptStorage = new Mock<IGoodsReceiptStorage>();
        _mockTransportOrderStorage = new Mock<IStockTransportOrderStorage>();
        _mockDistributionCenterStorage = new Mock<ILocalDistributionCenterStorage>();
        _mockLogger = new Mock<ILogger<GoodsReceiptService>>();
        _service = new GoodsReceiptService(_mockGoodsReceiptStorage.Object, _mockTransportOrderStorage.Object, _mockDistributionCenterStorage.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfGoodsReceipts()
    {
        // Arrange
        var goodsReceipts = new List<GoodsReceipt>
        {
            new() { StockTransportOrderId = Guid.NewGuid() },
            new() { StockTransportOrderId = Guid.NewGuid() }
        };

        _mockGoodsReceiptStorage.Setup(s => s.GetAllAsync()).ReturnsAsync(goodsReceipts);

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(goodsReceipts[0], result);
        Assert.Contains(goodsReceipts[1], result);
    }

    [Fact]
    public async Task Create_WithValidStockTransportOrder_ShouldUpdateLDCStockAndSafetyStock()
    {
        // Arrange
        var stoId = Guid.NewGuid();
        var goodsReceipt = new GoodsReceipt { StockTransportOrderId = stoId };
        var stockTransportOrder = new StockTransportOrder("Lego - Harry Potter", 10, "LEGO European Distribution Center", "Central Warehouse Europe");
        stockTransportOrder.UpdateStatus(StockTransportOrderStatus.Picked);

        // Initial setup for Local Distribution Center with a Safety Stock deficit
        var initialFinishedGoodsStock = 20;
        var initialSafetyStock = 5;
        var safetyStockThreshold = 10;
        var localDistributionCenter = new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", initialFinishedGoodsStock, initialSafetyStock, safetyStockThreshold);

        _mockGoodsReceiptStorage.Setup(s => s.AddAsync(goodsReceipt)).ReturnsAsync(goodsReceipt);
        _mockTransportOrderStorage.Setup(s => s.GetByIdAsync(goodsReceipt.StockTransportOrderId)).ReturnsAsync(stockTransportOrder);
        _mockDistributionCenterStorage.Setup(s => s.GetByNameAsync(stockTransportOrder.LocalDistributionCenterName)).ReturnsAsync(localDistributionCenter);

        // Act
        await _service.Create(goodsReceipt);

        // Calculate expected results
        var expectedSafetyStock = safetyStockThreshold;
        var expectedFinishedGoodsStock = initialFinishedGoodsStock + (safetyStockThreshold - initialSafetyStock);

        // Assert
        Assert.Equal(expectedFinishedGoodsStock, localDistributionCenter.FinishedGoodsStockQuantity);
        Assert.Equal(expectedSafetyStock, localDistributionCenter.SafetyStockQuantity);
        _mockGoodsReceiptStorage.Verify(s => s.AddAsync(goodsReceipt), Times.Once);

        // Verify structured log message
        _mockLogger.VerifyLog(LogLevel.Information,
            $"Updated stock for {localDistributionCenter.Name}: Finished Goods Stock = {localDistributionCenter.FinishedGoodsStockQuantity}, Safety Stock = {localDistributionCenter.SafetyStockQuantity}",
            Times.Once());
    }


    [Fact]
    public async Task Create_WithMissingStockTransportOrder_ShouldThrowArgumentException()
    {
        // Arrange
        var goodsReceipt = new GoodsReceipt { StockTransportOrderId = Guid.NewGuid() };
        _mockGoodsReceiptStorage.Setup(s => s.AddAsync(goodsReceipt)).ReturnsAsync(new GoodsReceipt());
        _mockTransportOrderStorage.Setup(s => s.GetByIdAsync(goodsReceipt.StockTransportOrderId)).ReturnsAsync((StockTransportOrder?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Create(goodsReceipt));
        Assert.Equal($"Stock transport order with ID {goodsReceipt.StockTransportOrderId} not found", ex.Message);
    }

    [Fact]
    public async Task Create_WithInvalidLDCName_ShouldThrowArgumentException()
    {
        // Arrange
        var stoId = Guid.NewGuid();
        var goodsReceipt = new GoodsReceipt { StockTransportOrderId = stoId };
        var stockTransportOrder = new StockTransportOrder("Lego - Harry Potter", 10, "LEGO European Distribution Center", "Invalid LDC");
        stockTransportOrder.UpdateStatus(StockTransportOrderStatus.Picked);

        _mockGoodsReceiptStorage.Setup(s => s.AddAsync(goodsReceipt)).ReturnsAsync(goodsReceipt);
        _mockTransportOrderStorage.Setup(s => s.GetByIdAsync(goodsReceipt.StockTransportOrderId)).ReturnsAsync(stockTransportOrder);
        _mockDistributionCenterStorage.Setup(s => s.GetByNameAsync(stockTransportOrder.LocalDistributionCenterName)).ReturnsAsync((LocalDistributionCenter?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Create(goodsReceipt));
        Assert.Equal($"Local distribution center '{stockTransportOrder.LocalDistributionCenterName}' not found", ex.Message);
    }

}
