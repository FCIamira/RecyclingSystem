using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Notifications.Commands
{
    public class SendNotificationCommand : IRequest<Result<bool>>
    {
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = request.UserId,
                Title = request.Title,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _unitOfWork.notification.Add(notification);
            return Result<bool>.Success(true);
        }
    }

}
