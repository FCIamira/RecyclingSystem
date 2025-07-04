using AutoMapper;
using MediatR;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace RecyclingSystem.Application.Feature.Rewards.Query
{
    public class GetAllRewardsQuery : IRequest<List<RewardDTO>>
    {
    }

    public class GetAllRewardsQueryHandler : IRequestHandler<GetAllRewardsQuery, List<RewardDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRewardsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<List<RewardDTO>> Handle(GetAllRewardsQuery request, CancellationToken cancellationToken)
        {
            var rewards = await unitOfWork.rewards.GetAll()
                ;
            var rewardsDTO = _mapper.Map<List<RewardDTO>>(rewards);
            return rewardsDTO;
        }
    }
}
