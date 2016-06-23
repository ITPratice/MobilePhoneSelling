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
                List<Staff> staffs = db.Staffs.OrderBy(s => s.Id).ToList();
                string oldStaffId = "";
                if (staffs.Count > 0) oldStaffId = staffs[staffs.Count - 1].Id;
                staff.Id = ParamHelper.Instance.GetNewId(oldStaffId, Constants.PREFIX_STAFF);
                string oldAccountId = "";
                List<Account> accounts = db.Accounts.OrderBy(a => a.Id).ToList();
                if (accounts.Count > 0) oldAccountId = accounts[accounts.Count - 1].Id;
                Account account = new Account();
                account.Id = ParamHelper.Instance.GetNewId(oldAccountId, Constants.PREFIX_ACCOUNT);
                account.Name = Constants.PREFIX_STAFF_ACCOUNT_NAME +
                    account.Id.Substring(Constants.PREFIX_ACCOUNT.Length);
                account.Password = ParamHelper.Instance.MD5Hash(account.Name);
                db.Accounts.Add(account);
                db.SaveChanges();
                staff.AccountId = account.Id;
                db.Staffs.Add(staff);
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
            DateTime now = DateTime.Now;
            staff.Birthday = now;
            ViewBag.PositionId = new SelectList(db.Positions, "Id", "Name", staff.PositionId);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Staff staff)
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
        [HttpGet]
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
            staff.Deleted = true;
            staff.Account.Deleted = true;
            db.Entry(staff.Account).State = EntityState.Modified;
            db.Entry(staff).State = EntityState.Modified;
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
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            staff.Deleted = false;
            staff.Account.Deleted = false;
            db.Entry(staff.Account).State = EntityState.Modified;
            db.Entry(staff).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditProfile(string id)
        {
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        [HttpPost]
        public ActionResult EditProfile(Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(staff);
        }

        public ActionResult ChangePassword(string id)
        {
            Account account = db.Accounts.Find(id);
            return View(account);
        }

        [HttpPost]
        public ActionResult ChangePassword(string id, string password)
        {
            Account account = db.Accounts.Find(id);
            account.Password = ParamHelper.Instance.MD5Hash(password);
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
            if (Session[Constants.SESSION_ROLE] == null || String.IsNullOrEmpty(Session[Constants.SESSION_ROLE].ToString()))
                return RedirectToAction("Index", "Home");
            if (Session[Constants.SESSION_ROLE].Equals("Admin"))
                return RedirectToAction("Index", "Products");
            if (Session[Constants.SESSION_ROLE].Equals("Nhân viên giao dịch"))
                return RedirectToAction("Index", "Orders");
            return RedirectToAction("Index", "Deliverires");
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
