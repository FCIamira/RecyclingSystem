using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Commands
{
    public class DeductPointsFromPickupRequestCommand : IRequest<Result<bool>>
    {
        public int UserId { get; set; }
        public int PointsToDeduct { get; set; }
    }

    public class DeductPointsFromPickupRequestCommandHandler : IRequestHandler<DeductPointsFromPickupRequestCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeductPointsFromPickupRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeductPointsFromPickupRequestCommand request, CancellationToken cancellationToken)
        {
            var pickupRequest = (await _unitOfWork.pickupRequest.GetAll())
                ?.FirstOrDefault(p => p.CustomerId == request.UserId);

            if (pickupRequest == null || pickupRequest.TotalPointsGiven < request.PointsToDeduct)
            {
                return Result<bool>.Failure(ErrorCode.BadRequest, "Invalid pickup request or insufficient points.");
            }

            pickupRequest.TotalPointsGiven -= request.PointsToDeduct;
            _unitOfWork.pickupRequest.Update(pickupRequest.Id, pickupRequest);
            return Result<bool>.Success(true);
        }
    }
}
