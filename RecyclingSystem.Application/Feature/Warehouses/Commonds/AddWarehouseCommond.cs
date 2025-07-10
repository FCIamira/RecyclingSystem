using MediatR;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Warehouses.Commonds
{
    public class AddWarehouseCommond : IRequest<string>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int ManagerId { get; set; }
    }

    public class AddWarehouseCommondHandler : IRequestHandler<AddWarehouseCommond, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddWarehouseCommondHandler> _logger;
        public AddWarehouseCommondHandler(IUnitOfWork unitOfWork, ILogger<AddWarehouseCommondHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<string> Handle(AddWarehouseCommond request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Add new warehouse by (Admin)");
            try
            {
                var manager = await _unitOfWork.applicationUser.GetById(request.ManagerId);
                if(manager == null)
                {
                    _logger.LogWarning("Manager with ID not found");
                    return "Manager not found";
                }

                var warehouse = new Warehouse
                {
                    Name = request.Name,
                    Location = request.Location,
                    ManagerId = request.ManagerId
                };

                await _unitOfWork.warehouse.Add(warehouse);
                await _unitOfWork.SaveChangesAsync();

                return "Warehouse added successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a warehouse.");
                return $"Error: {ex.Message}";
            }
        }
    }
}
