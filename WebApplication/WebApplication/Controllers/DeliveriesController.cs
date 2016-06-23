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

namespace WebApplication.Controllers
{
    public class DeliveriesController : Controller
    {
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();

        // GET: Deliveries
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var deliveries = db.Deliveries.Include(d => d.Order).Include(d => d.Staff);
            if (!String.IsNullOrEmpty(searchString))
            {
                deliveries = deliveries.Where(d => d.Staff.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    deliveries = deliveries.OrderByDescending(d => d.Date);
                    break;
                default:
                    deliveries = deliveries.OrderBy(d => d.Date);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(deliveries.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult ChooseOrders()
        {
            if (!String.IsNullOrEmpty(Request["ChooseOrders"]))
            {
                foreach (var orderId in Request["ChooseOrders"].Split(','))
                {
                    Delivery delivery = db.Deliveries.Find(orderId);
                    if (delivery != null)
                    {
                        delivery.IsDelivered = true;
                        db.Entry(delivery).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return new Rotativa.ViewAsPdf("Prints", delivery);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Deliveries/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // GET: Deliveries/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "CustomerId");
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name");
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderId,Date,StaffId")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Deliveries.Add(delivery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "Id", "CustomerId", delivery.OrderId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", delivery.StaffId);
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            Order order = delivery.Order;
            List<OrderDetail> orderDetails = order.OrderDetails.ToList();
            db.OrderDetails.RemoveRange(orderDetails);
            db.Orders.Remove(order);
            db.Deliveries.Remove(delivery);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
