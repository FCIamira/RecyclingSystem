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
            //var pickupRequests = await context.PickupRequests
            //    .Include(p => p.PickupItems.Where(pi => pi.MaterialId != null))
            //        .ThenInclude(pi => pi.Material)
            //    .ToListAsync();

            var pickupRequests = await context.PickupRequests
                .Include(p => p.Customer)
                .Include(p => p.PickupItems)
                    .ThenInclude(pi => pi.Material)
                .ToListAsync();

            foreach (var request in pickupRequests)
            {
                foreach (var item in request.PickupItems)
                {
                    if (item.Material == null)
                        Console.WriteLine($"PickupItem {item.Id} has NULL Material");

                    if (item.Quantity == 0)
                        Console.WriteLine($"PickupItem {item.Id} has Quantity 0");
                }
            }

            return pickupRequests;


        }

    }
}
