using AutoMapper;
using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.WharehosingInventoryDTOs;
using RecyclingSystem.Application.Mapping;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.WarehousingInventory.Commands
{
    public class AddWharehosingInventoryCommand:IRequest<Result<AddWharehosingInventoryDto>>
    {
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
    }
    public class AddWharehosingInventoryCommandHandler : IRequestHandler<AddWharehosingInventoryCommand, Result<AddWharehosingInventoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddWharehosingInventoryCommandHandler(IUnitOfWork unitOfWork,IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<AddWharehosingInventoryDto>> Handle(AddWharehosingInventoryCommand request, CancellationToken cancellationToken)
        {
           // var WharehosingResult =
            //    var dto = new AddWharehosingInventoryDto
            //    {
            //        MaterialId = request.MaterialId,
            //        Quantity = request.Quantity,
            //        //Message = "Inventory added successfully"
            //    };
            //var result = _mapper.Map<dto>(WarehouseInventory);
            //await _unitOfWork.warehouseInventory.Add(result);
            throw new NotImplementedException();
        }
    }
}
