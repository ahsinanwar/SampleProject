using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class LeaveSettingsController : Controller
    {
        //
        // GET: /LeaveSettings/
        //[HttpPost]

        TAS2013Entities db = new TAS2013Entities();
        public ActionResult Index()
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            ViewBag.EmployeeCat = new SelectList(db.Categories.Where(aa=>aa.CompanyID == LoggedInUser.CompanyID).ToList(), "CatID", "CatName");
            return View();
        }

        [HttpPost]
        public ActionResult CreateLeaveQuota()
        {
            int AL = Convert.ToInt32(Request.Form["ALeaves"].ToString());
            int CL = Convert.ToInt32(Request.Form["CLeaves"].ToString());
            int SL = Convert.ToInt32(Request.Form["SLeaves"].ToString());
            List<Emp> _Emp = new List<Emp>();
            List<LvType> _lvType = new List<LvType>();
            string empNo = Request.Form["EmpNo"].ToString();
            _Emp = db.Emps.Where(aa=>aa.EmpNo==empNo).ToList();
            User LoggedInUser = Session["LoggedUser"] as User;
            ViewBag.EmployeeCat = new SelectList(db.Categories.Where(aa => aa.CompanyID == LoggedInUser.CompanyID).ToList(), "CatID", "CatName");
            if (_Emp.Count > 0)
            {
                _lvType = db.LvTypes.Where(aa => aa.Enable == true).ToList();
                GenerateLeaveQuotaAttributes(_Emp, _lvType, AL, CL, SL);
                ViewBag.CMessage = "Leave Balance is created";
                return View("Index");
            }
            else
            {
                ViewBag.CMessage = "Employee No "+Request.Form["EmpNo"].ToString()+" not found";
                return View("Index");
            }
        }
        //
        // GET: /LeaveSettings/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LeaveSettings/Create
        [HttpPost]
        public ActionResult AddLeaveQuota(string EmployeeCat)
        {
            //int AL = Convert.ToInt32(Request.Form["ALeaves"].ToString());
            //int CL = Convert.ToInt32(Request.Form["CLeaves"].ToString());
            //int SL = Convert.ToInt32(Request.Form["SLeaves"].ToString());
            //using (var ctx = new TAS2013Entities())
            //{
            //    User LoggedInUser = Session["LoggedUser"] as User;
            //    List<LvConsumed> _LvConsumed = new List<LvConsumed>();
            //    List<LvConsumed> _TemLvQuota = new List<LvConsumed>();
            //    _LvConsumed = ctx.LvConsumeds.Where(aa => aa.CompanyID == LoggedInUser.CompanyID).ToList();
            //    List<Emp> _Emps = new List<Emp>();
            //    _Emps = ctx.Emps.Where(aa => aa.Status == true && aa.CompanyID == LoggedInUser.CompanyID).ToList();
            //    List<LvType> _lvType = new List<LvType>();
            //    _lvType = ctx.LvTypes.Where(aa => aa.Enable == true && aa.CompanyID == LoggedInUser.CompanyID).ToList();
            //    foreach (var emp in _Emps)
            //    {
            //        foreach (var lvType in _lvType)
            //        {
            //            string empType = emp.EmpID.ToString() + lvType.LvType1;
            //            _TemLvQuota = _LvConsumed.Where(aa => aa.EmpLvType == empType).ToList();
            //            switch (lvType.LvType1)
            //            {
            //                case "A"://CL
            //                    _TemLvQuota.FirstOrDefault().TotalForYear = CL;
            //                    _TemLvQuota.FirstOrDefault().YearRemaining = CL;
            //                    _TemLvQuota.FirstOrDefault().GrandTotal = _TemLvQuota.FirstOrDefault().GrandTotal + CL;
            //                    _TemLvQuota.FirstOrDefault().GrandTotalRemaining = _TemLvQuota.FirstOrDefault().GrandTotalRemaining + CL;
            //                    _TemLvQuota.FirstOrDefault().JanConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().FebConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().MarchConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().AprConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().MayConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().JuneConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().JulyConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().AugustConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().SepConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().OctConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().NovConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().DecConsumed = 0;
            //                    break;
            //                case "B"://AL
            //                    _TemLvQuota.FirstOrDefault().TotalForYear = AL;
            //                    _TemLvQuota.FirstOrDefault().YearRemaining = AL;
            //                    _TemLvQuota.FirstOrDefault().GrandTotal = _TemLvQuota.FirstOrDefault().GrandTotal + AL;
            //                    _TemLvQuota.FirstOrDefault().GrandTotalRemaining = _TemLvQuota.FirstOrDefault().GrandTotalRemaining + AL;
            //                    _TemLvQuota.FirstOrDefault().JanConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().FebConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().MarchConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().AprConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().MayConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().JuneConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().JulyConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().AugustConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().SepConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().OctConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().NovConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().DecConsumed = 0;
            //                    break;
            //                case "C"://SL
            //                    _TemLvQuota.FirstOrDefault().TotalForYear = SL;
            //                    _TemLvQuota.FirstOrDefault().YearRemaining = SL;
            //                    _TemLvQuota.FirstOrDefault().GrandTotal = _TemLvQuota.FirstOrDefault().GrandTotal + SL;
            //                    _TemLvQuota.FirstOrDefault().GrandTotalRemaining = _TemLvQuota.FirstOrDefault().GrandTotalRemaining + SL;
            //                    _TemLvQuota.FirstOrDefault().JanConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().FebConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().MarchConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().AprConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().MayConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().JuneConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().JulyConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().AugustConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().SepConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().OctConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().NovConsumed = 0;
            //                    _TemLvQuota.FirstOrDefault().DecConsumed = 0;
            //                    break;
            //            }
            //            ctx.SaveChanges();
            //        }
            //    }
            //    ctx.Dispose();
            //}
            return View("Index");
        }

        public void GenerateLeaveQuotaAttributes(List<Emp> _emp, List<LvType> _lvType,int AL,int CL,int SL)
        {
            using (var ctx = new TAS2013Entities())
            {
                foreach (var emp in _emp)
                {
                    foreach (var lvType in _lvType)
                    {
                        string empType = emp.EmpID.ToString() + lvType.LvType1;
                        LvConsumed lvConsumed = new LvConsumed();
                        lvConsumed.EmpLvType = empType;
                        lvConsumed.EmpID = emp.EmpID;
                        lvConsumed.LeaveType = lvType.LvType1;
                        lvConsumed.JanConsumed= 0;
                        lvConsumed.FebConsumed= 0;
                        lvConsumed.MarchConsumed= 0;
                        lvConsumed.AprConsumed= 0;
                        lvConsumed.MayConsumed= 0;
                        lvConsumed.JuneConsumed= 0;
                        lvConsumed.JulyConsumed= 0;
                        lvConsumed.AugustConsumed= 0;
                        lvConsumed.SepConsumed= 0;
                        lvConsumed.OctConsumed= 0;
                        lvConsumed.NovConsumed= 0;
                        lvConsumed.DecConsumed= 0;
                        lvConsumed.CompanyID = emp.CompanyID;
                        switch (lvType.LvType1)
                        {
                            case "A"://CL
                                lvConsumed.TotalForYear = CL;
                                lvConsumed.YearRemaining = CL;
                                lvConsumed.GrandTotal = CL;
                                lvConsumed.GrandTotalRemaining = CL;
                                break;
                            case "B"://AL
                                lvConsumed.TotalForYear = AL;
                                lvConsumed.YearRemaining = AL;
                                lvConsumed.GrandTotal = AL;
                                lvConsumed.GrandTotalRemaining = AL;
                                break;
                            case "C"://SL
                                lvConsumed.TotalForYear = SL;
                                lvConsumed.YearRemaining = SL;
                                lvConsumed.GrandTotal = SL;
                                lvConsumed.GrandTotalRemaining = SL;
                                break;
                        }
                        ctx.LvConsumeds.Add(lvConsumed);
                        ctx.SaveChanges();

                    }
                }

                ctx.Dispose();
            }

        }
        //
        // POST: /LeaveSettings/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /LeaveSettings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /LeaveSettings/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /LeaveSettings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /LeaveSettings/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
