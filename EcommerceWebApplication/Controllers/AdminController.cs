using EcommerceWebApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceWebApplication.Repository;
using PagedList;
using EcommerceWebApplication.Models.ViewModel;


namespace EcommerceWebApplication.Controllers
{
    public class AdminController : Controller
    {
        EcommerceDbEntities db = new EcommerceDbEntities();
        private Iecommerce emc = new EcommerceRepo();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCategory()
        {
            CategoryModel categoryModel = emc.categoryLevel();
            return View(categoryModel);
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryTable cat, HttpPostedFileBase imagefile)
        {
            if(ModelState.IsValid)
            {
                string path = uploadimgfile(imagefile);
                if (path.Equals("-1"))
                {
                    Response.Write("<script>alert('Image could not be uploaded');</script>");
                }
                else
                {
                    int adminid = Convert.ToInt32(Session["Ad_id"].ToString());
                    emc.createcategory(cat, path, adminid);
                    Response.Write("<script>alert('Category Created Sucessfully');</script>");
                }
            }
            
            return RedirectToAction("Category");
        }

        public ActionResult Category(int? page ,string search)
        {
            IPagedList<CategoryTable> categories= emc.category(page, search);
            return View(categories);
        }

        public ActionResult DeleteCategory(int? id)
        {
            emc.deletecategory(id);
            return RedirectToAction("Category");
        }

        public ActionResult EditCategory(int? id)
        {
            CategoryModel categoryModel=emc.EditCategoryget(id);
            return View("CreateCategory", categoryModel);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryModel categoryModel,HttpPostedFileBase imagefile)
        {
            if(ModelState.IsValid)
            {
                if (imagefile == null || imagefile.ContentLength < 0 || imagefile.ContentLength == 0)
                {
                    CategoryTable categoryTable = emc.EditCategorypostnullimage(categoryModel);
                    return RedirectToAction("Category");
                }
                else
                {
                    string path = uploadimgfile(imagefile);
                    if (path.Equals("-1"))
                    {
                        Response.Write("<script>alert('Image could not be uploaded');</script>");
                    }
                    else
                    {
                        CategoryTable category = emc.EditCategorypostlimage(categoryModel, path);
                        return RedirectToAction("Category");
                    }
                }
                    
            }
            return RedirectToAction("EditCategory");
        }

        public ActionResult UserList()
        {
            List<UserTable> users = db.UserTables.ToList();
            return View(users);
        }

        public ActionResult DeleteUser(int? id)
        {
            emc.DeleteUser(id);
            return RedirectToAction("UserList");
        }

       
        public ActionResult EditUser(int? id)
        {
            UserModel userModel = emc.EditUserget(id);
            return View(userModel);
        }

        [HttpPost]
        public ActionResult EditUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                emc.EditUserpost(model);
                return RedirectToAction("UserList");
            }           
            return View();

        }

        public ActionResult AdminOrders()
        {
            List<OrderTable> order=db.OrderTables.ToList();
            return View(order);
        }

        public ActionResult DetailsOrder(int? id)
        {
            List<orderDetailsTable> orderDetails=db.orderDetailsTables.Where(x=>x.fkOrderId==id).ToList();
            return View(orderDetails);
        }
        //________________________________________________________________________
        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/content/upload/" + random + Path.GetFileName(file.FileName);
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