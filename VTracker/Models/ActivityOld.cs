using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VTracker.Models
{
    public class ActivityOld: BaseModel
    {

        public int Id { get; set; }
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime ActivityDate { get; set; }

        public decimal Mileage { get; set; }
        public decimal Miles { get; set; }
        public decimal Gallons { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be longer that 100 characters.")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot be longer that 500 characters.")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:##.##}")]
        public decimal MPG { get; set; }

        public virtual Vehicle Vehicle { get; set; }
        public virtual Category Category { get; set; }

    }
}