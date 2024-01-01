using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using EcommerceWebApplication.Models;
using EcommerceWebApplication.Models.ViewModel;
using EcommerceWebApplication.Repository;
using PagedList;

namespace EcommerceWebApplication.Controllers
{

    public class HomeController : Controller
    {
        EcommerceDbEntities db = new EcommerceDbEntities();
        private Iecommerce emc = new EcommerceRepo();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserTable us)
        {
            if (ModelState.IsValid)
            {
                emc.SignUp(us);
            }
            return View("SignIn");
        }
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserTable us)
        {
            if (ModelState.IsValid)
            {
                UserTable user = db.UserTables.Where(x => x.email == us.email && x.password == us.password).SingleOrDefault();
                if (user != null) 
                {
                    Session["us_id"] = user.userId.ToString();
                    Session["us_name"] = user.userName.ToString();
                    return View("index");
                }
                else
                {
                    Response.Write("<script>alert('No User Found');</script>");
                    return View();
                }
            }
            return View("index");
        }
        public ActionResult Admin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Admin(AdminTable ad)
        {
            AdminTable admin = db.AdminTables.Where(x => x.adminName == ad.adminName && x.password == ad.password).SingleOrDefault();
            if (admin != null)
            {
                Session["Ad_id"] = admin.adminId.ToString();
                Session["Ad_Name"] = admin.adminName.ToString();
                return View("Index");
            }
            else
            {
                Response.Write("<script>alert('No Admin');</script>");
            }
            return View();
        }
        public ActionResult Signout()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Index");
        }

        public ActionResult PostAd()
        {
            List<CategoryTable> li = db.CategoryTables.ToList();
            ViewBag.category = new SelectList(li, "categoryId", "categoryName");
                return View();
        }

        [HttpPost]
        public ActionResult PostAd(ProductModel pr,HttpPostedFileBase imagefile )
        {
            if (ModelState.IsValid)
            {
                string path = uploadimgfile(imagefile);
                if (path.Equals("-1"))
                {
                    Response.Write("<script>alert('Image could not be uploaded');</script>");
                }
                else
                {
                    int userid= Convert.ToInt32(Session["us_id"].ToString());
                    emc.PostAd(pr,path,userid);
                    Response.Write("<script>alert('Post Created Sucessfully');</script>");
                    return RedirectToAction("Category","Admin");
                }
            }

            return View();
        }


        public ActionResult DisplayAd(int? page,int? id)
        {
            IPagedList<ProductTable> products=emc.DisplayAd(page,id);
            return View(products);
        }

        public ActionResult AdDetail(int? id)
        {
            ProductDetailsModel productDetails =emc.AdDetail(id);
            return View(productDetails);
        }

        public ActionResult Delete(int? id)
        {
            emc.Delete(id);
            return RedirectToAction("Category","Admin");
        }

        public ActionResult Edit(int? id)
        {
            ProductTable product =db.ProductTables.Where(x=>x.productId==id).FirstOrDefault();
            ViewBag.category = new SelectList(db.CategoryTables, "categoryId", "categoryName", product.fkCategoryId);
            ProductModelEdit model = emc.Editget(id);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductModelEdit pr,HttpPostedFileBase imagefile)
        {
            if (ModelState.IsValid)
            {
                int userid = Convert.ToInt32(Session["us_id"].ToString());
                if (imagefile == null||imagefile.ContentLength<0 || imagefile.ContentLength == 0 )
                {
                    emc.Editpostnullimage(pr, userid);
                    return RedirectToAction("Category", "Admin");
                }
                else
                {
                    string path = uploadimgfile(imagefile);
                    if (pr.fkCategoryId == null)
                    {
                       
                        if (path.Equals("-1"))
                        {
                            Response.Write("<script>alert('Image could not be uploaded');</script>");
                        }
                        else
                        {                            
                           emc.Editpostimage1(pr,path ,userid);
                            return RedirectToAction("Category", "Admin");
                        }
                    }
                    else
                    {                        
                        if (path.Equals("-1"))
                        {
                            Response.Write("<script>alert('Image could not be uploaded');</script>");
                        }
                        else
                        {                            
                            emc.Editpostimage2(pr,path ,userid);
                            db.SaveChanges();
                            return RedirectToAction("Category", "Admin");
                        }
                    }
                }                
            }
                    return View();        
        }

        public ActionResult Cart(CartModel cart,int? id,string name,double price)
        {
            if(Session["us_id"]==null)
            {
                Response.Write("<script>alert('Sign in is Necessary to Add to cart');</script>");
                return View("index");
            }
            else
            {
                int userid = Convert.ToInt32(Session["us_id"]);
                emc.cart(cart,id,name,price,userid);
                Response.Write("<script>alert('Added to Cart');</script>");
            }
           

            return RedirectToAction("Category", "Admin");
        }

        public ActionResult CartView()
        {
            int userId = Convert.ToInt32(Session["us_id"]);
            CartModel cartModel = emc.CartView(userId);
            return View(cartModel);
        }

        public ActionResult DeleteCart(int? id)
        {
            emc.DeleteCart(id);
            return RedirectToAction("CartView");
        }

        public ActionResult Order()
        {          
            return View();
        }

        [HttpPost]
        public ActionResult Order(OrderModel orderm,int total)  
        {
            if(ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["us_id"]);
                emc.Order(orderm, total, userId);
                emc.orderdetails(userId);
                return View("index");
            }
                return View();
        }

        public ActionResult OrderView(int? page)
        {
            int userId = Convert.ToInt32(Session["us_id"]);
            IPagedList<OrderTable> stu = emc.orders1(page, userId);
            return View(stu);
            
        }

        public ActionResult OrderDetails(int? id)
        {
            List<orderDetailsTable> orderDetails = db.orderDetailsTables.Where(x => x.fkOrderId == id).ToList();

            return View(orderDetails);
        }

        //________________________________________________________________________
        public string uploadimgfile(HttpPostedFileBase file)
        {
            
            string path = "-1";
            
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/content/upload"),  Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/content/upload/"  + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('only file type of .jpg .jepg .png are allowed ');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file');</script>");
                path = "-1";
            }

            return path;
        }
    }
}