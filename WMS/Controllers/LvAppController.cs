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
using WMS.CustomClass;
using WMS.Controllers.Filters;
using WMS.HelperClass;
namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class LvAppController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LvApp/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "LvType" ? "LvType_desc" : "LvType";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
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
            var lvapplications = db.LvApplications.Where(aa=>aa.CompanyID==LoggedInUser.CompanyID && aa.Active == true).Include(l => l.Emp).Include(l => l.LvType1);
            if (!String.IsNullOrEmpty(searchString))
            {
                lvapplications = lvapplications.Where(s => s.Emp.EmpName.ToUpper().Contains(searchString.ToUpper())
                     || s.LvType1.LvDesc.ToUpper().Contains(searchString.ToUpper())
                     || s.Emp.EmpNo.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    lvapplications = lvapplications.OrderByDescending(s => s.Emp.EmpName);
                    break;

                case "LvType_desc":
                    lvapplications = lvapplications.OrderByDescending(s => s.LvType1.LvDesc);
                    break;
                case "LvType":
                    lvapplications = lvapplications.OrderBy(s => s.LvType1.LvDesc);
                    break;
                case "Date_desc":
                    lvapplications = lvapplications.OrderByDescending(s => s.LvDate);
                    break;
                case "Date":
                    lvapplications = lvapplications.OrderBy(s => s.LvDate);
                    break;
                default:
                    lvapplications = lvapplications.OrderBy(s => s.Emp.EmpName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(lvapplications.ToPagedList(pageNumber, pageSize));
            //return View(readers.ToList());
                //return View(lvapplications.ToList());
        }

        // GET: /LvApp/Details/5
          [CustomActionAttribute]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvApplication lvapplication = db.LvApplications.Find(id);
            if (lvapplication == null)
            {
                return HttpNotFound();
            }
            return View(lvapplication);
        }

        // GET: /LvApp/Create
          [CustomActionAttribute]
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo");
            ViewBag.LvType = new SelectList(db.LvTypes, "LvType1", "LvDesc");
            return View();
        }
        
        // POST: /LvApp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "LvID,LvDate,LvType,EmpID,FromDate,ToDate,NoOfDays,IsHalf,HalfAbsent,LvReason,LvAddress,CreatedBy,ApprovedBy,Status")] LvApplication lvapplication)
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            if (lvapplication.FromDate.Date > lvapplication.ToDate.Date)
                ModelState.AddModelError("FromDate", "From Date should be smaller than To Date");

            string _EmpNo = Request.Form["EmpNo"].ToString();
            List<Emp> _emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo && aa.CompanyID == LoggedInUser.CompanyID).ToList();
            if (_emp.Count == 0 )
            {
                ModelState.AddModelError("EmpNo", "Emp No not exist");
            }
            else
            {
                lvapplication.EmpID = _emp.FirstOrDefault().EmpID;
            }
            if (ModelState.IsValid)
            {
                LeaveController LvProcessController = new LeaveController();
                if (lvapplication.IsHalf != true)
                {
                    lvapplication.NoOfDays = (float)((lvapplication.ToDate - lvapplication.FromDate).TotalDays) + 1;
                    lvapplication.Active = true;
                    if (LvProcessController.CheckDuplicateLeave(lvapplication))
                    {
                        //Check leave Balance
                        if (LvProcessController.CheckLeaveBalance(lvapplication))
                        {
                            
                            lvapplication.LvDate = DateTime.Today;
                            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                            lvapplication.CreatedBy = _userID;
                            lvapplication.CompanyID = LoggedInUser.CompanyID;
                            lvapplication.Active = true;
                            db.LvApplications.Add(lvapplication);
                            if (db.SaveChanges() > 0)
                            {
                                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Leave, (byte)MyEnums.Operation.Add, DateTime.Now);
                                LvProcessController.AddLeaveToLeaveData(lvapplication);
                                LvProcessController.AddLeaveToLeaveAttData(lvapplication);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ModelState.AddModelError("LvType", "There is an error while creating leave.");
                            }
                            
                        }
                        else
                            ModelState.AddModelError("LvType", "Leave Balance Exceeds, Please check the balance");
                    }
                    else
                        ModelState.AddModelError("FromDate", "This Employee already has leave of this date ");
                }
                else
                {
                    lvapplication.NoOfDays = (float)0.5;
                    if (LvProcessController.CheckDuplicateLeave(lvapplication))
                    {
                        if (LvProcessController.CheckHalfLeaveBalance(lvapplication))
                        {
                            lvapplication.LvDate = DateTime.Today;
                            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                            lvapplication.CreatedBy = _userID;
                            lvapplication.CompanyID = LoggedInUser.CompanyID;
                            lvapplication.Active = true;
                            db.LvApplications.Add(lvapplication);
                            if (db.SaveChanges()>0)
                            {
                                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Leave, (byte)MyEnums.Operation.Add, DateTime.Now);
                                LvProcessController.AddHalfLeaveToLeaveData(lvapplication);
                                LvProcessController.AddHalfLeaveToAttData(lvapplication);
                            }
                            
                            return RedirectToAction("Index");
                        }
                        else
                            ModelState.AddModelError("LvType", "Leave Balance Exceeds, Please check the balance");
                    }
                    else
                        ModelState.AddModelError("FromDate", "This Employee already has leave of this date ");
                }
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvapplication.EmpID);
            ViewBag.LvType = new SelectList(db.LvTypes, "LvType1", "LvDesc", lvapplication.LvType);
            return View(lvapplication);
        }

          [CustomActionAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvApplication lvapplication = db.LvApplications.Find(id);
            if (lvapplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvapplication.EmpID);
            ViewBag.LvType = new SelectList(db.LvTypes, "LvType1", "LvDesc", lvapplication.LvType);
            return View(lvapplication);
        }

        // POST: /LvApp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "LvID,LvDate,LvType,EmpID,FromDate,ToDate,NoOfDays,IsHalf,HalfAbsent,LvReason,LvAddress,CreatedBy,ApprovedBy,Status")] LvApplication lvapplication)
        {
            if (ModelState.IsValid)
            {
                User LoggedInUser = Session["LoggedUser"] as User;
                lvapplication.CompanyID = LoggedInUser.CompanyID;
                db.Entry(lvapplication).State = EntityState.Modified;
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Leave, (byte)MyEnums.Operation.Edit, DateTime.Now);
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvapplication.EmpID);
            ViewBag.LvType = new SelectList(db.LvTypes, "LvType1", "LvDesc", lvapplication.LvType);
            return View(lvapplication);
        }

        // GET: /LvApp/Delete/5
          [CustomActionAttribute]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvApplication lvapplication = db.LvApplications.Find(id);
            if (lvapplication == null)
            {
                return HttpNotFound();
            }
            return View(lvapplication);
        }

        // POST: /LvApp/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveController LvProcessController = new LeaveController();
            LvApplication lvapplication = db.LvApplications.Find(id);
            lvapplication.Active = false;
            db.Entry(lvapplication).State = EntityState.Modified;
            db.SaveChanges();
            if (lvapplication.IsHalf == false)
            {
                LvProcessController.DeleteFromLVData(lvapplication);
                LvProcessController.DeleteLeaveFromAttData(lvapplication);
                LvProcessController.UpdateLeaveBalance(lvapplication);
            }
            else
            {
                LvProcessController.DeleteHLFromLVData(lvapplication);
                LvProcessController.DeleteHLFromAttData(lvapplication);
                LvProcessController.UpdateHLeaveBalance(lvapplication);
            }
            //UpdateLeaveBalance(lvapplication);
            //db.LvApplications.Remove(lvapplication);
            //db.SaveChanges();
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Leave, (byte)MyEnums.Operation.Delete, DateTime.Now);
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
