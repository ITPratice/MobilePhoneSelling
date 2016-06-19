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
            var product = from p in db.Products.Where(p => p.Deleted != true) select p;
            return View(product.ToList());
        }

        public PartialViewResult FacebookChatPartial()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult AjaxSearch()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxSearch(string keyword)
        {
            var _prodQuery = (from n in db.Products
                              where n.Name.StartsWith(keyword) && n.Deleted != true
                              select new { n.Name });
            return Json(_prodQuery, JsonRequestBehavior.AllowGet);
        }
    }
}