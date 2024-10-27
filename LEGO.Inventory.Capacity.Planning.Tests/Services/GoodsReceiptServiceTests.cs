using LEGO.Inventory.Capacity.Planning.Domain;
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

        var localDistributionCenter = new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 20, 5, 5);

        _mockGoodsReceiptStorage.Setup(s => s.AddAsync(goodsReceipt)).Returns(Task.CompletedTask);
        _mockTransportOrderStorage.Setup(s => s.GetByIdAsync(goodsReceipt.StockTransportOrderId)).ReturnsAsync(stockTransportOrder);
        _mockDistributionCenterStorage.Setup(s => s.GetByNameAsync(stockTransportOrder.LocalDistributionCenterName)).ReturnsAsync(localDistributionCenter);

        // Act
        await _service.Create(goodsReceipt);

        // Assert
        Assert.Equal(30, localDistributionCenter.FinishedGoodsStockQuantity);
        Assert.Equal(localDistributionCenter.SafetyStockThreshold, localDistributionCenter.SafetyStockQuantity); // Safety stock reset
        _mockGoodsReceiptStorage.Verify(s => s.AddAsync(goodsReceipt), Times.Once);
        _mockLogger.VerifyLog(LogLevel.Information, $"{localDistributionCenter.Name}'s safety stock has been updated to {localDistributionCenter.SafetyStockQuantity}", Times.Once());
    }

    [Fact]
    public async Task Create_WithMissingStockTransportOrder_ShouldThrowArgumentException()
    {
        // Arrange
        var goodsReceipt = new GoodsReceipt { StockTransportOrderId = Guid.NewGuid() };
        _mockGoodsReceiptStorage.Setup(s => s.AddAsync(goodsReceipt)).Returns(Task.CompletedTask);
        _mockTransportOrderStorage.Setup(s => s.GetByIdAsync(goodsReceipt.StockTransportOrderId)).ReturnsAsync((StockTransportOrder?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Create(goodsReceipt));
        Assert.Equal("Missing stock transport order", ex.Message);
        _mockGoodsReceiptStorage.Verify(s => s.AddAsync(goodsReceipt), Times.Once);
    }

    [Fact]
    public async Task Create_WithInvalidLDCName_ShouldThrowArgumentException()
    {
        // Arrange
        var stoId = Guid.NewGuid();
        var goodsReceipt = new GoodsReceipt { StockTransportOrderId = stoId };
        var stockTransportOrder = new StockTransportOrder("Lego - Harry Potter", 10, "LEGO European Distribution Center", "Invalid LDC");
        stockTransportOrder.UpdateStatus(StockTransportOrderStatus.Picked);
        _mockGoodsReceiptStorage.Setup(s => s.AddAsync(goodsReceipt)).Returns(Task.CompletedTask);
        _mockTransportOrderStorage.Setup(s => s.GetByIdAsync(goodsReceipt.StockTransportOrderId)).ReturnsAsync(stockTransportOrder);
        _mockDistributionCenterStorage.Setup(s => s.GetByNameAsync(stockTransportOrder.LocalDistributionCenterName)).ReturnsAsync((LocalDistributionCenter?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Create(goodsReceipt));
        Assert.Equal("Invalid local distribution center name", ex.Message);
        _mockGoodsReceiptStorage.Verify(s => s.AddAsync(goodsReceipt), Times.Once);
    }
}
