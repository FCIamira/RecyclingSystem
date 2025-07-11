using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.API.Validators;
using RecyclingSystem.Application.Feature.UserInfo.Orchestration;
using RecyclingSystem.Application.Feature.UserInfo.Queries;

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

        #region GetAllGift

        [Authorize]
        [HttpGet("Get All Gift")]
        public async Task<IActionResult> GetAllGift()
        {
            var result = await _mediator.Send(new GetAllGiftForUserQueries());
            return result.ToActionResult();
        }
        #endregion
    }
}
