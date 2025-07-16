using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.Feature.UserInfo.Command;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Commands
{
    public class UpdateUserTotalPointForCancelCommand: IRequest<Result<string>>
    {
        public int UserId { get; set; }
        public int NewPoints { get; set; }
    }

    public class UpdateUserTotalPointForCancelCommandHandler : IRequestHandler<UpdateUserTotalPointForCancelCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserTotalPointForCancelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(UpdateUserTotalPointForCancelCommand request, CancellationToken cancellationToken)
        {
            //// Validate the request
            if (request.UserId <= 0 || request.NewPoints < 0)
            {
                return await Task.FromResult(Result<string>.Failure(ErrorCode.BadRequest, "Invalid User ID or points."));
            }
            // Update the user's total points in the database
            var user = await _unitOfWork.applicationUser.GetById(request.UserId);
            if (user == null)
            {
                return await Task.FromResult(Result<string>.Failure(ErrorCode.NotFound, "User not found."));
            }

            if (user.TotalPoints == 0)
            {
                return await Task.FromResult(Result<string>.Failure(ErrorCode.NotFound, "TotalPoint =0."));
            }
            user.TotalPoints -= request.NewPoints;
            return Result<string>.Success("User Points Has Been Updated");
        }
    }
}
