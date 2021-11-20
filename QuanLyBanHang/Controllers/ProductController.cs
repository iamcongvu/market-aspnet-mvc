using QuanLyBanHang.DAO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class ProductController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()//danh mục sản phẩm ở trang chủ
        {
            var model = new ProductCategoryDAO().ListAll();
            return PartialView(model);
        }

        /// <summary>
        /// ListName for Search Auto Complement
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public JsonResult ListName(string q)
        {
            var data = new ProductDAO().ListName(q);
            return Json(new { data = data, status = true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Category(int id, int pageIndex = 1, int pageSize = 2)
        {
            var category = new CategoryDAO().ViewDetial(id);
            ViewBag.Category = category;
            int totalRecord = 0;
            var model = new ProductDAO().ListByCategoryID(id, ref totalRecord, pageIndex, pageSize);

            ViewBag.TotalRecord = totalRecord;
            ViewBag.Page = pageIndex;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.FirstPage = 1;
            ViewBag.LastPage = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;


            return View(model);
        }

        public ActionResult Search(string keyword, int pageIndex = 1, int pageSize = 2)
        {
            int totalRecord = 0;
            var model = new ProductDAO().Search(keyword, ref totalRecord, pageIndex, pageSize);

            ViewBag.TotalRecord = totalRecord;
            ViewBag.Page = pageIndex;
            ViewBag.Keyword = keyword;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.FirstPage = 1;
            ViewBag.LastPage = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;


            return View(model);
        }

        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Detail(int id)
        {
            var product = new ProductDAO().ViewDetailProduct(id);
            ViewBag.Category = new ProductCategoryDAO().ViewDetail(product.CategoryID);
            ViewBag.RelatedProducts = new ProductDAO().ListRelatedProduct(id);
            return View(product);
        }
    }
}