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
    public class UserGiftRepo : GenericRepo<UserGift, int>, IUserGift
    {
        public UserGiftRepo(RecyclingDbContext _context) : base(_context)
        {
        }
    }
}
