using AutoMapper;
using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Application.DTOs.WharehosingInventoryDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.WarehousingInventory.query
{
    public class GetAllWarehouseInventoryQuery:IRequest<Result<List<WarehouseInventoryDataDTO>>>
    {
    }

    public class GetAllWarehouseInventoryQueryHandler : IRequestHandler<GetAllWarehouseInventoryQuery, Result<List<WarehouseInventoryDataDTO>>>
    {
        public IUnitOfWork unitOfWork { get; }
        public IMapper Mapper { get; }

        public GetAllWarehouseInventoryQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            Mapper = mapper;
        }

      public async  Task<Result<List<WarehouseInventoryDataDTO>>>Handle(GetAllWarehouseInventoryQuery request, CancellationToken cancellationToken)
        {
            var warehouseInventory = await unitOfWork.warehouseInventory.GetAll();

            var dtos = Mapper.Map<List<WarehouseInventoryDataDTO>>(warehouseInventory);


            if (dtos == null)
            {
                return Result<List<WarehouseInventoryDataDTO>>.Failure(ErrorCode.NotFound, "No warehouseInventory found ");
            }

            return Result<List<WarehouseInventoryDataDTO>>.Success(dtos);
        }
    }
    }
