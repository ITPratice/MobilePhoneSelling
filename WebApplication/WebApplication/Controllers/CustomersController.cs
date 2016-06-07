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

namespace WebApplication.Controllers
{
    public class CustomersController : Controller
    {
        #region Fields
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        #endregion

        #region ActionResult
        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
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

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Birthday,Address,PhoneNumber,Email,BankAccount,AccountName,Password,Deleted")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
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
        public ActionResult Edit([Bind(Include = "Id,Name,Birthday,Address,PhoneNumber,Email,BankAccount,AccountName,Password,Deleted")] Customer customer)
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
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public ActionResult Register(Customer _customer)
        {
            if (ModelState.IsValid)
            {
                var _newUser = db.Customers.Create();
                if (db.Customers.Any(x => x.AccountName == _customer.AccountName))
                {
                    ModelState.AddModelError("", "Tài khoản đã được sử dụng");
                }
                else
                {
                    _newUser.Id = GetId(_newUser);
                    _newUser.Name = _customer.Name;
                    _newUser.Birthday = _customer.Birthday;
                    _newUser.PhoneNumber = _customer.PhoneNumber;
                    _newUser.Address = _customer.Address;
                    _newUser.Email = _customer.Email;
                    _newUser.Password = ParamHelper.Instance.MD5Hash(_customer.Password);
                    _newUser.AccountName = _customer.AccountName;
                    _newUser.Deleted = false;
                    FormsAuthentication.SetAuthCookie(_newUser.Name, false);
                    db.Customers.Add(_newUser);
                    db.SaveChanges();
                    SendEmail(_newUser);
                    return RedirectToAction("ConfirmationStepTwo");
                }
                return View(_customer);
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
            bool _remember = Convert.ToBoolean(_form["chkRemember"]);
            if (ModelState.IsValid)
            {
                Customer _cus = db.Customers.SingleOrDefault(x => x.AccountName == _username && x.Password == _password);
                if (_cus != null)
                {
                    FormsAuthentication.SetAuthCookie(_cus.Name, _remember);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Thông tin không chính xác !");
                }
            }
            return View();
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
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
        private void SendEmail(Customer _cus)
        {
            string confirm = Url.Action("ConfirmationMail", "Customers", new { token = _cus.Id }, Request.Url.Scheme);
            dynamic email = new Email("TempleteEmail");
            email.To = "cuongdvt261@gmail.com";
            email.UserName = _cus.AccountName;
            email.ConfirmationToken = confirm;
            email.Send();
        }
        #endregion
    }
}
