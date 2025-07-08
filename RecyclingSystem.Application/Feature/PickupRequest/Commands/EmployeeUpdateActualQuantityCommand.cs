using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Commands
{
    public class EmployeeUpdateActualQuantityCommand : IRequest<Result<EmployeeUpdateActualQuantityResponse>>
    {
        public int PickupItemId { get; set; }
        public int ActualQuantity { get; set; }
    }

    public class EmployeeUpdateActualQuantityResponse
    {
        // This can be extended with more properties if needed
    }

    public class EmployeeUpdateActualQuantityCommandHandler : IRequestHandler<EmployeeUpdateActualQuantityCommand, Result<EmployeeUpdateActualQuantityResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeUpdateActualQuantityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<EmployeeUpdateActualQuantityResponse>> Handle(EmployeeUpdateActualQuantityCommand request, CancellationToken cancellationToken)
        {
            // Validate the pickup item exists
            var pickupItem = await _unitOfWork.pickupItem.GetById(request.PickupItemId);
            if (pickupItem == null)
            {
                throw new Exception("Pickup item not found.");
            }
            // Update the actual quantity
            if (request.ActualQuantity < 0)
            {
                return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Failure(ErrorCode.BadRequest, "Actual quantity cannot be negative."));
            }
            if(request.ActualQuantity > pickupItem.PlannedQuantity)
            {
                return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Failure(ErrorCode.BadRequest, "Actual quantity cannot exceed planned quantity."));
            }

            pickupItem.ActualQuantity = request.ActualQuantity;
            await _unitOfWork.pickupItem.Update(pickupItem.Id, pickupItem);
            
            return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Success(new EmployeeUpdateActualQuantityResponse(), "Actual quantity updated successfully."));
        }
    }
}
