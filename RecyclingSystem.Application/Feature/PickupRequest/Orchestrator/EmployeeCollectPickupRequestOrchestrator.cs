using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Application.Feature.PickupRequest.Commands;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetPickupRequestById;
using RecyclingSystem.Application.Feature.PointsHistories.Commands;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Orchestrator
{
    public class EmployeeCollectPickupRequestOrchestrator : IRequest<Result<EmployeeCollectPickupRequestResponse>>
    {
        public int PickupRequestId { get; set; }
        public List<UpdatePickupItemsActualQuantity> updatePickupItemsActualQuantities { get; set; }
    }

    public class EmployeeCollectPickupRequestResponse
    {

    }

    public class EmployeeCollectPickupRequestOrchestratorHandler : IRequestHandler<EmployeeCollectPickupRequestOrchestrator, Result<EmployeeCollectPickupRequestResponse>>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeCollectPickupRequestOrchestratorHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeCollectPickupRequestOrchestratorHandler(IMediator mediator, ILogger<EmployeeCollectPickupRequestOrchestratorHandler> logger,
            IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;

        }
        public async Task<Result<EmployeeCollectPickupRequestResponse>> Handle(EmployeeCollectPickupRequestOrchestrator request, CancellationToken cancellationToken)
        {

            var pickupRequest = await _mediator.Send(new GetPickupRequestByIdQuery { Id = request.PickupRequestId }, cancellationToken);

            if(pickupRequest == null || pickupRequest.Data == null)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.NotFound, "Pickup request not found."));
            }

            #region Check if the pickup request is in a valid state for collection

            if (pickupRequest.Data.Status != PickupStatus.Scheduled)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.BadRequest, "Pickup request is not in a valid state for collection."));
            }
            #endregion

            #region Check if the employee is authorized to collect this pickup request
            var employeeIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employeeIdString == null || !int.TryParse(employeeIdString, out int employeeId))
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.Unauthorized, "Employee Id is null or invalid."));
            }

            if (pickupRequest.Data.Employee == null || pickupRequest.Data.Employee.Id != employeeId)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.Unauthorized, "You are not authorized to collect this request."));
            }
            #endregion

            #region update the actual quantity of the pickup items
            foreach (var item in request.updatePickupItemsActualQuantities)
            {
                var result = await _mediator.Send(new EmployeeUpdateActualQuantityCommand
                {
                    PickupRequestId = request.PickupRequestId,
                    MaterialId = item.MaterialId,
                    ActualQuantity = item.ActualQuantity,
                }, cancellationToken);

                if (!result.IsSuccess)
                {
                    return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(result.Errorcode, result.Message));
                }
            }
            await _unitOfWork.SaveChangesAsync();
            #endregion

            #region Get the pickup request again to ensure we have the latest data
            pickupRequest = await _mediator.Send(new GetPickupRequestByIdQuery { Id = request.PickupRequestId }, cancellationToken);

            if (pickupRequest == null || pickupRequest.Data == null)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(ErrorCode.NotFound, "Pickup request not found."));
            }
            #endregion

            #region Calculate the Request Points based on actualQuantities x pointsPerItem
            var totalPoints = await CalculateRequestPoints.CalculateAsync(pickupRequest.Data.PickupItems, _mediator);
            #endregion

            #region Assign the Points to the Customer
            var assignPointsResult = await _mediator.Send(new AssignNewPointsForCustomerOrchestrator
            {
                CustomerId = pickupRequest.Data.Customer.Id,
                Points = totalPoints,
                PointsHistoryTypes = PointsHistoryTypes.Earned,
            }, cancellationToken);
            if(!assignPointsResult.IsSuccess)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(assignPointsResult.Errorcode, assignPointsResult.Message));
            }
            #endregion

            #region Update the Pickup Request Status to "Collected"
            var updatePickupRequestStatusResult =  await _mediator.Send(new UpdateEmployeeCollectPickupRequestCommand { PickupRequestId = request.PickupRequestId, TotalPointsGiven = totalPoints }, cancellationToken);
            if(!updatePickupRequestStatusResult.IsSuccess)
            {
                return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Failure(updatePickupRequestStatusResult.Errorcode, updatePickupRequestStatusResult.Message));
            }
            #endregion

            return await Task.FromResult(Result<EmployeeCollectPickupRequestResponse>.Success(new EmployeeCollectPickupRequestResponse(), "Pickup request collected successfully."));
        }
    }
}
