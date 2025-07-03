using AutoMapper;
using MediatR;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Rewards.Command
{
    public class DeleteRewardCommand:IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteRewardCommandHandler : IRequestHandler<DeleteRewardCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public DeleteRewardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public Task Handle(DeleteRewardCommand request, CancellationToken cancellationToken)
        {
         return   unitOfWork.rewards.Remove(request.Id);
        }
    }
}
