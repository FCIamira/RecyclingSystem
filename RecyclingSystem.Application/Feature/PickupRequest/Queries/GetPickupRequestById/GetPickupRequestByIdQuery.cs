using MediatR;
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
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries.GetPickupRequestById
{
    public class GetPickupRequestByIdQuery : IRequest<Result<PickupRequestDto>>
    {
        public int Id { get; set; }
    }

    public class GetPickupRequestByIdQueryHandler : IRequestHandler<GetPickupRequestByIdQuery, Result<PickupRequestDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPickupRequestByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PickupRequestDto>> Handle(GetPickupRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var pickupRequest = await _unitOfWork.pickupRequest.GetByIdWithDetails(request.Id);
            if (pickupRequest == null)
            {
                return Result<PickupRequestDto>.Failure(ErrorCode.NotFound, "Pickup request not found.");
            }

            if(pickupRequest.PickupItems == null)
            {
                return Result<PickupRequestDto>.Failure(ErrorCode.NotFound, "Pickup request has no items.");
            }

            var pickupRequestDto = new PickupRequestDto
            {
                Id = pickupRequest.Id,
                Employee = pickupRequest.Employee != null ? new UserInfoDto
                {
                    Id = pickupRequest.Employee.Id,   
                    FullName = pickupRequest.Employee.FullName,
                    Email = pickupRequest.Employee.Email,
                    Address = pickupRequest.Employee.Address,
                    PhoneNumber = pickupRequest.Employee.PhoneNumber,
                    ProfilePictureUrl = pickupRequest.Employee.ProfilePictureUrl,
                    CreatedAt = pickupRequest.Employee.CreatedAt,
                } : null,
                Customer = new UserInfoDto
                {
                    Id = pickupRequest.Customer.Id,
                    FullName = pickupRequest.Customer.FullName,
                    Email = pickupRequest.Customer.Email,
                    Address = pickupRequest.Customer.Address,
                    PhoneNumber = pickupRequest.Customer.PhoneNumber,
                    ProfilePictureUrl = pickupRequest.Customer.ProfilePictureUrl,
                    CreatedAt = pickupRequest.Customer.CreatedAt,
                },
                RequestedDate = pickupRequest.RequestedDate,
                ScheduledDate = pickupRequest.ScheduledDate,
                Address = pickupRequest.Address,
                LocationLat = pickupRequest.LocationLat,
                LocationLng = pickupRequest.LocationLng,
                Note = pickupRequest.Note,
                Status = pickupRequest.Status,
                DateCollected = pickupRequest.DateCollected,
                TotalPointsGiven = pickupRequest.TotalPointsGiven,
                PickupItems = pickupRequest.PickupItems.Select(pi => new PickupItemDto
                {
                    Id = pi.Id,
                    MaterialId = pi.MaterialId,
                    MaterialName = pi.Material.Name,
                    PlannedQuantity = pi.PlannedQuantity,
                    ActualQuantity = pi.ActualQuantity,
                }).ToList()
            };

            return Result<PickupRequestDto>.Success(pickupRequestDto, "Pickup request retrieved successfully.");
        }
    }

}
