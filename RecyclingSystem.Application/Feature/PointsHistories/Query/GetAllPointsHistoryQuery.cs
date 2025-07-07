using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.NotificationsDTOs;
using RecyclingSystem.Application.DTOs.PointsHistoryDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PointsHistories.Query
{
    public class GetAllPointsHistoryQuery : IRequest<Result<List<ShowPointsHistoryDto>>>
    {
    }

    public class GetAllPointsHistoryQueryHandler : IRequestHandler<GetAllPointsHistoryQuery, Result<List<ShowPointsHistoryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        public GetAllPointsHistoryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllPointsHistoryQuery> logger,
            IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<Result<List<ShowPointsHistoryDto>>> Handle(GetAllPointsHistoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all points history for user");
            try
            {
                var user = _contextAccessor.HttpContext.User;
                if (!user.Identity.IsAuthenticated)
                {
                    _logger.LogError("User is not authenticated.");
                    return Result<List<ShowPointsHistoryDto>>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");
                }
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

                var userHistory = _unitOfWork.pointsHistory
                    .GetAllWithFilter(u => u.UserId == userId)
                    .ProjectTo<ShowPointsHistoryDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToList();

                if (userHistory == null || !userHistory.Any())
                {
                    _logger.LogWarning("Not found points hidtory for user.");
                    return Result<List<ShowPointsHistoryDto>>.Failure(ErrorCode.NotFound, "Not found points hidtory for user.");
                }

                return Result<List<ShowPointsHistoryDto>>.Success(userHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving history for user.");
                return Result<List<ShowPointsHistoryDto>>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
