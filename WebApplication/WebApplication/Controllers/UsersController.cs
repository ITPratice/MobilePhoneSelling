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
    }
}