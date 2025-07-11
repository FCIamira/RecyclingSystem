using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupItems.Commands
{
    public class CreatePickupItemsCommand : IRequest<Result<CreatePickupItemsResponse>>
    {
        public int PickupRequestId { get; set; }
        public List<CreatePickupItemDto> PickupItems { get; set; }
    }

    public class CreatePickupItemsResponse
    {
        
    }

    public class CreatePickupItemsCommandHandler : IRequestHandler<CreatePickupItemsCommand, Result<CreatePickupItemsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreatePickupItemsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreatePickupItemsResponse>> Handle(CreatePickupItemsCommand request, CancellationToken cancellationToken)
        {
            var pickupItems = request.PickupItems.Select(item => new Domain.Models.PickupItem
            {
                PickupRequestId = request.PickupRequestId,
                MaterialId = item.ItemId,
                PlannedQuantity = item.PlannedQuantity
            }).ToList();

            pickupItems.ForEach(item => _unitOfWork.pickupItem.Add(item));
            await _unitOfWork.SaveChangesAsync();

            return await Task.FromResult(Result<CreatePickupItemsResponse>.Success(new CreatePickupItemsResponse()));
        }
    }
}
