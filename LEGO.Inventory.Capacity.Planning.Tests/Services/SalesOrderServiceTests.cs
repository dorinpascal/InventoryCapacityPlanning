using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using LEGO.Inventory.Capacity.Planning.Tests.Helper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LEGO.Inventory.Capacity.Planning.Tests.Services;

public class SalesOrderServiceTests
{
    private readonly Mock<ISalesOrderStorage> _mockSalesOrderStorage;
    private readonly Mock<ILocalDistributionCenterStorage> _mockLocalDistributionCenterStorage;
    private readonly Mock<ILogger<SalesOrderService>> _mockLogger;
    private readonly SalesOrderService _service;

    public SalesOrderServiceTests()
    {
        _mockSalesOrderStorage = new Mock<ISalesOrderStorage>();
        _mockLocalDistributionCenterStorage = new Mock<ILocalDistributionCenterStorage>();
        _mockLogger = new Mock<ILogger<SalesOrderService>>();

        _service = new SalesOrderService(
            _mockSalesOrderStorage.Object,
            _mockLocalDistributionCenterStorage.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidLDCName_ShouldAddOrderAndLogInformation()
    {
        // Arrange
        var salesOrder = new SalesOrder("Lego - Harry Potter", 10, "Western Warehouse Europe");
        var localDistributionCenter = new LocalDistributionCenter("Western Warehouse Europe",
            "LEGO European Distribution Center", "Lego - Harry Potter", 50, 20, 20);

        _mockLocalDistributionCenterStorage
            .Setup(s => s.GetByNameAsync(salesOrder.LocalDistributionCenterName))
            .ReturnsAsync(localDistributionCenter);

        _mockSalesOrderStorage
            .Setup(s => s.AddAsync(It.IsAny<SalesOrder>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.Create(salesOrder);

        // Assert
        _mockSalesOrderStorage.Verify(s => s.AddAsync(salesOrder), Times.Once);
        _mockLogger.VerifyLog(LogLevel.Information, $"new order created: {salesOrder.FinishedGoodsName} : {salesOrder.Quantity} -LDC: {salesOrder.LocalDistributionCenterName}", Times.Once());
    }

    [Fact]
    public async Task CreateAsync_WithInvalidLDCName_ShouldThrowArgumentException()
    {
        // Arrange
        var salesOrder = new SalesOrder("Lego - Harry Potter", 10, "Western Warehouse Europe");

        _mockLocalDistributionCenterStorage
            .Setup(s => s.GetByNameAsync(salesOrder.LocalDistributionCenterName))
            .ReturnsAsync((LocalDistributionCenter?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Create(salesOrder));
        Assert.Equal("Invalid local distribution center name", ex.Message);

        _mockSalesOrderStorage.Verify(s => s.AddAsync(It.IsAny<SalesOrder>()), Times.Never);
        _mockLogger.VerifyLog(LogLevel.Information, It.IsAny<string>(), Times.Never());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfSalesOrders()
    {
        // Arrange
        var salesOrders = new List<SalesOrder>
        {
            new("Lego - Harry Potter", 10, "Western Warehouse Europe"),
            new("Lego - Star Wars", 15, "Central Warehouse Europe")
        };

        _mockSalesOrderStorage.Setup(s => s.GetAllAsync()).ReturnsAsync(salesOrders);

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(salesOrders[0], result);
        Assert.Contains(salesOrders[1], result);
    }
}
