using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace VTracker.Models
{
    public class Vehicle: BaseModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Vehicle")]
        public string VehicleName { get; set; }
        public string AccountId { get; set; }

        //public virtual Account Account { get; set; }

    }
}