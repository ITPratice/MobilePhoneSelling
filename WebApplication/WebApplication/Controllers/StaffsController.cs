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
    public class StaffsController : Controller
    {
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = currentFilter;
            var staffs = from s in db.Staffs select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                staffs = staffs.Where(p => p.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    staffs = staffs.OrderByDescending(p => p.Name);
                    break;
                default:
                    staffs = staffs.OrderBy(p => p.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(staffs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Staffs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staffs/Create
        public ActionResult Create()
        {
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                List<Staff> staffs = db.Staffs.ToList();
                string oldId = "";
                if (staffs.Count > 0) oldId = staffs[staffs.Count - 1].Id;
                staff.Id = ParamHelper.Instance.GetNewId(oldId, Constants.PREFIX_STAFF);
                db.Staffs.Add(staff);
                db.SaveChanges();
                Account account = new Account();
                oldId = "";
                List<Account> accounts = db.Accounts.ToList();
                if (accounts.Count > 0) oldId = accounts[accounts.Count - 1].Id;
                account.Id = ParamHelper.Instance.GetNewId(oldId, Constants.PREFIX_ACCOUNT);
                account.Name = "DCM" + account.Id.Substring(Constants.PREFIX_ACCOUNT.Length);
                account.Password = account.Name;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", staff.PositionId);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", staff.PositionId);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Birthday,PhoneNumber,Address,Email,Position,AccountId,Deleted")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", staff.PositionId);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Staff staff = db.Staffs.Find(id);
            staff.Deleted = true;
            db.Entry(staff).State = EntityState.Modified;
            db.SaveChanges();
            Account account = staff.Account;
            account.Deleted = true;
            db.Entry(account).State = EntityState.Modified;
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
