using Application.Dtos;
using Application.Interfaces;
using FunBooksAndVideosAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class OrderProductControllerTests
    {
        private readonly Mock<IOrderProductService> mockService;
        private readonly OrderProductController controller;

        public OrderProductControllerTests()
        {
            mockService = new Mock<IOrderProductService>();
            controller = new OrderProductController(mockService.Object, Mock.Of<ILogger<OrderController>>());
        }

        [Fact]
        public async Task GetOrderProductByIdAsyncReturnsOkResultWhenOrderProductExists()
        {
            // Arrange
            int id = 1;
            var orderProduct = new OrderProductDto { Id = id };
            mockService.Setup(s => s.GetOrderProductByIdAsync(id)).ReturnsAsync(orderProduct);

            // Act
            var result = await controller.GetOrderProductByIdAsync(id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderProductByIdAsyncReturnsNotFoundResultWhenOrderProductDoesNotExist()
        {
            // Arrange
            int id = 1;
            mockService.Setup(s => s.GetOrderProductByIdAsync(id)).ReturnsAsync(null as OrderProductDto);

            // Act
            var result = await controller.GetOrderProductByIdAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderProductByIdAsyncReturnsInternalServerErrorResultWhenServiceThrowsException()
        {
            // Arrange
            int id = 1;
            mockService.Setup(s => s.GetOrderProductByIdAsync(id)).ThrowsAsync(new Exception());

            // Act
            var result = await controller.GetOrderProductByIdAsync(id);

            // Assert
            Assert.IsType<StatusCodeResult>(result.Result);
            var statusCodeResult = (StatusCodeResult)result.Result;
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddOrderProductAsyncReturnsCreatedWhenOrderProductIsValid()
        {
            // Arrange
            var orderProductDto = new OrderProductDto
            {
                OrderId = 1,
                ProductId = 2,
                Price = -10.99m,
                Quantity = -1
            };

            var addedOrderProduct = new OrderProductDto
            {
                OrderId = 1,
                ProductId = 2,
                Price = -10.99m,
                Quantity = -1
            };

            mockService.Setup(x => x.AddOrderProductAsync(orderProductDto))
                .ReturnsAsync(addedOrderProduct);

            // Act
            var result = await controller.AddOrderProductAsync(orderProductDto);

            // Assert
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task AddOrderProductAsyncReturnsBadRequestWhenOrderProductIsNull()
        {
            // Arrange
            OrderProductDto orderProductDto = null;

            // Act
            var result = await controller.AddOrderProductAsync(orderProductDto);

             // Assert
    var internalServerErrorResult = Assert.IsType<StatusCodeResult>(result.Result);
    Assert.Equal(500, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task AddOrderProductAsyncReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var orderProductDto = new OrderProductDto
            {
                OrderId = 1,
                ProductId = 2,
                Price = -10.99m,
                Quantity = -1
            };

            controller.ModelState.AddModelError("Price", "The price must be greater than zero.");
            controller.ModelState.AddModelError("Quantity", "The quantity must be greater than zero.");

            // Act
            var result = await controller.AddOrderProductAsync(orderProductDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task AddOrderProductAsyncReturnsInternalServerErrorWhenServiceThrowsException()
        {
            // Arrange
            var orderProductDto = new OrderProductDto
            {
                OrderId = 1,
                ProductId = 2,
                Price = -10.99m,
                Quantity = -1
            };

            mockService.Setup(x => x.AddOrderProductAsync(orderProductDto))
                .ThrowsAsync(new Exception("An error occurred while adding the order product."));

            // Act
            var result = await controller.AddOrderProductAsync(orderProductDto);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

     
    }

}
