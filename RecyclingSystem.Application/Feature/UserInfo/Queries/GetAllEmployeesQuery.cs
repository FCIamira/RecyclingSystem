using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.EmployeeInfoDTOs;
using RecyclingSystem.Application.Interfaces;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;

namespace RecyclingSystem.Application.Feature.UserInfo.Queries
{
    public class GetAllEmployeesQuery : IRequest<Result<EmployeeAvailabilityDto>>
    {
        public int RequestId { get; set; }
    }

    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployeesQuery, Result<EmployeeAvailabilityDto>>
    {
        private readonly IEmployeeAvailabilityService _employeeAvailability;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllEmployeeQueryHandler> _logger;

        public GetAllEmployeeQueryHandler(
            IEmployeeAvailabilityService employeeAvailability,
            IUnitOfWork unitOfWork,
            ILogger<GetAllEmployeeQueryHandler> logger)
        {
            _employeeAvailability = employeeAvailability;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<EmployeeAvailabilityDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get ALL Employees");

            try
            {
                var pickupRequest = await _unitOfWork.pickupRequest.GetById(request.RequestId);
                if (pickupRequest == null)
                {
                    return Result<EmployeeAvailabilityDto>.Failure(ErrorCode.NotFound, "Request not found");
                }

                var allEmployees = await _unitOfWork.applicationUser.GetEmployeeWithFilter(
                    u => u.Role == "Employee" && !u.IsDeleted
                );

                if (allEmployees == null || !allEmployees.Any())
                {
                    return Result<EmployeeAvailabilityDto>.Failure(ErrorCode.NotFound, "No employees found in the database");
                }

                var dto = new EmployeeAvailabilityDto();
                List<ApplicationUser> strictlyAvailableEmployees = new();

                foreach (var employee in allEmployees)
                {
                    var scheduledRequests = await _unitOfWork.pickupRequest.GetScheduledRequestsFor(employee.Email);

                    var weekCount = _employeeAvailability.CountInSameWeek(scheduledRequests, pickupRequest.ScheduledDate.Value);
                    var dayCount = _employeeAvailability.CountInSameDay(scheduledRequests, pickupRequest.ScheduledDate.Value);
                    var hasConflict = _employeeAvailability.HasTimeConflict(scheduledRequests, pickupRequest.ScheduledDate.Value);

                    if (weekCount <= 10 && dayCount < 2 && !hasConflict)
                    {
                        dto.StrictlyAvailableEmployees.Add(new EmployeeDto
                        {
                            Email = employee.Email,
                            UserName = employee.UserName
                        });

                        strictlyAvailableEmployees.Add(employee);
                    }
                }

                if (dto.StrictlyAvailableEmployees.Any())
                {
                    return Result<EmployeeAvailabilityDto>.Success(dto);
                }

                // Plan B: Suggestions
                List<SuggestionDto> suggestedTimes = new();

                foreach (var employee in allEmployees)
                {
                    var scheduledRequests = await _unitOfWork.pickupRequest.GetScheduledRequestsFor(employee.Email);
                    var alternativeTime = _employeeAvailability.FindClosestAvailableSlot(scheduledRequests, pickupRequest.ScheduledDate.Value);

                    if (alternativeTime != null)
                    {
                        suggestedTimes.Add(new SuggestionDto
                        {
                            Time = alternativeTime.Value,
                            EmployeeEmail = employee.Email,
                            UserName = employee.UserName
                        });
                    }
                }

                var bestSuggestion = suggestedTimes.OrderBy(s => s.Time).FirstOrDefault();

                if (bestSuggestion != null)
                {
                    dto.SuggestedEmployees = suggestedTimes;
                    dto.BestSuggestion = bestSuggestion;

                    return Result<EmployeeAvailabilityDto>.Success(dto);
                }
                else
                {
                    return Result<EmployeeAvailabilityDto>.Failure(ErrorCode.NotFound, "No alternative time found for any employee.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting available employees.");
                return Result<EmployeeAvailabilityDto>.Failure(ErrorCode.ServerError, "An error occurred while processing your request.");
            }
        }
    }
}



