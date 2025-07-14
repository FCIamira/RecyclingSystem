using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.NotificationsDTOs;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Application.Mapping;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Queries
{
    public class GetUserQuery : IRequest<Result<UserInfoDto>>
    {
        public int UserId { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetUserQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<UserInfoDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get user information");
            try
            {
                var user = await _unitOfWork.applicationUser.GetById(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("Not found user.");
                    return Result<UserInfoDto>.Failure(ErrorCode.NotFound, "Not found user.");
                }
                var userDto = _mapper.Map<UserInfoDto>(user);
                return Result<UserInfoDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user.");
                return Result<UserInfoDto>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
