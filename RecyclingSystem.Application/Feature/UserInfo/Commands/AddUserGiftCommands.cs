using MediatR;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;

public class AddUserGiftCommand : IRequest<Result<UserGift>>
{
    public int UserId { get; set; }
    public int GiftsEarned { get; set; }
    public int PointsPerGift { get; set; } 
    public int PointsThreshold { get; set; } 
}

public class AddUserGiftCommandHandler : IRequestHandler<AddUserGiftCommand, Result<UserGift>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddUserGiftCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserGift>> Handle(AddUserGiftCommand request, CancellationToken cancellationToken)
    {
        var userGift = await _unitOfWork.userGift.GetById(request.UserId);

        if (userGift == null)
        {
            userGift = new UserGift
            {
                UserId = request.UserId,
                GiftCount = request.GiftsEarned,
                PointsPerGift = request.PointsPerGift,
                LastUpdated = DateTime.UtcNow
            };
            await _unitOfWork.userGift.Add(userGift);
        }
        else
        {
            userGift.GiftCount += request.GiftsEarned;
            userGift.LastUpdated = DateTime.UtcNow;
            _unitOfWork.userGift.Update(userGift.Id, userGift);
        }

        return Result<UserGift>.Success(userGift);
    }
}
