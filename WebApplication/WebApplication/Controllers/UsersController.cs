using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        //MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        //// GET: Users
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Login(FormCollection _form)
        //{
        //    string _name = _form["txtName"].ToString();
        //    string _password = ParamHelper.Instance.MD5Hash(_form["txtPassword"].ToString());
        //    Account _acc = db.Accounts.SingleOrDefault(x => x.Name == _name && x.Password == _password);
        //    if (_acc != null)
        //    {
        //        ViewBag.LoginStatus = "Đăng nhập thành công !'";
        //        Session["Account"] = _acc;
        //        ViewBag.AccountName = _acc.Name;
        //        return View();
        //    }
        //    ViewBag.LoginStatus = "Đăng nhập không thành công !";
        //    return View();
        //}

        //[HttpGet]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Register(Account _acc)
        //{
        //    //Insert data to database
        //    db.Accounts.Add(_acc);
        //    //Save changes
        //    db.SaveChanges();
        //    return View();
        //}
    }
}