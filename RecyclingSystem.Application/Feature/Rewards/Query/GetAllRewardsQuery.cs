//using MediatR;
//using RecyclingSystem.Domain.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RecyclingSystem.Application.Feature.Rewards.Query
//{
//    public class GetAllRewardsQuery:IRequest
//    {
//    }

//    public class GetAllRewardsQueryHandler:IRequestHandler<GetAllRewardsQuery>
//    {
//        private readonly IUnitOfWork unitOfWork;
//        public GetAllRewardsQueryHandler( IUnitOfWork unitOfWork) { 
//        this.unitOfWork = unitOfWork;
        
//        }

//        public Task Handle(GetAllRewardsQuery request, CancellationToken cancellationToken)
//        {
//          return  unitOfWork.rewards.GetAll();
//        }
//    }
//}
