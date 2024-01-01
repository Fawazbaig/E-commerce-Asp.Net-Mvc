using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWebApplication.Models.ViewModel
{
    public class ProductModel
    {
        public int productId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string productName { get; set; }

        
        [Display(Name = "Image")]
        public string productImagePath { get; set; }

        [Range(1000,1000000, ErrorMessage = "Enter number between 1000 to 1000000")]
        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        public double price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string description { get; set; }
        public int status { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int fkCategoryId { get; set; }

       

        public int fkUserId { get; set; }
    }
}