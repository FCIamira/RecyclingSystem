using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class PickupRequestRepo : GenericRepo<PickupRequest, int>, IPickupRequest
    {
        private readonly RecyclingDbContext context;
        private Logger<PickupRequestRepo> _logger;
        public PickupRequestRepo(RecyclingDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<List<PickupRequest>> GetAllDetails()
        {


            var pickupRequests = await context.PickupRequests
                .Include(p => p.Customer)
                .Include(p => p.PickupItems)
                    .ThenInclude(pi => pi.Material)
                .ToListAsync();

            return pickupRequests;
        }

      

        public async Task<PickupRequest?> GetByIdWithDetails(int id)
        {
            var pickupRequest = await context.PickupRequests
                .Include(p => p.Customer)
                .Include(p => p.Employee)
                .Include(p => p.PickupItems)
                    .ThenInclude(pi => pi.Material)
                .FirstOrDefaultAsync(p => p.Id == id);
            return pickupRequest;
        }


      
        public async Task<EmployeeWarehouseHistory> GetLatestWarehouseByEmployeeIdAsync(int employeeId)
        {
            var result =  await context.EmployeesHistories
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.AssignedDate)
                .FirstOrDefaultAsync();
            return result;
        }



    }

}
