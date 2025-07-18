using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.API.Validators;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.Feature.PickupRequest.Commands;
using RecyclingSystem.Application.Feature.UserInfo.Queries;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region GetAllEmployees
        [Authorize(Roles = "Admin")]

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllEmployees(int id)
        {
            var result = await _mediator.Send(new GetAllEmployeesQuery() { RequestId = id });
            return result.ToActionResult();
        } 
        #endregion




        #region assign-employee
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-employee/{id:int}")]
        public async Task<IActionResult> AssignEmployeeToRequest(int id, [FromBody] AssignEmployeeToRequestDto requestDto)
        {
            var command = new AssignEmployeeToRequestCommand
            {
                RequestId = id,
                Email = requestDto.Email,
            };

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }


        #endregion

    }
}

