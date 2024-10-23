using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;
using LEGO.Inventory.Capacity.Planning.Services;

public class PreparationServiceTests
{
    [Fact]
    public void prepare_sales_order_enough_finished_goods_stock_reduce_stock_quantity()
    {
        // Arrange
        var mockStockTransportOrderService = new Mock<IStockTransportOrderService>();
        var storage = new Storage();
        var mockLogger = new Mock<ILogger<PreparationService>>();

        var service = new PreparationService(mockStockTransportOrderService.Object, storage, mockLogger.Object);

        var salesOrder = new SalesOrder("LEGO - Harry Potter", 10, "Central Warehouse Europe");

        // Act
        service.PrepareSalesOrder(salesOrder);

        // Assert
        var ldc = storage.LocalDistributionCenters.First(ldc => ldc.Name == salesOrder.LocalDistributionCenterName);
        Assert.Equal(40, ldc.FinishedGoodsStockQuantity);
    }

    [Fact]
    public void prepare_sales_order_not_enough_finished_goods_stock_use_safety_stock()
    {
        // Arrange
        var mockStockTransportOrderService = new Mock<IStockTransportOrderService>();
        var storage = new Storage();
        var mockLogger = new Mock<ILogger<PreparationService>>();

        var service = new PreparationService(mockStockTransportOrderService.Object, storage, mockLogger.Object);

        var salesOrder = new SalesOrder("LEGO - Harry Potter", 60, "Central Warehouse Europe");

        // Act
        service.PrepareSalesOrder(salesOrder);

        // Assert
        var ldc = storage.LocalDistributionCenters.First(ldc => ldc.Name == salesOrder.LocalDistributionCenterName);
        Assert.Equal(0, ldc.FinishedGoodsStockQuantity);
        Assert.Equal(10, ldc.SafetyStockQuantity);
    }
}
