using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests;
using RecyclingSystem.API.Validators;
using Microsoft.AspNetCore.Authorization;
using RecyclingSystem.Application.Feature.UserInfo.Queries;
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

        #region GetTotalQuantity
        [Authorize]
        [HttpGet("TotalQuantitywith-userName")]

        public async Task<IActionResult> GetTotalQuantity()
        {
            var result = await _mediator.Send(new GetUserTotalQuantityQuery());
            return result.ToActionResult();
        }
        #endregion

        #region GetAllGift

        [Authorize]
        [HttpGet("Get All Gift")]
        public async Task<IActionResult> GetAllGift()
        {
            var result =await _mediator.Send(new GetAllGiftForUserQueries());
            return result.ToActionResult();
        }
        #endregion


    }
}
