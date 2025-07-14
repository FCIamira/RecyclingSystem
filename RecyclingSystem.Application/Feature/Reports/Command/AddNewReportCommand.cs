using AutoMapper;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.NotificationsDTOs;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Reports.Command
{
    public class AddNewReportCommand:IRequest<Result<string>>
    {
        public CreateReportDTO Report;
    }
    public class AddNewReportCommandHandler : IRequestHandler<AddNewReportCommand, Result<string>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddNewReportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, 
            ILogger<AddNewReportCommandHandler> logger, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<string>> Handle(AddNewReportCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding new Report.");
            try
            {
                var employee = _contextAccessor.HttpContext.User;
                if (!employee.Identity.IsAuthenticated)
                {
                    _logger.LogError("Employee is not authenticated.");
                    return Result<string>.Failure(ErrorCode.Unauthorized, "Employee is not authenticated.");
                }
                int empId = int.Parse(employee.FindFirst(ClaimTypes.NameIdentifier).Value);

                var report = new Report
                {
                    EmployeeId = empId,
                    Type = request.Report.Type,
                    PickupRequestId = request.Report.PickupRequestId,
                    Description = request.Report.Description,
                };

                if (!string.IsNullOrEmpty(request.Report.WarehouseName))
                {
                    var warehouse = await _unitOfWork.warehouse
                        .GetSpecificWithFilter(w => w.Name == request.Report.WarehouseName);

                    if (warehouse == null)
                    {
                        _logger.LogWarning("Not found warehouse.");
                        return Result<string>.Failure(ErrorCode.NotFound, "Warehouse not found.");
                    }

                    report.WarehouseId = warehouse.Id;
                }

                await _unitOfWork.report.Add(report);
                await _unitOfWork.SaveChangesAsync();

                return Result<string>.Success("Report added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a warehouse.");
                return Result<string>.Failure(ErrorCode.ServerError, ex.Message);
            }
        }
    }
}
