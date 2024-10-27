using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Controllers;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Dtos.SalesOrder;
using LEGO.Inventory.Capacity.Planning.Models;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Controllers;

public class SalesOrderControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ISalesOrderService> _mockSalesOrderService;
    private readonly Mock<ISalesPreparationService> _mockPreparationService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<SalesOrderController>> _mockLogger;
    private readonly SalesOrderController _controller;

    public SalesOrderControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _mockSalesOrderService = _fixture.Freeze<Mock<ISalesOrderService>>();
        _mockPreparationService = _fixture.Freeze<Mock<ISalesPreparationService>>();
        _mockMapper = _fixture.Freeze<Mock<IMapper>>();
        _mockLogger = _fixture.Freeze<Mock<ILogger<SalesOrderController>>>();
        _controller = new SalesOrderController( _mockLogger.Object, _mockSalesOrderService.Object, _mockPreparationService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Create_WithValidSalesOrderRequest_ShouldReturnCreatedResult()
    {
        // Arrange
        var salesOrderRequestDto = _fixture.Create<SalesOrderRequestDto>();
        var salesOrder = _fixture.Create<SalesOrder>();

        _mockMapper.Setup(m => m.Map<SalesOrder>(salesOrderRequestDto)).Returns(salesOrder);
        _mockSalesOrderService.Setup(s => s.Create(salesOrder)).Returns(Task.CompletedTask);
        _mockPreparationService.Setup(s => s.Prepare(salesOrder)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(salesOrderRequestDto);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _mockSalesOrderService.Verify(s => s.Create(salesOrder), Times.Once);
        _mockPreparationService.Verify(s => s.Prepare(salesOrder), Times.Once);
        _mockLogger.VerifyLog(LogLevel.Information, $"Sales order created successfully for", Times.Once());
    }

    [Fact]
    public async Task Create_WhenServiceThrowsArgumentException_ShouldReturnBadRequest()
    {
        // Arrange
        var message = "Error";
        var salesOrderRequestDto = _fixture.Create<SalesOrderRequestDto>();
        var salesOrder = _fixture.Create<SalesOrder>();

        _mockMapper.Setup(m => m.Map<SalesOrder>(salesOrderRequestDto)).Returns(salesOrder);
        _mockSalesOrderService.Setup(s => s.Create(salesOrder)).ThrowsAsync(new ArgumentException(message));

        // Act
        var result = await _controller.Create(salesOrderRequestDto);

        // Assert
        var badRequestResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkObjectResult_WithMappedSalesOrders()
    {
        // Arrange
        var salesOrders = _fixture.CreateMany<SalesOrder>(3).ToList();
        var salesOrderDtos = _fixture.CreateMany<SalesOrderDto>(3).ToList();

        _mockSalesOrderService.Setup(s => s.GetAll()).ReturnsAsync(salesOrders);
        _mockMapper.Setup(m => m.Map<List<SalesOrderDto>>(salesOrders)).Returns(salesOrderDtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDtos = Assert.IsType<List<SalesOrderDto>>(okResult.Value);
        Assert.Equal(salesOrderDtos, returnedDtos);
        _mockSalesOrderService.Verify(s => s.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenServiceThrowsException_ShouldReturnServiceUnavailable()
    {
        // Arrange
        var exceptionMessage = "Service is unavailable";
        _mockSalesOrderService.Setup(s => s.GetAll()).ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var serviceUnavailableResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, serviceUnavailableResult.StatusCode);
        var responseMessage = Assert.IsType<ErrorResponseBody>(serviceUnavailableResult.Value);
        Assert.Equal("Service Unavailable", responseMessage.Message);
        Assert.Contains(exceptionMessage, responseMessage.Errors!);
    }
}
