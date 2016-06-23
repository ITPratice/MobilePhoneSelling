using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using PagedList;
using WebApplication.Common;
using System.Net;
using System.Data.Entity;

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

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var manufacturers = from m in db.Manufacturers select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                manufacturers = manufacturers.Where(m => m.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    manufacturers = manufacturers.OrderByDescending(m => m.Name);
                    break;
                default:
                    manufacturers = manufacturers.OrderBy(m => m.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(manufacturers.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                List<Manufacturer> manufacturers = db.Manufacturers.OrderBy(m => m.Id).ToList();
                string oldId = "";
                if (manufacturers.Count > 0) oldId = manufacturers[manufacturers.Count - 1].Id;
                manufacturer.Id = ParamHelper.Instance.GetNewId(oldId, Constants.PREFIX_MANUFACTURER);
                db.Manufacturers.Add(manufacturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturer manufacturer = db.Manufacturers.Find(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            return View(manufacturer);
        }

        [HttpPost]
        public ActionResult Edit(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manufacturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }
    }
}