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
    public class EmployeeWarehouseHistoryRepo : GenericRepo<EmployeeWarehouseHistory, int>, IEmployeeWarehouseHistory
    {
        private readonly RecyclingDbContext context;

        public EmployeeWarehouseHistoryRepo(RecyclingDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}

