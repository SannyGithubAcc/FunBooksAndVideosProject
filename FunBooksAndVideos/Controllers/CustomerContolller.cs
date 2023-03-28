using Application.Dtos;
using Application.Interfaces;
using FunBooksAndVideosAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ICustomerService customerService,ILogger<CustomerController> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersAsync()
        {
            try
            {
                var customers = await customerService.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting customers.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting customers.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await customerService.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }
               return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting customer by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting customer by ID.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> AddCustomerAsync(CustomerDto createCustomerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var addedCustomer = await customerService.AddAsync(createCustomerDto);

                return Created(new Uri($"/api/customer/{addedCustomer.Id}", UriKind.Relative), addedCustomer);

            }
            catch (BadRequestException ex)
            {
                logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating a customer");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomerAsync(int id, CustomerDto updateCustomerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var customer = await customerService.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }
                await customerService.UpdateAsync(updateCustomerDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while updating customer.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                await customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while deleting customer.");
            }
        }
    }
}

