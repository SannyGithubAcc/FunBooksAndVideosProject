using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using FunBooksAndVideosAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestProject
{
    public class CustomerControllerTests
    {
        private readonly Mock<IEntityService<Customer, CustomerDto>> _mockCustomerService;

        private readonly CustomerController customerController;

        public CustomerControllerTests()
        {
            _mockCustomerService = new Mock<IEntityService<Customer, CustomerDto>>();
            customerController = new CustomerController(_mockCustomerService.Object, null);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenCustomerExists()
        {
            // Arrange
            int id = 1;
            var customer = new CustomerDto { Id = id, Name = "John Doe", Email = "john@example.com", IsActive = true };
            _mockCustomerService.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(customer);

            // Act
            var result = await customerController.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<CustomerDto>(okResult.Value);
            Assert.Equal(id, model.Id);
            Assert.Equal(customer.Name, model.Name);
            Assert.Equal(customer.Email, model.Email);
            Assert.Equal(customer.IsActive, model.IsActive);
        }

        [Fact]
        public async Task GetById_ReturnsNotFoundResult_WhenCustomerDoesNotExist()
        {
            // Arrange
            int id = 1;
            _mockCustomerService.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((CustomerDto)null);

            // Act
            var result = await customerController.GetById(id);

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<CustomerDto>
            {
                new CustomerDto { Id = 1, Name = "John", Email = "john@example.com", IsActive = true },
                new CustomerDto { Id = 2, Name = "Jane", Email = "jane@example.com", IsActive = true },
                new CustomerDto { Id = 3, Name = "Bob", Email = "bob@example.com", IsActive = false }
            };
            _mockCustomerService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);


            // Act
            var result = await customerController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<CustomerDto>>(okResult.Value);
            Assert.Equal(customers.Count, model.Count);
        }
        [Fact]
        public async Task Add_ReturnsCreatedAtActionResult_WithNewCustomer()
        {
            // Arrange
            var newCustomer = new CustomerDto
            {
                Id = 1,
                Name = "John",
                Email = "johndoe@example.com",
                IsActive = true,
                Phone = "123-456-7890"
            };
            var createdCustomer = new Customer
            {
                Id = 1,
                Name = newCustomer.Name,
                Email = newCustomer.Email,
                IsActive = newCustomer.IsActive,
                Phone = newCustomer.Phone
            };
            _mockCustomerService.Setup(x => x.AddAsync(newCustomer)).Returns(Task.FromResult(newCustomer));

            // Act
            var result = await customerController.Add(newCustomer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var customerDto = Assert.IsType<CustomerDto>(createdAtActionResult.Value);
            Assert.Equal(createdCustomer.Id, customerDto.Id);
            Assert.Equal(createdCustomer.Name, customerDto.Name);
            Assert.Equal(createdCustomer.Email, customerDto.Email);
            Assert.Equal(createdCustomer.IsActive, customerDto.IsActive);
            Assert.Equal(createdCustomer.Phone, customerDto.Phone);
        }

        public async Task Update_ReturnsNoContent_WhenCustomerIsUpdated()
        {
            // Arrange
            int customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Test Customer", Email = "test@example.com" };
            var customerDto = new CustomerDto { Id = customerId, Name = "Updated Customer", Email = "updated@example.com" };
            _mockCustomerService.Setup(s => s.GetByIdAsync(customerId)).Returns(Task.FromResult(customerDto));

            // Act
            var result = await customerController.Update(customerId, customerDto);

            // Assert
            _mockCustomerService.Verify(s => s.UpdateAsync(customerId, customerDto), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task Delete_ExistingCustomer_ReturnsNoContent()
        {
            // Arrange
            var mockService = new Mock<IEntityService<Customer, CustomerDto>>();
            mockService.Setup(s => s.DeleteAsync(1)).Verifiable();



            // Act
            var result = await customerController.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistentCustomer_ReturnsNotFound()
        {
            // Arrange
            _mockCustomerService.Setup(s => s.DeleteAsync(1)).Throws(new NotFoundException("Customer not found"));
            // Act
            var result = await customerController.Delete(1);

            // Assert
            Assert.True(result.GetType() == typeof(NotFoundResult));
        }
    }
}




