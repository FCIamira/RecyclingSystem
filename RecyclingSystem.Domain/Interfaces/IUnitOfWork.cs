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
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
