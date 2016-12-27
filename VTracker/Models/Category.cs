using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace VTracker.Models
{
    public class Category : BaseModel
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public string AccountId { get; set; }
        public bool BuiltIn { get; set; }
        //public virtual Account Account { get; set; }

    }
}