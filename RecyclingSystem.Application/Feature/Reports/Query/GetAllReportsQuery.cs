using MediatR;
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
    public class GetAllReportsQuery : IRequest<Result<GetAllReportsResponse>>
    {
        public string? Status { get; set; }
    }

    public class GetAllReportsResponse
    {
        public List<ReportDto> Reports { get; set; } = new List<ReportDto>();
        public int TotalCount { get; set; }
    }

    public class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, Result<GetAllReportsResponse>>
    {
        // This handler would typically interact with a repository or database context to fetch the reports.
        private readonly IUnitOfWork _unitOfWork;
        public GetAllReportsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllReportsResponse>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
        {

            if(request.Status == null)
            {
                var reports = await _unitOfWork.report.GetAll();
                var response = new GetAllReportsResponse
                {
                    Reports = reports.Select(r => new ReportDto
                    {
                        Id = r.Id,
                        EmployeeId = r.EmployeeId,
                        Type = r.Type,
                        Description = r.Description,
                        Status = r.Status,
                        CreatedAt = r.DateCreated
                    }).ToList(),
                    TotalCount = reports.Count()
                };
                return Result<GetAllReportsResponse>.Success(response);
            }
            else
            {
                if (!Enum.TryParse<ReportStatus>(request.Status, true, out var statusEnum))
                {
                    // Handle invalid status value (e.g., return error or default behavior)
                    return Result<GetAllReportsResponse>.Failure(ErrorCode.BadRequest ,"Invalid status value");
                }

                // Filter reports by status if provided
                var reportsByStatus = _unitOfWork.report.GetAllWithFilter(r => r.Status == statusEnum);
                var responseByStatus = new GetAllReportsResponse
                {
                    Reports = reportsByStatus.Select(r => new ReportDto
                    {
                        Id = r.Id,
                        EmployeeId = r.EmployeeId,
                        Type = r.Type,
                        Description = r.Description,
                        Status = r.Status,
                        CreatedAt = r.DateCreated
                    }).ToList(),
                    TotalCount = reportsByStatus.Count()
                };
                return Result<GetAllReportsResponse>.Success(responseByStatus);

            }


           
        }
    }
}
