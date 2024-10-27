using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Controllers;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Models;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Controllers;

public class RegionalDistributionCenterControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRegionalDistributionCenterService> _mockRegionalDistributionCenterService;
    private readonly Mock<IGoodsReceiptService> _mockGoodsReceiptService;
    private readonly Mock<ILogger<RegionalDistributionCenterController>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RegionalDistributionCenterController _controller;

    public RegionalDistributionCenterControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockRegionalDistributionCenterService = _fixture.Freeze<Mock<IRegionalDistributionCenterService>>();
        _mockGoodsReceiptService = _fixture.Freeze<Mock<IGoodsReceiptService>>();
        _mockLogger = _fixture.Freeze<Mock<ILogger<RegionalDistributionCenterController>>>();
        _mockMapper = _fixture.Freeze<Mock<IMapper>>();
        _controller = new RegionalDistributionCenterController(
            _mockLogger.Object,
            _mockRegionalDistributionCenterService.Object,
            _mockGoodsReceiptService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task HandleStockTransportOrder_WithValidId_ShouldCreateGoodsReceipt()
    {
        // Arrange
        var stockTransportOrderId = _fixture.Create<Guid>();
        var quantityLeft = _fixture.Create<int>();
        var goodsReceipt = _fixture.Create<GoodsReceipt>();
        _mockRegionalDistributionCenterService
            .Setup(s => s.TryPickSTOAsync(stockTransportOrderId))
            .ReturnsAsync(quantityLeft);

        _mockGoodsReceiptService
            .Setup(s => s.Create(It.IsAny<GoodsReceipt>()))
            .ReturnsAsync(goodsReceipt);

        // Act
        var result = await _controller.HandleStockTransportOrder(stockTransportOrderId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _mockLogger.VerifyLog(LogLevel.Information, $"Order --{stockTransportOrderId}-- is being picked", Times.Once());
        _mockLogger.VerifyLog(LogLevel.Information, $"Quantity left: {quantityLeft}", Times.Once());
        _mockGoodsReceiptService.Verify(s => s.Create(It.Is<GoodsReceipt>(gr => gr.StockTransportOrderId == stockTransportOrderId)), Times.Once());
        _mockLogger.VerifyLog(LogLevel.Information, $"Stock transport order --{stockTransportOrderId}-- Has been finished", Times.Once());
    }

    [Fact]
    public async Task HandleStockTransportOrder_WithInsufficientStock_ShouldBadRequest()
    {
        // Arrange
        var stockTransportOrderId = _fixture.Create<Guid>();

        _mockRegionalDistributionCenterService
            .Setup(s => s.TryPickSTOAsync(stockTransportOrderId))
            .ReturnsAsync(0);

        // Act
        var result = await _controller.HandleStockTransportOrder(stockTransportOrderId);

        // Assert
        var serviceUnavailableResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, serviceUnavailableResult.StatusCode);
        var responseMessage = Assert.IsType<ErrorResponseBody>(serviceUnavailableResult.Value);
        Assert.Contains("Insufficient stock", responseMessage.Message);
    }

    [Fact]
    public async Task HandleStockTransportOrder_WhenServiceThrowsException_ShouldReturnServiceUnavailable()
    {
        // Arrange
        var stockTransportOrderId = _fixture.Create<Guid>();
        var exceptionMessage = "Unexpected error";

        _mockRegionalDistributionCenterService
            .Setup(s => s.TryPickSTOAsync(stockTransportOrderId))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.HandleStockTransportOrder(stockTransportOrderId);

        // Assert
        var serviceUnavailableResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, serviceUnavailableResult.StatusCode);
        var responseMessage = Assert.IsType<ErrorResponseBody>(serviceUnavailableResult.Value);
        Assert.Equal("Service Unavailable", responseMessage.Message);
        Assert.Contains(exceptionMessage, responseMessage.Errors!);
    }
}
