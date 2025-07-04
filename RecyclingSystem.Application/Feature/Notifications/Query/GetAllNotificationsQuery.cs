using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using System.Data.Entity;
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
        private readonly IMapper _mapper;

        public GetAllNotificationsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllNotificationsQueryHandler> logger,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
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

                var userNotifications = _unitOfWork.notification
                    .GetAllWithFilter(u => u.UserId == userId)
                    .OrderByDescending(u => u.CreatedAt)
                    .ProjectTo<GetAllNotificationDto>(_mapper.ConfigurationProvider)
                    .ToList();

                if (userNotifications == null || !userNotifications.Any())
                {
                    _logger.LogWarning("Not found Notifications.");
                    return Result<List<GetAllNotificationDto>>.Failure(ErrorCode.NotFound, "No notifications available.");
                }

                return Result<List<GetAllNotificationDto>>.Success(userNotifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving notifications for user.");
                return Result<List<GetAllNotificationDto>>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
