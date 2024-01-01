using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWebApplication.Models.ViewModel
{
    public class CartModel
    {
        public int cartId { get; set; }
        public int fkProductId { get; set; }

        [Display(Name = "Product Name")]
        public string productName { get; set; }
        public int fkUserId { get; set; }

        [Display(Name = "Price")]
        public double price { get; set; }
        public int status { get; set; }

        public List<CartTable> listData { get; set; }
        public int isNoData { get; set; } 
    }
}