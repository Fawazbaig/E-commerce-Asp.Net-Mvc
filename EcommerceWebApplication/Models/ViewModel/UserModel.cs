using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EcommerceWebApplication.Models.ViewModel
{
    public class UserModel
    {
        public int id { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "User Name is required")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid User Name")]
        public string userName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9!#$%^&*.]+@[a-zA-Z]+[.]{1}[a-zA-Z]+$", ErrorMessage = "Please enter a valid Email")]
        public string email { get; set; }

        [Display(Name = "Phone.No")]
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Please enter a valid Phone")]
        public string phone { get; set; }

        [Range(15, 90, ErrorMessage = "Enter number between 15 to 90")]
        [Required(ErrorMessage = "Age is required")]
        [Display(Name = "Age")]
        public int age { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        public string address { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{5,}", ErrorMessage = "Your password must be at least 5 characters long and contain at least 1 letter and 1 number")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}