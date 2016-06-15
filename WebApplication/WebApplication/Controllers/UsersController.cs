using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();

        // GET: Users/
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                string _id = User.Identity.GetUserId();
                Customer _cus = db.Customers.Find(_id);
                ViewBag.CusID = _cus.Id;
                _cus.Name = customer.Name;
                _cus.Address = customer.Address;
                _cus.Birthday = customer.Birthday;
                _cus.Email = customer.Email;
                _cus.PhoneNumber = customer.PhoneNumber;
                db.Entry(_cus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Data is not correct !");
            }
            return View(customer);
        }

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection _form)
        {
            string _username = _form["txtUsername"].ToString();
            string _password = ParamHelper.Instance.MD5Hash(_form["txtPassword"].ToString());

            bool _remember = Convert.ToBoolean(Request["chkRemember"]);
            string user = Request["checkUser"];
            if (ModelState.IsValid)
            {
                if (user.Equals("customer"))
                {
                    Customer _cus = db.Customers.SingleOrDefault(x => x.AccountName == _username && x.Password == _password);
                    if (_cus != null)
                    {
                        Session["Account"] = _cus;
                        ViewBag.Account = _cus.Name;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thông tin không chính xác !");
                    }
                }
                else
                {
                    Staff _staff = db.Staffs.SingleOrDefault(x => x.AccountName == _username && x.Password == _password);
                    if (_staff != null)
                    {
                        return RedirectToAction("Index", "Products");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Login fail");
                    }
                }
            }
            return View();
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session["Account"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}