using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using PagedList;
using WebApplication.Common;

namespace WebApplication.Controllers
{
    public class PromotionsController : Controller
    {
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();

        // GET: Promotions
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var promotions = from p in db.Promotions select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                promotions = promotions.Where(p => p.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    promotions = promotions.OrderByDescending(p => p.Name);
                    break;
                default:
                    promotions = promotions.OrderBy(p => p.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(promotions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Promotions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Promotions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                List<Promotion> promotions = db.Promotions.OrderBy(pr=>pr.Id).ToList();
                string oldId = "";
                if (promotions.Count > 0) oldId = promotions[promotions.Count - 1].Id;
                promotion.Id = ParamHelper.Instance.GetNewId(oldId, Constants.PREFIX_PROMOTION);
                promotion.CreatedDate = DateTime.Now;
                promotion.ModifiedDate = DateTime.Now;
                promotion.Deleted = false;
                db.Promotions.Add(promotion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(promotion);
        }

        // GET: Promotions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                promotion.ModifiedDate = DateTime.Now;
                db.Entry(promotion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(promotion);
        }

        // GET: Promotions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            promotion.Deleted = true;
            db.Entry(promotion).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Restore(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            promotion.Deleted = false;
            db.Entry(promotion).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region ProductPromotion
        public ViewResult Details(string promotionId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.PromotionId = promotionId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var products = from p in db.Products
                           from pr in p.Promotions.Where(tmp => tmp.Id == promotionId)
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChooseProducts(string promotionId, string sortOrder, string searchString)
        {
            ViewBag.PromotionId = promotionId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var products = from p in db.Products
                           where !(from pr in db.Products
                                   where pr.Promotions.Any(pro => pro.Id == promotionId)
                                   select pr.Id).Contains(p.Id)
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult AddProducts(string promotionId)
        {
            if (!String.IsNullOrEmpty(Request["ChooseProducts"]))
            {
                foreach (var productId in Request["ChooseProducts"].Split(','))
                {
                    Promotion promotion = db.Promotions.Find(promotionId);
                    Product product = db.Products.Find(productId);
                    promotion.Products.Add(product);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteProduct(string promotionId, string productId)
        {
            Promotion promotion = db.Promotions.Find(promotionId);
            Product product = db.Products.Find(productId);
            promotion.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
