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
    public class OrdersController : Controller
    {
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();

        // GET: Orders
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var orders = from o in db.Orders select o;
            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Customer.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(o => o.Customer.Name);
                    break;
                default:
                    orders = orders.OrderBy(o => o.Customer.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ShowDeliverer(string id, string sortOrder,
            string currentFilter, string searchString, int? page)
        {
            Session["OrderId"] = id;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var staffs = from s in db.Staffs select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                staffs = staffs.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    staffs = staffs.OrderByDescending(s => s.Name);
                    break;
                default:
                    staffs = staffs.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(staffs.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChooseDeliverer(string staffId)
        {
            string orderId = Session["OrderId"].ToString();
            Delivery delivery = new Delivery();
            List<Delivery> deliveries = db.Deliveries.ToList();
            string oldId = "";
            if (deliveries.Count > 0) oldId = deliveries[deliveries.Count - 1].Id;
            delivery.Id = ParamHelper.Instance.GetNewId(oldId, Constants.PREFIX_DELIVERY);
            delivery.OrderId = orderId;
            delivery.Date = DateTime.Now;
            delivery.StaffId = staffId;
            db.Deliveries.Add(delivery);
            db.SaveChanges();
            Order order = db.Orders.Find(orderId);
            order.IsDelivered = true;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Orders/Details/5
        public ActionResult Details(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var orderDetails = from o in db.OrderDetails select o;
            if (!String.IsNullOrEmpty(searchString))
            {
                orderDetails = orderDetails.Where(o => o.Product.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orderDetails = orderDetails.OrderByDescending(o => o.Product.Name);
                    break;
                default:
                    orderDetails = orderDetails.OrderBy(o => o.Product.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orderDetails.ToPagedList(pageNumber, pageSize));
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,CustomerId,Deleted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.Deleted = true;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return View(order);
        }

        public ActionResult DeleteDetail(string orderId, string productId)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(orderId, productId);
            db.OrderDetails.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("Details");
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
