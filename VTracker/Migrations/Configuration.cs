namespace VTracker.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<VTracker.DAL.TrackerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VTracker.DAL.TrackerContext context)
        {
            //  This method will be called after migrating to the latest version.

            var vehicles = new List<Vehicle>
            {
                new Vehicle { VehicleName="Chevy", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Vehicle { VehicleName="Mini", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Vehicle { VehicleName="Honda", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Vehicle { VehicleName="Toyota", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Vehicle { VehicleName="Ford", AccountId="ae36944e-16c2-491e-bc35-e487a3c6500d", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Vehicle { VehicleName="Harley", AccountId="ae36944e-16c2-491e-bc35-e487a3c6500d", DateCreated=DateTime.Today, DateUpdated=DateTime.Today }
            };
            vehicles.ForEach(v => context.Vehicles.Add(v));
            context.SaveChanges();

            var categories = new List<Category>
            {
                new Category {Description="Oil Change", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today, BuiltIn = true },
                new Category {Description="Gas", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today, BuiltIn = true },
                new Category {Description="New Tires", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today, BuiltIn = true },
                new Category {Description="Brakes", AccountId="ae36944e-16c2-491e-bc35-e487a3c6500d", DateCreated=DateTime.Today, DateUpdated=DateTime.Today, BuiltIn = true },
                new Category {Description="Wiper Blades", AccountId="3ca76a1a-28ff-408e-9298-863e668bba81", DateCreated=DateTime.Today, DateUpdated=DateTime.Today, BuiltIn = true },
                new Category {Description="Car Wash", AccountId="ae36944e-16c2-491e-bc35-e487a3c6500d", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Category {Description="Air Freshener", AccountId="ae36944e-16c2-491e-bc35-e487a3c6500d", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Category {Description="Cabin Air Filter", AccountId="ae36944e-16c2-491e-bc35-e487a3c6500d", DateCreated=DateTime.Today, DateUpdated=DateTime.Today }
            };

            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var activities = new List<Activity>
            {
                new Activity { ActivityDate = DateTime.Today, CategoryId = 1, Mileage = 30814.6M, VehicleId = 1, DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Activity { ActivityDate = DateTime.Today, CategoryId = 2, Gallons = 12.3M, Miles = 308.6M, VehicleId = 1, DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Activity { ActivityDate = DateTime.Today, CategoryId = 2, Gallons = 11.4M, Miles = 277.2M, VehicleId = 2, DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Activity { ActivityDate = DateTime.Today, CategoryId = 2, Gallons = 14.7M, Miles = 290.8M, VehicleId = 1, DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new Activity { ActivityDate = DateTime.Today, CategoryId = 2, Gallons = 10.3M, Miles = 324.5M, VehicleId = 4, DateCreated=DateTime.Today, DateUpdated=DateTime.Today }
            };

            activities.ForEach(a => context.Activities.Add(a));
            context.SaveChanges();

            var builtIn = new List<BuiltInCategories>
            {
                new BuiltInCategories { Description = "Gas", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new BuiltInCategories { Description = "Oil Change", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new BuiltInCategories { Description = "Brakes", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new BuiltInCategories { Description = "Wiper Blades", DateCreated=DateTime.Today, DateUpdated=DateTime.Today },
                new BuiltInCategories { Description = "Tires", DateCreated=DateTime.Today, DateUpdated=DateTime.Today }
            };
            builtIn.ForEach(b => context.BuiltInCategories.Add(b));
            context.SaveChanges();

        }
    }
}
