using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Reports.Query
{
    public class GetAllReportsQuery : IRequest<Result<List<ReportDto>>>
    {
    }

    public class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, Result<List<ReportDto>>>
    {
        // This handler would typically interact with a repository or database context to fetch the reports.
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllReportsQueryHandler> _logger;
        public GetAllReportsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllReportsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<List<ReportDto>>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all reports.");
            try
            {
                var reports = await _unitOfWork.report.GetAllReportWithDetailsAsync();
                if(reports == null)
                {
                    _logger.LogWarning("Reports not found.");
                    return Result<List<ReportDto>>.Failure(ErrorCode.NotFound, "Reports not found.");
                }

                var reportsDto = reports.Select(r => new ReportDto
                {
                    Id = r.Id,
                    EmployeeId = r.EmployeeId,
                    EmployeeName = r.Employee.FullName,
                    Type = r.Type,
                    PickupRequestId = r.PickupRequestId,
                    CustomerName = r.PickupRequest?.Customer?.FullName,
                    RequestAddress = r.PickupRequest?.Address,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.DateCreated
                }).ToList();
                return Result<List<ReportDto>>.Success(reportsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving reports.");
                return Result<List<ReportDto>>.Failure(ErrorCode.ServerError, ex.Message);
            }
           
        }
    }
}
