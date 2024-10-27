/*using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Services;

public class SalesOrderServiceTests
{
    [Fact]
    public void create_sales_order_with_valid_ldc_name()
    {
        // Arrange
        var mockStorage = new Mock<IRegionalDistributionCenterStorage>();
        var mockLogger = new Mock<ILogger<SalesOrderService>>();
        var salesOrder = new SalesOrder("Lego - Harry Potter", 10, "Western Warehouse Europe");

        var localDistributionCenter = new LocalDistributionCenter("Western Warehouse Europe",
            "LEGO European Distribution Center", "Lego - Harry Potter", 50, 20, 20);

        mockStorage.Setup(s => s.LocalDistributionCenters)
            .Returns(new List<LocalDistributionCenter> { localDistributionCenter });

        mockStorage.Setup(s => s.SalesOrders).Returns(new List<SalesOrder>());

        var sut = new SalesOrderService(mockStorage.Object, mockLogger.Object);

        // Act
        sut.Create(salesOrder);

        // Assert
        Assert.Single(mockStorage.Object.SalesOrders);
        Assert.Contains(salesOrder, mockStorage.Object.SalesOrders);
    }

    [Fact]
    public void create_sales_order_with_invalid_ldc_name()
    {
        // Arrange
        var mockStorage = new Mock<IRegionalDistributionCenterStorage>();
        var mockLogger = new Mock<ILogger<SalesOrderService>>();
        var salesOrder = new SalesOrder("Lego - Harry Potter", 10, "Western Warehouse Europe");

        var localDistributionCenter = new LocalDistributionCenter("Center Warehouse Europe",
            "LEGO European Distribution Center", "Lego - Harry Potter", 50, 20, 20);

        mockStorage.Setup(s => s.LocalDistributionCenters)
            .Returns(new List<LocalDistributionCenter> { localDistributionCenter });

        mockStorage.Setup(s => s.SalesOrders).Returns(new List<SalesOrder>());

        var sut = new SalesOrderService(mockStorage.Object, mockLogger.Object);

        // Assert
        var ex = Assert.Throws<Exception>(() => sut.Create(salesOrder));
        Assert.Equal("invalid local distribution center name", ex.Message);
    }
}*/