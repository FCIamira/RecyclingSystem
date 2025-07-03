using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Interfaces
{
    public interface INotification : IGenericRepo<Notification, int>
    {
        Task<List<Notification>> GetNotifications(Expression<Func<Notification, bool>> filter);
    }
}
