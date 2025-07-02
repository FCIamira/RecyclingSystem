using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.Rewards.Query;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly IMediator mediator;
        public RewardsController(IMediator mediator)
        {

            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
         var rewards=   mediator.Send(new GetAllRewardsQuery());
            return Ok(rewards);
        }
    }
}
