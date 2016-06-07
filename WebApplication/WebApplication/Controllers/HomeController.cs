using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public PartialViewResult FacebookChatPartial()
        {
            return PartialView();
        }
    }
}