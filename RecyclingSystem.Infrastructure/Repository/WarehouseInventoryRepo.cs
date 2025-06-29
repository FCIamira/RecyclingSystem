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
    public class WarehouseInventoryRepo:GenericRepo<WarehouseInventory, int>, IWarehouseInventory
    {
        private readonly RecyclingContext context;

    public WarehouseInventoryRepo(RecyclingContext _context) : base(_context)
    {
        context = _context;
    }
    
    }
}
