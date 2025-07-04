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
    internal class RewardsRepo:GenericRepo<Rewards,int>,IRewards
    {
        private readonly RecyclingDbContext context;

        public RewardsRepo(RecyclingDbContext _context) : base(_context) => context = _context;


    }
}
