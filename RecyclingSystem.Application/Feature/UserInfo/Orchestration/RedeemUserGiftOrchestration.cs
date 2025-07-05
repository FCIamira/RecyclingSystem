using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.Feature.UserInfo.Commands;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Orchestration
{
    public class RedeemUserGiftOrchestration:IRequest<Result<bool>>
    {
        public int PointsEarned { get; set; }
        public int PointsPerGift { get; set; } = 5;
        public int PointsThreshold { get; set; } = 100;
        public int userId { get; set; }
    }

        public class RedeemUserGiftOrchestrationCommandHandler : IRequestHandler<RedeemUserGiftOrchestration, Result<bool>>
        {
            private readonly IMediator _mediator;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<RedeemUserGiftOrchestrationCommandHandler> _logger;

            public RedeemUserGiftOrchestrationCommandHandler(
                IMediator mediator,
                IUnitOfWork unitOfWork,
                ILogger<RedeemUserGiftOrchestrationCommandHandler> logger)
            {
                _mediator = mediator;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Result<bool>> Handle(RedeemUserGiftOrchestration request, CancellationToken cancellationToken)
            {
                try
                {
                   
                    // Step 2: Calculate gift count
                    int giftsEarned = request.PointsEarned / request.PointsThreshold;
                    if (giftsEarned == 0)
                        return Result<bool>.Success(true); // No gifts to redeem

                    int pointsToDeduct = giftsEarned * request.PointsPerGift;

                    // Step 3: Add or Update Gift
                    var giftResult = await _mediator.Send(new AddUserGiftCommand
                    {
                        UserId=request.userId,
                        GiftsEarned = giftsEarned,
                        PointsPerGift = request.PointsPerGift,
                        PointsThreshold = request.PointsThreshold
                    });

                    if (!giftResult.IsSuccess)
                        return Result<bool>.Failure(ErrorCode.ServerError, "Failed to update gifts");

                    // Step 4: Deduct from pickup request
                    //var deductResult = await _mediator.Send(new DeductPointsFromPickupRequestCommand
                    //{
                    //    UserId = request.userId,
                    //    PointsToDeduct = pointsToDeduct
                    //});

                    //if (!deductResult.IsSuccess)
                    //    return Result<bool>.Failure(ErrorCode.ServerError, "Failed to deduct pickup request points");

                    // Step 5: Log in points history
                    var logResult = await _mediator.Send(new LogPointsHistoryCommand
                    {
                        UserId = request.userId,
                        PointsChanged = -pointsToDeduct,
                        Type = PointsHistoryTypes.Earned,
                        Reason = $"Points deducted due to earning {giftsEarned} gift(s)"
                    });

                    if (!logResult.IsSuccess)
                        return Result<bool>.Failure(ErrorCode.ServerError, "Failed to log points history");

                    // Step 6: Send Notification
                    var notifyResult = await _mediator.Send(new SendNotificationCommand
                    {
                        UserId = request.userId,
                        Title = " You've Earned a Gift!",
                        Message = $"Congratulations! You've earned {giftsEarned} gift(s) for collecting {request.PointsEarned} points."
                    });

                    if (!notifyResult.IsSuccess)
                        return Result<bool>.Failure(ErrorCode.ServerError, "Failed to send notification");

                    // Step 7: Commit
                    await _unitOfWork.SaveChangesAsync();

                    return Result<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while redeeming gift in orchestration");
                    return Result<bool>.Failure(ErrorCode.ServerError, "Unexpected error occurred");
                }
            }
        }
    }


