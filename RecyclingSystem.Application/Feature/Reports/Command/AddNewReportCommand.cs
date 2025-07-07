using AutoMapper;
using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AddNewReportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(AddNewReportCommand request, CancellationToken cancellationToken)
        {
            var report=mapper.Map<Report>(request.Report);

            if (report != null) {

               await unitOfWork.report.Add(report);
                await unitOfWork.SaveChangesAsync();

                return Result<string>.Success("Reaport created successfully.");
            }



            return Result<string>.Failure(ErrorCode.ValidationError, "Report data is invalid.");
        }
    }
}
