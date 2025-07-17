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
        public int PickupRequestId { get; set; }
        public int MaterialId { get; set; }
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
            if (request.PickupRequestId <= 0 || request.MaterialId <= 0)
            {
                return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Failure(ErrorCode.BadRequest, "Invalid Pickup Request ID or Material ID."));
            }
            
            if (request.ActualQuantity < 0)
            {
                return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Failure(ErrorCode.BadRequest, "Actual quantity cannot be negative."));
            }

            // Fetch the pickup item based on PickupRequestId and MaterialId
            var pickupItem = await _unitOfWork.pickupItem.GetByRequestIdAndMaterialId(request.PickupRequestId, request.MaterialId);
            // Check if the pickup item exists

            try
            {
                if (pickupItem == null)
                {
                    // add it to the database if it doesn't exist
                    pickupItem = new Domain.Models.PickupItem
                    {
                        PickupRequestId = request.PickupRequestId,
                        MaterialId = request.MaterialId,
                        PlannedQuantity = 0,
                        ActualQuantity = request.ActualQuantity
                    };
                    await _unitOfWork.pickupItem.Add(pickupItem);
                    return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Success(new EmployeeUpdateActualQuantityResponse(), "Pickup item created and actual quantity updated successfully."));
                }


                // Update the actual quantity
                pickupItem.ActualQuantity = request.ActualQuantity;
                await _unitOfWork.pickupItem.Update(pickupItem.Id, pickupItem);

                return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Success(new EmployeeUpdateActualQuantityResponse(), "Actual quantity updated successfully."));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(Result<EmployeeUpdateActualQuantityResponse>.Failure(ErrorCode.ServerError, $"An error occurred while fetching the pickup item: {ex.Message}"));
            }
            
        }
    }
}
