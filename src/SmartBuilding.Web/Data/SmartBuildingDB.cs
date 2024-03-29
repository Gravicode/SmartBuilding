﻿using Microsoft.EntityFrameworkCore;
using SmartBuilding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBuilding.Web.Data;

namespace SmartBuilding.Web.Data
{
    public class SmartBuildingDB : DbContext
    {

        public SmartBuildingDB()
        {
        }

        public SmartBuildingDB(DbContextOptions<SmartBuildingDB> options)
            : base(options)
        {
        }
        public DbSet<AccessBuilding> AccessBuildings { get; set; }
        public DbSet<Face> Faces { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<StreamRawData> StreamRawDatas { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);

            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");
            */
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            /*
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();
            */
            return base.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(AppConstants.SQLConn, ServerVersion.AutoDetect(AppConstants.SQLConn));
            }
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            /*
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
            */
        }

    }
}
