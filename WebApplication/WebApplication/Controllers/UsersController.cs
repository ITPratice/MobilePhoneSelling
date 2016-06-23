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
                Account account = db.Accounts.SingleOrDefault(a => a.Name == _username &&
                    a.Password == _password && a.Deleted != true);
                if (account != null)
                {
                    Customer customer = db.Customers.SingleOrDefault(c => c.AccountId == account.Id);
                    if (customer != null)
                    {
                        Session[Constants.SESSION_ACCOUNT] = customer;
                        Session[Constants.SESSION_ACCOUNT_ID] = customer.Id;
                        Session[Constants.SESSION_ROLE] = "Khách hàng";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Staff staff = db.Staffs.SingleOrDefault(s => s.AccountId == account.Id);
                        if (staff != null)
                        {
                            Session[Constants.SESSION_ACCOUNT] = staff;
                            Session[Constants.SESSION_ACCOUNT_ID] = staff.Id;
                            Session[Constants.SESSION_ROLE] = staff.Position.Name;
                            if (staff.Position.Name.Equals("Admin"))
                            {
                                return RedirectToAction("Index", "Products");
                            }
                            else if (staff.Position.Name.Equals("Nhân viên giao dịch"))
                            {
                                return RedirectToAction("Index", "Orders");
                            }
                            else if (staff.Position.Name.Equals("Nhân viên giao hàng"))
                            {
                                return RedirectToAction("Index", "Deliveries");
                            }
                        }
                        else ModelState.AddModelError("", "Thông tin không chính xác!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại!");
                }
            }
            return View();
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session[Constants.SESSION_ACCOUNT] = null;
            Session[Constants.SESSION_ACCOUNT_ID] = null;
            Session[Constants.SESSION_ROLE] = null;
            Session["Cart"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}