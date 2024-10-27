using AutoFixture;
using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Controllers;
using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Dtos.StockTransportOrders;
using LEGO.Inventory.Capacity.Planning.Models;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Controllers;

public class StockTransportOrderControllerTests
{
    private readonly Mock<IStockTransportOrderService> _mockStockTransportOrderService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<StockTransportOrderController>> _mockLogger;
    private readonly StockTransportOrderController _controller;
    private readonly Fixture _fixture;

    public StockTransportOrderControllerTests()
    {
        _mockStockTransportOrderService = new Mock<IStockTransportOrderService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<StockTransportOrderController>>();
        _controller = new StockTransportOrderController(_mockLogger.Object, _mockStockTransportOrderService.Object, _mockMapper.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAll_WithValidLDCName_ShouldReturnOkResultWithMappedDto()
    {
        // Arrange
        var ldcName = "Central Warehouse Europe";
        var status = "Open";

        var stockTransportOrders = _fixture.Create<List<StockTransportOrder>>();
        var stockTransportOrderDtos = _fixture.Create<List<StockTransportOrderDto>>();

        _mockStockTransportOrderService.Setup(s => s.GetByLDC(ldcName, status)).ReturnsAsync(stockTransportOrders);
        _mockMapper.Setup(m => m.Map<List<StockTransportOrderDto>>(stockTransportOrders)).Returns(stockTransportOrderDtos);

        // Act
        var result = await _controller.GetAll(ldcName, status);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDtos = Assert.IsType<List<StockTransportOrderDto>>(okResult.Value);
        Assert.Equal(stockTransportOrderDtos, returnedDtos);
    }

    [Fact]
    public async Task GetAll_WhenServiceThrowsException_ShouldReturnServiceUnavailable()
    {
        // Arrange
        var ldcName = "Central Warehouse Europe";
        var status = "Open";
        var exceptionMessage = "Service is unavailable";

        _mockStockTransportOrderService.Setup(s => s.GetByLDC(ldcName, status))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.GetAll(ldcName, status);

        // Assert
        var serviceUnavailableResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, serviceUnavailableResult.StatusCode);
        var responseMessage = Assert.IsType<ErrorResponseBody>(serviceUnavailableResult.Value);
        Assert.Equal("Service Unavailable", responseMessage.Message);
        Assert.Contains(exceptionMessage, responseMessage.Errors!);
    }

    [Fact]
    public async Task PickStockTransportOrder_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();

        _mockStockTransportOrderService.Setup(s => s.PickStockTransportOrder(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.PickStockTransportOrder(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("STO picked successfully.", okResult.Value);
        _mockLogger.VerifyLog(LogLevel.Information, $"STO with ID {id} picked successfully.", Times.Once());
    }

    [Fact]
    public async Task PickStockTransportOrder_WhenServiceThrowsException_ShouldReturnServiceUnavailable()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var exceptionMessage = "STO not found";

        _mockStockTransportOrderService.Setup(s => s.PickStockTransportOrder(id))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.PickStockTransportOrder(id);

        // Assert
        var serviceUnavailableResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, serviceUnavailableResult.StatusCode);
        var responseMessage = Assert.IsType<ErrorResponseBody>(serviceUnavailableResult.Value);
        Assert.Equal("Service Unavailable", responseMessage.Message);
        Assert.Contains(exceptionMessage, responseMessage.Errors!);
    }
}
