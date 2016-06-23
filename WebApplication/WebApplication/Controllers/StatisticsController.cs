using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class StatisticsController : Controller
    {
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetJsonData()
        {
            List<ManufacturerStatistic> list = new List<ManufacturerStatistic>();
            List<Manufacturer> manufacturers = db.Manufacturers.ToList();
            foreach (Manufacturer manufacturer in manufacturers)
            {
                var manufacturerId = manufacturer.Id;
                int count = db.Products.Where(p => p.ManufacturerId.Equals(manufacturerId)).Count();
                list.Add(new ManufacturerStatistic(manufacturer.Name, count));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductStatistic()
        {
            List<ProductStatistic> productStatistic = new List<ProductStatistic>();
            List<Product> products = db.Products.ToList();
            foreach (Product product in products)
            {
                int count = 0;
                var productId = product.Id;
                var orderDetail = db.OrderDetails.Where(od => od.ProductId.Equals(productId));
                if (orderDetail.Count() > 0)
                {
                    count = orderDetail.Sum(od => od.Quantity);
                }
                productStatistic.Add(new ProductStatistic(product.Name, count));
            }
            return PartialView(productStatistic);
        }
    }
}