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

namespace RecyclingSystem.Application.Feature.Material.Commands
{
    public class UpdateMaterialCommand : IRequest<Result<MaterialDto>>
    {
        public MaterialDto MaterialDto { get; set; }
    }

    public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, Result<MaterialDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateMaterialCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MaterialDto>> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
        {
            
            var material = await _unitOfWork.materials.GetById(request.MaterialDto.Id);
            if (material == null)
            {
                return Result<MaterialDto>.Failure(ErrorCode.NotFound, "Material not found.");
            }

            material.Name = request.MaterialDto.Name;
            material.Description = request.MaterialDto.Description;
            material.PointsPerUnit = request.MaterialDto.PointsPerUnit;
            material.UnitType = request.MaterialDto.UnitType;
            material.IsActive = request.MaterialDto.IsActive;

            await _unitOfWork.materials.Update(material.Id, material);
            await _unitOfWork.SaveChangesAsync();

            return Result<MaterialDto>.Success(new MaterialDto
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                PointsPerUnit = material.PointsPerUnit,
                UnitType = material.UnitType,
                IsActive = material.IsActive,
            }, $"The Material {material.Id} has been updated");
        }
    }
}
