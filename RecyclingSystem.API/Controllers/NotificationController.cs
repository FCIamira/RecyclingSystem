using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.API.Validators;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.Feature.Notifications.Query;
using RecyclingSystem.Domain.Enums;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly IMediator _mediator;
        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "Invalid model state").ToActionResult();
            }

            var notifications = await _mediator.Send(new GetAllNotificationsQuery());
            return Ok(notifications);
        }
    }
}
