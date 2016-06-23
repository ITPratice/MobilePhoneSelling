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
            return View(product.Take(16).ToList());
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

        public ActionResult GetProduct()
        {
            var topProductID = db.OrderDetails
                .GroupBy(x => x.ProductId)
                .OrderByDescending(g => g.Count())
                .Take(8)
                .Select(x => x.Key)
                .ToList();
            var topProducts = db.Products
                .Where(x => topProductID.Contains(x.Id));
            return PartialView(topProducts.ToList());
        }
    }
}