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

namespace RecyclingSystem.Application.Feature.Rewards.Query
{
    public class GetAllRewardsByTitleQuery:IRequest<List<RewardDTO>>
    {

        public string Title { get; set; }
    }

    public class GetAllRewardsByTitleHandler : IRequestHandler<GetAllRewardsByTitleQuery, List<RewardDTO>>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRewardsByTitleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<List<RewardDTO>> Handle(GetAllRewardsByTitleQuery request, CancellationToken cancellationToken)
        {
            var rewards = await unitOfWork.rewards
       .GetAllWithFilter(r => string.IsNullOrEmpty(request.Title) || r.Title.ToLower().Contains(request.Title.ToLower()))
         .ProjectTo<RewardDTO>(_mapper.ConfigurationProvider)
       .ToListAsync();
            return rewards;


        }
    }
}
