using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Queries
{
    public class GetUserTotalQuantityQuery():IRequest<Result<UserQuantityDto>>
    {

    }
    public class GetUserTotalQuantityQueryHandler:IRequestHandler<GetUserTotalQuantityQuery,Result<UserQuantityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetUserTotalQuantityQueryHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetUserTotalQuantityQueryHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            ILogger<GetUserTotalQuantityQueryHandler> logger, IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<Result<UserQuantityDto>> Handle(GetUserTotalQuantityQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return Result<UserQuantityDto>.Failure(ErrorCode.Unauthorized, "No HttpContext.");

            var user = await _userManager.GetUserAsync(httpContext.User);
            if (!httpContext.User.Identity.IsAuthenticated)
                return Result<UserQuantityDto>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");

            var pickupRequests = await _unitOfWork.pickupRequest.GetAllDetails();
            var successfulRequests = pickupRequests.Where(p => p.CustomerId == user.Id && p.Status == PickupStatus.Collected);

            int totalQuantity = successfulRequests
                .SelectMany(r => r.PickupItems)
                .Sum(item => item.Quantity);

            var resultDto = new UserQuantityDto
            {
                Name = user.UserName,
                TotalQuantity = totalQuantity
            };

            return Result<UserQuantityDto>.Success(resultDto);
        }

    }

}
