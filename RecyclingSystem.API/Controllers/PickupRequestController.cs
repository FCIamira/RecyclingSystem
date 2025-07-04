using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.Feature.PickupRequest.Orchestrator;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickupRequestController : ControllerBase
    {

        private readonly IMediator _mediator;

        public PickupRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> CreatePickupRequestWithPickupItems([FromBody] CreatePickupRequestDto request)
        {
            if (request == null ||  request.PickupItems == null || !request.PickupItems.Any())
            {
                return BadRequest("Invalid pickup request data.");
            }

            var response = await _mediator.Send(new CreatePickupRequestWithPickupItemsOrchestrator{ CreatePickupRequestDto = request});
            
            return Ok(response);
        }
    }
}
