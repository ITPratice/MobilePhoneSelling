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
            //var _curPromotion = from c in db.Promotions.Where(pr => pr.StartDate.CompareTo(DateTime.Now) <= 0 && pr.EndDate.CompareTo(DateTime.Now) >= 0)
            //                    select c;
            //var _custProList = (from c in _curPromotion
            //                    from p in db.Products.Where(pr => pr.Promotions.Contains(c))
            //                    select p);
            //ViewBag.SaleOff = _curPromotion.ToList()[0].SaleOff;
            return View(db.Products.ToList());
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
                              where n.Name.StartsWith(keyword)
                              select new { n.Name });
            return Json(_prodQuery, JsonRequestBehavior.AllowGet);
        }
    }
}