using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries
{
    public class TotalPickupRequestsForEmployeeDto
    {
        public int TotalPickupRequestsAssigned { get; set; }
        public int TotalPickupScheduled { get; set; }
        public int TotalPickupCollected { get; set; }
    }
    public class GetTotalRequestsForEmployeeQuery : IRequest<Result<TotalPickupRequestsForEmployeeDto>> 
    {
        public int EmployeeId { get; set; }
    }

    public class GetTotalRequestsForEmployeeQueryHandler : IRequestHandler<GetTotalRequestsForEmployeeQuery, Result<TotalPickupRequestsForEmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetTotalRequestsForEmployeeQueryHandler> _logger;
        public GetTotalRequestsForEmployeeQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTotalRequestsForEmployeeQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<TotalPickupRequestsForEmployeeDto>> Handle(GetTotalRequestsForEmployeeQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all total assigned, completed and collected requests for employee.");
            try
            {
                var totalAssigned = await _unitOfWork.pickupRequest.CountAsync(p => p.EmployeeId == request.EmployeeId);
                var totalScheduled = await _unitOfWork.pickupRequest.CountAsync(s => s.EmployeeId == request.EmployeeId && s.Status == PickupStatus.Scheduled);
                var totalCollected = await _unitOfWork.pickupRequest.CountAsync(c => c.EmployeeId == request.EmployeeId && c.Status == PickupStatus.Collected);
                var dto = new TotalPickupRequestsForEmployeeDto
                {
                    TotalPickupCollected = totalCollected,
                    TotalPickupRequestsAssigned = totalAssigned,
                    TotalPickupScheduled = totalScheduled,
                };
                return Result<TotalPickupRequestsForEmployeeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a warehouse.");
                return Result<TotalPickupRequestsForEmployeeDto>.Failure(ErrorCode.BadRequest, ex.Message);
            }
        }
    }
}
