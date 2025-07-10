using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.DTOs.WharehosingInventoryDTOs;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequestsForEmployee;
using RecyclingSystem.Application.Mapping;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.WarehousingInventory.Commands
{
    public class AddWharehosingInventoryCommand : IRequest<Result<AddWharehosingInventoryDto>>
    {
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
    }
    public class AddWharehosingInventoryCommandHandler : IRequestHandler<AddWharehosingInventoryCommand, Result<AddWharehosingInventoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddWharehosingInventoryCommandHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public AddWharehosingInventoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddWharehosingInventoryCommandHandler> logger,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        public async Task<Result<AddWharehosingInventoryDto>> Handle(AddWharehosingInventoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all pickup requests for the current user.");
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HttpContext is null");
                    return Result<AddWharehosingInventoryDto>.Failure(ErrorCode.Unauthorized, "No HttpContext.");
                }

                var employee = await _userManager.GetUserAsync(httpContext.User);
                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    _logger.LogError("User is not authenticated.");
                    return Result<AddWharehosingInventoryDto>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");
                }

                var employeeWarehouseHistory = await _unitOfWork.pickupRequest.GetLatestWarehouseByEmployeeIdAsync(employee.Id);
                if (employeeWarehouseHistory == null)
                {
                    return Result<AddWharehosingInventoryDto>.Failure(ErrorCode.NotFound, "Employee warehouse not found.");
                }

                var warehouseID = employeeWarehouseHistory.WarehouseId;

                var existingInventory = await _unitOfWork.warehouseInventory
                    .GetAllWithFilter(w => w.MaterialId == request.MaterialId && w.WarehouseId == warehouseID)
                    .FirstOrDefaultAsync();

                WarehouseInventory inventory;
                if (existingInventory != null)
                {
                    existingInventory.Quantity += request.Quantity;
                    existingInventory.LastUpdated = DateTime.UtcNow;

                    _unitOfWork.warehouseInventory.Update(existingInventory.Id, existingInventory);
                    inventory = existingInventory;
                }
                else
                {
                    var newInventory = new WarehouseInventory
                    {
                        MaterialId = request.MaterialId,
                        Quantity = request.Quantity,
                        WarehouseId = warehouseID,
                        LastUpdated = DateTime.UtcNow
                    };

                    await _unitOfWork.warehouseInventory.Add(newInventory);
                    inventory = newInventory;
                }

                // ✅ Step: Update PickupRequest Status
                var pickupRequest = await _unitOfWork.pickupRequest.GetAllWithFilter(
                    p => p.EmployeeId == employee.Id &&
                         p.PickupItems.Any(i => i.MaterialId == request.MaterialId) &&
                         p.Status != PickupStatus.Collected
                ).FirstOrDefaultAsync();

                if (pickupRequest != null)
                {
                    pickupRequest.Status = PickupStatus.Collected;
                    pickupRequest.DateCollected = DateTime.UtcNow;
                    _unitOfWork.pickupRequest.Update(pickupRequest.Id, pickupRequest);
                }

                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<AddWharehosingInventoryDto>(inventory);
                return Result<AddWharehosingInventoryDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving pickup requests for user.");
                return Result<AddWharehosingInventoryDto>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }

    }
}
