using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Services;

public class StockTransportOrderServiceTests
{
    private readonly Mock<IStockTransportOrderStorage> _mockStoStorage;
    private readonly Mock<IRegionalDistributionCenterStorage> _mockRdcStorage;
    private readonly StockTransportOrderService _service;

    public StockTransportOrderServiceTests()
    {
        _mockStoStorage = new Mock<IStockTransportOrderStorage>();
        _mockRdcStorage = new Mock<IRegionalDistributionCenterStorage>();
        _service = new StockTransportOrderService(_mockStoStorage.Object);
    }

    [Fact]
    public async Task GetByLDC_ShouldReturnMatchingOrders()
    {
        // Arrange
        var ldcName = "Central Warehouse Europe";
        var sto1 = new StockTransportOrder("Lego - Harry Potter", 10, "LEGO European Distribution Center", ldcName);
        sto1.UpdateStatus(StockTransportOrderStatus.Open);
        var sto2 = new StockTransportOrder("Lego - Star Wars", 15, "LEGO European Distribution Center", ldcName);
        sto2.UpdateStatus(StockTransportOrderStatus.Picked);
        var sto3 = new StockTransportOrder("Lego - Ninjago", 5, "LEGO European Distribution Center", "Other LDC");
        sto3.UpdateStatus(StockTransportOrderStatus.Open);

        _mockStoStorage.Setup(s => s.GetAllAsync()).ReturnsAsync([sto1, sto2, sto3]);

        // Act
        var result = await _service.GetByLDC(ldcName, "Open");

        // Assert
        Assert.Single(result);
        Assert.Contains(sto1, result);
        Assert.DoesNotContain(sto2, result);
        Assert.DoesNotContain(sto3, result);
    }

    [Fact]
    public async Task Create_ShouldAddOrderToStorage()
    {
        // Arrange
        var sto = new StockTransportOrder("Lego - Star Wars", 10, "LEGO European Distribution Center", "Western Warehouse Europe");

        // Act
        await _service.Create(sto);

        // Assert
        _mockStoStorage.Verify(s => s.AddAsync(sto), Times.Once);
    }
}
