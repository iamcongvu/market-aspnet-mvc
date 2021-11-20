using Common;
using Newtonsoft.Json.Linq;
using QuanLyBanHang.DAO;
using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QuanLyBanHang.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        /// <summary>
        /// hiển thị list product trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItemDTO>();

            if (cart != null)
            {
                list = (List<CartItemDTO>)cart;//model
            }

            return View(list);
        }

        public JsonResult GetAllCartItem(int? Pages)
        {
            var cart = (List<CartItemDTO>)Session[CartSession];
            if (Pages == null) Pages = 1;
            int pageSize = 2;
            var list = new List<CartItemDTO>();
            if (cart != null)
            {
                list = cart;
            }
            decimal totalRecord = list.Count();
            decimal totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            int skip = (Pages.Value - 1) * pageSize;

            var cartItem = list.OrderBy(x => x.Product.ID).Skip(skip).Take(pageSize).ToList();

            cartItem.ForEach(x => { x.index = (skip++) + 1; });

            return Json(new { totalPage, cartItem }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AddItem(int productID, int quantity)
        {
            var product = new ProductDAO().ViewDetailProduct(productID);
            var cart = Session[CartSession];
            if (cart != null)// đã có prod
            {
                var list = (List<CartItemDTO>)cart;
                if (list.Exists(x => x.Product.ID == productID))
                {

                    foreach (var item in list)
                    {
                        if (item.Product.ID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    CartItemDTO item = new CartItemDTO();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;//gán lại list prod vào session
            }
            else
            {
                //tạo mới đối tượng cart item
                CartItemDTO item = new CartItemDTO();
                item.Product = product;// gán tất cả thông tin của 1 đối tượng Product
                item.Quantity = quantity;
                var list = new List<CartItemDTO>();
                list.Add(item);

                //gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItemDTO>>(cartModel);// du lieu view gui ve
            var sessionCart = (List<CartItemDTO>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.FirstOrDefault(x => x.Product.ID == item.Product.ID);//tim ra item trong session khop voi data ma view gui ve
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;//update sesstion
            return Json(new { status = true });
        }

        public JsonResult DeleteAllProduct()
        {
            Session[CartSession] = null;
            return Json(new { status = true });
        }

        public JsonResult DeleteProduct(int id)
        {
            var sessionCart = (List<CartItemDTO>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;//update sesstion
            return Json(new { status = true });
        }

        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItemDTO>();
            if (cart != null)
            {
                list = (List<CartItemDTO>)cart;//model
            }
            return View(list);
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            Order order = new Order();
            order.CreateDate = DateTime.Now;
            order.ShipName = shipName;
            order.ShipMobile = mobile;
            order.ShipAddress = address;
            order.ShipEmail = email;

            try
            {
                var id = new OrderDAO().Insert(order);
                var cart = (List<CartItemDTO>)Session[CartSession];
                OrderDetailDAO DetailDAO = new OrderDetailDAO();
                decimal total = 0;

                foreach (var item in cart)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;

                    DetailDAO.Insert(orderDetail);

                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                    ViewBag.Total = total;
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/template/NewOrder.html"));//send mail

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ Shoppe", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Shoppe", content);

            }
            catch (Exception)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            return Redirect("/hoan-thanh");

        }

        public ActionResult Success()
        {
            return View();
        }


        /// <summary>
        /// Momo Payment
        /// </summary>
        /// <returns></returns>
        //public ActionResult ReturnUrl()
        //{
        //    return View();
        //}
        //public ActionResult NotifyUrl()
        //{
        //    return View();
        //}
    }
}