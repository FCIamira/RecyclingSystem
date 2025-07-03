using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class NotificationRepo : GenericRepo<Notification, int>, INotification
    {
        private readonly RecyclingDbContext _context;
        public NotificationRepo(RecyclingDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Notification>> GetNotifications(Expression<Func<Notification, bool>> filter)
        {
            var notifications = _context.Notifications
                .Where(filter)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return notifications;
        }
    }
    
}
