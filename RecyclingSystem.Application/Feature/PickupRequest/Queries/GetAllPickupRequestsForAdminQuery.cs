using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries
{
    public class GetAllPickupRequestsForAdminQuery : IRequest<Result<List<PickupRequestDto>>>
    {
    }
    public class GetAllPickupRequestsForAdminQueryHandler : IRequestHandler<GetAllPickupRequestsForAdminQuery, Result<List<PickupRequestDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllPickupRequestsForAdminQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<PickupRequestDto>>> Handle(GetAllPickupRequestsForAdminQuery request, CancellationToken cancellationToken)
        {
            
            var pickupRequests = await unitOfWork.pickupRequest.GetAllDetails();
            if (pickupRequests == null || !pickupRequests.Any())
            {
                return Result<List<PickupRequestDto>>.Failure(ErrorCode.NotFound, "No pickup requests found.");
            }
            var pickupRequestDtos = pickupRequests.Select(pr => new PickupRequestDto
            {
                Id = pr.Id,
                Employee = pr.Employee != null ? new UserInfoDto
                {
                    Id = pr.Employee.Id,
                    FullName = pr.Employee.FullName,
                    Email = pr.Employee.Email,
                    PhoneNumber = pr.Employee.PhoneNumber,
                    Address = pr.Employee.Address,
                    ProfilePictureUrl = pr.Employee.ProfilePictureUrl,
                } : null,
                Customer = new UserInfoDto
                {
                    Id = pr.Customer.Id,
                    FullName = pr.Customer.FullName,
                    Email = pr.Customer.Email,
                    PhoneNumber = pr.Customer.PhoneNumber,
                    Address = pr.Customer.Address,
                    ProfilePictureUrl = pr.Customer.ProfilePictureUrl,
                },
                RequestedDate = pr.RequestedDate,
                ScheduledDate = pr.ScheduledDate,
                Address = pr.Address,
                LocationLat = pr.LocationLat,
                LocationLng = pr.LocationLng,
                Note = pr.Note,
                Status = pr.Status,
                DateCollected = pr.DateCollected,
                TotalPointsGiven = pr.TotalPointsGiven,
                PickupItems = pr.PickupItems.Select(pi => new PickupItemDto
                {
                    Id = pi.Id,
                    MaterialId = pi.MaterialId,
                    MaterialName = pi.Material.Name,
                    PlannedQuantity = pi.PlannedQuantity,
                    ActualQuantity = pi.ActualQuantity,
                }).ToList()
            }).ToList();
            return Result<List<PickupRequestDto>>.Success(pickupRequestDtos);
        }
    }
}
