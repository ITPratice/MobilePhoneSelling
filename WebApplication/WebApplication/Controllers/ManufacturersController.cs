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

        public ActionResult ProductsByManufacturerId(string _manuId, double minPrice = 0, double maxPrice = 10000)
        {
            Manufacturer _manu = db.Manufacturers.SingleOrDefault(x => x.Id == _manuId);
            string filter = Request.Form["rdFilter"];
            if (_manu == null)
            {
                return HttpNotFound();
            }

            #region Filter Product
            switch (filter)
            {
                case "from100":
                    minPrice = 0;
                    maxPrice = 100;
                    Session["Filter"] = "from100";
                    break;
                case "100to300":
                    minPrice = 100;
                    maxPrice = 300;
                    Session["Filter"] = "100to300";
                    break;
                case "300to700":
                    minPrice = 300;
                    maxPrice = 700;
                    Session["Filter"] = "300to700";
                    break;
                case "700to1000":
                    minPrice = 700;
                    maxPrice = 1000;
                    Session["Filter"] = "700to1000";
                    break;
                case "from1000":
                    minPrice = 1000;
                    maxPrice = 10000;
                    Session["Filter"] = "from1000";
                    break;
                default:
                    minPrice = 0;
                    maxPrice = 10000;
                    Session["Filter"] = null;
                    break;
            }
            #endregion

            List<Product> _lstProduct = db.Products.Where(x => x.ManufacturerId == _manuId && x.Price <= maxPrice && x.Price >= minPrice).OrderBy(x => x.Price).ToList();
            if (_lstProduct != null)
            {
                return View(_lstProduct);
            }
            return View(_lstProduct);
        }

        public void FilerProducts(string filter, double minPrice, double maxPrice)
        {
            switch (filter)
            {
                case "from100":
                    minPrice = 0;
                    maxPrice = 100;
                    Session["Filter"] = "from100";
                    break;
                case "100to300":
                    minPrice = 100;
                    maxPrice = 300;
                    Session["Filter"] = "100to300";
                    break;
                case "300to700":
                    minPrice = 300;
                    maxPrice = 700;
                    Session["Filter"] = "300to700";
                    break;
                case "700to1000":
                    minPrice = 700;
                    maxPrice = 1000;
                    Session["Filter"] = "700to1000";
                    break;
                case "from1000":
                    minPrice = 1000;
                    maxPrice = 10000;
                    Session["Filter"] = "from1000";
                    break;
                default:
                    minPrice = 0;
                    maxPrice = 10000;
                    Session["Filter"] = null;
                    break;
            }
        }
    }
}