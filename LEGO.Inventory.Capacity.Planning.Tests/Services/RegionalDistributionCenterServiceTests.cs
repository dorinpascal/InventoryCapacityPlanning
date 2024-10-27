using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using LEGO.Inventory.Capacity.Planning.Tests.Helper;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Services;

public class RegionalDistributionCenterServiceTests
{
    private readonly Mock<IRegionalDistributionCenterStorage> _mockRdcStorage;
    private readonly Mock<IStockTransportOrderStorage> _mockStoStorage;
    private readonly Mock<ILogger<RegionalDistributionCenterService>> _mockLogger;
    private readonly RegionalDistributionCenterService _service;

    public RegionalDistributionCenterServiceTests()
    {
        _mockRdcStorage = new Mock<IRegionalDistributionCenterStorage>();
        _mockStoStorage = new Mock<IStockTransportOrderStorage>();
        _mockLogger = new Mock<ILogger<RegionalDistributionCenterService>>();
        _service = new RegionalDistributionCenterService(_mockRdcStorage.Object, _mockStoStorage.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task TryPickSTOAsync_WithEnoughStock_ShouldPickOrder()
    {
        // Arrange
        var stoId = Guid.NewGuid();
        var stockTransportOrder = new StockTransportOrder(stoId, "Lego - Harry Potter", 10, "LEGO European Distribution Center", "Central Warehouse Europe", StockTransportOrderStatus.Open);
        var rdc = new RegionalDistributionCenter("LEGO European Distribution Center", "Lego - Harry Potter", 20);
        _mockStoStorage.Setup(s => s.GetAllAsync()).ReturnsAsync([stockTransportOrder]);
        _mockRdcStorage.Setup(s => s.GetAsync()).ReturnsAsync(rdc);
        _mockStoStorage.Setup(s => s.AddAsync(It.IsAny<StockTransportOrder>())).ReturnsAsync(stockTransportOrder);
        _mockRdcStorage.Setup(s => s.UpdateAsync(rdc)).Returns(Task.CompletedTask);

        // Act
        var quantityPicked = await _service.TryPickSTOAsync(stoId);

        // Assert
        Assert.Equal(10, quantityPicked); // Ensure the correct quantity was picked
        Assert.Equal(10, rdc.FinishedGoodsStockQuantity); // Ensure RDC stock was updated
        _mockStoStorage.Verify(s => s.AddAsync(It.Is<StockTransportOrder>(sto => sto.Status == StockTransportOrderStatus.Picked)), Times.Once);
        _mockLogger.VerifyLog(LogLevel.Information, $"STO with Id: {stoId} has been picked. Remaining stock: {rdc.FinishedGoodsStockQuantity}", Times.Once());
    }

    [Fact]
    public async Task TryPickSTOAsync_WithInsufficientStock_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var stoId = Guid.NewGuid();
        var stockTransportOrder = new StockTransportOrder(stoId, "Lego - Harry Potter", 25, "LEGO European Distribution Center", "Central Warehouse Europe", StockTransportOrderStatus.Open);
        var rdc = new RegionalDistributionCenter("LEGO European Distribution Center", "Lego - Harry Potter", 20);

        _mockStoStorage.Setup(s => s.GetAllAsync()).ReturnsAsync([stockTransportOrder]);
        _mockRdcStorage.Setup(s => s.GetAsync()).ReturnsAsync(rdc);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TryPickSTOAsync(stoId));
        Assert.Contains("Insufficient stock for product", exception.Message);

        _mockStoStorage.Verify(s => s.AddAsync(It.IsAny<StockTransportOrder>()), Times.Never); // Ensure no changes to STO status
        _mockLogger.VerifyLog(LogLevel.Information, It.IsAny<string>(), Times.Never());
    }

    [Fact]
    public async Task TryPickSTOAsync_WithNonExistentSTO_ShouldThrowArgumentException()
    {
        // Arrange
        var stoId = Guid.NewGuid();
        _mockStoStorage.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.TryPickSTOAsync(stoId));
        Assert.Equal("Missing stock transport order", exception.Message);

        _mockLogger.VerifyLog(LogLevel.Information, It.IsAny<string>(), Times.Never());
    }
}
