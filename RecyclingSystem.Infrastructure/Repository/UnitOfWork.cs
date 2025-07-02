using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Domain.Interfaces;

using RecyclingSystem.Infrastructure.Context;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecyclingDbContext _context;
        private IFactoryOrder _factoryOrders;
        private IWarehouse _warehouse;
        private IWarehouseInventory _warehouseInventory;
        public UnitOfWork(RecyclingDbContext applicationDBContext)
        {
            _context = applicationDBContext;
        }


        #region factoryOrders
        public IFactoryOrder factoryOrders
        {
            get
            {
                if (_factoryOrders is null)
                {
                    _factoryOrders = new FactoryOrdersRepo(_context);
                }
                return _factoryOrders;
            }
        }
        #endregion

        #region warehouse
        public IWarehouse warehouse
        {
            get
            {
                if (_warehouse is null)
                {
                    _warehouse = new WarehouseRepo(_context);

                }
                return _warehouse;
            }
        }
        #endregion


        #region warehouseInventory
        public IWarehouseInventory warehouseInventory
        {
            get
            {
                if (_warehouseInventory is null)
                {
                    _warehouseInventory = new WarehouseInventoryRepo(_context);

                }
                return _warehouseInventory;
            }
        }

        public IEmployeeWarehouseHistory employeeWarehouseHistory => throw new NotImplementedException();

        public IMaterials materials => throw new NotImplementedException();

        public INotification notification => throw new NotImplementedException();

        public IPickupItem pickupItem => throw new NotImplementedException();

        public IPickupRequest pickupRequest => throw new NotImplementedException();

        public IPointsHistory pointsHistory => throw new NotImplementedException();

        public IRewardRedemptions rewardRedemptions => throw new NotImplementedException();

        public IRewards rewards => throw new NotImplementedException();
        #endregion
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}




