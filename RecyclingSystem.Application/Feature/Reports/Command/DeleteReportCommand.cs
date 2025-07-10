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
    public class DeleteReportCommand : IRequest<Result<string>>
    {
        public int ReportId { get; set; }
    }
    
    public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteReportCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            var report = await unitOfWork.report.GetById(request.ReportId);
            if (report == null)
            {
                return Result<string>.Failure(ErrorCode.NotFound, "Report not found.");
            }
            await unitOfWork.report.Remove(report.Id);
            await unitOfWork.SaveChangesAsync();

            return Result<string>.Success("Report deleted successfully.");
        }
    }
}
