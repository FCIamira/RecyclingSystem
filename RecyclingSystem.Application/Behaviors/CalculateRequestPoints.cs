using MediatR;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Application.Feature.Material.Queries;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Behaviors
{
    public class CalculateRequestPoints
    {
        public static async Task<int> CalculateAsync(ICollection<PickupItemDto> pickupItems, IMediator _mediator)
        {
            int totalPoints = 0;
            foreach (var item in pickupItems)
            {
                var material = await _mediator.Send(new GetMaterialByIdQuery { Id = item.MaterialId });
                if (material != null)
                {
                    totalPoints += material.Data.PointsPerUnit * (item.ActualQuantity ?? 0);
                }
            }
            return totalPoints;
        }

    }
}
