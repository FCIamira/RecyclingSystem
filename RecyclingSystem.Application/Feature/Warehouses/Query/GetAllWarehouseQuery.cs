using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.NotificationsDTOs;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Application.DTOs.WarehouseDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Warehouses.Query
{
    public class GetAllWarehouseQuery : IRequest<Result<List<GetAllWarehouseDto>>>
    {
    }

    public class GetAllWarehouseQueryHandler : IRequestHandler<GetAllWarehouseQuery, Result<List<GetAllWarehouseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllWarehouseQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetAllWarehouseQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllWarehouseQueryHandler> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllWarehouseDto>>> Handle(GetAllWarehouseQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all warehouses");
            try
            {
                var warehouses = await _unitOfWork.warehouse.GetAll();

                if (warehouses == null)
                {
                    _logger.LogWarning("Not found warehouses.");
                    return Result<List<GetAllWarehouseDto>>.Failure(ErrorCode.NotFound, "Not found warehouses.");
                }

                var warehouseDto = warehouses.Select(w => new GetAllWarehouseDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Location = w.Location,
                    ManagerId = w.ManagerId,
                }).ToList();
                return Result<List<GetAllWarehouseDto>>.Success(warehouseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving warehouses for user.");
                return Result<List<GetAllWarehouseDto>>.Failure(ErrorCode.ServerError, "An unexpected error occurred while processing your request.");
            }
        }
    }
}
