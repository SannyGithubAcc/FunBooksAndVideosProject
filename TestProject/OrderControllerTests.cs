using Application.Dtos;
using Application.Interfaces;
using FunBooksAndVideosAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> mockOrderService;
        private readonly Mock<ILogger<OrderController>> mockLogger;
        private readonly OrderController controller;

        public OrderControllerTests()
        {
            mockOrderService = new Mock<IOrderService>();
            mockLogger = new Mock<ILogger<OrderController>>();
            controller = new OrderController(mockOrderService.Object, mockLogger.Object);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsOk()
        {
            // Arrange
            var orders = new List<OrderDto>
            {
                new OrderDto { Id = 1, CustomerId = 1, Date = DateTime.UtcNow },
                new OrderDto { Id = 2, CustomerId = 2, Date = DateTime.UtcNow }
            };
            mockOrderService.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await controller.GetOrdersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(okResult.Value);
            Assert.Equal(orders.Count, model.Count());
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithValidId_ReturnsOk()
        {
            // Arrange
            var orderId = 1;
            var order = new OrderDto { Id = orderId, CustomerId = 1, Date = DateTime.UtcNow };
            mockOrderService.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await controller.GetOrderByIdAsync(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<OrderDto>(okResult.Value);
            Assert.Equal(order.Id, model.Id);
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var orderId = 1;
            mockOrderService.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync((OrderDto)null);

            // Act
            var result = await controller.GetOrderByIdAsync(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddOrderAsync_WithValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var orderDto = new OrderDto { CustomerId = 1, Date =  DateTime.Today, };
            var orderId = 1;
            var order = new OrderDto { Id = orderId, CustomerId = orderDto.CustomerId, Date = orderDto.Date };
            mockOrderService.Setup(x => x.AddAsync(orderDto)).Callback<OrderDto>(x => x.Id = orderId);

            // Act
            var result = await controller.AddOrderAsync(orderDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<OrderDto>(createdAtActionResult.Value);
            Assert.Equal(order.Id, model.Id);
        }

        [Fact]
        public async Task AddOrderAsync_ReturnsCreatedAtAction()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                CustomerId = 1,
                Date = DateTime.Now,
                Price = 48.50m,
                OrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    OrderId = 1,
                    ProductId = 1,
                    Price = 24.50m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                   OrderId = 1,
                    ProductId = 2,
                    Price = 24.00m,
                    Quantity = 1
                }
            }
            };

            var addedOrderDto = new OrderDto
            {
                Id = 1,
                CustomerId = 1,
                Date = DateTime.Now,
                Price = 48.50m,
                OrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Price = 24.50m,
                    Quantity = 1,
                    Product = new ProductDto
                    {
                        Id = 1,
                        Name = "Comprehensive First Aid Training",
                        Barcode = "1234567890",
                        IsActive = true,
                        Description = "Video training course for first aid",
                        Price = 24.50m,
                        Category = "Training"
                    }
                },
                new OrderProductDto
                {
                    Id = 2,
                    OrderId = 1,
                    ProductId = 2,
                    Price = 24.00m,
                    Quantity = 1,
                    Product = new ProductDto
                    {
                        Id = 2,
                        Name = "The Girl on the Train",
                        Barcode = "0987654321",
                        IsActive = true,
                        Description = "Mystery novel",
                        Price = 24.00m,
                        Category = "Fiction"
                    }
                }
            }
            };


            mockOrderService.Setup(x => x.AddAsync(orderDto)).ReturnsAsync(addedOrderDto);

            var controller = new OrderController(mockOrderService.Object, mockLogger.Object);

            // Act
            var result = await controller.AddOrderAsync(orderDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<OrderDto>(createdAtActionResult.Value);
            Assert.Equal(addedOrderDto.CustomerId, returnValue.CustomerId);
        }
        [Fact]
        public async Task AddOrderAsync_ReturnsBadRequest_WhenInvalidModel()
        {
            // Arrange
            var orderDto = new OrderDto();

            var controller = new OrderController(mockOrderService.Object, mockLogger.Object);
            controller.ModelState.AddModelError("CustomerId", "Customer Id is required");

            // Act
            var result = await controller.AddOrderAsync(orderDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;

            var orderDto = new OrderDto
            {
                Id = orderId,
                CustomerId = 1,
                Date = DateTime.Today,
                OrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    OrderId = 1,
                    ProductId = 1,
                    Price = 24.50m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                   OrderId = 1,
                    ProductId = 2,
                    Price = 24.00m,
                    Quantity = 1
                }
            }

            };
            mockOrderService.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(orderDto);

            var controller = new OrderController(mockOrderService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetOrderByIdAsync(orderId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsType<OrderDto>(okResult.Value);
            var orderResult = okResult.Value as OrderDto;
            Assert.Equal(orderDto.Id, orderResult.Id);
            Assert.Equal(orderDto.CustomerId, orderResult.CustomerId);
            Assert.Equal(orderDto.OrderProducts.Count, orderResult.OrderProducts.Count);
            for (int i = 0; i < orderDto.OrderProducts.Count; i++)
            {
                Assert.Equal(orderDto.OrderProducts[i].Id, orderResult.OrderProducts[i].Id);
                Assert.Equal(orderDto.OrderProducts[i].Price, orderResult.OrderProducts[i].Price);
            }
        }

        [Fact]
        public async Task UpdateOrderAsync_ReturnsNoContent_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var updateOrderDto = new OrderDto
            {
                Id = orderId,
                CustomerId = 1,
                OrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto
                {
                    OrderId = 1,
                    ProductId = 1,
                    Price = 24.50m,
                    Quantity = 1
                },
                new OrderProductDto
                {
                    OrderId = 1,
                    ProductId = 2,
                    Price = 24.00m,
                    Quantity = 1
                }
            }
            };


            mockOrderService.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(updateOrderDto);

            var controller = new OrderController(mockOrderService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateOrderAsync(orderId, updateOrderDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrderAsync_ReturnsNoContent_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            mockOrderService.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(new OrderDto { Id = orderId });
            var controller = new OrderController(mockOrderService.Object, mockLogger.Object);
            // Act
            var result = await controller.DeleteOrderAsync(orderId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrderAsync_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;
            mockOrderService.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync((OrderDto)null);
            var controller = new OrderController(mockOrderService.Object, mockLogger.Object);
            // Act
            var result = await controller.DeleteOrderAsync(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


    }
}


