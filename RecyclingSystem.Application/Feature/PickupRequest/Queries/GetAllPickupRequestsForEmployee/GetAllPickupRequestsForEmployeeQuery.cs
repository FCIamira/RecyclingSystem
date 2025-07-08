using MediatR;
using Microsoft.AspNetCore.Http;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequestsForEmployee
{
    public class GetAllPickupRequestsForEmployeeQuery : IRequest<Result<List<GetEmployeePickupRequestDto>>>
    {

    }

    public class GetAllPickupRequestsForEmployeeQueryHandler : IRequestHandler<GetAllPickupRequestsForEmployeeQuery, Result<List<GetEmployeePickupRequestDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetAllPickupRequestsForEmployeeQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task<Result<List<GetEmployeePickupRequestDto>>> Handle(GetAllPickupRequestsForEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employeeIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(employeeIdString) || !int.TryParse(employeeIdString, out int employeeId))
            {
                return Task.FromResult(Result<List<GetEmployeePickupRequestDto>>.Failure(ErrorCode.Unauthorized, "Invalid employee ID."));
            }
            

            var result = unitOfWork.pickupRequest.GetAllWithFilter(p => p.EmployeeId == employeeId);

            var pickupRequests = result.Select(p => new GetEmployeePickupRequestDto
            {
                Id = p.Id,
                Customer = new UserInfoDto
                {
                    Id = p.Customer!.Id,
                    FullName = p.Customer!.FullName,
                    Email = p.Customer.Email,
                    PhoneNumber = p.Customer.PhoneNumber
                },
                RequestedDate = p.RequestedDate,
                ScheduledDate = p.ScheduledDate,
                Address = p.Address,
                LocationLat = p.LocationLat,
                LocationLng = p.LocationLng,
                Note = p.Note,
                Status = p.Status,
                DateCollected = p.DateCollected,
                TotalPointsGiven = p.TotalPointsGiven,
                PickupItems = p.PickupItems.Select(pi => new PickupItemDto
                {
                    Id = pi.Id,
                    MaterialId = pi.MaterialId,
                    MaterialName = pi.Material!.Name ?? "Unknown",
                    PlannedQuantity = pi.PlannedQuantity,
                    ActualQuantity = pi.ActualQuantity
                }).ToList()
            }).ToList();

            return Task.FromResult(Result<List<GetEmployeePickupRequestDto>>.Success(pickupRequests));
        }
    }
}
