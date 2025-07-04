using MediatR;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupItems.Commands
{
    public class CreatePickupItemsCommand : IRequest
    {
        public int PickupRequestId { get; set; }
        public List<CreatePickupItemDto> PickupItems { get; set; }
    }

    public class CreatePickupItemsCommandHandler : IRequestHandler<CreatePickupItemsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreatePickupItemsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task Handle(CreatePickupItemsCommand request, CancellationToken cancellationToken)
        {
            var pickupItems = request.PickupItems.Select(item => new Domain.Models.PickupItem
            {
                PickupRequestId = request.PickupRequestId,
                MaterialId = item.ItemId,
                Quantity = item.Quantity
            }).ToList();

            pickupItems.ForEach(item => _unitOfWork.pickupItem.Add(item));
            _unitOfWork.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}
