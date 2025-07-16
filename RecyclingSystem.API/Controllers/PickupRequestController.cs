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
using RecyclingSystem.Application.Feature.PickupRequest.Commands;
using RecyclingSystem.Application.Feature.PickupRequest.Queries;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Application.DTOs.CustomerInfoDTOs;


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

        #region GetAllPickupRequestsForAdmin
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPickupRequestsForAdmin()
        {
            var result = await _mediator.Send(new GetAllPickupRequestsForAdminQuery());
            return Ok(result);
        } 
        #endregion

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


        #region assign-employee
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-employee/{id:int}")]
        public async Task<IActionResult> AssignEmployeeToRequest(int id, [FromBody] AssignEmployeeToRequestDto requestDto)
        {
            //if (!Enum.TryParse<PickupStatus>(requestDto.Status, ignoreCase: true, out var parsedStatus))
            //{
            //    return BadRequest("Invalid status value.");
            //}
            var command = new AssignEmployeeToRequestCommand
            {
                RequestId = id,
                Email = requestDto.Email,
               // Status = parsedStatus,
            };

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }


        #endregion
        #region GetAllStatus
        [HttpGet("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            var result = await _mediator.Send(new GetAllPickupStatusesQuery());
            return result.ToActionResult();
        }
        #endregion


        #region GetPickupRequestById
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPickupRequestById(int id)
        {
            var result = await _mediator.Send(new GetPickupRequestByIdQuery { Id = id });
            return Ok(result);
        }

        #endregion


        [Authorize(Roles = "Employee")]
        [HttpGet("employee")]
        public async Task<IActionResult> GetAllPickupRequestsForEmployee()
        {
            var result = await _mediator.Send(new GetAllPickupRequestsForEmployeeQuery());
            return Ok(result);
        }

        #region CollectPickupRequest
        [Authorize(Roles = "Employee")]
        [HttpPut("employee-collect/{id:int}")]
        public async Task<IActionResult> CollectPickupRequest(int id, [FromBody] List<UpdatePickupItemsActualQuantity> materialsActualQuantities)
        {
            var result = await _mediator.Send(new EmployeeCollectPickupRequestOrchestrator { PickupRequestId = id, updatePickupItemsActualQuantities = materialsActualQuantities });
            return Ok(result);
        }
        #endregion

        [HttpPut("Cancel/Customer")]
        public async Task<IActionResult> CancelRequestForCustomer(CancelRequestForCustomerDto cancelRequestDto )
        {
            var result =await _mediator.Send(new CancelRequestForCustomerOrchestrator
            {
                RequestId = cancelRequestDto.RequestId,
            });
            return result.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("TotalRequests&Rewards/{id:int}")]
        public async Task<IActionResult> GetTotalRequestsAndRewards(int id)
        {
            var result = await _mediator.Send(new GetTotalRequestsForCustomerQuery { CustomerId = id });
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("TotalCollected&Scheduled/{id:int}")]
        public async Task<IActionResult> GetTotalRequestsAssignedAndCollected(int id)
        {
            var result = await _mediator.Send(new GetTotalRequestsForEmployeeQuery { EmployeeId = id});
            return Ok(result);
        }
    }
}
