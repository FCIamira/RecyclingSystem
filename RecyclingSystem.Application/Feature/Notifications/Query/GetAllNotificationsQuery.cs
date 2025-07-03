using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.NotificationsDTOs;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Notifications.Query
{
    public class GetAllNotificationsQuery : IRequest<Result<List<GetAllNotificationDto>>>
    {
    }

    public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, Result<List<GetAllNotificationDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        public GetAllNotificationsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllNotificationsQueryHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<GetAllNotificationDto>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all notifications for the current user.");
            try
            {
                var User = _contextAccessor.HttpContext.User;
                if (!User.Identity.IsAuthenticated)
                {
                    _logger.LogError("User is not authenticated.");
                    return Result<List<GetAllNotificationDto>>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");
                }
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var userNotifications = await _unitOfWork.notification.GetNotifications(u => u.UserId == userId);
                if (userNotifications == null || !userNotifications.Any())
                {
                    _logger.LogWarning("Not found Notifications.");
                    return Result<List<GetAllNotificationDto>>.Failure(ErrorCode.NotFound, "No notifications available.");
                }

                var notificationsDto = userNotifications.Select(n => new GetAllNotificationDto
                {
                    UserId = n.UserId,
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt
                }).ToList();
                return Result<List<GetAllNotificationDto>>.Success(notificationsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving notifications for user.");
                return Result<List<GetAllNotificationDto>>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
