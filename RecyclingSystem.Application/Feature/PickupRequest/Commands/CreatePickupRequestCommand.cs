using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Commands
{
    public class CreatePickupRequestCommand : IRequest<Result<CreatePickupRequestCommandResponse>>
    {
        public int CustomerId { get; set; }
        public CreatePickupRequestDto CreatePickupRequestDto { get; set; }
    }

    public class CreatePickupRequestCommandResponse
    {
        public int PickupRequestId { get; set; }
    }

    public class CreatePickupRequestCommandHandler : IRequestHandler<CreatePickupRequestCommand, Result<CreatePickupRequestCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllPickupRequestsQueryHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreatePickupRequestCommandHandler(IUnitOfWork unitOfWork,
            ILogger<GetAllPickupRequestsQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<CreatePickupRequestCommandResponse>> Handle(CreatePickupRequestCommand request, CancellationToken cancellationToken)
        {
            // Implement the logic to handle the command here
            // For now, returning a success result with an empty response

            var pickupRequestDto = request.CreatePickupRequestDto;
            if (pickupRequestDto == null)
            {
                return Result<CreatePickupRequestCommandResponse>.Failure(ErrorCode.BadRequest, "Pickup request data is null.");
            }

            
            //string? CustomerIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //int? CustomerId = int.TryParse(CustomerIdString, out int customerId) ? customerId : null;

            var pickupRequestObject = new Domain.Models.PickupRequest
            {
                CustomerId = request.CustomerId,
                Address = pickupRequestDto.Address,
                LocationLat = pickupRequestDto.Latitude,
                LocationLng = pickupRequestDto.Longitude,
                ScheduledDate = pickupRequestDto.PreferredDate,
                RequestedDate = DateTime.Now,
                Note = pickupRequestDto.Note,
                Status = PickupStatus.Pending,
                //PickupItems = pickupRequestDto.PickupItems.Select(item => new PickupItem
                //{
                //    MaterialId = item.ItemId,
                //    Quantity = item.Quantity
                //}).ToList()
            };

            await _unitOfWork.pickupRequest.Add(pickupRequestObject);

            await _unitOfWork.SaveChangesAsync();

            var pickupRequestId = pickupRequestObject.Id;

            if (pickupRequestId <= 0)
            {
                return Result<CreatePickupRequestCommandResponse>.Failure(ErrorCode.ServerError, "Failed to create pickup request.");
            }

            return Result<CreatePickupRequestCommandResponse>.Success(new CreatePickupRequestCommandResponse{ PickupRequestId = pickupRequestId}, "Pickup request created successfully.");
        }
    }

}
