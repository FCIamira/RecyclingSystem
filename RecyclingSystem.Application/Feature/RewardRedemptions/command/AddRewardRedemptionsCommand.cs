using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.RewardRedemptionsDTOs;
using RecyclingSystem.Application.Feature.Notifications.Commands;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.RewardRedemptions.command
{
    public class AddRewardRedemptionsCommand : IRequest<Result<string>>
    {
        public AddRewardRedemptionsDTO redemptionsDTO { get; set; }
    }

    public class AddRewardRedemptionsCommandHandler : IRequestHandler<AddRewardRedemptionsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public AddRewardRedemptionsCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
        }

        public async Task<Result<string>> Handle(AddRewardRedemptionsCommand request, CancellationToken cancellationToken)
        {
            var reward = await unitOfWork.rewards.GetById(request.redemptionsDTO.RewardId);
            if (reward == null)
                return Result<string>.Failure(ErrorCode.NotFound, "Reward not found");

            var user = await unitOfWork.applicationUser.GetById(request.redemptionsDTO.UserId);
            if (user == null)
                return Result<string>.Failure(ErrorCode.NotFound, "User not found");

            if (request.redemptionsDTO.Quantity > reward.StockQuantity)
            {
                return Result<string>.Failure(ErrorCode.ValidationError, $"Can't redeem more than {reward.StockQuantity} in stock.");
            }

            int totalPointsToReplaced = request.redemptionsDTO.Quantity * reward.PointsRequired;

            if (user.TotalPoints < totalPointsToReplaced)
            {
                return Result<string>.Failure(ErrorCode.ValidationError, "You don't have enough points to redeem this reward.");
            }

            var redemption = new Domain.Models.RewardRedemptions
            {
                UserId = user.Id,
                RewardId = reward.Id,
                DateRedeemed = DateTime.UtcNow,
                RedemptionStatus = Status.Approved,
                Quantity = request.redemptionsDTO.Quantity,
                TotalPoints = totalPointsToReplaced
            };

            reward.StockQuantity -= request.redemptionsDTO.Quantity;
            user.TotalPoints -= totalPointsToReplaced;
            var pointsHistory = new PointsHistory
            {
                UserId = user.Id,
                Type = PointsHistoryTypes.Redeemed, 
                PointsChanged = -totalPointsToReplaced,
                Reason = $"Redeemed reward '{reward.Title}'",
                Datetime = DateTime.UtcNow
            }; 

            await unitOfWork.rewards.Update(reward.Id,reward);
            await unitOfWork.applicationUser.Update(user.Id, user);
            await unitOfWork.pointsHistory.Add(pointsHistory);
            await unitOfWork.rewardRedemptions.Add(redemption);
            await unitOfWork.SaveChangesAsync();

            await mediator.Send(new SendNotificationCommand
            {
                UserId = user.Id,
                Title = "Reward Redeemed Successfully",
                Message = $"You have successfully redeemed '{reward.Title}' using {totalPointsToReplaced} points."
            });


            return Result<string>.Success("Redemption added successfully.");
        }
    }

}
