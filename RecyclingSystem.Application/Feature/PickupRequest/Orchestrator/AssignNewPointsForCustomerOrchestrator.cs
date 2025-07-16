using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.Feature.Notifications.Commands;
using RecyclingSystem.Application.Feature.PointsHistories.Commands;
using RecyclingSystem.Application.Feature.UserInfo.Command;
using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Orchestrator
{
    public class AssignNewPointsForCustomerOrchestrator : IRequest<Result<AssignNewPointsForCustomerResponse>>
    {
        public int CustomerId { get; set; }
        public int Points { get; set; }
        public PointsHistoryTypes PointsHistoryTypes { get; set; } = PointsHistoryTypes.Earned; // Default to Earned
    }

    public class AssignNewPointsForCustomerResponse
    {
        public int CustomerId { get; set; }
        public int PointsAssigned { get; set; }
    }

    public class AssignNewPointsForCustomerOrchestratorHandler : IRequestHandler<AssignNewPointsForCustomerOrchestrator, Result<AssignNewPointsForCustomerResponse>>
    {
        private readonly IMediator _mediator;
        public AssignNewPointsForCustomerOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result<AssignNewPointsForCustomerResponse>> Handle(AssignNewPointsForCustomerOrchestrator request, CancellationToken cancellationToken)
        {
            // Validate the request
            if (request.CustomerId <= 0 || request.Points <= 0)
            {
                return await Task.FromResult(Result<AssignNewPointsForCustomerResponse>.Failure(ErrorCode.BadRequest, $"Invalid Customer ID or points. | CustomerId: {request.CustomerId} | Points: {request.Points}"));
            }

            #region Create points history entry
            var createPointsHistoryCommand = new CreatePointsHistoryCommand
            {
                CustomerId = request.CustomerId,
                Points = request.Points,
                Type = request.PointsHistoryTypes
            };
            var pointsHistoryResult = await _mediator.Send(createPointsHistoryCommand);
            if (!pointsHistoryResult.IsSuccess)
            {
                return await Task.FromResult(Result<AssignNewPointsForCustomerResponse>.Failure(pointsHistoryResult.Errorcode, pointsHistoryResult.Message));
            }
            #endregion

            #region Update user's total points

            var updateUserTotalPointsCommand = new UpdateUserTotalPointsCommand
            {
                UserId = request.CustomerId,
                NewPoints = request.Points
            };
            var updateUserTotalPointsResult = await _mediator.Send(updateUserTotalPointsCommand);
            if (!updateUserTotalPointsResult.IsSuccess)
            {
                return await Task.FromResult(Result<AssignNewPointsForCustomerResponse>.Failure(updateUserTotalPointsResult.Errorcode, updateUserTotalPointsResult.Message));
            }


            #endregion

            #region Create a Notification
            var createNotificationCommand = new CreateNotificationCommand
            {
                UserId = request.CustomerId,
                Title = "Points Assigned",
                Message = $"You have been assigned {request.Points} points."
            };
            var notificationResult = await _mediator.Send(createNotificationCommand);
            if (!notificationResult.IsSuccess)
            {
                return await Task.FromResult(Result<AssignNewPointsForCustomerResponse>.Failure(notificationResult.Errorcode, notificationResult.Message));
            }
            #endregion


            return Result<AssignNewPointsForCustomerResponse>.Success(new AssignNewPointsForCustomerResponse
            {
                CustomerId = request.CustomerId,
                PointsAssigned = request.Points
            }, "Points Assigned Successfully for Customer");
        }
    }
}
