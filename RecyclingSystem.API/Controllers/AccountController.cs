using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.API.Validators;
using RecyclingSystem.Application.DTOs.AccountDTOs;
using RecyclingSystem.Application.Feature.Account.Orchestrator;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {

            this.mediator = mediator;
        }


        #region Register
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "Invalid model state").ToActionResult();
            }

            var result = await mediator.Send(new RegisterOrchestrator { RegisterRequest = model });
            return result.ToActionResult();
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "Invalid model state").ToActionResult();
            }

            var result = await mediator.Send(new LoginOrchestrator { loginRequest = model });
            return result.ToActionResult();
        }
        #endregion



    }

}
