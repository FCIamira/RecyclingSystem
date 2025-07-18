using RecyclingSystem.Application.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Services
{
    public class EmployeeAvailabilityService:IEmployeeAvailabilityService
    {
        public bool HasTimeConflict(IEnumerable<PickupRequest> requests, DateTime targetDateTime)
        {
            TimeSpan margin = TimeSpan.FromMinutes(30);

            return requests.Any(r =>
                r.ScheduledDate.HasValue &&
                Math.Abs((r.ScheduledDate.Value - targetDateTime).TotalMinutes) <= margin.TotalMinutes);
        }


        public int CountInSameDay(IEnumerable<PickupRequest> requests, DateTime targetDateTime)
        {
            return requests.Count(r =>
                r.ScheduledDate.HasValue &&
                r.ScheduledDate.Value.Date == targetDateTime.Date);
        }


        public int CountInSameWeek(IEnumerable<PickupRequest> requests, DateTime targetDateTime)
        {
            var calendar = CultureInfo.CurrentCulture.Calendar;
            var weekRule = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            int targetWeek = calendar.GetWeekOfYear(targetDateTime, weekRule, firstDayOfWeek);

            return requests.Count(r =>
                r.ScheduledDate.HasValue &&
                calendar.GetWeekOfYear(r.ScheduledDate.Value, weekRule, firstDayOfWeek) == targetWeek &&
                r.ScheduledDate.Value.Year == targetDateTime.Year);
        }


        public DateTime? FindClosestAvailableSlot(IEnumerable<PickupRequest> scheduledRequests, DateTime targetDate)
        {
            const int maxWeeklyRequests = 10;
            const int maxDailyRequests = 2;

            // نبدأ من اليوم اللي العميل طلب فيه
            DateTime startDate = targetDate;
            DateTime endDate = targetDate.AddDays(14); // نبحث لمدة أسبوعين قادمين كحد أقصى

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                int weekCount = CountInSameWeek(scheduledRequests, date);
                int dayCount = CountInSameDay(scheduledRequests, date);
                bool hasConflict = HasTimeConflict(scheduledRequests, date);

                if (weekCount < maxWeeklyRequests && dayCount < maxDailyRequests && !hasConflict)
                {
                    return date; // أول وقت متاح
                }
            }

            // مفيش وقت مناسب في الأسبوعين الجايين
            return null;
        }

    }
}
