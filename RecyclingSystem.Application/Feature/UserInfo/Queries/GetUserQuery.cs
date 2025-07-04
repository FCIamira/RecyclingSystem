//using MediatR;
//using RecyclingSystem.Application.Behaviors;
//using RecyclingSystem.Application.DTOs.UserInfDTOs;
//using RecyclingSystem.Domain.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RecyclingSystem.Application.Feature.UserInfo.Queries
//{
//    public class GetUserQuery : IRequest<Result<UserInfoDto>>
//    {
//    }

//    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserInfoDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        public GetUserQueryHandler(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public Task<Result<UserInfoDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
//        {
            
//        }
//    }
//}
