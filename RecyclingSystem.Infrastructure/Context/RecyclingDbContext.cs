using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Domain.Common;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Infrastructure.Context
{
    public class RecyclingDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<ApplicationUser> Users {  get; set; }
        public DbSet<EmployeeWarehouseHistory> EmployeesHistories { get; set; }
        public DbSet<FactoryOrders> FactoryOrders { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PickupItem> PickupItems { get; set; }
        public DbSet<PickupRequest> PickupRequests { get; set; }
        public DbSet<PointsHistory> PointsHistories { get; set; }
        public DbSet<RewardRedemptions> RewardRedemptions { get; set; }
        public DbSet<Rewards> Rewards { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseInventory> WarehouseInventories { get; set; }

        public RecyclingDbContext() { }
        public RecyclingDbContext(DbContextOptions<RecyclingDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<int>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseModel<>).IsAssignableFrom(entityType.ClrType.BaseType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("IsDeleted")
                        .HasDefaultValue(false);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("DateCreated")
                        .HasDefaultValueSql("GETUTCDATE()");
                }
            }
        }

    }
}
