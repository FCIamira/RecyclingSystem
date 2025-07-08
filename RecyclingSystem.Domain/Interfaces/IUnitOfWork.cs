using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IFactoryOrder factoryOrders { get; }
        IWarehouse warehouse { get; }
        IWarehouseInventory warehouseInventory { get; }
        IEmployeeWarehouseHistory employeeWarehouseHistory { get; }
        IMaterials materials { get; }
        INotification notification { get; }
        IPickupItem pickupItem { get; }
        IPickupRequest pickupRequest { get; }
        IPointsHistory pointsHistory { get; }
        IRewardRedemptions rewardRedemptions { get; }
        IRewards rewards { get; }
        IUserGift userGift { get; }
        IApplicationUser applicationUser { get; }
        IReport report { get; }
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
