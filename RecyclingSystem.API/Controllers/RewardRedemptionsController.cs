using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.DTOs.RewardRedemptionsDTOs;
using RecyclingSystem.Application.Feature.RewardRedemptions.command;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardRedemptionsController : ControllerBase
    {
        private readonly IMediator mediator;
        public RewardRedemptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        #region add RewardRedemption
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewRewardRedemption( AddRewardRedemptionsDTO redemptionsDTO)
        {
            var result=await mediator.Send(new AddRewardRedemptionsCommand
            { 
            redemptionsDTO=redemptionsDTO
            });

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion
    }
}
