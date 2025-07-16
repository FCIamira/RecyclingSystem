using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class PickupItemRepo : GenericRepo<PickupItem, int>, IPickupItem
    {
        private readonly RecyclingDbContext context;

        public PickupItemRepo(RecyclingDbContext _context) : base(_context)
        {
            context = _context;
        }

        public Task<PickupItem?> GetByRequestIdAndMaterialId(int pickupRequestId, int materialId)
        {
            return Task.FromResult(context.PickupItems
                .FirstOrDefault(p => p.PickupRequestId == pickupRequestId && p.MaterialId == materialId));

        }
    }
}
