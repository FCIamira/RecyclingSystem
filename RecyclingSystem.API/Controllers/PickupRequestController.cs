using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests;
using RecyclingSystem.API.Validators;
using Microsoft.AspNetCore.Authorization;
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






    }
}
