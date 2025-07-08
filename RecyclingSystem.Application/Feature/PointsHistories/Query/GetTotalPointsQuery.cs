using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PointsHistoryDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PointsHistories.Query
{
    public class GetTotalPointsQuery : IRequest<Result<ShowTotalPointsDto>>
    {
    }

    public class GetTotalPointsQueryHandler : IRequestHandler<GetTotalPointsQuery, Result<ShowTotalPointsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        public GetTotalPointsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTotalPointsQueryHandler> logger, 
            IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<ShowTotalPointsDto>> Handle(GetTotalPointsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all total points (Earned - Awarded - Redeemed)");
            try
            {
                double TotalEarnedPoints = 0, TotalRedeemedPoints = 0, TotalAwardedPoints = 0;

                var user = _contextAccessor.HttpContext.User;
                if (!user.Identity.IsAuthenticated)
                {
                    _logger.LogError("User is not authenticated.");
                    return Result<ShowTotalPointsDto>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");
                }
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);


                #region TotalEarnedPoints
                //get total earned points
                var EarnedPoints = _unitOfWork.pointsHistory
                    .GetAllWithFilter(p => p.Type == Domain.Enums.PointsHistoryTypes.Earned && p.UserId == userId)
                    .ToList();
                if(!EarnedPoints.Any())
                {
                    _logger.LogWarning("Not found points history for recycling requests.");
                }

                TotalEarnedPoints = EarnedPoints.Sum(p => p.PointsChanged);
                #endregion

                #region TotalRedeemedPoints
                //get total redeemed points
                var ReedemedPoints = _unitOfWork.pointsHistory
                    .GetAllWithFilter(p => p.Type == Domain.Enums.PointsHistoryTypes.Redeemed && p.UserId == userId)
                    .ToList();
                if (!ReedemedPoints.Any())
                {
                    _logger.LogWarning("Not found points history spended.");
                }

                TotalRedeemedPoints = ReedemedPoints.Sum(p => p.PointsChanged);
                #endregion

                #region TotalAwardedPoints
                //get total awarded points
                var AwardedPoints = _unitOfWork.pointsHistory
                    .GetAllWithFilter(p => p.Type == Domain.Enums.PointsHistoryTypes.Bonus && p.UserId == userId)
                    .ToList();
                if (!AwardedPoints.Any())
                {
                    _logger.LogWarning("Not found points history as gift.");
                }

                TotalAwardedPoints = AwardedPoints.Sum(p => p.PointsChanged);
                #endregion

                var points = new ShowTotalPointsDto
                {
                    TotalPointsEarned = TotalEarnedPoints,
                    TotalPointsRedeemed = TotalRedeemedPoints,
                    TotalPointsAwarded = TotalAwardedPoints
                };

                return Result<ShowTotalPointsDto>.Success(points);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving total points history for user.");
                return Result<ShowTotalPointsDto>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
