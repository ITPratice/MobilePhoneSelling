using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        // GET: Cart
        public List<ShoppingCart> GetShoppingCart()
        {
            List<ShoppingCart> _lstGioHang = Session["ShoppingCart"] as List<ShoppingCart>;
            if (_lstGioHang == null)
            {
                _lstGioHang = new List<ShoppingCart>();
                Session["ShoppingCart"] = _lstGioHang;
            }
            return _lstGioHang;
        }

        //Thêm giỏ hàng
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

        //Cập nhật giỏ hàng
        public ActionResult UpdateShoppingCart(string _idProduct, FormCollection _form)
        {
            Product _sach = db.Products.SingleOrDefault(x => x.Id == _idProduct);
            if (_sach == null)
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
            return View("GioHang");
        }

        //Xóa giỏ hàng
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
            if (Session["ShoppingCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCart> _lstGioHang = GetShoppingCart();
            return View(_lstGioHang);
        }

        //Tổng số lương
        public int GetQuantity()
        {
            int _tong = 0;
            List<ShoppingCart> _lstGioHang = Session["ShoppingCart"] as List<ShoppingCart>;
            if (_lstGioHang != null)
            {
                _tong = _lstGioHang.Sum(x => x.Quantity);
            }
            return _tong;
        }

        //Tính tổng tiền
        public double GetSum()
        {
            double _sum = 0;
            List<ShoppingCart> _lstGioHang = Session["ShoppingCart"] as List<ShoppingCart>;
            if (_lstGioHang != null)
            {
                _sum = _lstGioHang.Sum(x => x.ThanhTien);
            }
            return _sum;
        }

        //Tạo Partial Giỏ hàng
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
            if (Session["ShoppingCart"] == null)
            {
                return PartialView();
            }           
            return PartialView(_lstGioHang);
        }
    }
}