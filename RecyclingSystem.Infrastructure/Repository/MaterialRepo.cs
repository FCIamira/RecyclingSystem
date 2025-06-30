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
    public class MaterialRepo : GenericRepo<Material, int>, IMaterials
    {
        private readonly RecyclingContext context;

        public MaterialRepo(RecyclingContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
