using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.DTOs.AccountDTOs;
using RecyclingSystem.Application.Feature.AdminFeature.Command;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator mediator;
        public AdminController( IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpPost("registerEmployee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeDTO dto)
        {
            var command = new AddNewEmployeeCommand { Dto = dto };
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
