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
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /LeaveSettings/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LeaveSettings/Create
        public ActionResult AddLeaveQuota()
        {
            int AL = Convert.ToInt32(Request.Form["ALeaves"].ToString());
            int CL = Convert.ToInt32(Request.Form["CLeaves"].ToString());
            int SL = Convert.ToInt32(Request.Form["SLeaves"].ToString());
            using (var ctx = new TAS2013Entities())
            {
                List<LvQuota> _LvQuota = new List<LvQuota>();
                List<LvQuota> _TemLvQuota = new List<LvQuota>();
                _LvQuota = ctx.LvQuotas.ToList();
                List<Emp> _Emps = new List<Emp>();
                _Emps = ctx.Emps.Where(aa => aa.Status == true).ToList();
                foreach (var emp in _Emps)
                {
                    _TemLvQuota = _LvQuota.Where(aa => aa.EmpID == emp.EmpID).ToList();
                    if (_TemLvQuota.Count() > 0)
                    {
                        _TemLvQuota.FirstOrDefault().A = AL;
                        _TemLvQuota.FirstOrDefault().B = CL;
                        _TemLvQuota.FirstOrDefault().C = SL;
                        _TemLvQuota.FirstOrDefault().TA = AL + _TemLvQuota.FirstOrDefault().TA;
                        _TemLvQuota.FirstOrDefault().TB = CL + _TemLvQuota.FirstOrDefault().TB;
                        _TemLvQuota.FirstOrDefault().TC = SL + _TemLvQuota.FirstOrDefault().TC;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        LvQuota _lvQuota = new LvQuota();
                        _lvQuota.EmpID = emp.EmpID;
                        _lvQuota.A = AL;
                        _lvQuota.B = CL;
                        _lvQuota.C = SL;
                        _lvQuota.TA = AL;
                        _lvQuota.TB = CL;
                        _lvQuota.TC = SL;
                        ctx.LvQuotas.Add(_lvQuota);
                        ctx.SaveChanges();
                    }
                }
                ctx.Dispose();
            }
            return View("Index");
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
