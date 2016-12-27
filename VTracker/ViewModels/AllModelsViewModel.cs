using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTracker.Models;

namespace VTracker.ViewModels
{
    public class AllModelsViewModel
    {

        public List<Activity> allActivities { get; set; }
        public List<Vehicle> allVehicles { get; set; }
        public List<Category> allCategories { get; set; }

    }
}