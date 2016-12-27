using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTracker.Models
{
    public class BaseModel
    {

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool Deleted { get; set; }
    }
}