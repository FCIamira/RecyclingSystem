using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PointsHistories.Commands
{
    public class CreatePointsHistoryCommand : IRequest<Result<CreatePointsHistoryResponse>>
    {
        public int CustomerId { get; set; }
        public int Points { get; set; }
        public PointsHistoryTypes Type { get; set; } = PointsHistoryTypes.Earned; // Default to Earned
        public string Reason { get; set; }

        public CreatePointsHistoryCommand()
        {
            switch(Type)
            {
                case PointsHistoryTypes.Earned:
                    Reason = "Points assigned for pickup request completion";
                    break;
                case PointsHistoryTypes.Redeemed:
                    Reason = "Points redeemed for rewards";
                    break;
                case PointsHistoryTypes.Bonus:
                    Reason = "Bonus points awarded for special promotion";
                    break;
                default:
                    Reason = "Points assigned";
                    break;
            }
        }
    }
    
    public class CreatePointsHistoryResponse
    {
        public int CustomerId { get; set; }
        public int PointsAssigned { get; set; }
    }


    public class CreatePointsHistoryCommandHandler : IRequestHandler<CreatePointsHistoryCommand, Result<CreatePointsHistoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreatePointsHistoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreatePointsHistoryResponse>> Handle(CreatePointsHistoryCommand request, CancellationToken cancellationToken)
        {
            // Validate the request
            if (request.CustomerId <= 0)
            {
                return await Task.FromResult(Result<CreatePointsHistoryResponse>.Failure(ErrorCode.BadRequest, "Invalid Customer ID."));
            }

            if (request.Points <= 0)
            {
                return await Task.FromResult(Result<CreatePointsHistoryResponse>.Failure(ErrorCode.BadRequest, "Points must be greater than zero."));
            }

            var newPointsHistory = new Domain.Models.PointsHistory
            {
                UserId = request.CustomerId,
                PointsChanged = request.Points,
                Reason = request.Reason,
                Type = request.Type,
            };

            await _unitOfWork.pointsHistory.Add(newPointsHistory);
            await _unitOfWork.SaveChangesAsync();

            return await Task.FromResult(Result<CreatePointsHistoryResponse>.Success(new CreatePointsHistoryResponse
            {
                CustomerId = request.CustomerId,
                PointsAssigned = request.Points
            }));
        }
    }
}
