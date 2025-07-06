using AutoMapper;
using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Rewards.Command
{
    public class CreateRewardCommand : IRequest<Result<string>>
    {
        public CreateRewardDTO CreateRewardDTO { get; set; }
    }

    public class CreateRewardCommandHandler : IRequestHandler<CreateRewardCommand, Result<string>>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CreateRewardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
        {
            var reward = mapper.Map<Reward>(request.CreateRewardDTO);

            if (reward == null)
            {
                return Result<string>.Failure(ErrorCode.ValidationError, "Reward data is invalid.");
            }

          await  unitOfWork.rewards.Add(reward);
            await unitOfWork.SaveChangesAsync();

            return Result<string>.Success("Reward created successfully.");
        }
    }
}
