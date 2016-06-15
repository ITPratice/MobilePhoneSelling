﻿using Postal;
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
            customer.Deleted = true;
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
            db.Entry(customer).State = EntityState.Modified;
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
                    //FormsAuthentication.SetAuthCookie(_newUser.Name, false);
                    Session["Account"] = _newUser;
                    db.Customers.Add(_newUser);
                    db.SaveChanges();
                    SendEmail(_newUser, Constants.TEMPLATE_EMAIL_REGISTER, "ConfirmationMail", "Customers");
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
            if (ModelState.IsValid)
            {
                Customer _cus = db.Customers.SingleOrDefault(x => x.Email == _customer.Email);
                if (_cus != null)
                {
                    Session["ForgetPass"] = _cus;
                    SendEmail(_cus, Constants.TEMPLATE_EMAIL_RESET_PASS, "ResetPassword", "Customers");
                    return RedirectToAction("ForgetPasswordStep2", "Customers");
                }
                else
                {
                    ModelState.AddModelError("", "Khong tim thay Email !");
                }
            }
            else
                ModelState.AddModelError("", "Error");
            return View(_customer);
        }

        public ActionResult ForgetPasswordStep2()
        {
            return View();
        }

        public ActionResult ResetPassword(string token)
        {
            Customer _customer = db.Customers.Find(token);
            if (ModelState.IsValid)
            {
                if (_customer != null)
                {
                    string _newPass = ParamHelper.Instance.GeneratePassword(8, 10);
                    ViewBag.NewPassword = _newPass;
                    _customer.Password = ParamHelper.Instance.MD5Hash(_newPass);
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
            email.UserName = _cus.AccountName;
            email.ConfirmationToken = confirm;
            email.Send();
        }
        #endregion
    }
}
