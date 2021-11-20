using QuanLyBanHang.Common;
using QuanLyBanHang.DAO;
using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        

        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDAO().ListAllSlide();
            ViewBag.NewProduct = new ProductDAO().ListNewProduct(4);
            ViewBag.FeatureProduct   = new ProductDAO().ListFeatureProduct(4);

            //set seo website
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CartSession];
            List<CartItemDTO> list = new List<CartItemDTO>();
            if (cart != null)
            {
                list = (List<CartItemDTO>)cart;//model
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult MainMenu()
        {
            var model =  new MenuDAO().ListByGroupID(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24,Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult TopMenu()
        {
            var model = new MenuDAO().ListByGroupID(2);
            return PartialView(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult Footer()
        {
            var model = new FooterDAO().GetFooter();
            return PartialView(model);
        }
    }
}