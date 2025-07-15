using MediatR;
using Microsoft.AspNetCore.Identity;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.Feature.Notifications.Commands;
using RecyclingSystem.Application.Feature.PickupRequest.Queries;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Commands
{
    public class AssignEmployeeToRequestCommand : IRequest<Result<AssignEmployeeToRequestDto>>
    {

        public string Email { get; set; }
        public int RequestId { get; set; }
      //  public PickupStatus Status { get; set; }
    }

  
    public class AssignEmployeeToRequestCommandHandler : IRequestHandler<AssignEmployeeToRequestCommand, Result<AssignEmployeeToRequestDto>>
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AssignEmployeeToRequestCommandHandler(IMediator mediator, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<AssignEmployeeToRequestDto>> Handle(AssignEmployeeToRequestCommand request, CancellationToken cancellationToken)
        {

            //if (!Enum.TryParse<PickupStatus>(request.Status.ToString(), out var parsedStatus))
            //{
            //    return Result<AssignEmployeeToRequestDto>.Failure(ErrorCode.BadRequest, "Invalid status selected.");
            //}
            var employee = await _userManager.FindByEmailAsync(request.Email);
            var employeeId = employee?.Id;
            if (employeeId == null)
            {
                Result<AssignEmployeeToRequestDto>.Failure(ErrorCode.Unauthorized, "Employee Email Unauthorized ");
            }
            var Requestinfo = await _unitOfWork.pickupRequest.GetById(request.RequestId);
            if (Requestinfo == null)
            {
                Result<AssignEmployeeToRequestDto>.Failure(ErrorCode.NotFound, "Requestid Not Found ");
            }
            if (Requestinfo.EmployeeId == null)
            {
                Requestinfo.EmployeeId = employeeId;
                Requestinfo.Status = PickupStatus.Scheduled;

                await _unitOfWork.pickupRequest.Update(Requestinfo.Id, Requestinfo);
                await _mediator.Send(new SendNotificationCommand
                {
                    UserId = Requestinfo.EmployeeId,
                    Title = "New Request Assigned to You",
                    Message = $"A new pickup request has been assigned to you by customer ID {Requestinfo.CustomerId}. Please follow up through your dashboard."

                });
                await _unitOfWork.SaveChangesAsync();
                return Result<AssignEmployeeToRequestDto>.Success(new AssignEmployeeToRequestDto
                {
                    Email = request.Email,
                   // Status= request.Status.ToString(),
                }, $"{request.Email} has been assigned to the request successfully");

            }

            return Result<AssignEmployeeToRequestDto>.Failure(ErrorCode.Unauthorized, "alredy this Request Have Employee Email ");


        }
    }
}