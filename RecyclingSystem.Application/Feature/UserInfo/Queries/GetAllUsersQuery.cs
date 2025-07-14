using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Queries
{
    public class GetAllUsersQuery : IRequest<Result<List<UserInfoDto>>>
    {
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserInfoDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllUsersQueryHandler> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<List<UserInfoDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all users.");
            try
            {
                var users = await _unitOfWork.applicationUser.GetAll();
                if (users == null)
                {
                    _logger.LogWarning("Not found users.");
                    return Result<List<UserInfoDto>>.Failure(ErrorCode.NotFound, "Not found users.");
                }

                var usersDto = _mapper.Map<List<UserInfoDto>>(users);
                return Result<List<UserInfoDto>>.Success(usersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return Result<List<UserInfoDto>>.Failure(ErrorCode.ServerError, ex.Message);
            }
        }
    }
}
