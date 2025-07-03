using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests
{
    #region Query
    public class GetAllPickupRequestsQuery : IRequest<Result<List<GetAllRequestDto>>>
    {
    }
    #endregion

    #region Handler
    public class GetAllPickupRequestsQueryHandler : IRequestHandler<GetAllPickupRequestsQuery, Result<List<GetAllRequestDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllPickupRequestsQueryHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllPickupRequestsQueryHandler(IUnitOfWork unitOfWork,
            ILogger<GetAllPickupRequestsQueryHandler> logger,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<Result<List<GetAllRequestDto>>> Handle(GetAllPickupRequestsQuery request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            //{
            //    return Result<List<GetAllRequestDto>>.Failure(ErrorCode.Unauthorized,"User not authenticated");
            //}
            throw new NotImplementedException();
        }
    }

    #endregion

}
