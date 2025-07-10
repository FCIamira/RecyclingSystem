using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Reports.Command
{
    public class AdminReportRespondCommand : IRequest<Result<string>>
    {
        public int ReportId { get; set; }
        public ReportStatus Status { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class AdminReportRespondHandler : IRequestHandler<AdminReportRespondCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        public AdminReportRespondHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(AdminReportRespondCommand request, CancellationToken cancellationToken)
        {
            var report = await unitOfWork.report.GetById(request.ReportId);
            if (report == null)
            {
                return Result<string>.Failure(ErrorCode.NotFound, "Report not found.");
            }

            report.Response = request.ResponseMessage;
            report.Status = request.Status;

            await unitOfWork.report.Update(report.Id, report);
            await unitOfWork.SaveChangesAsync();

            return Result<string>.Success("Response to the report has been successfully sent.");
        }
    }
}
