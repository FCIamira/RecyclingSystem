using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Material.Queries
{
    public class GetAllMaterialsQuery : IRequest<Result<GetAllMaterialResponse>>
    {

    }

    public class GetAllMaterialResponse
    {
        public List<MaterialDto> Materials { get; set; } = new List<MaterialDto>();
    }

    public class GetAllMaterialsQueryHandler : IRequestHandler<GetAllMaterialsQuery, Result<GetAllMaterialResponse>>
    {
        // Assuming you have a repository or service to fetch materials
        private readonly IUnitOfWork _unitOfWork;
        public GetAllMaterialsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllMaterialResponse>> Handle(GetAllMaterialsQuery request, CancellationToken cancellationToken)
        {
            var materials = await _unitOfWork.materials.GetAllMaterialsAsync();
            if (materials == null || !materials.Any())
            {
                return Result<GetAllMaterialResponse>.Failure(ErrorCode.NotFound, "No materials found.");
            }
            var response = new GetAllMaterialResponse
            {
                Materials = materials.Select(m => new MaterialDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    PointsPerUnit = m.PointsPerUnit,
                    UnitType = m.UnitType
                }).ToList()
            };
            return Result<GetAllMaterialResponse>.Success(response);
        }
    }

}
