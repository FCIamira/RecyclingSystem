using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.CustomerInfoDTOs;
using RecyclingSystem.Application.Feature.Notifications.Commands;
using RecyclingSystem.Application.Feature.PickupRequest.Commands;
using RecyclingSystem.Application.Feature.UserInfo.Commands;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System.Security.Claims;

namespace RecyclingSystem.Application.Feature.PickupRequest.Orchestrator
{
    #region Orchestrator Command Definition
    public class CancelRequestForCustomerOrchestrator : IRequest<Result<CancelRequestForCustomerDto>>
    {
        public int DecressPoint { get; set; } = 5;
        public int RequestId { get; set; }
    }
    #endregion

    #region Orchestrator Handler
    public class CancelRequestForCustomerOrchestratorHandler : IRequestHandler<CancelRequestForCustomerOrchestrator, Result<CancelRequestForCustomerDto>>
    {
        #region Fields
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CancelRequestForCustomerOrchestratorHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public CancelRequestForCustomerOrchestratorHandler(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CancelRequestForCustomerOrchestratorHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Handle Method
        public async Task<Result<CancelRequestForCustomerDto>> Handle(CancelRequestForCustomerOrchestrator request, CancellationToken cancellationToken)
        {
            #region Extract Current User Id
            var customerIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (customerIdString == null || !int.TryParse(customerIdString, out int customerId))
            {
                _logger.LogWarning("Unauthorized attempt to cancel pickup request. User not authenticated.");
                return Result<CancelRequestForCustomerDto>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");
            }
            #endregion

            _logger.LogInformation("Customer {CustomerId} attempting to cancel request {RequestId}.", customerId, request.RequestId);

            #region Cancel Pickup Request
            var cancelResult = await _mediator.Send(new UpdateCustomerRequestToCancelCommand
            {
                CustomerId = customerId,
                RequestId=request.RequestId,
            });

            var cancellationMessage = cancelResult.Message;
            bool shouldDeductPoints = cancelResult.Data.ShouldDeductPoints;
            if (!cancelResult.IsSuccess)
            {
                _logger.LogWarning("Failed to cancel request {RequestId} by customer {CustomerId}: {Message}", request.RequestId, customerId, cancelResult.Message);
                return Result<CancelRequestForCustomerDto>.Failure(ErrorCode.BadRequest, cancelResult.Message);
            }

            _logger.LogInformation("Pickup request {RequestId} cancelled successfully by customer {CustomerId}.", request.RequestId, customerId);
            #endregion

            #region Deduct Points From Customer
            if (shouldDeductPoints)
            {
                var pointResult = await _mediator.Send(new UpdateUserTotalPointForCancelCommand
                {
                    UserId = customerId,
                    NewPoints = request.DecressPoint
                }, cancellationToken);

                if (!pointResult.IsSuccess)
                {
                    _logger.LogWarning("Failed to deduct points for customer {CustomerId} after cancelling request {RequestId}: {Message}", customerId, request.RequestId, pointResult.Message);
                    return Result<CancelRequestForCustomerDto>.Failure(ErrorCode.NotFound, pointResult.Message);
                }

                _logger.LogInformation("Points deducted ({Points}) from customer {CustomerId} for cancelling request {RequestId}.", request.DecressPoint, customerId, request.RequestId);
            }
             #endregion


            #region Notification

            // Get pickup request data to validate and access employee information
            var requestData = await _unitOfWork.pickupRequest.GetById(request.RequestId);
            if (requestData == null)
            {
                _logger.LogWarning("Pickup request {RequestId} not found while sending notifications.", request.RequestId);
            }
            else
            {
                // Notification to customer
                await _mediator.Send(new SendNotificationCommand()
                {
                    UserId = customerId,
                    Title = "Pickup Request Cancelled",
                    Message = shouldDeductPoints
                        ? $"Your pickup request #{request.RequestId} has been cancelled. {request.DecressPoint} points were deducted from your account."
                        : $"Your pickup request #{request.RequestId} has been cancelled without any point deduction."
                });

                // Notification to employee
                if (requestData.EmployeeId.HasValue)
                {
                    await _mediator.Send(new SendNotificationCommand()
                    {
                        UserId = requestData.EmployeeId.Value,
                        Title = "Request Cancelled by Customer",
                        Message = shouldDeductPoints
                            ? $"The customer cancelled pickup request #{request.RequestId}. Points were deducted."
                            : $"The customer cancelled pickup request #{request.RequestId} without point deduction."
                    });
                }

            }

            #endregion

            #region Return Success Result
            await _unitOfWork.SaveChangesAsync();
            return Result<CancelRequestForCustomerDto>.Success(new CancelRequestForCustomerDto
            {
                RequestId = request.RequestId
            });
            #endregion

          
    }
        #endregion
    }
    #endregion
}
