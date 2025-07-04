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
    public class GetMaterialByIdQuery : IRequest<Result<MaterialDto>>
    {
        public int Id { get; set; }
    }

    public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, Result<MaterialDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetMaterialByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<MaterialDto>> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
        {
            
            var material = await _unitOfWork.materials.GetById(request.Id);
            if (material == null)
            {
               return Result<MaterialDto>.Failure(ErrorCode.NotFound ,$"Material with ID {request.Id} not found.");
            }
            var materialDto = new MaterialDto
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                PointsPerUnit = material.PointsPerUnit
            };
            return Result<MaterialDto>.Success(materialDto);
        }
    }
}
