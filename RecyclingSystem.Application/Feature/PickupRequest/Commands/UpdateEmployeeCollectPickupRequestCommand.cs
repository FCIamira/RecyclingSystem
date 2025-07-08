using MediatR;
using Microsoft.AspNetCore.Http;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetPickupRequestById;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Commands
{
    public class UpdateEmployeeCollectPickupRequestCommand : IRequest<Result<EmployeeCollectPickupRequestResponse>>
    {
        public int PickupRequestId { get; set; }
        public int TotalPointsGiven { get; set; } = 0;
    }

    public class EmployeeCollectPickupRequestResponse
    {

    }

    public class UpdateEmployeeCollectPickupRequestCommandHandler : IRequestHandler<UpdateEmployeeCollectPickupRequestCommand, Result<EmployeeCollectPickupRequestResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;
        public UpdateEmployeeCollectPickupRequestCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        public async Task<Result<EmployeeCollectPickupRequestResponse>> Handle(UpdateEmployeeCollectPickupRequestCommand request, CancellationToken cancellationToken)
        {
            // Logic to update the pickup request as collected by an employee

            var employeeIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (employeeIdString == null || !int.TryParse(employeeIdString, out int employeeId))
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.Unauthorized, "Employee Id is null"));
            }

            var pickupRequest = await _unitOfWork.pickupRequest.GetById(request.PickupRequestId);

            if (pickupRequest == null)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.NotFound, "Pickup request not found."));
            }

            if (pickupRequest.Status != PickupStatus.Scheduled)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.BadRequest, "Pickup request is not in a valid state for collection."));
            }

            if(pickupRequest.EmployeeId != employeeId)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.Unauthorized, "You are not authorized to collect this request"));
            }

            pickupRequest.Status = PickupStatus.Collected;
            pickupRequest.TotalPointsGiven = request.TotalPointsGiven;
            pickupRequest.DateCollected = DateTime.UtcNow;

            await _unitOfWork.pickupRequest.Update(pickupRequest.Id, pickupRequest);
            await _unitOfWork.SaveChangesAsync();

            var response = new EmployeeCollectPickupRequestResponse();
            return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Success(response, "Pickup request updated successfully."));
        }
    }
}
