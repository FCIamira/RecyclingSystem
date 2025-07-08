using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.PointsHistories.Query;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PointsHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserHistory()
        {
            var history = await _mediator.Send(new GetAllPointsHistoryQuery());
            return Ok(history);
        }

        [HttpGet("TotalPoints")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllPointsTypes()
        {
            var points = await _mediator.Send(new GetTotalPointsQuery());
            return Ok(points);
        }
    }
}
