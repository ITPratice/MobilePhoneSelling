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
    }
}