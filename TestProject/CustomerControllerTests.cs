using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using FunBooksAndVideosAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class CustomerControllerTests
    {
        private readonly IMapper mapper;
        private readonly Mock<ICustomerService> mockCustomerService;
        private readonly Mock<ILogger<CustomerController>> mockLogger;

        public CustomerControllerTests()
        {
            // Create a mock mapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<CustomerDto, Customer>();
            });
            mapper = mapperConfig.CreateMapper();

            // Create a mock customer service
            mockCustomerService = new Mock<ICustomerService>();

            // Create a mock logger
            mockLogger = new Mock<ILogger<CustomerController>>();
        }



        [Fact]
        public async Task GetCustomersAsync_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var expectedCustomers = new List<CustomerDto>
            {
                new CustomerDto { Id = 1, Name = "John Doe", Email = "john.doe@example.com", IsActive = true, Phone = "123-456-7890" },
                new CustomerDto { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com", IsActive = true, Phone = "555-555-1212" }
            };
            mockCustomerService.Setup(s => s.GetAllAsync()).ReturnsAsync(expectedCustomers);

            var controller = new CustomerController(mockCustomerService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetCustomersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCustomers = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(okResult.Value);
            Assert.Equal(expectedCustomers, actualCustomers);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var expectedCustomer = new CustomerDto { Id = 1, Name = "John Doe", Email = "john.doe@example.com", IsActive = true, Phone = "123-456-7890" };
            mockCustomerService.Setup(s => s.GetByIdAsync(expectedCustomer.Id)).ReturnsAsync(expectedCustomer);

            var controller = new CustomerController(mockCustomerService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetCustomerByIdAsync(expectedCustomer.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCustomer = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(expectedCustomer, actualCustomer);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsNotFoundResult()
        {
            // Arrange
            mockCustomerService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CustomerDto)null);

            var controller = new CustomerController(mockCustomerService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetCustomerByIdAsync(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddCustomerAsync_ReturnsCreatedAtAction()
        {
            // Arrange
            var createCustomerDto = new CustomerDto { Name = "John Doe", Email = "john.doe@example.com" };
            var addedCustomerDto = new CustomerDto { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            mockCustomerService.Setup(x => x.AddAsync(createCustomerDto)).ReturnsAsync(addedCustomerDto);
            var controller = new CustomerController(mockCustomerService.Object, mockLogger.Object);
            // Act
            var result = await controller.AddCustomerAsync(createCustomerDto);

            // Assert
            
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task DeleteCustomerAsyncReturnsNoContent()
        {
            // Arrange
            int id = 1;
            mockCustomerService.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);
            var controller = new CustomerController(mockCustomerService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteCustomerAsync(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockCustomerService.Verify(x => x.DeleteAsync(id), Times.Once);
        }




    }


}