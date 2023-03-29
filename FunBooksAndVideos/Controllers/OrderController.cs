using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FunBooksAndVideosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IEntityService<Order, OrderDto> orderService;
        private readonly ILogger<OrderController> logger;

        public OrderController(IEntityService<Order, OrderDto> orderService, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            try
            {
                var entity = await orderService.GetByIdAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                return Ok(entity);
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex, $"Error while getting order with ID {id}");
                return NotFound();
            }
            catch (NoContentException ex)
            {
                logger.LogError(ex, $"Error while getting order with ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while getting order with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            try
            {
                var entities = await orderService.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting all orders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Add(OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var entity = await orderService.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while adding order");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await orderService.UpdateAsync(id, dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (NoContentException ex)
            {
                logger.LogError(ex, $"Error while updating order with ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while updating order with ID {id}");
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await orderService.DeleteAsync(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (NoContentException ex)
            {
                logger.LogError(ex, $"Error while deleting order with ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while deleting order with ID {id}");
                return StatusCode(500, "Internal server error");
            }
            return NoContent();
        }
    }
}
