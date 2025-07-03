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
        private IPickupItem _pickupItem;
        private IPickupRequest _pickupRequest;
        private IMaterials _materials;
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
        #endregion


        #region pickupItem
        public IPickupItem pickupItem
        {
            get
            {
                if (_pickupItem is null)
                {
                    _pickupItem = new PickupItemRepo(_context);
                }
                return _pickupItem;
            }
        }
        #endregion


        #region pickupRequest
        public IPickupRequest pickupRequest
            {
            get
            {
                if (_pickupRequest is null)
                {
                    _pickupRequest = new PickupRequestRepo(_context);
                }
                return _pickupRequest;
            }
        }
        #endregion

        #region materials
        public IMaterials materials
        {
            get
            {
                if (_materials is null)
                {
                    _materials = new MaterialRepo(_context);
                }
                return _materials;
            }
        }
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




