using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ManufacturersController : Controller
    {
        MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        // GET: Manufacturers
        public ActionResult ManufacturersPartial()
        {
            return PartialView(db.Manufacturers.Take(10).ToList());
        }

        public ActionResult ProductsByManufacturerId(string _manuId = "")
        {
            Manufacturer _manu = db.Manufacturers.SingleOrDefault(x => x.Id == _manuId);

            if (_manu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Product> _lstProduct = db.Products.Where(x => x.ManufacturerId == _manuId).OrderBy(x => x.Price).ToList();
            if (_lstProduct != null)
            {
                return View(_lstProduct);
            }
            return View();
        }
    }
}