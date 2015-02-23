using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.HelperClass;
using WMS.Models;

namespace WMS.Controllers
{
    public class RosterController : Controller
    {
        //
        // GET: /Roster/
        private TAS2013Entities db = new TAS2013Entities();
        public ActionResult Index()
        {
            ViewBag.RosterType = new SelectList(db.RosterTypes, "ID", "Name");
            ViewBag.ShiftList = new SelectList(db.Shifts, "ShiftID", "ShiftName");
            ViewBag.CrewList = new SelectList(db.Crews, "CrewID", "CrewName");
            ViewBag.SectionList = new SelectList(db.Sections, "SectionID", "SectionName");
            return View();
            //return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            int _RosterType = Convert.ToInt32(Request.Form["RosterType"].ToString());
            DateTime _StartDate = Convert.ToDateTime(Request.Form["dateStart"]);
            DateTime _EndDate = Convert.ToDateTime(Request.Form["dateEnd"]);
            TimeSpan _DutyTime = MyHelper.ConvertTime(Request.Form["dutyTime"]);
            int _WorkMin = Convert.ToInt16(Request.Form["mints"]);
            bool _Repeat = false;
            if (Request.Form["chkBox"].ToString() == "0")
            {
                _Repeat = false;
            }
            else
                _Repeat = true;
            if (IsValid(_RosterType,_StartDate,_EndDate,_Repeat) == "No")
            {
                int RosterApplyValue ;
                switch (Request.Form["cars"].ToString())
                {
                    case "shift":
                        RosterApplyValue = Convert.ToInt32(Request.Form["ShiftList"].ToString());
                        
                        break;
                    case "crew":
                        RosterApplyValue =Convert.ToInt32(Request.Form["CrewList"].ToString());
                        
                        break;
                    case "section":
                        RosterApplyValue = Convert.ToInt32(Request.Form["SectionList"].ToString());
                        
                        break;
                    case "employee":
                        //jobCardApp.CriteriaData = _Emp.FirstOrDefault().EmpID;
                        
                        break;
                }
                return View(CalculateRosterFields(_RosterType, _StartDate, _WorkMin, _DutyTime));
            }
            else
            {
                ViewBag.RosterType = new SelectList(db.RosterTypes, "ID", "Name");
                ViewBag.ShiftList = new SelectList(db.Shifts, "ShiftID", "ShiftName");
                ViewBag.CrewList = new SelectList(db.Crews, "CrewID", "CrewName");
                ViewBag.SectionList = new SelectList(db.Sections, "SectionID", "SectionName");
                return View("Index");
            }
            
        }

        public ActionResult CreateRoster(RosterAttributes form)
        {
            for (int i = 0; i < 7; i++)
            {
                string Time = Request.Form["StudentList["+i.ToString()+"].Date"].ToString();
            }
                
            return View();
        }

        private string IsValid(int _RosterType, DateTime _StartDate, DateTime _EndDate, bool _Repeat)
        {
            string _returnValue = "";
            if (_Repeat == true)
            {
                switch (_RosterType)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                }
            }
            else
            {
                switch (_RosterType)
                {
                    case 1:

                        break;
                    case 2:
                        if ((((_EndDate - _StartDate).Days+1) % 7) == 0)
                        {
                            _returnValue = "No";
                        }
                        else
                        {

                        }
                        break;
                    case 3:
                        if ((((_EndDate - _StartDate).Days + 1) % 15) == 0)
                        {
                            _returnValue = "No";
                        }
                        else
                        {

                        }
                        break;
                    case 4:
                        int endPoint = System.DateTime.DaysInMonth(_StartDate.Year, _StartDate.Month);
                        if ((((_EndDate - _StartDate).Days + 1) % endPoint) == 0)
                        {
                            _returnValue = "No";
                        }
                        else
                        {

                        }
                        break;
                }
            }
            return _returnValue;
        }

        private RosterModel CalculateRosterFields(int _RosterType, DateTime _StartDate,int _WorkMin,TimeSpan _DutyTime)
        {
            int endPoint = 0;
            if (_RosterType == 2)
                endPoint = 7;
            else if (_RosterType == 3)
                endPoint = 15;
            else if (_RosterType == 4)
            {
                endPoint = System.DateTime.DaysInMonth(_StartDate.Year, _StartDate.Month);
            }
            RosterModel _objstudentmodel = new RosterModel();
            _objstudentmodel._RosterAttributes = new List<RosterAttributes>();
            for (int i = 1; i <= endPoint; i++)
            {
                string _day = _StartDate.Date.ToString("dddd");
                string _date = _StartDate.Date.ToString("dd-MMM-yyyy");
                string _DTime = _DutyTime.Hours.ToString("00")+_DutyTime.Minutes.ToString("00");
                _objstudentmodel._RosterAttributes.Add(new RosterAttributes { ID = i, DateString = _date, Day = _day, DutyDate = _StartDate.Date, DutyTimeString = _DTime, DutyTime = _DutyTime, WorkMin = _WorkMin });
                _StartDate =  _StartDate.AddDays(1);
            }
            return _objstudentmodel;
        }
	}
    public class RosterModel
    {
        public List<RosterAttributes> _RosterAttributes { get; set; }
    }
    public class RosterAttributes
    {
        public int ID { get; set; }
        public string DateString { get; set; }
        public string Day { get; set; }
        public DateTime DutyDate { get; set; }
        public int WorkMin { get; set; }
        public string DutyTimeString { get; set; }
        public TimeSpan DutyTime { get; set; }

    }
}