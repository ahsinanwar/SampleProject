using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using PagedList;
using WMS.HelperClass;
using WMS.Controllers.Filters;

namespace WMS.Controllers
{
        [CustomControllerAttributes]
    public class LvQuotaController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LvQuota/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.LocationSortParm = sortOrder == "LvType" ? "LvType_desc" : "LvType";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            User LoggedInUser = Session["LoggedUser"] as User;
            ViewBag.CurrentFilter = searchString;
            var lvquotas = db.LvConsumeds.Where(aa => aa.CompanyID == LoggedInUser.CompanyID).Include(l => l.Emp);
            if (!String.IsNullOrEmpty(searchString))
            {
                lvquotas = lvquotas.Where(s => s.Emp.EmpName.ToUpper().Contains(searchString.ToUpper())
                     || s.Emp.EmpNo.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    lvquotas = lvquotas.OrderByDescending(s => s.Emp.EmpName);
                    break;
                default:
                    lvquotas = lvquotas.OrderBy(s => s.Emp.EmpName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(lvquotas.ToPagedList(pageNumber, pageSize));

            //var lvquotas = db.LvQuotas.Include(l => l.Emp);
                //return View(lvquotas.ToList());
        }

        // GET: /LvQuota/Details/5
                 [CustomActionAttribute]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvQuota lvquota = db.LvQuotas.Find(id);
            if (lvquota == null)
            {
                return HttpNotFound();
            }
            return View(lvquota);
        }

        // GET: /LvQuota/Create
                 [CustomActionAttribute]
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo");
            return View();
        }

        // POST: /LvQuota/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "EmpID,A,B,C,D,E,F,G,H,I,J,K,L")] LvQuota lvquota)
        {
            string _EmpNo = Request.Form["EmpNo"].ToString();
            List<Emp> _emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo).ToList();
            if (_emp.Count == 0)
            {
                ModelState.AddModelError("EmpNo", "Emp No not exist");
            }
            else
            {
                lvquota.EmpID = _emp.FirstOrDefault().EmpID;
            }
            if (ModelState.IsValid)
            {
                User LoggedInUser = Session["LoggedUser"] as User;
                lvquota.CompanyID = LoggedInUser.CompanyID;
                db.LvQuotas.Add(lvquota);
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.LeaveQuota, (byte)MyEnums.Operation.Add, DateTime.Now);
                return RedirectToAction("Index");
            }

            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvquota.EmpID);
            return View(lvquota);
        }

        // GET: /LvQuota/Edit/5
                 [CustomActionAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvQuota lvquota = db.LvQuotas.Find(id);
            if (lvquota == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvquota.EmpID);
            return View(lvquota);
        }

        // POST: /LvQuota/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "EmpID,A,B,C,D,E,F,G,H,I,J,K,L")] LvQuota lvquota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lvquota).State = EntityState.Modified;
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.LeaveQuota, (byte)MyEnums.Operation.Edit, DateTime.Now);
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvquota.EmpID);
            return View(lvquota);
        }

        // GET: /LvQuota/Delete/5
                 [CustomActionAttribute]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvQuota lvquota = db.LvQuotas.Find(id);
            if (lvquota == null)
            {
                return HttpNotFound();
            }
            return View(lvquota);
        }

        // POST: /LvQuota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(int id)
        {
            LvQuota lvquota = db.LvQuotas.Find(id);
            db.LvQuotas.Remove(lvquota);
            db.SaveChanges();
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.LeaveQuota, (byte)MyEnums.Operation.Delete, DateTime.Now);
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
