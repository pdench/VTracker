using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using VTracker.Models;

namespace VTracker.DAL
{
    public class TrackerContext : DbContext
    {
        public TrackerContext() : base("VTrackerConn")
        {
            Database.SetInitializer<TrackerContext>(null);
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BuiltInCategories> BuiltInCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }
}