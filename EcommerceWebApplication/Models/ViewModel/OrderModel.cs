using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWebApplication.Models.ViewModel
{
    public class OrderModel
    {
        public int orderId { get; set; }

        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid Name")]
        [Display(Name = "Reciver's Name")]
        public string reciverName { get; set; }
        public int fkUserId { get; set; }


        [Required]
        [Display(Name = "Reciver's Address")]
        public string address { get; set; }

        [Required]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Please enter a valid Phone")]
        [Display(Name = "Reciver's Number")]
        public string reciverNumber { get; set; }

        public int TotalPrice { get; set; }
    }
}