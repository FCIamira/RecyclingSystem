
using AutoMapper;
using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RecyclingSystem.Application.Feature.Rewards.Command
{
    public class DeleteRewardCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }
    }

    public class DeleteRewardCommandHandler : IRequestHandler<DeleteRewardCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DeleteRewardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(DeleteRewardCommand request, CancellationToken cancellationToken)
        {
            Domain.Models.Rewards reward = await unitOfWork.rewards.GetById(request.Id);

            if (reward == null)
            {
                return Result<string>.Failure(ErrorCode.NotFound, "Reward not found.");
            }

            await unitOfWork.rewards.Remove(request.Id);
        

            return Result<string>.Success("Reward deleted successfully.");
        }
    }
}
