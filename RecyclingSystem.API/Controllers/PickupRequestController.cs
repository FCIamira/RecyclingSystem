using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.API.Validators;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.Feature.PickupRequest.Commands;
using RecyclingSystem.Application.Feature.PickupRequest.Orchestrator;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequestsForEmployee;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetPickupRequestById;
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

        #region CreatePickupRequest
        [HttpPost]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> CreatePickupRequest([FromBody] CreatePickupRequestDto request)
        {
            if (!ModelState.IsValid || request == null || !request.PickupItems.Any())
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new CreatePickupRequestWithPickupItemsOrchestrator { CreatePickupRequestDto = request });

            return Ok(result);
        }
        #endregion

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPickupRequestById(int id)
        {
            var result = await _mediator.Send(new GetPickupRequestByIdQuery { Id = id });
            return Ok(result);
        }



        [Authorize(Roles = "Employee")]
        [HttpGet("employee")]
        public async Task<IActionResult> GetAllPickupRequestsForEmployee()
        {
            var result = await _mediator.Send(new GetAllPickupRequestsForEmployeeQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpPut("employee-collect/{id:int}")]
        public async Task<IActionResult> CollectPickupRequest(int id, [FromBody] List<UpdatePickupItemsActualQuantity> actualQuantities)
        {
            var result = await _mediator.Send(new EmployeeCollectPickupRequestOrchestrator { PickupRequestId = id, updatePickupItemsActualQuantities = actualQuantities });
            return Ok(result);
        }
    }
}
