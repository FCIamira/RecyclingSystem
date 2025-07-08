using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Application.Feature.PickupItems.Commands;
using RecyclingSystem.Application.Feature.PickupRequest.Commands;
using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Orchestrator
{
    public class CreatePickupRequestWithPickupItemsOrchestrator : IRequest<Result<CreatePickupRequestWithPickupItemsResponse>>
    {
        public CreatePickupRequestDto CreatePickupRequestDto { get; set; }
    }

    public class CreatePickupRequestWithPickupItemsResponse
    {
        public int PickupRequestId { get; set; }
    }

    public class CreatePickupRequestWithPickupItemsHandler : IRequestHandler<CreatePickupRequestWithPickupItemsOrchestrator, Result<CreatePickupRequestWithPickupItemsResponse>>
    {

        private readonly IMediator _mediator;
        private readonly ILogger<CreatePickupRequestWithPickupItemsHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreatePickupRequestWithPickupItemsHandler(IMediator mediator,
            ILogger<CreatePickupRequestWithPickupItemsHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<Result<CreatePickupRequestWithPickupItemsResponse>> Handle(CreatePickupRequestWithPickupItemsOrchestrator request, CancellationToken cancellationToken)
        {
            string? CustomerIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int? CustomerId = int.TryParse(CustomerIdString, out int customerId) ? customerId : null;

            if (CustomerId == null)
            {
                _logger.LogError("Customer ID is null. Cannot create pickup request.");
                throw new InvalidOperationException($"Customer ID {CustomerIdString} is required to create a pickup request.");
            }

            var newRequestId = await _mediator.Send(new CreatePickupRequestCommand { CreatePickupRequestDto = request.CreatePickupRequestDto, CustomerId = CustomerId ?? 0 }, cancellationToken);

            var result = await _mediator.Send(new CreatePickupItemsCommand
            {
                PickupRequestId = newRequestId.Data.PickupRequestId,
                PickupItems = request.CreatePickupRequestDto.PickupItems.Select(item => new CreatePickupItemDto
                {
                    ItemId = item.ItemId,
                    PlannedQuantity = item.PlannedQuantity
                }).ToList()
            }, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create pickup items for request ID {PickupRequestId}. Error: {ErrorMessage}", newRequestId.Data.PickupRequestId, result.Errorcode);
                return Result<CreatePickupRequestWithPickupItemsResponse>.Failure(result.Errorcode, result.Message);
            }

            return Result<CreatePickupRequestWithPickupItemsResponse>.Success(new CreatePickupRequestWithPickupItemsResponse
            {
                PickupRequestId = newRequestId.Data.PickupRequestId,
            }, "Pickup request created successfully.");
        }
    }
}
