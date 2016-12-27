using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VTracker.Models;

namespace VTracker.ViewModels
{
    [NotMapped]
    public class VehicleViewModel : Vehicle
    {

        public bool FirstLogin { get; set; }

        public List<Activity> GasPurchases { get; set; }
        public List<Activity> NonGasPurchases { get; set; }

    }
}