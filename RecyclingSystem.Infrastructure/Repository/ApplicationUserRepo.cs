using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class ApplicationUserRepo : IdentityRepository<ApplicationUser>, IApplicationUser
    {
        private readonly RecyclingDbContext _context;
        public ApplicationUserRepo(RecyclingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetEmployeeWithFilter(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _context.Users
                .Where(filter)
                .ToListAsync();
        }

    }
}
