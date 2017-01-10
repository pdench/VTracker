using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VTracker.ViewModels
{
    public class ContactViewModel
    {

        [Required]
        [StringLength(50)]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must include your comment.")]
        [StringLength(500)]
        [Display(Name = "Comment (up to 500 characters)")]
        public string Comment { get; set; }
    }
}