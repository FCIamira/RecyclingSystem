using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Material.Commands
{
    public class CreateMaterialCommand : IRequest<Result<CreateMaterialResponse>>
    {
        public CreateMaterialDto CreateMaterialDto { get; set; }
    }


    public class CreateMaterialResponse
    {
        public int Id { get; set; }
    }

    public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, Result<CreateMaterialResponse>> 
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateMaterialCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreateMaterialResponse>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            // Logic to create a material

            var newMaterial = new Domain.Models.Material
            {
                Name = request.CreateMaterialDto.Name,
                Description = request.CreateMaterialDto.Description,
                UnitType = request.CreateMaterialDto.UnitType,
                PointsPerUnit = request.CreateMaterialDto.PointsPerUnit
            };

            await _unitOfWork.materials.Add(newMaterial);

            await _unitOfWork.SaveChangesAsync();

            var response = new CreateMaterialResponse
            {
                Id = newMaterial.Id
            };

            return await Task.FromResult(Result<CreateMaterialResponse>.Success(response, "Material created successfully."));
        }
    }
}
