﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecyclingDbContext _context;
        private IFactoryOrder _factoryOrders;
        private IWarehouse _warehouse;
        private IWarehouseInventory _warehouseInventory;
        private IEmployeeWarehouseHistory _employeeWarehouseHistory;
        private IMaterials _materials;
        private INotification _notification;
        private IPickupItem _pickupItem;
        private IPickupRequest _pickupRequest;
        private IPointsHistory _pointsHistory;
        private IRewardRedemptions _rewardRedemptions;
        private IRewards _rewards;
        private IApplicationUser _user;
        private IReport _report;
        IUserGift _userGift;
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

        #region employeeWarehouseHistory
        public IEmployeeWarehouseHistory employeeWarehouseHistory

        {
            get
            {
                if (_employeeWarehouseHistory is null)
                {
                    _employeeWarehouseHistory = new EmployeeWarehouseHistoryRepo(_context);
                }
                return _employeeWarehouseHistory;
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


        #region notification
        public INotification notification {
            get
            {
                if (_notification is null)
                {
                    _notification = new NotificationRepo(_context);
                }
                return _notification;
            }
        }
        #endregion

        #region pickupItem
        public IPickupItem pickupItem {
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

        #region pointsHistory
        public IPointsHistory pointsHistory
        {
            get
            {
                if (_pointsHistory is null)
                {
                    _pointsHistory = new PointsHistoryRepo(_context);

                }
                return _pointsHistory;
            }
        }
        #endregion

        #region rewardRedemptions
        public IRewardRedemptions rewardRedemptions
        {
            get
            {
                if (_rewardRedemptions is null)
                {
                    _rewardRedemptions = new RewardRedemptionsRepo(_context);

                }
                return _rewardRedemptions;
            }
        }

        #endregion

        #region rewards
        public IRewards rewards
        {
            get
            {
                if (_rewards is null)
                {
                    _rewards = new RewardsRepo(_context);

                }
                return _rewards;
            }
        }
        #endregion

        #region applicationUser
        public IApplicationUser applicationUser
        {
            get
            {
                if (_user is null)
                {
                    _user = new ApplicationUserRepo(_context);

                }
                return _user;
            }
        }
        #endregion



        #region Report
        public IReport report
        {
            get
            {
                if (_report is null)
                {
                    _report = new ReportRepo(_context);

                }
                return _report;
            }
        }

        #endregion
        public IUserGift userGift
        {
            get
            {
                if (_userGift is null)
                {
                    _userGift = new UserGiftRepo(_context);

                }
                return _userGift;
            }
        }
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




