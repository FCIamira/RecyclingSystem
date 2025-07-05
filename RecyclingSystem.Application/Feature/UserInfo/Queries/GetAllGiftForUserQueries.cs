using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;

namespace RecyclingSystem.Application.Feature.UserInfo.Queries
{
    public class GetAllGiftForUserQueries:IRequest<Result<List<GetAllGiftDto>>>
    {
    }
    public class GetAllGiftForUserQueriesHandler : IRequestHandler<GetAllGiftForUserQueries, Result<List<GetAllGiftDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetAllGiftForUserQueriesHandler> _logger;

        public GetAllGiftForUserQueriesHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            ILogger<GetAllGiftForUserQueriesHandler> logger, IMediator mediator, IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<Result<List<GetAllGiftDto>>> Handle(GetAllGiftForUserQueries request, CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    return Result<List<GetAllGiftDto>>.Failure(ErrorCode.Unauthorized, "No HttpContext.");

                var user = await _userManager.GetUserAsync(httpContext.User);
                if (!httpContext.User.Identity.IsAuthenticated)
                    return Result<List<GetAllGiftDto>>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");

                var userGifts = await _unitOfWork.userGift.GetAll();
                var filtered = userGifts.Where(g => g.UserId == user.Id).ToList();
                var result = _mapper.Map<List<GetAllGiftDto>>(filtered);
                return Result<List<GetAllGiftDto>>.Success(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving pickup requests for user.");
                return Result<List<GetAllGiftDto>>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
            throw new NotImplementedException();
        }
    }
}
