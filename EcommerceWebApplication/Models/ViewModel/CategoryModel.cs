using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWebApplication.Models.ViewModel
{
    public class CategoryModel
    {
        
        public int CategoryId { get; set;}

        public string Level { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string categoryName { get; set; }

       
        [Display(Name = "Image")]
        public string categoryImagePath { get; set; }

    }
}