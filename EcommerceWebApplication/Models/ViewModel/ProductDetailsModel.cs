using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWebApplication.Models.ViewModel
{
    public class ProductDetailsModel
    {
        public int adminId { get; set; }
        public string categoryName { get; set; }
        public int categoryId { get; set; }
        public string productIamgePath { get; set; }

        public string productName { get; set; }
        public int ProductId { get; set; }
        public double price { get; set; }
        public int ProductUSerId { get; set; }
        public string Description { get; set; }

        public int userId { get; set; }
        public string UserName { get; set; }

        public string PhoneNo { get; set; }
        public string email { get; set; }
        public string Contact { get; set; }


    }
}