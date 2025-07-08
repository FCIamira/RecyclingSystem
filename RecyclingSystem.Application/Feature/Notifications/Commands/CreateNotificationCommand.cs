using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<Result<CreateNotificationResponse>>
    {
        public int UserId { get; set; } // The ID of the user to whom the notification is sent
        public string Title { get; set; } // The title of the notification
        public string Message { get; set; } // The content of the notification message
    }
    

    public class CreateNotificationResponse
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
    }

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<CreateNotificationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreateNotificationResponse>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            // Validate the request
            if (request.UserId <= 0 || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Message))
            {
                return await Task.FromResult(Result<CreateNotificationResponse>.Failure(ErrorCode.BadRequest, "Invalid notification data."));
            }
            // Create the notification in the database
            var notification = new Domain.Models.Notification
            {
                UserId = request.UserId,
                Title = request.Title,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.notification.Add(notification);
            await _unitOfWork.SaveChangesAsync();
            return Result<CreateNotificationResponse>.Success(new CreateNotificationResponse
            {
                NotificationId = notification.Id,
                Message = "Notification created successfully."
            });
        }
    }
}
