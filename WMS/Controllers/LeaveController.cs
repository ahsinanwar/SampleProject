using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.Controllers
{
    public class LeaveController
    {
        #region -- Add Leaves--
        List<LvQuota> _empQuota = new List<LvQuota>();
        public string AddLeavesToLvApplication()
        {
            string error = "";

            return error;
        }

        //Check Duplication of Leave for a date
        public bool CheckDuplicateLeave(LvApplication lvappl)
        {
            List<LvApplication> _Lv = new List<LvApplication>();
            DateTime _DTime = new DateTime();
            DateTime _DTimeLV = new DateTime();
            using (var context = new TAS2013Entities())
            {
                _Lv = context.LvApplications.Where(aa => aa.EmpID == lvappl.EmpID).ToList();
                foreach (var item in _Lv)
                {
                    _DTime = item.FromDate;
                    _DTimeLV = lvappl.FromDate;
                    while (_DTime <= item.ToDate)
                    {
                        while (_DTimeLV <= lvappl.ToDate)
                        {
                            if (_DTime == _DTimeLV)
                                return false;
                            _DTimeLV = _DTimeLV.AddDays(1);
                        }
                        _DTime = _DTime.AddDays(1);
                    }
                }
            }
            return true;
        }

        public bool CheckLeaveBalance(LvApplication _lvapp)
        {
            _empQuota.Clear();
            int _EmpID;
            decimal RemainingLeaves;
            _EmpID = _lvapp.EmpID;
            using (var context = new TAS2013Entities())
            {
                _empQuota = context.LvQuotas.Where(aa => aa.EmpID == _EmpID).ToList();
                if (_empQuota.Count > 0)
                {
                    switch (_lvapp.LvType)
                    {
                        case "A":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().A;
                            break;
                        case "B":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().B;
                            break;
                        case "C":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().C;
                            break;
                        case "D":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().D;
                            break;
                        case "E":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().E;
                            break;
                        case "F":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().F;
                            break;
                        case "G":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().G;
                            break;
                        case "H":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().H;
                            break;
                        case "I":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().I;
                            break;
                        case "J":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().J;
                            break;
                        case "K":
                            RemainingLeaves = (decimal)_empQuota.FirstOrDefault().K;
                            break;
                        default:
                            RemainingLeaves = 1;
                            break;
                    }
                    if ((RemainingLeaves - Convert.ToDecimal(_lvapp.NoOfDays)) >= 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;

            }
        }

        public bool AddLeaveToLeaveAttData(LvApplication lvappl)
        {
            try
            {
                DateTime datetime = new DateTime();
                datetime = lvappl.FromDate;
                for (int i = 0; i < lvappl.NoOfDays; i++)
                {
                    string _EmpDate = lvappl.EmpID + datetime.Date.ToString("yyMMdd");
                    using (var context = new TAS2013Entities())
                    {
                        if (context.AttProcesses.Where(aa => aa.ProcessDate == datetime).Count() > 0)
                        {
                            AttData _EmpAttData = new AttData();
                            _EmpAttData = context.AttDatas.First(aa => aa.EmpDate == _EmpDate);
                            _EmpAttData.TimeIn = null;
                            _EmpAttData.TimeOut = null;
                            _EmpAttData.WorkMin = null;
                            _EmpAttData.LateIn = null;
                            _EmpAttData.LateOut = null;
                            _EmpAttData.EarlyIn = null;
                            _EmpAttData.EarlyOut = null;
                            _EmpAttData.StatusAB= false;
                            _EmpAttData.StatusLeave = true;
                            _EmpAttData.StatusEI = null;
                            _EmpAttData.StatusEO = null;
                            _EmpAttData.StatusLI = null;
                            _EmpAttData.StatusLI= null;
                            _EmpAttData.StatusLO = null;
                            _EmpAttData.StatusDO = null;
                                _EmpAttData.StatusGZ = null;
                            _EmpAttData.StatusGZOT = null;
                            _EmpAttData.StatusMN = null;
                            _EmpAttData.StatusOD = null;
                            if (lvappl.LvType == "A")//Casual Leave
                                _EmpAttData.Remarks = "[CL]";
                            if (lvappl.LvType == "B")//Sick Leave
                                _EmpAttData.Remarks = "[SL]";
                            if (lvappl.LvType == "C")//Casual Leave
                                _EmpAttData.Remarks = "[AL]";
                            _EmpAttData.StatusAB = false;
                            _EmpAttData.StatusLeave = true;
                            context.SaveChanges();
                        }
                    }
                    datetime = datetime.AddDays(1);
                }
            }
            catch (Exception  ex)
            {
                return false;
            }
            return true;

        }

        public bool AddLeaveToLeaveData(LvApplication lvappl)
        {
            DateTime datetime = new DateTime();
            datetime = lvappl.FromDate;
            for (int i = 0; i < lvappl.NoOfDays; i++)
            {
                string _EmpDate = lvappl.EmpID + datetime.Date.ToString("yyMMdd");
                LvData _LVData = new LvData();
                _LVData.EmpID = lvappl.EmpID;
                _LVData.EmpDate = _EmpDate;
                _LVData.Remarks = lvappl.LvReason;
                _LVData.LvID = lvappl.LvID;
                _LVData.AttDate = datetime.Date;
                _LVData.LvCode = lvappl.LvType;
                try
                {
                    using (var context = new TAS2013Entities())
                    {
                        context.LvDatas.Add(_LVData);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
                datetime = datetime.AddDays(1);
                // Balance Leaves from Emp Table
            }
            BalanceLeaves(lvappl);
            return true;
        }

        public void BalanceLeaves(LvApplication lvappl)
        {
            _empQuota.Clear();
            using (var context = new TAS2013Entities())
            {
                _empQuota = context.LvQuotas.Where(aa => aa.EmpID == lvappl.EmpID).ToList();
                float _NoOfDays = lvappl.NoOfDays;
                if (_empQuota.Count > 0)
                {
                    switch (lvappl.LvType)
                    {
                        case "A":
                            _empQuota.FirstOrDefault().A = (float)(_empQuota.FirstOrDefault().A - _NoOfDays);
                            break;
                        case "B":
                            _empQuota.FirstOrDefault().B = (float)(_empQuota.FirstOrDefault().B - _NoOfDays);
                            break;
                        case "C":
                            _empQuota.FirstOrDefault().C = (float)(_empQuota.FirstOrDefault().C - _NoOfDays);
                            break;
                        case "D":
                            _empQuota.FirstOrDefault().D = (float)(_empQuota.FirstOrDefault().D - _NoOfDays);
                            break;
                        case "E":
                            _empQuota.FirstOrDefault().E = (float)(_empQuota.FirstOrDefault().E - _NoOfDays);
                            break;
                        case "F":
                            _empQuota.FirstOrDefault().F = (float)(_empQuota.FirstOrDefault().F - _NoOfDays);
                            break;
                        case "G":
                            _empQuota.FirstOrDefault().G = (float)(_empQuota.FirstOrDefault().G - _NoOfDays);
                            break;
                        case "H":
                            _empQuota.FirstOrDefault().H = (float)(_empQuota.FirstOrDefault().H - _NoOfDays);
                            break;
                        case "I":
                            _empQuota.FirstOrDefault().I = (float)(_empQuota.FirstOrDefault().I - _NoOfDays);
                            break;
                        case "J":
                            _empQuota.FirstOrDefault().J = (float)(_empQuota.FirstOrDefault().J - _NoOfDays);
                            break;
                        case "K":
                            _empQuota.FirstOrDefault().K = (float)(_empQuota.FirstOrDefault().K - _NoOfDays);
                            break;
                        case "L":
                            _empQuota.FirstOrDefault().L = (float)(_empQuota.FirstOrDefault().L - _NoOfDays);
                            break;
                        default:
                            break;
                    }
                    context.SaveChanges();
                }
            }
        }

        #endregion

        #region -- Add Half Leave--

        public void AddHalfLeaveToLeaveData(LvApplication lvappl)
        {
            DateTime datetime = new DateTime();
            datetime = lvappl.FromDate;
            string _EmpDate = lvappl.EmpID + datetime.Date.ToString("yyMMdd");
            LvData _LVData = new LvData();
            _LVData.EmpID = lvappl.EmpID;
            _LVData.EmpDate = _EmpDate;
            _LVData.Remarks = lvappl.LvReason;
            _LVData.HalfLeave = true;
            _LVData.LvID = lvappl.LvID;
            _LVData.AttDate = datetime.Date;
            _LVData.LvCode = lvappl.LvType;
            try
            {
                using (var db = new TAS2013Entities())
                {
                    db.LvDatas.Add(_LVData);
                    db.SaveChanges(); 
                }
            }
            catch (Exception ex)
            {

            }
            // Balance Leaves from Emp Table
            BalanceLeaves(lvappl);
        }

        public void AddHalfLeaveToAttData(LvApplication lvappl)
        {
            DateTime datetime = new DateTime();
            datetime = lvappl.FromDate;
            string _EmpDate = lvappl.EmpID + datetime.Date.ToString("yyMMdd");
            using (var db = new TAS2013Entities())
            {
                if (db.AttProcesses.Where(aa => aa.ProcessDate == datetime).Count() > 0)
                {
                    AttData _EmpAttData = new AttData();
                    _EmpAttData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
                    if (lvappl.LvType == "A")//Casual Leave
                        _EmpAttData.Remarks = _EmpAttData.Remarks+"[HCL]";
                    if (lvappl.LvType == "B")//Sick Leave
                        _EmpAttData.Remarks = _EmpAttData.Remarks+"[HSL]";
                    if (lvappl.LvType == "C")//Casual Leave
                        _EmpAttData.Remarks = _EmpAttData.Remarks+"[HAL]";
                    _EmpAttData.StatusAB = false;
                    _EmpAttData.StatusLeave = true;
                    _EmpAttData.StatusP = true;
                    _EmpAttData.StatusLeave = true;
                    _EmpAttData.StatusEO = false;
                    _EmpAttData.EarlyOut = 0;
                    _EmpAttData.LateIn = 0;
                    _EmpAttData.StatusLI = false;
                    db.SaveChanges();
                } 
            }
        }

        public bool CheckHalfLeaveBalance(LvApplication lvapplication)
        {
            _empQuota.Clear();
            int _EmpID;
            float RemainingLeaves;
            _EmpID = lvapplication.EmpID;
            using (var context = new TAS2013Entities())
            {
                _empQuota = context.LvQuotas.Where(aa => aa.EmpID == _EmpID).ToList();
                if (_empQuota.Count > 0)
                {
                    switch (lvapplication.LvType)
                    {
                        case "A":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().A;
                            break;
                        case "B":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().B;
                            break;
                        case "C":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().C;
                            break;
                        case "D":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().D;
                            break;
                        case "E":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().E;
                            break;
                        case "F":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().F;
                            break;
                        case "G":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().G;
                            break;
                        case "H":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().H;
                            break;
                        case "I":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().I;
                            break;
                        case "J":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().J;
                            break;
                        case "K":
                            RemainingLeaves = (float)_empQuota.FirstOrDefault().K;
                            break;
                        default:
                            RemainingLeaves = 1;
                            break;
                    }
                    if ((RemainingLeaves - (float)lvapplication.NoOfDays) >= 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;

            }
        }

        #endregion

        #region -- Delete Leaves --
        public void DeleteFromLVData(LvApplication lvappl)
        {
            int _EmpID = lvappl.EmpID;
            DateTime Date = lvappl.FromDate;
            while (Date <= lvappl.ToDate)
            {
                using (var context = new TAS2013Entities())
                {
                    var _id = context.LvDatas.Where(aa => aa.EmpID == _EmpID && aa.AttDate == Date.Date).FirstOrDefault().EmpDate;
                    if (_id != null)
                    {
                        LvData lvvdata = context.LvDatas.Find(_id);
                        lvvdata.Active = false;
                        //context.LvDatas.Remove(lvvdata);
                        context.SaveChanges();
                    }
                }
                Date = Date.AddDays(1);
            }
        }

        public void DeleteLeaveFromAttData(LvApplication lvappl)
        {
            int _EmpID = lvappl.EmpID;
            DateTime Date = lvappl.FromDate;
            while (Date <= lvappl.ToDate)
            {
                using (var context = new TAS2013Entities())
                {
                    if (context.AttProcesses.Where(aa => aa.ProcessDate == Date.Date).Count() > 0)
                    {
                        string _empdate = _EmpID.ToString() + Date.Date.ToString("yyMMdd");
                        var _AttData = context.AttDatas.Where(aa => aa.EmpDate == _empdate);
                        if (_AttData != null)
                        {
                            _AttData.FirstOrDefault().StatusLeave = false;
                            _AttData.FirstOrDefault().Remarks.Replace("[SL]", "");
                            _AttData.FirstOrDefault().Remarks.Replace("[CL]", "");
                            _AttData.FirstOrDefault().Remarks.Replace("[AL]", "");
                            if (_AttData.FirstOrDefault().TimeIn == null && _AttData.FirstOrDefault().TimeOut == null && _AttData.FirstOrDefault().DutyCode == "D")
                            {
                                _AttData.FirstOrDefault().Remarks = "[Absent]";
                                _AttData.FirstOrDefault().StatusAB = true;
                            }
                            //context.LvDatas.Remove(lvvdata);
                            context.SaveChanges();
                        }
                    }
                }
                Date = Date.AddDays(1);
            }
        }

        public void UpdateLeaveBalance(LvApplication lvappl)
        {
            _empQuota.Clear();
            float LvDays = (float)lvappl.NoOfDays;
            using (var context = new TAS2013Entities())
            {
                _empQuota = context.LvQuotas.Where(aa => aa.EmpID == lvappl.EmpID).ToList();
                if (_empQuota.Count > 0)
                {
                    switch (lvappl.LvType)
                    {
                        case "A":
                            _empQuota.FirstOrDefault().A = (float)(_empQuota.FirstOrDefault().A + LvDays);
                            break;
                        case "B":
                            _empQuota.FirstOrDefault().B = (float)(_empQuota.FirstOrDefault().B + LvDays);
                            break;
                        case "C":
                            _empQuota.FirstOrDefault().C = (float)(_empQuota.FirstOrDefault().C + LvDays);
                            break;
                        case "D":
                            _empQuota.FirstOrDefault().D = (float)(_empQuota.FirstOrDefault().D + LvDays);
                            break;
                        case "E":
                            _empQuota.FirstOrDefault().E = (float)(_empQuota.FirstOrDefault().E + LvDays);
                            break;
                        case "F":
                            _empQuota.FirstOrDefault().F = (float)(_empQuota.FirstOrDefault().F + LvDays);
                            break;
                        case "G":
                            _empQuota.FirstOrDefault().G = (float)(_empQuota.FirstOrDefault().G + LvDays);
                            break;
                        case "H":
                            _empQuota.FirstOrDefault().H = (float)(_empQuota.FirstOrDefault().H + LvDays);
                            break;
                        case "I":
                            _empQuota.FirstOrDefault().I = (float)(_empQuota.FirstOrDefault().I + LvDays);
                            break;
                        case "J":
                            _empQuota.FirstOrDefault().J = (float)(_empQuota.FirstOrDefault().J + LvDays);
                            break;
                        case "K":
                            _empQuota.FirstOrDefault().K = (float)(_empQuota.FirstOrDefault().K + LvDays);
                            break;
                        case "L":
                            _empQuota.FirstOrDefault().L = (float)(_empQuota.FirstOrDefault().L + LvDays);
                            break;
                    }
                    context.SaveChanges();
                }
            }
        }

        #endregion

        #region -- Delete Half Leaves --
        public void DeleteHLFromLVData(LvApplication lvappl)
        {
            int _EmpID = lvappl.EmpID;
            DateTime Date = lvappl.FromDate;
            while (Date <= lvappl.ToDate)
            {
                using (var context = new TAS2013Entities())
                {
                    var _id = context.LvDatas.Where(aa => aa.EmpID == _EmpID && aa.AttDate == Date.Date).FirstOrDefault().EmpDate;
                    if (_id != null)
                    {
                        LvData lvvdata = context.LvDatas.Find(_id);
                        lvvdata.Active = false;
                        //context.LvDatas.Remove(lvvdata);
                        context.SaveChanges();
                    }
                }
                Date = Date.AddDays(1);
            }
        }

        public void DeleteHLFromAttData(LvApplication lvappl)
        {
            int _EmpID = lvappl.EmpID;
            DateTime Date = lvappl.FromDate;
            while (Date <= lvappl.ToDate)
            {
                using (var context = new TAS2013Entities())
                {
                    if (context.AttProcesses.Where(aa => aa.ProcessDate == Date.Date).Count() > 0)
                    {
                        string _empdate = _EmpID.ToString() + Date.Date.ToString("yyMMdd");
                        var _AttData = context.AttDatas.Where(aa => aa.EmpDate == _empdate);
                        if (_AttData != null)
                        {
                            _AttData.FirstOrDefault().StatusLeave = false;
                            _AttData.FirstOrDefault().Remarks.Replace("[HSL]", "");
                            _AttData.FirstOrDefault().Remarks.Replace("[HCL]", "");
                            _AttData.FirstOrDefault().Remarks.Replace("[HAL]", "");
                            if (_AttData.FirstOrDefault().TimeIn == null && _AttData.FirstOrDefault().TimeOut == null && _AttData.FirstOrDefault().DutyCode == "D")
                            {
                                _AttData.FirstOrDefault().Remarks = "[Absent]";
                                _AttData.FirstOrDefault().StatusAB = true;
                            }
                            context.SaveChanges();
                        }
                    }
                }
                Date = Date.AddDays(1);
            }
        }

        public void UpdateHLeaveBalance(LvApplication lvappl)
        {
            _empQuota.Clear();
            float LvDays = (float)lvappl.NoOfDays;
            using (var context = new TAS2013Entities())
            {
                _empQuota = context.LvQuotas.Where(aa => aa.EmpID == lvappl.EmpID).ToList();
                if (_empQuota.Count > 0)
                {
                    switch (lvappl.LvType)
                    {
                        case "A":
                            _empQuota.FirstOrDefault().A = (float)(_empQuota.FirstOrDefault().A + LvDays);
                            break;
                        case "B":
                            _empQuota.FirstOrDefault().B = (float)(_empQuota.FirstOrDefault().B + LvDays);
                            break;
                        case "C":
                            _empQuota.FirstOrDefault().C = (float)(_empQuota.FirstOrDefault().C + LvDays);
                            break;
                        case "D":
                            _empQuota.FirstOrDefault().D = (float)(_empQuota.FirstOrDefault().D + LvDays);
                            break;
                        case "E":
                            _empQuota.FirstOrDefault().E = (float)(_empQuota.FirstOrDefault().E + LvDays);
                            break;
                        case "F":
                            _empQuota.FirstOrDefault().F = (float)(_empQuota.FirstOrDefault().F + LvDays);
                            break;
                        case "G":
                            _empQuota.FirstOrDefault().G = (float)(_empQuota.FirstOrDefault().G + LvDays);
                            break;
                        case "H":
                            _empQuota.FirstOrDefault().H = (float)(_empQuota.FirstOrDefault().H + LvDays);
                            break;
                        case "I":
                            _empQuota.FirstOrDefault().I = (float)(_empQuota.FirstOrDefault().I + LvDays);
                            break;
                        case "J":
                            _empQuota.FirstOrDefault().J = (float)(_empQuota.FirstOrDefault().J + LvDays);
                            break;
                        case "K":
                            _empQuota.FirstOrDefault().K = (float)(_empQuota.FirstOrDefault().K + LvDays);
                            break;
                        case "L":
                            _empQuota.FirstOrDefault().L = (float)(_empQuota.FirstOrDefault().L + LvDays);
                            break;
                    }
                    context.SaveChanges();
                }
            }
        }

        #endregion

        #region -- Add Short Leave --
        public void AddShortLeaveToAttData(LvShort lvshort)
        {
            DateTime datetime = new DateTime();
            using (var db = new TAS2013Entities())
            {
                if (db.AttProcesses.Where(aa => aa.ProcessDate == datetime).Count() > 0)
                {
                    AttData _EmpAttData = new AttData();
                    _EmpAttData = db.AttDatas.First(aa => aa.EmpDate == lvshort.EmpDate);
                    _EmpAttData.StatusAB = false;
                    _EmpAttData.StatusSL = true;
                    _EmpAttData.Remarks = _EmpAttData.Remarks + "[Short Leave]";
                    db.SaveChanges();
                }
            }
        }
        #endregion
    }
}