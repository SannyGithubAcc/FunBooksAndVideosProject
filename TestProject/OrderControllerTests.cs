using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using AutoMapper;
using Domain.Models;
using FunBooksAndVideosAPI.Controllers;
using FunBooksAndVideosAPI.Mappers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestProject
{
    public class OrderControllerTests
    {
        private readonly Mock<IEntityService<Order, OrderDto>> _mockOrderService;
        private readonly IMapper _mapper;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IEntityService<Order, OrderDto>>();
            _orderController = new OrderController(_mockOrderService.Object, null);
        }


        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenOrderIsDeleted()
        {
            // Arrange
            _mockOrderService.Setup(s => s.DeleteAsync(1)).Verifiable();

            // Act
            var result = await _orderController.Delete(1);

            // Assert
            _mockOrderService.Verify();
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenOrderIsNotFound()
        {
            // Arrange
             _mockOrderService.Setup(s => s.DeleteAsync(1)).Throws(new NotFoundException("Order not found"));

            // Act
            var result = await _orderController.Delete(1);

            // Assert
            Assert.True(result.GetType() == typeof(NotFoundResult));
        }

        [Fact]
        public async Task Update_ReturnsNoContentResult_WhenOrderIsUpdated()
        {
            // Arrange
            var orderId = 1;
            var orderDto = new OrderDto { Id = orderId };
            _mockOrderService.Setup(s => s.UpdateAsync(orderId, orderDto)).Verifiable();

            // Act
            var result = await _orderController.Update(orderId, orderDto);

            // Assert
            _mockOrderService.Verify();
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFoundResult_WhenOrderIsNotFound()
        {
            // Arrange
            var orderId = 1;
            var orderDto = new OrderDto { Id = orderId };
            _mockOrderService.Setup(s => s.UpdateAsync(orderId, orderDto)).Throws(new NotFoundException("Order not found"));

            // Act
            var result = await _orderController.Update(orderId, orderDto);

            // Assert
            Assert.True(result.GetType() == typeof(NotFoundResult));
        }

        [Fact]
        public async Task GetById_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId };
            var orderDto = new OrderDto { Id = orderId };
            _mockOrderService.Setup(s => s.GetByIdAsync(orderId)).Returns(Task.FromResult(orderDto));

            // Act
            var result = await _orderController.GetById(orderId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsType<OrderDto>(okObjectResult.Value);
            var orderResult = okObjectResult.Value as OrderDto;
            Assert.Equal(orderDto.Id, orderResult.Id);
        }
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;
            _mockOrderService.Setup(s => s.GetByIdAsync(orderId)).Returns(Task.FromResult((OrderDto)null));

            // Act
            var result = await _orderController.GetById(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task GetAll_ReturnsOrders_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order { Id = 1 },
            new Order { Id = 2 }
        };
            var orderDto = new List<OrderDto>
        {
            new OrderDto { Id = 1 },
            new OrderDto { Id = 2 }
        };
            _mockOrderService.Setup(s => s.GetAllAsync()).Returns(Task.FromResult(orderDto));

            // Act
            var result = await _orderController.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsType<List<OrderDto>>(okObjectResult.Value);
            var orderResults = okObjectResult.Value as List<OrderDto>;
            Assert.Equal(orderDto.Count, orderResults.Count);
            for (var i = 0; i < orderDto.Count; i++)
            {
                Assert.Equal(orderDto[i].Id, orderResults[i].Id);
            }
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoOrdersExist()
        {
            // Arrange
            var orders = new List<OrderDto>();
            _mockOrderService.Setup(s => s.GetAllAsync()).Returns(Task.FromResult(orders));

            // Act
            var result = await _orderController.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsType<List<OrderDto>>(okObjectResult.Value);
            var orderResults = okObjectResult.Value as List<OrderDto>;
            Assert.Empty(orderResults);
        }




    }
}