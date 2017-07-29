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
    public class ActivityViewModel : Activity
    {

        //public bool FirstLogin { get; set; }
        //public List<Activity> GasPurchases { get; set; }
        //public List<Activity> NonGasPurchases { get; set; }

        public string ActDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:##.##}")]
        public int Mpg { get; set; }
        public string VehicleName { get; set; }
        public string MonthName { get; set; }

    }
}