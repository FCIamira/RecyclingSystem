using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
namespace RecyclingSystem.Application.Feature.Rewards.Query
{
    public class GetAllRewardsByRangeOfPointsQuery:IRequest<Result< List<RewardDTO>>>
    {
        public int Min {  get; set; }
        public int Max { get; set; }
    }

        public class GetAllRewardsByRangeOfPointsQueryHandler
            : IRequestHandler<GetAllRewardsByRangeOfPointsQuery, Result<List<RewardDTO>>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper _mapper;

            public GetAllRewardsByRangeOfPointsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                this._mapper = mapper;
            }

            public async Task<Result<List<RewardDTO>>> Handle(GetAllRewardsByRangeOfPointsQuery request, CancellationToken cancellationToken)
            {
                var rewards = await unitOfWork.rewards
                    .GetAllWithFilter(r => r.PointsRequired <= request.Max && r.PointsRequired >= request.Min)
                    .ProjectTo<RewardDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                if (rewards == null || !rewards.Any())
                {
                    return Result<List<RewardDTO>>.Failure(ErrorCode.NotFound, "No rewards found in this range.");
                }

                return Result<List<RewardDTO>>.Success(rewards);
            }
        }
    }


