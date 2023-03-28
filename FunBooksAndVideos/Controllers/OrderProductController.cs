using Application.Dtos;
using Application.Interfaces;
using FunBooksAndVideosAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly IOrderProductService orderProductService;

        private readonly ILogger<OrderController> logger;

        public OrderProductController(IOrderProductService orderProductService, ILogger<OrderController> logger)
        {
            this.orderProductService = orderProductService;
            this.logger = logger;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderProductDto>> GetOrderProductByIdAsync(int id)
        {
            try
            {
                var orderProduct = await orderProductService.GetOrderProductByIdAsync(id);

                if (orderProduct == null) {

                    return NotFound();
                }
                    
                return Ok(orderProduct);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 Internal Server Error response
                // Note: in a production system, it is recommended to log the exception details and not just the message
                // to aid in debugging issues
                logger.LogError($"Error getting order product by ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderProductDto>> AddOrderProductAsync(OrderProductDto orderProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var addedOrderProduct = await orderProductService.AddOrderProductAsync(orderProductDto);
                return Created(new Uri($"/api/orderproduct/{addedOrderProduct.Id}", UriKind.Relative), addedOrderProduct);
            }
            catch (BadRequestException ex)
            {
                logger.LogError($"Bad request while adding order product: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error adding order product: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderProductAsync(int id, [FromBody] OrderProductDto orderProductUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var orderProduct = await orderProductService.GetOrderProductByIdAsync(id);
                if (orderProduct == null)
                {
                    return NotFound();
                }
                await orderProductService.UpdateOrderProductAsync(orderProductUpdateDto);

                return NoContent();
            }
            catch (BadRequestException ex)
            {
                logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating an order product");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var orderProduct = await orderProductService.GetOrderProductByIdAsync(id);
                if (orderProduct == null)
                {
                    return NotFound();
                }

                await orderProductService.DeleteOrderProductAsync(id);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting an order product");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderProductDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderProductDto>>> GetAllOrderProductsAsync()
        {
            try
            {
                var orderProducts = await orderProductService.GetAllOrderProductsAsync();
                return Ok(orderProducts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting all order products");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

}
