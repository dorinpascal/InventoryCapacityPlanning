using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Services;

public class PreparationServiceTests
{
    private readonly Mock<IStockTransportOrderService> _mockStockTransportOrderService;
    private readonly Mock<ILocalDistributionCenterStorage> _mockLocalDistributionCenterStorage;
    private readonly Mock<IRegionalDistributionCenterStorage> _mockRegionalDistributionCenterStorage;
    private readonly Mock<ILogger<PreparationService>> _mockLogger;
    private readonly PreparationService _service;

    public PreparationServiceTests()
    {
        _mockStockTransportOrderService = new Mock<IStockTransportOrderService>();
        _mockLocalDistributionCenterStorage = new Mock<ILocalDistributionCenterStorage>();
        _mockRegionalDistributionCenterStorage = new Mock<IRegionalDistributionCenterStorage>();
        _mockLogger = new Mock<ILogger<PreparationService>>();
        _service = new PreparationService(_mockStockTransportOrderService.Object, _mockLocalDistributionCenterStorage.Object, _mockRegionalDistributionCenterStorage.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Prepare_SufficientFinishedGoodsStock_ReducesStockWithoutSto()
    {
        // Arrange
        var salesOrder = new SalesOrder("LEGO - Harry Potter", 10, "Central Warehouse Europe");
        var localDistributionCenter = new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "LEGO - Harry Potter", 50, 20, 20);

        _mockLocalDistributionCenterStorage.Setup(s => s.GetByNameAsync(salesOrder.LocalDistributionCenterName))
            .ReturnsAsync(localDistributionCenter);

        // Act
        await _service.Prepare(salesOrder);

        // Assert
        Assert.Equal(40, localDistributionCenter.FinishedGoodsStockQuantity);
        Assert.Equal(20, localDistributionCenter.SafetyStockQuantity);
        _mockStockTransportOrderService.Verify(s => s.Create(It.IsAny<StockTransportOrder>()), Times.Never);
        _mockLocalDistributionCenterStorage.Verify(s => s.UpdateAsync(localDistributionCenter), Times.Once);
    }

    [Fact]
    public async Task Prepare_InsufficientFinishedGoodsStock_UsesSafetyStock()
    {
        // Arrange
        var salesOrder = new SalesOrder("LEGO - Harry Potter", 60, "Central Warehouse Europe");
        var localDistributionCenter = new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "LEGO - Harry Potter", 50, 20, 20);
        var regionalDistributionCenter = new RegionalDistributionCenter("LEGO European Distribution Center", "LEGO - Harry Potter", 100);

        _mockLocalDistributionCenterStorage
            .Setup(s => s.GetByNameAsync(salesOrder.LocalDistributionCenterName))
            .ReturnsAsync(localDistributionCenter);

        _mockRegionalDistributionCenterStorage
            .Setup(s => s.GetAsync())
            .ReturnsAsync(regionalDistributionCenter);

        // Act
        await _service.Prepare(salesOrder);

        // Assert
        Assert.Equal(0, localDistributionCenter.FinishedGoodsStockQuantity);
        Assert.Equal(10, localDistributionCenter.SafetyStockQuantity);
        _mockStockTransportOrderService.Verify(s => s.Create(It.IsAny<StockTransportOrder>()), Times.Once);
        _mockLocalDistributionCenterStorage.Verify(s => s.UpdateAsync(localDistributionCenter), Times.Once);
    }


    [Fact]
    public async Task Prepare_InsufficientFinishedGoodsAndSafetyStock_CreatesSto()
    {
        // Arrange
        var salesOrder = new SalesOrder("LEGO - Harry Potter", 90, "Central Warehouse Europe");
        var localDistributionCenter = new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "LEGO - Harry Potter", 50, 20, 20);
        var regionalDistributionCenter = new RegionalDistributionCenter("LEGO European Distribution Center", "LEGO - Harry Potter", 100);

        _mockLocalDistributionCenterStorage.Setup(s => s.GetByNameAsync(salesOrder.LocalDistributionCenterName))
            .ReturnsAsync(localDistributionCenter);

        _mockRegionalDistributionCenterStorage.Setup(s => s.GetAsync())
            .ReturnsAsync(regionalDistributionCenter);

        // Act
        await _service.Prepare(salesOrder);

        // Assert
        Assert.Equal(0, localDistributionCenter.FinishedGoodsStockQuantity);
        Assert.Equal(0, localDistributionCenter.SafetyStockQuantity);
        _mockStockTransportOrderService.Verify(s => s.Create(It.Is<StockTransportOrder>(sto => sto.Quantity == 20 && sto.FinishedGoodsName == salesOrder.FinishedGoodsName && sto.LocalDistributionCenterName == localDistributionCenter.Name && sto.RegionalDistributionCenterName == regionalDistributionCenter.Name)), Times.Once);

        _mockLocalDistributionCenterStorage.Verify(s => s.UpdateAsync(localDistributionCenter), Times.Once);
    }
}