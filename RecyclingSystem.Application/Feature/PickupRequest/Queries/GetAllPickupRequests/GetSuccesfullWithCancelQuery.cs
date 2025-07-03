using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
using AutoMapper;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests
{
    #region Query
    public class GetSuccesfullWithCancelQuery : IRequest<Result<List<GetSuccessfullwithCancelDto>>>
    {
    }
    #endregion

    #region Handler
    public class GetSuccesfullWithCancelQueryHandler : IRequestHandler<GetSuccesfullWithCancelQuery, Result<List<GetSuccessfullwithCancelDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetSuccesfullWithCancelQueryHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetSuccesfullWithCancelQueryHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            ILogger<GetSuccesfullWithCancelQueryHandler> logger,IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<Result<List<GetSuccessfullwithCancelDto>>> Handle(GetSuccesfullWithCancelQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all pickup requests for the current user.");
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HttpContext is null");
                    return Result<List<GetSuccessfullwithCancelDto>>.Failure(ErrorCode.Unauthorized, "No HttpContext.");
                }

                var user = await _userManager.GetUserAsync(httpContext.User);
                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    _logger.LogError("User is not authenticated.");
                    return Result<List<GetSuccessfullwithCancelDto>>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");
                }
                var pickupRequests = await _unitOfWork.pickupRequest.GetAllDetails();
                var userPickupRequests = pickupRequests?.Where(p => p.CustomerId == user.Id && (p.Status == PickupStatus.Cancelled || p.Status == PickupStatus.Collected)).ToList();
                if (pickupRequests == null)
                {
                    _logger.LogWarning("No pickup requests found.");
                    return Result<List<GetSuccessfullwithCancelDto>>.Failure(ErrorCode.NotFound, "No pickup requests available.");
                }
                var pickupRequestDTo = _mapper.Map<List<GetSuccessfullwithCancelDto>>(userPickupRequests);
                return Result<List<GetSuccessfullwithCancelDto>>.Success(pickupRequestDTo);
            }

            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while retrieving pickup requests for user.");
                return Result<List<GetSuccessfullwithCancelDto>>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }
    }

    #endregion

}
