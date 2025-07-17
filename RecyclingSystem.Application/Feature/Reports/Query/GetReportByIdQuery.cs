using AutoMapper;
using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Reports.Query
{
    public class GetReportByIdQuery : IRequest<Result<ReportDto>>
    {
        public int Id { get; set; }
    }

    public class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, Result<ReportDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetReportByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ReportDto>> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.report.GetReportById(request.Id);
            if (report == null)
            {
                return Result<ReportDto>.Failure(Domain.Enums.ErrorCode.NotFound, "Report not found");
            }

            var reportDto = new ReportDto
            {
                Id = report.Id,
                EmployeeId = report.EmployeeId,
                EmployeeName = report.Employee.FullName,
                Type = report.Type,
                PickupRequestId = report.PickupRequestId,
                CustomerName = report.PickupRequest?.Customer?.FullName,
                RequestAddress = report.PickupRequest?.Address,
                Description = report.Description,
                CreatedAt = report.DateCreated,
                Status = report.Status,
            };

            return Result<ReportDto>.Success(reportDto, "Report retrieved successfully.");
        }
    }
}
