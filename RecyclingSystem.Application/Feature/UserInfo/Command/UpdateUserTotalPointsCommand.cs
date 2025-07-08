using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Command
{
    public class UpdateUserTotalPointsCommand : IRequest<Result<UpdateUserTotalPointsResponse>>
    {
        public int UserId { get; set; }
        public int NewPoints { get; set; }
    }

    public class UpdateUserTotalPointsResponse
    {
        public int UserId { get; set; }
        public int TotalPoints { get; set; }
    }

    public class UpdateUserTotalPointsCommandHandler : IRequestHandler<UpdateUserTotalPointsCommand, Result<UpdateUserTotalPointsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserTotalPointsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<UpdateUserTotalPointsResponse>> Handle(UpdateUserTotalPointsCommand request, CancellationToken cancellationToken)
        {
            // Validate the request
            if (request.UserId <= 0 || request.NewPoints < 0)
            {
                return await Task.FromResult(Result<UpdateUserTotalPointsResponse>.Failure(ErrorCode.BadRequest, "Invalid User ID or points."));
            }
            // Update the user's total points in the database
            var user = await _unitOfWork.applicationUser.GetById(request.UserId);
            if (user == null)
            {
                return await Task.FromResult(Result<UpdateUserTotalPointsResponse>.Failure(ErrorCode.NotFound, "User not found."));
            }
            user.TotalPoints += request.NewPoints;
            await _unitOfWork.SaveChangesAsync();
            return Result<UpdateUserTotalPointsResponse>.Success(new UpdateUserTotalPointsResponse
            {
                UserId = user.Id,
                TotalPoints = user.TotalPoints
            }, "User Points Has Been Updated");
        }
    }
}
