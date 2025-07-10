using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.Rewards.Query;
using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Application.Feature.Rewards.Command;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Application.Behaviors;
using Microsoft.AspNetCore.Http.HttpResults;
using RecyclingSystem.API.Validators;
using Microsoft.AspNetCore.Hosting;
namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RewardsController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
        {

            this.mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }

        #region get all rewards
        [HttpGet]
        public async Task<IActionResult> GetAllRewards()
        {
            var query = new GetAllRewardsQuery { };
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

     
        #endregion
        #region search areward by title
        [HttpGet("search")]
        
        public async Task<IActionResult> GetAllRewardsByTitle(string title)
        {
           
            var query = new GetAllRewardsByTitleQuery { Title = title };
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }
        #endregion

        #region filter reward by point
        [HttpGet("filterByPoint")]

        public async Task<IActionResult> GetAllRewardsByPoints(int max,int min)
        {
            var query = new GetAllRewardsByRangeOfPointsQuery { Min = min, Max = max };
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        #endregion


        #region add new reward

        [HttpPost]

        public async Task<IActionResult> AddNewReward([FromForm]CreateRewardDTO rewardFromRequest)
        {
            
            var result= await mediator.Send(new CreateRewardCommand { CreateRewardDTO=rewardFromRequest});
            return result.ToActionResult();
        }
        #endregion



        #region remove reward
        [HttpDelete("{id}")]

        public async Task<IActionResult> remove(int id)
        {
            var result = await mediator.Send(new DeleteRewardCommand { Id=id });
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion
    }
}
