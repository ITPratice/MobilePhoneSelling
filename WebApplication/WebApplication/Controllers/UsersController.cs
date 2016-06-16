using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using System.Web.Security;
using WebApplication.Common;

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

        public ActionResult ChangeStaffPassword(string id)
        {
            Staff staff = db.Staffs.Find(id);
            return View(staff);
        }

        [HttpPost]
        public ActionResult ChangeStaffPassword(string oldPassword, string newPassword)
        {
            Staff staff = (Staff)Session[Constants.SESSION_ACCOUNT];
            Session[Constants.SESSION_ACCOUNT] = null;
            Session[Constants.SESSION_ACCOUNT_ID] = null;
            return RedirectToAction("Login");
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
                        Session[Constants.SESSION_ACCOUNT] = _cus;
                        Session[Constants.SESSION_ACCOUNT_ID] = _cus.Id;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thông tin không chính xác!");
                    }
                }
                else
                {
                    Staff _staff = db.Staffs.SingleOrDefault(x => x.AccountName == _username && x.Password == _password);
                    if (_staff != null)
                    {
                        Session[Constants.SESSION_ACCOUNT] = _staff;
                        Session[Constants.SESSION_ACCOUNT_ID] = _staff.Id;
                        //if (_staff.Password.Equals(ParamHelper.Instance.MD5Hash(_staff.AccountName)))
                        //{
                        //    return RedirectToAction("ChangeStaffPassword", "Users", new { id = _staff.Id });
                        //}
                        //else
                        //{
                            Session["Role"] = _staff.Position.Name;
                            if (_staff.Position.Name.Equals("Admin"))
                            {
                                return RedirectToAction("Index", "Products");
                            }
                            else if (_staff.Position.Name.Equals("Nhân viên giao dịch"))
                            {
                                return RedirectToAction("Index", "Orders");
                            }
                            else if (_staff.Position.Name.Equals("Nhân viên giao hàng"))
                            {
                                return RedirectToAction("Index", "Deliveries");
                            }
                        //}
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại!");
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