using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Commands
{
    public class LogPointsHistoryCommand : IRequest<Result<bool>>
    {
        public int UserId { get; set; }
        public int PointsChanged { get; set; }
        public string Reason { get; set; }
        public PointsHistoryTypes Type { get; set; }
    }

    public class LogPointsHistoryCommandHandler : IRequestHandler<LogPointsHistoryCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogPointsHistoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(LogPointsHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = new PointsHistory
            {
                UserId = request.UserId,
                PointsChanged = request.PointsChanged,
                Reason = request.Reason,
                Type = request.Type,
                Datetime = DateTime.UtcNow
            };
            await _unitOfWork.pointsHistory.Add(history);
            return Result<bool>.Success(true);
        }
    }
}
