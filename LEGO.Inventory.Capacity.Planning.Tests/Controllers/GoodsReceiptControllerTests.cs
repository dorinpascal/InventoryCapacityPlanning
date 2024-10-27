using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using LEGO.Inventory.Capacity.Planning.Controllers;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Dtos.GoodsReceipt;
using LEGO.Inventory.Capacity.Planning.Models;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LEGO.Inventory.Capacity.Planning.Tests.Controllers
{
    public class GoodsReceiptControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IGoodsReceiptService> _mockGoodsReceiptService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GoodsReceiptController>> _mockLogger;
        private readonly GoodsReceiptController _controller;

        public GoodsReceiptControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockGoodsReceiptService = _fixture.Freeze<Mock<IGoodsReceiptService>>();
            _mockMapper = _fixture.Freeze<Mock<IMapper>>();
            _mockLogger = _fixture.Freeze<Mock<ILogger<GoodsReceiptController>>>();
            _controller = new GoodsReceiptController(_mockLogger.Object, _mockGoodsReceiptService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkObjectResult_WithMappedGoodsReceipts()
        {
            // Arrange
            var goodsReceipts = _fixture.CreateMany<GoodsReceipt>(2).ToList();
            var goodsReceiptDtos = _fixture.CreateMany<GoodsReceiptDto>(2).ToList();

            _mockGoodsReceiptService.Setup(s => s.GetAll()).ReturnsAsync(goodsReceipts);
            _mockMapper.Setup(m => m.Map<List<GoodsReceiptDto>>(goodsReceipts)).Returns(goodsReceiptDtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDtos = Assert.IsType<List<GoodsReceiptDto>>(okResult.Value);
            Assert.Equal(goodsReceiptDtos, returnedDtos);
        }

        [Fact]
        public async Task GetAll_WhenServiceThrowsException_ShouldReturnServiceUnavailable()
        {
            // Arrange
            var exceptionMessage = _fixture.Create<string>();
            _mockGoodsReceiptService.Setup(s => s.GetAll()).ThrowsAsync(new Exception(exceptionMessage));

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
}
