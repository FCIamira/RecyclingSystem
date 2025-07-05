using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.UserInfo.Orchestration;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GiftController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("schedule-redeem")]
        public IActionResult ScheduleRedeemGift()
        {


            return Ok("Job Scheduled Successfully via Hangfire");
        }
    }
}
