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
    public class OrderProductController : ControllerBase
    {
        private readonly IEntityService<OrderProduct, OrderProductDto> orderProductService;
        private readonly ILogger<OrderProductController> logger;

        public OrderProductController(IEntityService<OrderProduct, OrderProductDto> orderProductService, ILogger<OrderProductController> logger)
        {
            this.orderProductService = orderProductService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderProductDto>> GetById(int id)
        {
            try
            {
                var entity = await orderProductService.GetByIdAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                return Ok(entity);
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex, "OrderProduct not found.");
                return NotFound(ex.Message);
            }
            catch (NoContentException ex)
            {
                logger.LogError(ex, "No order products found.");
                return NoContent();
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while getting the OrderProduct.");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderProductDto>>> GetAll()
        {
            try
            {
                var entities = await orderProductService.GetAllAsync();

                return Ok(entities);
            }
            catch (NoContentException ex)
            {
                logger.LogError(ex, "No order products found.");
                return NoContent();
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while getting the OrderProducts.");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderProductDto>> Add(OrderProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var entity = await orderProductService.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex, "OrderProduct not found.");
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while adding the OrderProduct.");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await orderProductService.UpdateAsync(id, dto);
            }
            catch (NotFoundException ex)
            {        
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while updating the OrderProduct.");
                return StatusCode(500);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await orderProductService.DeleteAsync(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while deleting the OrderProduct.");
                return StatusCode(500);
            }
            return NoContent();
        }
    }
}