using Microsoft.EntityFrameworkCore;
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
    public class ReportRepo:GenericRepo<Report,int>,IReport
    {

        private readonly RecyclingDbContext context;

        public ReportRepo(RecyclingDbContext _context) : base(_context) => context = _context;

        public Task<List<Report>> GetAllReportWithDetailsAsync()
        {
            return context.Reports
                .Include(r => r.Employee)
                .Include(r => r.PickupRequest)
                .ThenInclude(p => p.Customer)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                .ToListAsync();
        }

        public async Task<Report> GetReportById(int id)
        {
            return await context.Reports
                .Include(r => r.Employee)
                .Include(r => r.PickupRequest)
                .ThenInclude(p => p.Customer)
                .FirstOrDefaultAsync();
        }
    }
}
