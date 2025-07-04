using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.Rewards.Query;
using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Application.Feature.Rewards.Command;
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
        public async Task<IActionResult> GetAllRewards()
        {
            var rewards = await mediator.Send(new GetAllRewardsQuery());
            return Ok(rewards);
        }


        [HttpGet("search")]
        
        public async Task<IActionResult> GetAllRewardsByTitle(string title)
        {
            var rewards = await mediator.Send(new GetAllRewardsByTitleQuery {Title=title});
            return Ok(rewards);
        }

        [HttpGet("filterByPoint")]

        public async Task<IActionResult> GetAllRewardsByPoints(int max,int min)
        {
            var rewards = await mediator.Send(new GetAllRewardsByRangeOfPointsQuery { Max = max,Min=min });
            return Ok(rewards);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> remove(int id)
        {
            await mediator.Send(new DeleteRewardCommand { Id=id });
            return Ok();
        }
    }
}
