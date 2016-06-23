using Postal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication.Common;
using WebApplication.Models;
using PagedList;

namespace WebApplication.Controllers
{
    public class CustomersController : Controller
    {
        #region Fields
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        #endregion

        #region ActionResult
        // GET: Customers
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var customers = from c in db.Customers select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.Name);
                    break;
                default:
                    customers = customers.OrderBy(c => c.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(customers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Customers/Edit/5
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

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null) return HttpNotFound();
            customer.Deleted = true;
            customer.Account.Deleted = true;
            db.Entry(customer.Account).State = EntityState.Modified;
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Restore(string id)
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
            customer.Deleted = false;
            customer.Account.Deleted = false;
            db.Entry(customer.Account).State = EntityState.Modified;
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditProfile(string id)
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
        public ActionResult EditProfile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer _customer, string accountName, string password)
        {
            if (ModelState.IsValid)
            {
                if (Session[Constants.SESSION_ACCOUNT] != null)
                    return HttpNotFound();
                var _newUser = db.Customers.Create();
                _newUser.Id = GetId(_newUser);
                _newUser.Name = _customer.Name;
                _newUser.Birthday = _customer.Birthday;
                _newUser.PhoneNumber = _customer.PhoneNumber;
                _newUser.Address = _customer.Address;
                _newUser.Email = _customer.Email;
                _newUser.Deleted = false;
                Account account = new Account();
                List<Account> accounts = db.Accounts.OrderBy(a => a.Id).ToList();
                string oldAccountId = "";
                if (accounts.Count > 0) oldAccountId = accounts[accounts.Count - 1].Id;
                account.Id = ParamHelper.Instance.GetNewId(oldAccountId, Constants.PREFIX_ACCOUNT);
                account.Name = accountName;
                account.Password = ParamHelper.Instance.MD5Hash(password);
                db.Accounts.Add(account);
                db.SaveChanges();
                _newUser.AccountId = account.Id;
                Session[Constants.SESSION_ACCOUNT] = _newUser;
                Session[Constants.SESSION_ACCOUNT_ID] = _newUser.Id;
                Session[Constants.SESSION_ROLE] = "Khách hàng";
                db.Customers.Add(_newUser);
                db.SaveChanges();
                SendEmail(_newUser, Constants.TEMPLATE_EMAIL_REGISTER, "ConfirmationMail", "Customers");
                return RedirectToAction("ConfirmationStepTwo");
            }
            else
            {
                ModelState.AddModelError("", "Dữ liệu nhập không chính xác !");
            }
            return RedirectToAction("Home", "Index");
        }

        public ActionResult ConfirmationStepTwo()
        {
            return View();
        }

        public ActionResult ConfirmationMail(string token)
        {
            Customer _customer = db.Customers.Find(token);
            if (ModelState.IsValid)
            {
                if (_customer != null)
                {
                    _customer.IsActivated = true;
                    Session[Constants.SESSION_ACCOUNT] = _customer;
                    db.Entry(_customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Customer is not correct");
                }
            }
            else
                ModelState.AddModelError("", "Data is not correct");
            return View(_customer);
        }

        #endregion

        #region Reset Password
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(Customer _customer)
        {
            if (Session[Constants.SESSION_ACCOUNT] != null)
                return HttpNotFound();
            Customer _cus = db.Customers.SingleOrDefault(x => x.Email == _customer.Email);
            if (_cus != null)
            {
                Session[Constants.SESSION_RESET_PASSWORD] = _cus;
                SendEmail(_cus, Constants.TEMPLATE_EMAIL_RESET_PASS, "ResetPassword", "Customers");
                return RedirectToAction("ForgetPasswordStep2", "Customers");
            }
            else
            {
                ModelState.AddModelError("", "Khong tim thay Email !");
            }
            return View(_customer);
        }

        public ActionResult ForgetPasswordStep2()
        {
            if (Session[Constants.SESSION_RESET_PASSWORD] == null || Session[Constants.SESSION_ACCOUNT] != null)
                return HttpNotFound();
            return View();
        }

        public ActionResult ResetPassword(string token)
        {
            Customer _customer = db.Customers.Find(token);
            if (ModelState.IsValid)
            {
                if (Session[Constants.SESSION_ACCOUNT] != null)
                    return HttpNotFound();
                if (_customer != null)
                {
                    string _newPass = ParamHelper.Instance.GeneratePassword(8, 10);
                    ViewBag.NewPassword = _newPass;
                    _customer.Account.Password = ParamHelper.Instance.MD5Hash(_newPass);
                    db.Entry(_customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Customer is not correct");
                }
            }
            else
                ModelState.AddModelError("", "Data is not correct");
            return View(_customer);
        }
        #endregion

        #endregion

        #region Method
        /// <summary>
        /// Get Id of products
        /// </summary>
        /// <param name="_newUser"></param>
        /// <returns></returns>
        public string GetId(Customer _newUser)
        {
            var _lstUser = db.Customers.ToList();
            string oldId = "";
            if (_lstUser.Count > 0) oldId = _lstUser[_lstUser.Count - 1].Id;
            _newUser.Id = ParamHelper.Instance.GetNewId(oldId, WebApplication.Common.Constants.PREFIX_CUSTOMER);
            return _newUser.Id;
        }

        /// <summary>
        /// Send mail Confirm Registration
        /// </summary>
        /// <param name="_cus"></param>
        private void SendEmail(Customer _cus, string _template, string actionName, string controllerName)
        {
            string confirm = Url.Action(actionName, controllerName, new { token = _cus.Id }, Request.Url.Scheme);
            dynamic email = new Email(_template);
            email.To = _cus.Email;
            email.UserName = _cus.Account.Name;
            email.ConfirmationToken = confirm;
            email.Send();
        }

        public string CheckUserName(string uname)
        {
            var userExist = "";
            var user = db.Accounts.FirstOrDefault((n) => n.Name == uname);
            if (user == null)
            {
                userExist = "No";
            }
            else
            {
                userExist = "Yes";
            }
            return userExist;
        }

        public string CheckEmail(string email)
        {
            var emailExist = "";
            var user = db.Customers.FirstOrDefault((n) => n.Email == email);
            if (user == null)
            {
                emailExist = "No";
            }
            else
            {
                emailExist = "Yes";
            }
            return emailExist;
        }
        #endregion
    }
}
