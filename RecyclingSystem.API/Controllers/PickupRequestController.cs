using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using RecyclingSystem.API.Validators;
using Microsoft.AspNetCore.Authorization;
using RecyclingSystem.Application.Feature.UserInfo.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests;
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

        #region GetAll
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPickupRequestsQuery());
            return result.ToActionResult();
        }
        #endregion

        #region GetSuccesfullWithCancel
        [Authorize]
        [HttpGet("successful-and-cancelled")]

        public async Task<IActionResult> GetSuccesfullWithCancel()
        {
            var result = await _mediator.Send(new GetSuccesfullWithCancelQuery());
            return result.ToActionResult();
        }
        #endregion

        //[HttpGet("claims-debug")]
        //[Authorize]
        //public IActionResult ClaimsDebug()
        //{
        //    var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        //    return Ok(new
        //    {
        //        IsAuthenticated = User.Identity?.IsAuthenticated,
        //        Name = User.Identity?.Name,
        //        Claims = claims
        //    });
        //}

        #region GetTotalQuantity
        [Authorize]
        [HttpGet("TotalQuantitywith-userName")]

        public async Task<IActionResult> GetTotalQuantity()
        {
            var result = await _mediator.Send(new GetUserTotalQuantityQuery());
            return result.ToActionResult();
        }
        #endregion

        [HttpPost]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> CreatePickupRequest([FromBody] CreatePickupRequestDto request)
        {
            if (!ModelState.IsValid || request == null || !request.PickupItems.Any())
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new CreatePickupRequestWithPickupItemsOrchestrator { CreatePickupRequestDto = request});

            return Ok(result);
        }
    }
}
