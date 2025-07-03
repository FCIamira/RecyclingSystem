using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace RecyclingSystem.Application.Feature.Rewards.Query
{
    public class GetAllRewardsByRangeOfPointsQuery:IRequest<List<RewardDTO>>
    {
        public int Min {  get; set; }
        public int Max { get; set; }
    }
    public class GetAllRewardsByRangeOfPointsQueryHandler : IRequestHandler<GetAllRewardsByRangeOfPointsQuery, List<RewardDTO>>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRewardsByRangeOfPointsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public Task<List<RewardDTO>> Handle(GetAllRewardsByRangeOfPointsQuery request, CancellationToken cancellationToken)
        {
          var rewards=unitOfWork.rewards.
                GetAllWithFilter(r=>r.PointsRequired<=request.Max&&r.PointsRequired>=request.Min)
                .ProjectTo<RewardDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return rewards;   


        }
    }
}
