using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Rewards.Command
{
    public class UpdateRewardCommand : IRequest<Result<bool>>
    {
        public int rewardId { get; set; }
        public UpdateRewardDTO UpdatedReward { get; set; }
    }
    public class UpdateRewardCommandHandler : IRequestHandler<UpdateRewardCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateRewardCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(UpdateRewardCommand request, CancellationToken cancellationToken)
        {
            
            if (request.UpdatedReward == null)
            {
                return Result<bool>.Failure(ErrorCode.BadRequest, "Reward data is required.");
            }
            var existingReward = await unitOfWork.rewards.GetById(request.rewardId);
            if (existingReward == null)
            {
                return Result<bool>.Failure(ErrorCode.NotFound, "Reward not found.");
            }
            if (existingReward.Id != request.rewardId)
            {
                return Result<bool>.Failure(ErrorCode.BadRequest, "Reward ID mismatch.");
            }
            existingReward.Title = request.UpdatedReward.Title ?? existingReward.Title;
            existingReward.Description = request.UpdatedReward.Description ?? existingReward.Description;
            existingReward.ImageUrl = request.UpdatedReward.ImageUrl ?? existingReward.ImageUrl;
            existingReward.PointsRequired = request.UpdatedReward.PointsRequired ?? existingReward.PointsRequired;
            existingReward.StockQuantity = request.UpdatedReward.StockQuantity ?? existingReward.StockQuantity;
            existingReward.IsActive = request.UpdatedReward.IsActive ?? existingReward.IsActive;
            await unitOfWork.rewards.Update(existingReward.Id, existingReward);
            await unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true, "Reward updated successfully.");
        }
    }
}
