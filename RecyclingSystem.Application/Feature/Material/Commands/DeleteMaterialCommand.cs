using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Material.Commands
{
    public class DeleteMaterialCommand : IRequest<Result<DeleteMaterialResponse>>
    {
        public int Id { get; set; }
    }

    public class DeleteMaterialResponse
    {
        public int Id { get; set; }
    }

    public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, Result<DeleteMaterialResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteMaterialCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<DeleteMaterialResponse>> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await _unitOfWork.materials.GetById(request.Id);

            if (material == null)
            {
                return Result<DeleteMaterialResponse>.Failure(ErrorCode.NotFound, "Material not found.");
            }

            await _unitOfWork.materials.Remove(request.Id);
            await _unitOfWork.SaveChangesAsync();

            return Result<DeleteMaterialResponse>.Success(new DeleteMaterialResponse { Id = request.Id }, "Material deleted successfully.");
        }
    }
    
}
