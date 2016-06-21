using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Common;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        // GET: Cart
        public List<ShoppingCart> GetShoppingCart()
        {
            List<ShoppingCart> _lstGioHang = Session["cart"] as List<ShoppingCart>;
            if (_lstGioHang == null)
            {
                _lstGioHang = new List<ShoppingCart>();
                Session["cart"] = _lstGioHang;
            }
            return _lstGioHang;
        }

        //Add Shopping Cart
        public ActionResult AddShoppongCart(string _idProducts, string _strUrl)
        {
            Product _products = db.Products.SingleOrDefault(x => x.Id == _idProducts);
            if (_products == null)
            {
                Response.StatusCode = 404; //return 404 error
                return null;
            }

            List<ShoppingCart> _lstGioHang = GetShoppingCart();
            ShoppingCart _cart = _lstGioHang.Find(x => x.IdProduct == _idProducts);
            if (_cart == null)
            {
                _cart = new ShoppingCart(_idProducts);
                //Thêm sản phẩm vào List
                _lstGioHang.Add(_cart);
                return Redirect(_strUrl);
            }
            else
            {
                _cart.Quantity++;
                return Redirect(_strUrl);
            }
        }

        //Update Shopping Cart
        public ActionResult UpdateShoppingCart(string _idProduct, FormCollection _form)
        {
            Product _product = db.Products.SingleOrDefault(x => x.Id == _idProduct);
            if (_product == null)
            {
                Response.StatusCode = 404;  //return 404 error
                return null;
            }
            List<ShoppingCart> _lstGioHang = GetShoppingCart();
            ShoppingCart _cart = _lstGioHang.SingleOrDefault(x => x.IdProduct == _idProduct);
            if (_cart != null)
            {
                _cart.Quantity = int.Parse(_form["txtSoLuong"].ToString());
            }
            return RedirectToAction("ShoppingCart");
        }

        //Delete Shopping Cart
        public ActionResult DeleteShoppingCart(string _idProduct, FormCollection _form)
        {
            Product _product = db.Products.SingleOrDefault(x => x.Id == _idProduct);
            if (_product == null)
            {
                Response.StatusCode = 404;  //return 404 error
                return null;
            }
            List<ShoppingCart> _lstGioHang = GetShoppingCart();
            ShoppingCart _cart = _lstGioHang.SingleOrDefault(x => x.IdProduct == _idProduct);
            if (_cart != null)
            {
                _lstGioHang.RemoveAll(x => x.IdProduct == _idProduct);
            }
            if (_lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ShoppingCart");
        }

        //Trang giỏ hàng
        public ActionResult ShoppingCart()
        {
            if (Session["cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCart> _lstGioHang = GetShoppingCart();
            ViewBag.TongTien = GetSum();
            return View(_lstGioHang);
        }

        //Get Total Quantity
        public int GetQuantity()
        {
            int _tong = 0;
            List<ShoppingCart> _lstGioHang = Session["cart"] as List<ShoppingCart>;
            if (_lstGioHang != null)
            {
                _tong = _lstGioHang.Sum(x => x.Quantity);
            }
            return _tong;
        }

        //Get Sum Price
        public double GetSum()
        {
            double _sum = 0;
            List<ShoppingCart> _lstGioHang = Session["cart"] as List<ShoppingCart>;
            if (_lstGioHang != null)
            {
                _sum = _lstGioHang.Sum(x => x.ThanhTien);
            }
            return _sum;
        }

        //Create Shopping Cart Partial
        public ActionResult ShoppingCartPartial()
        {
            if (GetQuantity() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = GetQuantity();
            ViewBag.TongTien = GetSum();
            return PartialView();
        }

        public ActionResult CartPartial()
        {
            List<ShoppingCart> _lstGioHang = GetShoppingCart();
            if (Session["cart"] == null)
            {
                return PartialView();
            }
            return PartialView(_lstGioHang);
        }

        /// <summary>
        /// Edit Shopping Cart
        /// </summary>
        /// <returns></returns>
        public ActionResult EditShoppingCart()
        {
            if (Session["cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCart> _lstCart = GetShoppingCart();
            return View(_lstCart);
        }

        #region Order
        //Init Order
        public ActionResult Order(Order _order)
        {
            if (Session["cart"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            Paypal pp = new Paypal();
            string strReturn = pp.GetPayPalResponse(Request.QueryString["tx"]);
            ViewBag.tx = Request.QueryString["tx"];
            if (ViewBag.tx == null)
                return HttpNotFound();
            else
            {
                OrderDetail _orderDetails = new OrderDetail();
                _order = GetOrder();
                _orderDetails = GetOrderDetails(_order);
                db.Orders.Add(_order);
                db.SaveChanges();
                Session["cart"] = null;
                return View();
            }
        }

        /// <summary>
        /// Get Order Infomation
        /// </summary>
        /// <returns></returns>
        public Order GetOrder()
        {
            Order _order = new Order();
            Customer _acc = (Customer)Session["Account"];
            Customer _customer = db.Customers.SingleOrDefault(x => x.Id == _acc.Id);
            var _ord = db.Orders.ToList();
            string _oldId = "";
            if (_ord.Count == 0)
            {
                _order.Id = ParamHelper.Instance.GetNewId("", Constants.PREFIX_ORDER);
            }
            else
            {
                var _lstOrder = _ord[_ord.Count - 1];
                _oldId = _lstOrder.Id;
                _order.Id = ParamHelper.Instance.GetNewId(_oldId, Constants.PREFIX_ORDER);
            }
            _order.CustomerId = _customer.Id;
            _order.Date = DateTime.Now;
            _order.Deleted = false;
            return _order;
        }

        /// <summary>
        /// Get Information of Order Details
        /// </summary>
        /// <param name="_order"></param>
        /// <returns></returns>
        public OrderDetail GetOrderDetails(Order _order)
        {
            OrderDetail _orderDetail = new OrderDetail();
            List<ShoppingCart> _lstCart = GetShoppingCart();
            foreach (var _item in _lstCart)
            {
                OrderDetail _orderDetails = new OrderDetail();
                _orderDetails.OrderId = _order.Id;
                _orderDetails.ProductId = _item.IdProduct;
                _orderDetails.Quantity = _item.Quantity;
                _orderDetails.Price = _item.ThanhTien;
                db.OrderDetails.Add(_orderDetails);
            }
            return _orderDetail;
        }
        #endregion

        #region Paypal Payment
        #endregion
    }
}