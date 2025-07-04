using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Application.Feature.Material.Commands;
using RecyclingSystem.Application.Feature.Material.Queries;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class MaterialController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMaterials()
        {
            var result = await _mediator.Send(new GetAllMaterialsQuery());

            if (result.IsSuccess)
            {
                return Ok(result.Data.Materials);
            }

            return NotFound(result.Message);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            var result = await _mediator.Send(new GetMaterialByIdQuery { Id = id });

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid material data.");
            }
            var result = await _mediator.Send(new CreateMaterialCommand { CreateMaterialDto = request });

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetMaterialById), new { id = result.Data.Id }, result);
            }
            return BadRequest(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var result = await _mediator.Send(new DeleteMaterialCommand { Id = id });
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateMaterial([FromBody] MaterialDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid material data.");
            }
            // Assuming you have an UpdateMaterialCommand similar to CreateMaterialCommand
            var result = await _mediator.Send(new UpdateMaterialCommand { MaterialDto = request });
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
