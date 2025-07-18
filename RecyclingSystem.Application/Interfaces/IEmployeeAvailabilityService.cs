using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Interfaces
{
    public interface IEmployeeAvailabilityService
    {
        bool HasTimeConflict(IEnumerable<PickupRequest> requests, DateTime targetDateTime);
        int CountInSameWeek(IEnumerable<PickupRequest> requests, DateTime targetDateTime);
        int CountInSameDay(IEnumerable<PickupRequest> requests, DateTime targetDateTime);
        DateTime? FindClosestAvailableSlot(IEnumerable<PickupRequest> scheduledRequests, DateTime targetDate);

    }
}
