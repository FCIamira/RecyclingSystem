using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.CustomerInfoDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Commands
{
    public class UpdateCustomerRequestToCancelCommand : IRequest<Result<CancelRequestResultDto>>
    {
        public int RequestId { get; set; }
        public int CustomerId { get; set; }
    }
    public class UpdateCustomerRequestToCancelCommandHandler : IRequestHandler<UpdateCustomerRequestToCancelCommand, Result<CancelRequestResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCustomerRequestToCancelCommandHandler> _logger;

        public UpdateCustomerRequestToCancelCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCustomerRequestToCancelCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<CancelRequestResultDto>> Handle(UpdateCustomerRequestToCancelCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.pickupRequest.GetById(request.RequestId);

            if (data == null)
            {
                _logger.LogWarning("Pickup request with ID {RequestId} not found.", request.RequestId);
                return Result<CancelRequestResultDto>.Failure(ErrorCode.NotFound, "Pickup request not found.");
            }

            if (data.CustomerId != request.CustomerId)
            {
                _logger.LogWarning("Customer {CustomerId} tried to cancel request {RequestId} which doesn't belong to them.", request.CustomerId, request.RequestId);
                return Result<CancelRequestResultDto>.Failure(ErrorCode.Unauthorized, "You are not authorized to cancel this request.");
            }

            if (data.Status == PickupStatus.Collected || data.Status == PickupStatus.Cancelled)
            {
                _logger.LogInformation("Request {RequestId} already completed or cancelled.", request.RequestId);
                return Result<CancelRequestResultDto>.Failure(ErrorCode.ValidationError, "Request is already completed or cancelled.");
            }

            var nowDate = DateTime.Now.Date;
            var scheduledDateOnly = data.ScheduledDate?.Date;

            bool isLateCancellation = scheduledDateOnly.HasValue && scheduledDateOnly.Value == nowDate;
            bool shouldDeductPoints = isLateCancellation; //&& data.TotalPointsGiven.HasValue && data.TotalPointsGiven.Value > 0;

            _logger.LogInformation("NowDate: {Now}, ScheduledDateOnly: {Scheduled}, TotalPointsGiven: {Points}, Late: {IsLate}",
    nowDate, scheduledDateOnly, data.TotalPointsGiven, isLateCancellation);


            data.Status = PickupStatus.Cancelled;
            await _unitOfWork.pickupRequest.Update(data.Id, data);

            _logger.LogInformation("Customer {CustomerId} successfully cancelled pickup request {RequestId}. Late cancellation: {IsLate}", request.CustomerId, request.RequestId, isLateCancellation);

            return Result<CancelRequestResultDto>.Success(new CancelRequestResultDto
            {
                Message = shouldDeductPoints
                    ? "Pickup request cancelled successfully with point deduction."
                    : "Pickup request cancelled successfully without point deduction.",
                ShouldDeductPoints = shouldDeductPoints
            });
        }

    }

}
