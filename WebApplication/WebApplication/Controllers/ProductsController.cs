using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApplication.Models;
using System.IO;
using WebApplication.Common;
using System.Drawing;

namespace WebApplication.Controllers
{
    public class ProductsController : Controller
    {
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var products = from p in db.Products select p;
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

        #region Admin

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    List<FileUploadModel> uploadFileModel = new List<FileUploadModel>();
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath(Constants.PATH_IMAGE), fileName);
                    if (!System.IO.File.Exists(path))
                    {
                        Bitmap resizedImage = ParamHelper.Instance.ResizeImage(
                            Bitmap.FromStream(fileUpload.InputStream), Constants.IMAGE_WIDTH, Constants.IMAGE_HEIGHT);
                        resizedImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    product.Image = fileName;
                }
                List<Product> products = db.Products.OrderBy(p=>p.Id).ToList();
                int numberOfProducts = products.Count;
                string oldId = "";
                if (numberOfProducts > 0) oldId = products[numberOfProducts - 1].Id;
                product.Id = ParamHelper.Instance.GetNewId(oldId, Constants.PREFIX_PRODUCT);
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                if (ModelState.IsValid)
                {
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        List<FileUploadModel> uploadFileModel = new List<FileUploadModel>();
                        string fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath(Constants.PATH_IMAGE), fileName);
                        if (!System.IO.File.Exists(path))
                        {
                            Bitmap resizedImage = ParamHelper.Instance.ResizeImage(
                                Bitmap.FromStream(fileUpload.InputStream), Constants.IMAGE_WIDTH, Constants.IMAGE_HEIGHT);
                            resizedImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        product.Image = fileName;
                    }
                    product.ModifiedDate = DateTime.Now;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            product.Deleted = true;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Restore(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            product.Deleted = false;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult TechnicalDetail(string id)
        {
            Product product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            return View(product);
        }
        #endregion

        #region Get Details of Products
        public ActionResult ProductDetails(string id = "")
        {

            Product _prod = db.Products.SingleOrDefault(x => x.Id == id);
            if (_prod == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(_prod);
        }
        #endregion

        #region Get Relative Products
        public PartialViewResult RelativeProducts()
        {
            return PartialView();
        }
        #endregion

        #region Get Promotion of Products
        public ActionResult GetPromotion(string id)
        {
            var promotion = from pr in db.Promotions
                            where (from p in db.Promotions
                                   where p.Products.Any(x => x.Id == id)
                                   select p.Id).Contains(pr.Id)
                            select pr.Name;
            if (promotion == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(promotion);
        }
        #endregion

        #region Compare Products
        public List<Product> GetProducts()
        {
            List<Product> _lstProducts = Session["compare"] as List<Product>;
            if (_lstProducts == null)
            {
                _lstProducts = new List<Product>();
                Session["compare"] = _lstProducts;
            }
            return _lstProducts;
        }

        public ActionResult AddProducts(string ProductID, string _strUrl)
        {
            Product _products = db.Products.SingleOrDefault(x => x.Id == ProductID);
            if (_products == null)
            {
                return HttpNotFound();
            }
            List<Product> _lstProducts = GetProducts();
            _lstProducts.Add(_products);
            return Redirect(_strUrl);
        }

        public ActionResult Compare()
        {
            if (Session["compare"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Product> _lstProducts = GetProducts();
            return View(_lstProducts);
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
