using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EcommerceWebApplication.Models;
using EcommerceWebApplication.Models.ViewModel;
using EcommerceWebApplication.Repository;
using PagedList;
namespace EcommerceWebApplication.Repository
{
    internal interface Iecommerce
    {
      
        void SignUp(UserTable us);

        void orderdetails(int id);

        CategoryModel categoryLevel();

        IPagedList<CategoryTable> category(int? page, string search );

        void deletecategory(int? id);

        void createcategory(CategoryTable cat, string path, int adminid);

        CategoryModel EditCategoryget(int? id);

        CategoryTable EditCategorypostnullimage(CategoryModel categoryModel);

        CategoryTable EditCategorypostlimage(CategoryModel categoryModel,string path);

        void DeleteUser(int? id);

        UserModel EditUserget(int? id);

        void EditUserpost(UserModel model);

        void PostAd(ProductModel pr,string path, int userid);

        IPagedList<ProductTable> DisplayAd(int? page, int? id);

        ProductDetailsModel AdDetail(int? id);

        void Delete(int? id);

        ProductModelEdit Editget(int? id);

        void Editpostnullimage(ProductModelEdit pr ,int userid);

        void Editpostimage1(ProductModelEdit pr,string path ,int userid);

        void Editpostimage2(ProductModelEdit pr, string path, int userid);

        void cart(CartModel cart, int? id, string name, double price,int userid);

        CartModel CartView(int userId);

        void DeleteCart(int? id);

        void Order(OrderModel order, int total,int userId);

        IPagedList<OrderTable> orders1(int? page,int userId);
    }
}
