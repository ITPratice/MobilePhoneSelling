using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Common;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection f)
        {
            string keyword = f["txtTimKiem"].ToString();
            using (MobilePhoneSellingEntities db = new MobilePhoneSellingEntities())
            {
                Product product = db.Products.SingleOrDefault(x => x.Name == keyword);
                if (product == null)
                {
                    List<Product> _lstProd = db.Products.Where(p => p.Name.Contains(keyword)).ToList();
                    return View(_lstProd);
                }
                else
                {
                    string strID = product.Id;
                    return RedirectToAction("ProductDetails", "Products", new { id = strID });
                }
            }
        }
    }
}