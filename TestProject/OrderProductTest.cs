using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using FunBooksAndVideosAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace TestProject
{
    public class OrderProductControllerTests
    {
        private readonly Mock<IEntityService<OrderProduct, OrderProductDto>> _mockOrderProductService;
        private readonly OrderProductController _controller;

        public OrderProductControllerTests()
        {
            _mockOrderProductService = new Mock<IEntityService<OrderProduct, OrderProductDto>>();
            _controller = new OrderProductController(_mockOrderProductService.Object, null);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithOrderProduct()
        {
            // Arrange
            var orderProductId = 1;
            var orderProduct = new OrderProduct { Id = orderProductId, Order_ID = 1, Product_ID = 1, Membership_ID = 0, Price = 100, Quantity = 1 };
            var orderProductDto = new OrderProductDto { Id = orderProductId, OrderId = 1, ProductId = 1, MembershipId = null, Price = 100, Quantity = 1 };

            _mockOrderProductService.Setup(x => x.GetByIdAsync(orderProductId)).Returns(Task.FromResult(orderProductDto));

            // Act
            var result = await _controller.GetById(orderProductId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsType<OrderProductDto>(okResult.Value);

            var resultDto = okResult.Value as OrderProductDto;
            Assert.Equal(orderProductId, resultDto.Id);
            Assert.Equal(orderProduct.Order_ID, resultDto.OrderId);
            Assert.Equal(orderProduct.Product_ID, resultDto.ProductId);
            Assert.Equal(orderProduct.Price, resultDto.Price);
            Assert.Equal(orderProduct.Quantity, resultDto.Quantity);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenOrderProductNotFound()
        {
            // Arrange
            var orderProductId = 1;
            _mockOrderProductService.Setup(x => x.GetByIdAsync(orderProductId)).Returns(Task.FromResult((OrderProductDto)null));

            // Act
            var result = await _controller.GetById(orderProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithOrderProducts()
        {
            // Arrange
            

            var orderProductDtos = new List<OrderProductDto>
    {
        new OrderProductDto { Id = 1 },
        new OrderProductDto { Id = 2 }
    };
            _mockOrderProductService.Setup(s => s.GetAllAsync()).Returns(Task.FromResult(orderProductDtos));

           

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            var orderProductResult = Assert.IsAssignableFrom<IEnumerable<OrderProductDto>>(okResult.Value);

            Assert.Equal(orderProductDtos.Count, orderProductResult.Count());
            for (int i = 0; i < orderProductDtos.Count; i++)
            {
                Assert.Equal(orderProductDtos[i].Id, orderProductResult.ElementAt(i).Id);
            }
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenOrderProductIsDeleted()
        {
            // Arrange
            _mockOrderProductService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.True(result.GetType() == typeof(NoContentResult));
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenOrderProductIsNotFound()
        {
            // Arrange
            _mockOrderProductService.Setup(s => s.DeleteAsync(1)).Throws(new NotFoundException("OrderProduct not found"));

            // Act
            var result = await _controller.Delete(1);
            // Assert
            Assert.True(result.GetType() == typeof(NotFoundObjectResult));
        }

        [Fact]
        public async Task Update_ReturnsNoContentResult_WhenOrderProductIsUpdated()
        {
            // Arrange
            var orderProductDto = new OrderProductDto { Id = 1, OrderId = 1, ProductId = 1, MembershipId = null, Price = 10.00m, Quantity = 2 };
            _mockOrderProductService.Setup(s => s.UpdateAsync(1, orderProductDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, orderProductDto);

            // Assert
            Assert.True(result.GetType() == typeof(NoContentResult));
        }

        [Fact]
        public async Task Update_ReturnsNotFoundResult_WhenOrderProductIsNotFound()
        {
            // Arrange
            var orderProductDto = new OrderProductDto { Id = 1, OrderId = 1, ProductId = 1, MembershipId = null, Price = 10.00m, Quantity = 2 };
            _mockOrderProductService.Setup(s => s.UpdateAsync(1, orderProductDto)).Throws(new NotFoundException("OrderProduct not found"));

            // Act
            var result = await _controller.Update(1, orderProductDto);

            // Assert

            Assert.True(result.GetType() == typeof(NotFoundObjectResult));
        }


    }
}
