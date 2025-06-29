﻿using System;
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
        private readonly RecyclingContext _context;
        private IFactoryOrder _factoryOrders;
        private IWarehouse _warehouse;
        private IWarehouseInventory _warehouseInventory;
        public UnitOfWork(RecyclingContext applicationDBContext)
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




