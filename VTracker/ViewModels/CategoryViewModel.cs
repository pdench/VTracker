using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VTracker.Models;

namespace VTracker.ViewModels
{
    [NotMapped]
    public class CategoryViewModel : Category
    {

        public bool FirstLogin { get; set; }
        public List<Category> Categories { get; set; }
    }
}