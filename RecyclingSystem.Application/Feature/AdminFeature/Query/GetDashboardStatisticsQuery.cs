using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Application.Mapping;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.AdminFeature.Query
{
    public class GetDashboardStatisticsQuery : IRequest<Result<DashboardStatisticsResponse>>
    {

    }

    public class DashboardStatisticsResponse
    {
        public int TotalCustomers { get; set; }
        public int AllTimePointsGiven { get; set; }
        public int TodayPickups { get; set; }
        public decimal TotalWeightCollected { get; set; }
        public List<MaterialBreakdownDto> materialBreakdown { get; set; } = new List<MaterialBreakdownDto>();
        public List<DailyCount> WeeklyPickups { get; set; } = new List<DailyCount>();
    }

    public class DailyCount
    {
        public string Day { get; set; }
        public int Count { get; set; }
    }

    public class GetDashboardStatisticsQueryHandler : IRequestHandler<GetDashboardStatisticsQuery, Result<DashboardStatisticsResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetDashboardStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<DashboardStatisticsResponse>> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
        {
            var totalCustomers = unitOfWork.applicationUser.GetAllWithFilter(u => u.Role == "Customer").Count();

            var totalPointsGiven = unitOfWork.pickupRequest.GetAllWithFilter(r => r.Status == Domain.Enums.PickupStatus.Collected).Sum(r => r.TotalPointsGiven ?? 0);

            var todayPickups = unitOfWork.pickupRequest.GetAllWithFilter(r =>  r.DateCollected == DateTime.UtcNow.Date && r.Status == Domain.Enums.PickupStatus.Collected).Count();

            var allRequests = unitOfWork.pickupRequest.GetAllWithFilter(r => r.Status == Domain.Enums.PickupStatus.Collected).ToList();
            var requestIds = allRequests.Select(r => r.Id).ToList();

            var allPickupItems = unitOfWork.pickupItem
                .GetAllWithFilter(pi => requestIds.Contains(pi.PickupRequestId))
                .ToList();
            var allMaterials = await unitOfWork.materials.GetAll();

            var totalPickupItems = allPickupItems.Sum(pi => pi.ActualQuantity);

            var materialsBreakdown = new List<MaterialBreakdownDto>();
            foreach (var item in allMaterials)
            {
                var materialPrecentage = totalPickupItems > 0 ? (allPickupItems.Where(pi => pi.MaterialId == item.Id).Sum(pi => pi.ActualQuantity) / (decimal)totalPickupItems) * 100 : 0;
                materialsBreakdown.Add(new MaterialBreakdownDto
                {
                    MaterialName = item.Name,
                    precentage = Math.Round(materialPrecentage, 1),
                });
            }
            var totalWeightCollected = totalPickupItems * 0.1;

            // calculate the weekly pickups on each day of the week
            var weeklyPickups = new List<DailyCount>();
            foreach (var item in allRequests)
            {
                var dayOfWeek = item.DateCollected?.DayOfWeek.ToString() ?? "Unknown";
                var existingEntry = weeklyPickups.FirstOrDefault(w => w.Day == dayOfWeek);
                if (existingEntry != null)
                {
                    existingEntry.Count++;
                }
                else
                {
                    weeklyPickups.Add(new DailyCount { Day = dayOfWeek, Count = 1 });
                }
            }

            // Logic to fetch dashboard statistics goes here
            var response = new DashboardStatisticsResponse 
            {
                TotalCustomers = totalCustomers,
                AllTimePointsGiven = totalPointsGiven,
                TodayPickups = todayPickups,
                TotalWeightCollected = (decimal)totalWeightCollected,
                materialBreakdown = materialsBreakdown,
                WeeklyPickups = weeklyPickups,
            };
            return await Task.FromResult(Result<DashboardStatisticsResponse>.Success(response));
        }
    }
}
