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
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                Customer _cus = db.Customers.Find(Session["AccId"].ToString());
                Customer _currentCus = Session["Account"] as Customer;
                _cus.Name = customer.Name;
                _cus.Address = customer.Address;
                _cus.Birthday = customer.Birthday;
                _cus.Email = customer.Email;
                _cus.PhoneNumber = customer.PhoneNumber;
                _cus.Password = _currentCus.Password;
                _cus.IsActivated = _currentCus.IsActivated;
                _cus.Deleted = _currentCus.Deleted;
                _cus.BankAccount = _currentCus.BankAccount;
                db.Entry(_cus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(customer);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Customer customer)
        {
            if (ModelState.IsValid)
            {
                Customer _cus = db.Customers.Find(Session["AccId"].ToString());
                Customer _currentCus = Session["Account"] as Customer;
                _cus.Name = _currentCus.Name;
                _cus.Address = _currentCus.Address;
                _cus.Birthday = _currentCus.Birthday;
                _cus.Email = _currentCus.Email;
                _cus.PhoneNumber = _currentCus.PhoneNumber;
                _cus.Password = ParamHelper.Instance.MD5Hash(customer.Password);
                _cus.IsActivated = _currentCus.IsActivated;
                _cus.Deleted = _currentCus.Deleted;
                _cus.BankAccount = _currentCus.BankAccount;
                db.Entry(_cus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Error");
            return View();
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
                        Session["AccId"] = _cus.Id;
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
            Session["AccId"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}