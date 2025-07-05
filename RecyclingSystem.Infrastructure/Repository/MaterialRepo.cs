using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class MaterialRepo : GenericRepo<Material, int>, IMaterials
    {
        private readonly RecyclingDbContext context;

        public MaterialRepo(RecyclingDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<IQueryable<Material>> GetAllMaterialsAsync()
        {
            return await Task.FromResult(context.Materials.Where(m => !m.IsDeleted).AsQueryable());
        }
    }
}
