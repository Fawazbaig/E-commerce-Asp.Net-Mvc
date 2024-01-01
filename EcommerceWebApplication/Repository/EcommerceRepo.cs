using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using EcommerceWebApplication.Repository;
using EcommerceWebApplication.Models;
using EcommerceWebApplication.Models.ViewModel;
using System.IO;

namespace EcommerceWebApplication.Repository
{

    public class EcommerceRepo : Iecommerce
    {


        private EcommerceDbEntities db = new EcommerceDbEntities();



        public void SignUp(UserTable us)
        {
            UserTable user = new UserTable();
            user.userName = us.userName;
            user.email = us.email;
            user.phone = us.phone;
            user.address = us.address;
            user.age = us.age;
            user.password = us.password;
            db.UserTables.Add(user);
            db.SaveChanges();

        }


        public void orderdetails(int id)
        {
            List<CartTable> cartTable = db.CartTables.Where(x => x.fkUserId == id && x.status == 0).ToList();

            orderDetailsTable orderDetails = new orderDetailsTable();

            OrderTable order = db.OrderTables.OrderByDescending(x => x.orderId).FirstOrDefault();
            foreach (var item in cartTable)
            {
                orderDetails.dateOfOrder = DateTime.Now.Date;
                orderDetails.fkOrderId = order.orderId;
                orderDetails.fkUserId = id;
                orderDetails.productName = item.productName;
                orderDetails.pricePerProduct = Convert.ToInt32(item.price);
                orderDetails.totalOrderPrice = order.totalPrice;
                db.orderDetailsTables.Add(orderDetails);

                item.status = 1;
                db.SaveChanges();
            }

        }

        public CategoryModel categoryLevel()
        {
            var level = "Create";
            CategoryModel categoryModel = new CategoryModel();
            categoryModel.Level = level;
            return categoryModel;
        }

        public IPagedList<CategoryTable> category(int? page, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                int pagesize = 6, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.CategoryTables.Where(x => x.categoryName.StartsWith(search)).ToList();
                IPagedList<CategoryTable> stu = list.ToPagedList(pageindex, pagesize);
                return stu;
            }
            else
            {
                int pagesize = 6, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.CategoryTables.Where(x => x.status == 0).OrderByDescending(x => x.categoryId).ToList();
                IPagedList<CategoryTable> stu = list.ToPagedList(pageindex, pagesize);


                return stu;
            }
        }

        public void deletecategory(int? id)
        {
            CategoryTable category = db.CategoryTables.Where(x => x.categoryId == id).SingleOrDefault();
            db.CategoryTables.Remove(category);
            db.SaveChanges();
        }

        public void createcategory(CategoryTable cat, string path, int adminid)
        {

            CategoryTable catTable = new CategoryTable();
            catTable.categoryName = cat.categoryName;
            catTable.categoryImagePath = path;
            catTable.fkCreatedByAdminId = adminid;
            db.CategoryTables.Add(catTable);
            db.SaveChanges();

        }

        public CategoryModel EditCategoryget(int? id)
        {
            var level = "Edit";
            CategoryTable category = db.CategoryTables.Where(x => x.categoryId == id).FirstOrDefault();
            CategoryModel categoryModel = new CategoryModel();
            categoryModel.CategoryId = category.categoryId;
            categoryModel.categoryName = category.categoryName;
            categoryModel.categoryImagePath = category.categoryImagePath;
            categoryModel.Level = level;
            return categoryModel;
        }

        public CategoryTable EditCategorypostnullimage(CategoryModel categoryModel)
        {
            CategoryTable category = db.CategoryTables.Where(x => x.categoryId == categoryModel.CategoryId).SingleOrDefault();
            category.categoryName = categoryModel.categoryName;
            category.categoryImagePath = category.categoryImagePath;
            db.SaveChanges();
            return category;
        }

        public CategoryTable EditCategorypostlimage(CategoryModel categoryModel, string path)
        {
            CategoryTable category = db.CategoryTables.Where(x => x.categoryId == categoryModel.CategoryId).SingleOrDefault();
            category.categoryName = categoryModel.categoryName;
            category.categoryImagePath = path;
            db.SaveChanges();
            return category;
        }
        public void DeleteUser(int? id)
        {
            UserTable userTable = db.UserTables.Where(x => x.userId == id).SingleOrDefault();
            db.UserTables.Remove(userTable);
            db.SaveChanges();
        }

        public UserModel EditUserget(int? id)
        {
            UserTable userTable = db.UserTables.Where(x => x.userId == id).SingleOrDefault();
            UserModel userModel = new UserModel();
            userModel.id = userTable.userId;
            userModel.address = userTable.address;
            userModel.email = userTable.email;
            userModel.phone = userTable.phone;
            userModel.age = userTable.age;
            userModel.userName = userTable.userName;
            userModel.password = userTable.password;
            return userModel;
        }

        public void EditUserpost(UserModel model)
        {
            UserTable user = db.UserTables.Where(x => x.userId == model.id).SingleOrDefault();
            user.phone = model.phone;
            user.age = model.age;
            user.userName = model.userName;
            user.address = model.address;
            user.email = model.email;

            db.SaveChanges();
        }

        public void PostAd(ProductModel pr, string path, int userid)
        {
            ProductTable product = new ProductTable();
            product.productName = pr.productName;
            product.price = pr.price;
            product.description = pr.description;
            product.fkCategoryId = pr.fkCategoryId;
            product.productImagePath = path;
            product.fkUserId = userid;

            db.ProductTables.Add(product);
            db.SaveChanges();
        }

        public IPagedList<ProductTable> DisplayAd(int? page, int? id)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.ProductTables.Where(x => x.fkCategoryId == id).OrderByDescending(x => x.productId).ToList();
            IPagedList<ProductTable> stu = list.ToPagedList(pageindex, pagesize);
            return stu;
        }

        public ProductDetailsModel AdDetail(int? id)
        {
            ProductDetailsModel productDetails = new ProductDetailsModel();
            ProductTable product = db.ProductTables.Where(x => x.productId == id).SingleOrDefault();
            productDetails.price = product.price;
            productDetails.Description = product.description;
            productDetails.productIamgePath = product.productImagePath;
            productDetails.ProductId = product.productId;
            productDetails.categoryId = product.fkCategoryId;
            productDetails.productName = product.productName;

            CategoryTable category = db.CategoryTables.Where(x => x.categoryId == product.fkCategoryId).SingleOrDefault();
            productDetails.categoryName = category.categoryName;

            UserTable user = db.UserTables.Where(x => x.userId == product.fkUserId).SingleOrDefault();
            productDetails.UserName = user.userName;
            productDetails.email = user.email;
            productDetails.userId = user.userId;
            productDetails.ProductUSerId = user.userId;
            productDetails.PhoneNo = user.phone;
            return productDetails;
        }

        public void Delete(int? id)
        {
            ProductTable product = db.ProductTables.Where(x => x.productId == id).SingleOrDefault();
            db.ProductTables.Remove(product);
            db.SaveChanges();
        }

        public ProductModelEdit Editget(int? id)
        {
            ProductTable product = db.ProductTables.Where(x => x.productId == id).SingleOrDefault();
            ProductModelEdit model = new ProductModelEdit();
            model.productName = product.productName;
            model.price = product.price;
            model.description = product.description;
            model.productId = product.productId;
            model.productImagePath = product.productImagePath;
            return model;
        }

        public void Editpostnullimage(ProductModelEdit pr, int userid)
        {
            ProductTable product = db.ProductTables.Where(x => x.productId == pr.productId).SingleOrDefault();
            if (pr.fkCategoryId == null)
            {
                product.productName = pr.productName;
                product.price = pr.price;
                product.description = pr.description;
                product.productImagePath = product.productImagePath;
                product.fkCategoryId = product.fkCategoryId;
                product.fkUserId = userid;
                db.SaveChanges();

            }
            else
            {
                product.productName = pr.productName;
                product.price = pr.price;
                product.description = pr.description;
                product.productImagePath = product.productImagePath;
                product.fkCategoryId = (int)pr.fkCategoryId;
                product.fkUserId = userid;
                db.SaveChanges();

            }
        }

        public void Editpostimage1(ProductModelEdit pr, string path, int userid)
        {
            ProductTable product = db.ProductTables.Where(x => x.productId == pr.productId).SingleOrDefault();
            product.productName = pr.productName;
            product.price = pr.price;
            product.description = pr.description;
            product.fkCategoryId = product.fkCategoryId;
            product.productImagePath = path;
            product.fkUserId = userid;
            db.SaveChanges();
        }

        public void Editpostimage2(ProductModelEdit pr, string path, int userid)
        {
            ProductTable product = db.ProductTables.Where(x => x.productId == pr.productId).SingleOrDefault();
            product.productName = pr.productName;
            product.price = pr.price;
            product.description = pr.description;
            product.fkCategoryId = (int)pr.fkCategoryId;
            product.productImagePath = path;
            product.fkUserId = userid;
            db.SaveChanges();
        }

        public void cart(CartModel cart, int? id, string name, double price, int userid)
        {
            CartTable carttab = new CartTable();
            carttab.price = price;
            carttab.fkProductId = Convert.ToInt32(id);
            carttab.productName = name;
            carttab.fkUserId = userid;
            db.CartTables.Add(carttab);
            db.SaveChanges();
        }

        public CartModel CartView(int userId)
        {
            List<CartTable> cartTable = db.CartTables.Where(temp => temp.fkUserId == userId && temp.status == 0).ToList();
            CartModel cartModel = new CartModel();
            cartModel.listData = cartTable;
            if (cartTable.Count > 0)
            {
                cartModel.isNoData = 1;
            }
            else
            {
                cartModel.isNoData = 0;
            }
            return cartModel;
        }

        public void  DeleteCart(int? id)
        {
            CartTable cart = db.CartTables.Where(x => x.fkProductId == id).FirstOrDefault();
            db.CartTables.Remove(cart);
            db.SaveChanges();
        }

        public void Order(OrderModel order, int total,int userId)
        {
            OrderTable orderTable = new OrderTable();
            orderTable.fkUserId = userId;
            orderTable.receivesName = order.reciverName;
            orderTable.reciverNumber = order.reciverNumber;
            orderTable.address = order.address;
            orderTable.totalPrice = total;

            db.OrderTables.Add(orderTable);
            db.SaveChanges();
        }

        public IPagedList<OrderTable> orders1(int? page, int userId)
        {
            int pagesize = 3, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.OrderTables.Where(x => x.fkUserId == userId).OrderByDescending(x => x.orderId).ToList();
            IPagedList<OrderTable> stu = list.ToPagedList(pageindex, pagesize);
            return stu;
        }
    }
}