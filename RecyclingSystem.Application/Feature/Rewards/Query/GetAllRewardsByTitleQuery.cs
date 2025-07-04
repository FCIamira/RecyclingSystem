using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;

namespace RecyclingSystem.Application.Feature.Rewards.Query
{
    public class GetAllRewardsByTitleQuery:IRequest<Result<List<RewardDTO>>>
    {

        public string Title { get; set; }
    }

    public class GetAllRewardsByTitleHandler : IRequestHandler<GetAllRewardsByTitleQuery, Result<List<RewardDTO>>>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRewardsByTitleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<Result<List<RewardDTO>>> Handle(GetAllRewardsByTitleQuery request, CancellationToken cancellationToken)
        {
            var rewards = await unitOfWork.rewards
       .GetAllWithFilter(r => string.IsNullOrEmpty(request.Title) || r.Title.ToLower().Contains(request.Title.ToLower()))
         .ProjectTo<RewardDTO>(_mapper.ConfigurationProvider)
       .ToListAsync();
            if (rewards == null || !rewards.Any())
            {
                return Result<List<RewardDTO>>.Failure(ErrorCode.NotFound, "No rewards found in this range.");
            }

            return Result<List<RewardDTO>>.Success(rewards);


        }
    }
}
