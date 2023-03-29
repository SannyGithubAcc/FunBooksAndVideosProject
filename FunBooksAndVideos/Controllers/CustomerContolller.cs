using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace FunBooksAndVideosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IEntityService<Customer, CustomerDto> customerService;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(IEntityService<Customer, CustomerDto> customerService, ILogger<CustomerController> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            try
            {
                var entity = await customerService.GetByIdAsync(id);

                return Ok(entity);
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CustomerDto>>> GetAll()
        {
            var entities = await customerService.GetAllAsync();
            return Ok(entities);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> Add(CustomerDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid model state");
                    return BadRequest(ModelState);
                }
                var entity = await customerService.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            catch (ValidationException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, CustomerDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid model state");
                    return BadRequest(ModelState);
                }
                await customerService.UpdateAsync(id, dto);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await customerService.DeleteAsync(id);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
