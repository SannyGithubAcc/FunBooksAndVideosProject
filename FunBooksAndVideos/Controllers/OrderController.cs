using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FunBooksAndVideosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ILogger<OrderController> logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersAsync()
        {
            try
            {
                var orders = await orderService.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting orders.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await orderService.GetByIdAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting order by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting order by ID.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddOrderAsync(OrderDto createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }             
                await orderService.AddAsync(createOrderDto);

                return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = createOrderDto.Id }, createOrderDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating an order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderAsync(int id, OrderDto updateOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var order = await orderService.GetByIdAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                updateOrderDto.Id = id;
                await orderService.UpdateAsync(updateOrderDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while updating order.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                var order = await orderService.GetByIdAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                await orderService.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while deleting order.");
            }
        }
    }
}
