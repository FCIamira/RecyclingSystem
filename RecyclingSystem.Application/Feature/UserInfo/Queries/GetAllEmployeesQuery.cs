using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.EmployeeInfoDTOs;
using RecyclingSystem.Application.Interfaces;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Queries
{
    public class GetAllEmployeesQuery:IRequest<Result<EmployeeAvailabilityDto>>
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
                    // Step 1: Get the original pickup request
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

                // Step 2: Plan A - Get strictly available employees

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
                    }
                    }

                    // Step 3: Return strictly available employees if found
                    if (dto.StrictlyAvailableEmployees.Any())
                    {
                        var emails = string.Join(", ", strictlyAvailableEmployees.Select(e => e.Email));
                        var msg = $" Strictly available employees: {emails}";
                    return Result<EmployeeAvailabilityDto>.Success(dto);
                }

                // Step 4: Plan B - Suggest alternative times
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
                                EmployeeEmail = employee.Email
                            });
                        }
                    }

                    // Step 5: Choose best suggestion
                    var bestSuggestion = suggestedTimes.OrderBy(s => s.Time).FirstOrDefault();

                    if (bestSuggestion != null)
                    {
                        var msg = $" No strictly available employees at {pickupRequest.ScheduledDate.Value}.\n" +
                                  $"Suggested: {bestSuggestion.EmployeeEmail} at {bestSuggestion.Time}";
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


