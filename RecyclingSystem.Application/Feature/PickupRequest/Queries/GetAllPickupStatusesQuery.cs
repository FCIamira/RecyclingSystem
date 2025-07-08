using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.PickupRequest.Queries
{
    public class GetAllPickupStatusesQuery : IRequest<Result<List<PickupStatusDto>>> { }

    public class PickupStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GetAllPickupStatusesQueryHandler : IRequestHandler<GetAllPickupStatusesQuery, Result<List<PickupStatusDto>>>
    {
        public Task<Result<List<PickupStatusDto>>> Handle(GetAllPickupStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = Enum.GetValues(typeof(PickupStatus))
                               .Cast<PickupStatus>()
                               .Select(status => new PickupStatusDto
                               {
                                   Id = (int)status,
                                   Name = status.ToString()
                               })
                               .ToList();

            return Task.FromResult(Result<List<PickupStatusDto>>.Success(statuses));
        }
    }
}
