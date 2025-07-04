﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.Feature.UserInfo.Command;
using RecyclingSystem.Application.Feature.UserInfo.Queries;
using RecyclingSystem.Domain.Enums;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromForm] CheckEmailCommand command)
        {
            var isExist = await _mediator.Send(command);
            return Ok(new { isExist });
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordCommand command)
        {
            var message = await _mediator.Send(command);
            return Ok(new { message });
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo(int id)
        {
            var user = await _mediator.Send(new GetUserQuery { UserId = id });
            return Ok(user);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserCommand command)
        {
            var message = await _mediator.Send(command);
            return Ok(new { message }); 
        }
    }
}
