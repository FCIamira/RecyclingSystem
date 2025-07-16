using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries
{
    public class TotalPickupRequestsAndRewardsDto
    {
        public int TotalPickupRequests { get; set; }
        public int TotalRewards { get; set; }
    }
    public class GetTotalRequestsForCustomerQuery : IRequest<Result<TotalPickupRequestsAndRewardsDto>>
    {
        public int CustomerId { get; set; } 
    }

    public class GetTotalRequestsForCustomerQueryHandler : IRequestHandler<GetTotalRequestsForCustomerQuery, Result<TotalPickupRequestsAndRewardsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetTotalRequestsForCustomerQueryHandler> _logger;
        public GetTotalRequestsForCustomerQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTotalRequestsForCustomerQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<TotalPickupRequestsAndRewardsDto>> Handle(GetTotalRequestsForCustomerQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting total pickup requests and rewards for each user.");
            try
            {
                var totalRequests = await _unitOfWork.pickupRequest.CountAsync(p => p.CustomerId == request.CustomerId);

                var totalRewards = await _unitOfWork.rewardRedemptions.CountAsync(r => r.UserId == request.CustomerId);

                var dto = new TotalPickupRequestsAndRewardsDto
                {
                    TotalPickupRequests = totalRequests,
                    TotalRewards = totalRewards
                };
                return Result<TotalPickupRequestsAndRewardsDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a warehouse.");
                return Result<TotalPickupRequestsAndRewardsDto>.Failure(ErrorCode.BadRequest, ex.Message);
            }
        }
    }
}
