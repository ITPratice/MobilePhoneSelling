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

        public ActionResult ProductsByManufacturerId(string _manuId = "", double minPrice = 0, double maxPrice = 100000000)
        {
            Manufacturer _manu = db.Manufacturers.SingleOrDefault(x => x.Id == _manuId);
            if(_manu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //FilterProducts(minPrice, maxPrice);
            List<Product> _lstProduct = db.Products.Where(x => x.ManufacturerId == _manuId && x.Price <= maxPrice && x.Price >= minPrice).OrderBy(x => x.Price).ToList();
            if (_lstProduct != null)
            {
                return View(_lstProduct);
            }
            return View();
        }

        public void FilterProducts(double minPrice, double maxPrice)
        {
            string filter = Request["chkFilter"];
            if (filter.Equals("chk1"))
            {
                minPrice = 0;
                maxPrice = 1000000;
            }
            if (filter.Equals("chk1to3"))
            {
                minPrice = 1000000;
                maxPrice = 3000000;
            }
            if (filter.Equals("chk3to7"))
            {
                minPrice = 3000000;
                maxPrice = 7000000;
            }
            if (filter.Equals("chk7to10"))
            {
                minPrice = 7000000;
                maxPrice = 10000000;
            }
            if(filter.Equals("chk10"))
            {
                minPrice = 10000000;
                maxPrice = 100000000;
            }
        }
    }
}