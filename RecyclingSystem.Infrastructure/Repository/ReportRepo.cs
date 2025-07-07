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

    }
}
