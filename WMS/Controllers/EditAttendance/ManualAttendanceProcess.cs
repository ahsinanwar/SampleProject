using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Controllers.Filters;
using WMS.Models;

namespace WMS.Controllers.EditAttendance
{
    
    public class ManualAttendanceProcess
    {
        TAS2013Entities context = new TAS2013Entities();
        AttData _OldAttData = new AttData();
        AttData _NewAttData = new AttData();
        AttDataManEdit _ManualEditData = new AttDataManEdit();

        public ManualAttendanceProcess(string EmpDate,string JobCardName,bool JobCardStatus,DateTime NewTimeIn, DateTime NewTimeOut,string NewDutyCode,int _UserID,TimeSpan _NewDutyTime , string _Remarks)
        {
            _OldAttData = context.AttDatas.First(aa => aa.EmpDate == EmpDate);
            if (_OldAttData != null)
            {
                if (JobCardStatus == false)
                {
                    SaveOldAttData(_OldAttData, _UserID);
                    if (SaveNewAttData(NewTimeIn, NewTimeOut, NewDutyCode,_NewDutyTime,_Remarks))
                    {
                        _OldAttData.TimeIn = NewTimeIn;
                        _OldAttData.TimeOut = NewTimeOut;
                        _OldAttData.DutyCode = NewDutyCode;
                        _OldAttData.DutyTime = _NewDutyTime;
                        _OldAttData.WorkMin = 0;
                        _OldAttData.GZOTMin = 0;
                        _OldAttData.OTMin = 0;
                        _OldAttData.LateIn = 0;
                        _OldAttData.EarlyIn = 0;
                        _OldAttData.EarlyOut = 0;
                        _OldAttData.LateOut = 0;
                        _OldAttData.StatusAB = false;
                        _OldAttData.StatusP = true;
                        _OldAttData.StatusEO = null;
                        _OldAttData.StatusEI = null;
                        _OldAttData.StatusOT = null;
                        _OldAttData.StatusLI = null;
                        _OldAttData.StatusLO = null;
                        _OldAttData.StatusDO = null;
                        _OldAttData.StatusGZOT = null;
                        _OldAttData.StatusMN = true;
                        _OldAttData.Remarks = "";
                        if (NewDutyCode == "G")
                            _OldAttData.StatusGZ = true;
                        else
                            _OldAttData.StatusGZ = false;
                        if (NewDutyCode == "R")
                            _OldAttData.StatusDO = true;
                        else
                            _OldAttData.StatusDO = false;
                        if (_Remarks != "")
                            _OldAttData.Remarks = "[" + _Remarks + "]";
                        _OldAttData.StatusLeave = null;
                        ProcessDailyAttendance(_OldAttData);
                    }
                }
            }
        }

        private bool SaveNewAttData(DateTime _NewTimeIn, DateTime _NewTimeOut, string _NewDutyCode,TimeSpan _NewDutyTime,string _remarks)
        {
            bool check = false;
            _ManualEditData.NewTimeIn = _NewTimeIn;
            _ManualEditData.NewTimeOut = _NewTimeOut;
            _ManualEditData.NewDutyCode = _NewDutyCode;
            _ManualEditData.EditDateTime = DateTime.Now;
            _ManualEditData.NewDutyTime = _NewDutyTime;
            _ManualEditData.NewRemarks = "["+_remarks+"]";
            try
            {
                context.AttDataManEdits.Add(_ManualEditData);
                context.SaveChanges();
                check =  true;
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        private void SaveOldAttData(AttData _OldAttData,int Userid)
        {
            _ManualEditData.OldDutyCode = _OldAttData.DutyCode;
            _ManualEditData.OldTimeIn = _OldAttData.TimeIn;
            _ManualEditData.OldTimeOut = _OldAttData.TimeOut;
            _ManualEditData.EmpDate = _OldAttData.EmpDate;
            _ManualEditData.UserID = Userid;
            _ManualEditData.EditDateTime = DateTime.Now;
            _ManualEditData.EmpID = _OldAttData.EmpID;
            _ManualEditData.OldRemarks = _OldAttData.Remarks;
        }

        #region --Process Daily Attendance--
        public void ProcessDailyAttendance(AttData _attData)
        {
            try
            {
                AttData attendanceRecord = _attData;
                Emp employee = attendanceRecord.Emp;
                Shift shift = employee.Shift;
                //If TimeIn and TimeOut are not null, then calculate other Atributes
                if (attendanceRecord.TimeIn != null && attendanceRecord.TimeOut != null)
                    {
                        //If TimeIn = TimeOut then calculate according to DutyCode
                        if (attendanceRecord.TimeIn == attendanceRecord.TimeOut)
                        {
                            CalculateInEqualToOut(attendanceRecord);
                        }
                        else
                        {
                            if (shift.OpenShift == true)
                            {
                                CalculateOpenShiftTimes(attendanceRecord, shift);
                            }
                            else
                            {
                                if (attendanceRecord.TimeIn.Value.Date.Day == attendanceRecord.TimeOut.Value.Date.Day)
                                {
                                    CalculateShiftTimes(attendanceRecord, shift);
                                }
                                else
                                {
                                    CalculateOpenShiftTimes(attendanceRecord, shift);
                                }
                            }
                        }
                        
                    }
            }
            catch (Exception ex)
            {
            }

            context.SaveChanges();
            context.Dispose();
        }

        private void CalculateInEqualToOut(AttData attendanceRecord)
        {
            switch (attendanceRecord.DutyCode)
            {
                case "G":
                    attendanceRecord.StatusAB = false;
                    attendanceRecord.StatusGZ = true;
                    attendanceRecord.WorkMin = 0;
                    attendanceRecord.EarlyIn = 0;
                    attendanceRecord.EarlyOut = 0;
                    attendanceRecord.LateIn = 0;
                    attendanceRecord.LateOut = 0;
                    attendanceRecord.OTMin = 0;
                    attendanceRecord.GZOTMin = 0;
                    attendanceRecord.StatusGZOT = false;
                    attendanceRecord.TimeIn = null;
                    attendanceRecord.TimeOut = null;
                    break;
                case "R":
                    attendanceRecord.StatusAB = false;
                    attendanceRecord.StatusGZ = false;
                    attendanceRecord.WorkMin = 0;
                    attendanceRecord.EarlyIn = 0;
                    attendanceRecord.EarlyOut = 0;
                    attendanceRecord.LateIn = 0;
                    attendanceRecord.LateOut = 0;
                    attendanceRecord.OTMin = 0;
                    attendanceRecord.GZOTMin = 0;
                    attendanceRecord.StatusGZOT = false;
                    attendanceRecord.TimeIn = null;
                    attendanceRecord.TimeOut = null;
                    attendanceRecord.StatusDO = true;
                    break;
                case "D":
                    attendanceRecord.StatusAB = true;
                    attendanceRecord.StatusGZ = false;
                    attendanceRecord.WorkMin = 0;
                    attendanceRecord.EarlyIn = 0;
                    attendanceRecord.EarlyOut = 0;
                    attendanceRecord.LateIn = 0;
                    attendanceRecord.LateOut = 0;
                    attendanceRecord.OTMin = 0;
                    attendanceRecord.GZOTMin = 0;
                    attendanceRecord.StatusGZOT = false;
                    attendanceRecord.TimeIn = null;
                    attendanceRecord.TimeOut = null;
                    attendanceRecord.StatusDO = false;
                    break;
            }
        }
        TimeSpan OpenShiftThresholdStart = new TimeSpan(17, 00, 00);

        TimeSpan OpenShiftThresholdEnd = new TimeSpan(11, 00, 00);

        private void CalculateTimeINOUTOpenShift(AttData attendanceRecord, PollData up)
        {
            try
            {
                switch (up.RdrDuty)
                {
                    case 1: //IN
                        if (attendanceRecord.Tin0 == null)
                        {
                            if (up.EntTime.TimeOfDay < OpenShiftThresholdEnd)
                            {
                                DateTime dt = new DateTime();
                                dt = up.EntDate.Date.AddDays(-1);
                                var _attData = context.AttDatas.FirstOrDefault(aa => aa.AttDate == dt && aa.EmpID == up.EmpID);
                                if (_attData != null)
                                {

                                    if (_attData.TimeIn != null)
                                    {
                                        if (_attData.TimeIn.Value.TimeOfDay > OpenShiftThresholdStart)
                                        {
                                            //attdata - 1 . multipleTimeIn =  up.EntTime 

                                        }
                                        else
                                        {
                                            attendanceRecord.Tin0 = up.EntTime;
                                            attendanceRecord.TimeIn = up.EntTime;
                                            attendanceRecord.StatusAB = false;
                                            attendanceRecord.StatusP = true;
                                            attendanceRecord.Remarks = null;
                                        }
                                    }
                                    else
                                    {
                                        attendanceRecord.Tin0 = up.EntTime;
                                        attendanceRecord.TimeIn = up.EntTime;
                                        attendanceRecord.StatusAB = false;
                                        attendanceRecord.StatusP = true;
                                        attendanceRecord.Remarks = null;
                                    }
                                }
                                else
                                {
                                    attendanceRecord.Tin0 = up.EntTime;
                                    attendanceRecord.TimeIn = up.EntTime;
                                    attendanceRecord.StatusAB = false;
                                    attendanceRecord.StatusP = true;
                                    attendanceRecord.Remarks = null;
                                }
                            }
                            else
                            {
                                attendanceRecord.Tin0 = up.EntTime;
                                attendanceRecord.TimeIn = up.EntTime;
                                attendanceRecord.StatusAB = false;
                                attendanceRecord.StatusP = true;
                                attendanceRecord.Remarks = null;
                            }

                        }
                        else if (attendanceRecord.Tin1 == null)
                        {
                            attendanceRecord.Tin1 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin2 == null)
                        {
                            attendanceRecord.Tin2 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin3 == null)
                        {
                            attendanceRecord.Tin3 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin4 == null)
                        {
                            attendanceRecord.Tin4 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin5 == null)
                        {
                            attendanceRecord.Tin5 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin6 == null)
                        {
                            attendanceRecord.Tin6 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin7 == null)
                        {
                            attendanceRecord.Tin7 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin8 == null)
                        {
                            attendanceRecord.Tin8 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin9 == null)
                        {
                            attendanceRecord.Tin9 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin10 == null)
                        {
                            attendanceRecord.Tin10 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else
                        {
                            attendanceRecord.Tin11 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        break;
                    case 5: //OUT
                        if (up.EntTime.TimeOfDay < OpenShiftThresholdEnd)
                        {
                            DateTime dt = up.EntDate.AddDays(-1);
                            if (context.AttDatas.Where(aa => aa.AttDate == dt && aa.EmpID == up.EmpID).Count() > 0)
                            {
                                AttData AttDataOfPreviousDay = context.AttDatas.FirstOrDefault(aa => aa.AttDate == dt && aa.EmpID == up.EmpID);
                                if (AttDataOfPreviousDay.TimeIn != null)
                                {
                                    if (AttDataOfPreviousDay.TimeIn.Value.TimeOfDay > OpenShiftThresholdStart)
                                    {
                                        //AttDate -1, Possible TimeOut = up.entryTime
                                        MarkOUTForOpenShift(up.EntTime, AttDataOfPreviousDay);
                                    }
                                    else
                                    {
                                        // Mark as out of that day
                                        MarkOUTForOpenShift(up.EntTime, attendanceRecord);
                                    }
                                }
                                else
                                    MarkOUTForOpenShift(up.EntTime, attendanceRecord);
                            }
                            else
                            {
                                // Mark as out of that day
                                MarkOUTForOpenShift(up.EntTime, attendanceRecord);
                            }


                        }
                        else
                        {
                            //Mark as out of that day
                            MarkOUTForOpenShift(up.EntTime, attendanceRecord);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void MarkOUTForOpenShift(DateTime _pollTime, AttData _attendanceRecord)
        {
            if (_attendanceRecord.Tout0 == null)
            {
                _attendanceRecord.Tout0 = _pollTime;
                _attendanceRecord.TimeOut = _pollTime;
                SortingOutTime(_attendanceRecord);
            }
            else if (_attendanceRecord.Tout1 == null)
            {
                _attendanceRecord.Tout1 = _pollTime;
                _attendanceRecord.TimeOut = _pollTime;
                SortingOutTime(_attendanceRecord);
            }
            else if (_attendanceRecord.Tout2 == null)
            {
                _attendanceRecord.Tout2 = _pollTime;
                _attendanceRecord.TimeOut = _pollTime;
                SortingOutTime(_attendanceRecord);
            }
            else if (_attendanceRecord.Tout3 == null)
            {
                _attendanceRecord.Tout3 = _pollTime;
                _attendanceRecord.TimeOut = _pollTime;
                SortingOutTime(_attendanceRecord);
            }
            else
            {
                _attendanceRecord.Tout4 = _pollTime;
                _attendanceRecord.TimeOut = _pollTime;
                SortingOutTime(_attendanceRecord);
            }
            //else if (_attendanceRecord.Tout5 == null)
            //{
            //    _attendanceRecord.Tout5 = up.EntTime;
            //    _attendanceRecord.TimeOut = up.EntTime;
            //    SortingOutTime(_attendanceRecord);
            //}
            //else if (_attendanceRecord.Tout6 == null)
            //{
            //    _attendanceRecord.Tout6 = up.EntTime;
            //    _attendanceRecord.TimeOut = up.EntTime;
            //    SortingOutTime(_attendanceRecord);
            //}
            //else if (_attendanceRecord.Tout7 == null)
            //{
            //    _attendanceRecord.Tout7 = up.EntTime;
            //    _attendanceRecord.TimeOut = up.EntTime;
            //    SortingOutTime(_attendanceRecord);
            //}
            //else if (_attendanceRecord.Tout8 == null)
            //{
            //    _attendanceRecord.Tout8 = up.EntTime;
            //    _attendanceRecord.TimeOut = up.EntTime;
            //    SortingOutTime(_attendanceRecord);
            //}
            //else if (_attendanceRecord.Tout9 == null)
            //{
            //    _attendanceRecord.Tout9 = up.EntTime;
            //    _attendanceRecord.TimeOut = up.EntTime;
            //    SortingOutTime(_attendanceRecord);
            //}
            //else if (_attendanceRecord.Tout10 == null)
            //{
            //    _attendanceRecord.Tout10 = up.EntTime;
            //    _attendanceRecord.TimeOut = up.EntTime;
            //    SortingOutTime(_attendanceRecord);
            //}
            //else
            //{
            //    _attendanceRecord.Tout11 = up.EntTime;
            //    _attendanceRecord.TimeOut = up.EntTime;
            //    SortingOutTime(_attendanceRecord);
            //}
        }

        private void CalculateTimeINOUT(AttData attendanceRecord, PollData up)
        {
            try
            {
                switch (up.RdrDuty)
                {
                    case 1: //IN
                        if (attendanceRecord.Tin0 == null)
                        {
                            attendanceRecord.Tin0 = up.EntTime;
                            attendanceRecord.TimeIn = up.EntTime;
                            attendanceRecord.StatusAB = false;
                            attendanceRecord.StatusP = true;
                            attendanceRecord.Remarks = null;
                        }
                        else if (attendanceRecord.Tin1 == null)
                        {
                            attendanceRecord.Tin1 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin2 == null)
                        {
                            attendanceRecord.Tin2 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin3 == null)
                        {
                            attendanceRecord.Tin3 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin4 == null)
                        {
                            attendanceRecord.Tin4 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin5 == null)
                        {
                            attendanceRecord.Tin5 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin6 == null)
                        {
                            attendanceRecord.Tin6 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin7 == null)
                        {
                            attendanceRecord.Tin7 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin8 == null)
                        {
                            attendanceRecord.Tin8 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin9 == null)
                        {
                            attendanceRecord.Tin9 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tin10 == null)
                        {
                            attendanceRecord.Tin10 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        else
                        {
                            attendanceRecord.Tin11 = up.EntTime;
                            SortingInTime(attendanceRecord);
                        }
                        break;
                    case 5: //OUT
                        if (attendanceRecord.Tout0 == null)
                        {
                            attendanceRecord.Tout0 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout1 == null)
                        {
                            attendanceRecord.Tout1 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout2 == null)
                        {
                            attendanceRecord.Tout2 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout3 == null)
                        {
                            attendanceRecord.Tout3 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout4 == null)
                        {
                            attendanceRecord.Tout4 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout5 == null)
                        {
                            attendanceRecord.Tout5 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout6 == null)
                        {
                            attendanceRecord.Tout6 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout7 == null)
                        {
                            attendanceRecord.Tout7 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout8 == null)
                        {
                            attendanceRecord.Tout8 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout9 == null)
                        {
                            attendanceRecord.Tout9 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else if (attendanceRecord.Tout10 == null)
                        {
                            attendanceRecord.Tout10 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        else
                        {
                            attendanceRecord.Tout11 = up.EntTime;
                            attendanceRecord.TimeOut = up.EntTime;
                            SortingOutTime(attendanceRecord);
                        }
                        break;
                    case 8: //DUTY
                        if (attendanceRecord.Tin0 != null)
                        {
                            if (attendanceRecord.Tout0 == null)
                            {
                                attendanceRecord.Tout0 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin1 == null)
                            {
                                attendanceRecord.Tin1 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout1 == null)
                            {
                                attendanceRecord.Tout1 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin2 == null)
                            {
                                attendanceRecord.Tin2 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout2 == null)
                            {
                                attendanceRecord.Tout2 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin3 == null)
                            {
                                attendanceRecord.Tin3 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout3 == null)
                            {
                                attendanceRecord.Tout3 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin4 == null)
                            {
                                attendanceRecord.Tin4 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout4 == null)
                            {
                                attendanceRecord.Tout4 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin5 == null)
                            {
                                attendanceRecord.Tin5 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout5 == null)
                            {
                                attendanceRecord.Tout5 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin6 == null)
                            {
                                attendanceRecord.Tin6 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout6 == null)
                            {
                                attendanceRecord.Tout6 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            //
                            else if (attendanceRecord.Tin7 == null)
                            {
                                attendanceRecord.Tin7 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout7 == null)
                            {
                                attendanceRecord.Tout7 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin8 == null)
                            {
                                attendanceRecord.Tin8 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout8 == null)
                            {
                                attendanceRecord.Tout8 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin9 == null)
                            {
                                attendanceRecord.Tin9 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout9 == null)
                            {
                                attendanceRecord.Tout9 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin10 == null)
                            {
                                attendanceRecord.Tin10 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout10 == null)
                            {
                                attendanceRecord.Tout10 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin11 == null)
                            {
                                attendanceRecord.Tin11 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout11 == null)
                            {
                                attendanceRecord.Tout11 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin12 == null)
                            {
                                attendanceRecord.Tin12 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tout12 == null)
                            {
                                attendanceRecord.Tout12 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else if (attendanceRecord.Tin13 == null)
                            {
                                attendanceRecord.Tin13 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                            else
                            {
                                attendanceRecord.Tout13 = up.EntTime;
                                attendanceRecord.TimeOut = up.EntTime;
                            }
                        }
                        else
                        {
                            attendanceRecord.Tin0 = up.EntTime;
                            attendanceRecord.TimeIn = up.EntTime;
                            attendanceRecord.StatusAB = false;
                            attendanceRecord.StatusP = true;
                            attendanceRecord.Remarks = null;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //Error in TimeIN/OUT
            }
        }
        // Sorting Time In
        private void SortingInTime(AttData attendanceRecord)
        {
            List<DateTime> _InTimes = new List<DateTime>();

            if (attendanceRecord.Tin0 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin0);
            if (attendanceRecord.Tin1 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin1);
            if (attendanceRecord.Tin2 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin2);
            if (attendanceRecord.Tin3 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin3);
            if (attendanceRecord.Tin4 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin4);
            if (attendanceRecord.Tin5 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin5);
            if (attendanceRecord.Tin6 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin6);
            if (attendanceRecord.Tin7 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin7);
            if (attendanceRecord.Tin8 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin8);
            if (attendanceRecord.Tin9 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin9);
            if (attendanceRecord.Tin10 != null)
                _InTimes.Add((DateTime)attendanceRecord.Tin10);

            var list = _InTimes.OrderBy(x => x.TimeOfDay.Hours).ToList();
            PlacedSortedInTime(attendanceRecord, list);

        }

        private void PlacedSortedInTime(AttData attendanceRecord, List<DateTime> _InTimes)
        {
            for (int i = 0; i < _InTimes.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        attendanceRecord.Tin0 = _InTimes[i];
                        attendanceRecord.TimeIn = _InTimes[i];
                        break;
                    case 1:
                        attendanceRecord.Tin1 = _InTimes[i];
                        break;
                    case 2:
                        attendanceRecord.Tin2 = _InTimes[i];
                        break;
                    case 3:
                        attendanceRecord.Tin3 = _InTimes[i];
                        break;
                    case 4:
                        attendanceRecord.Tin4 = _InTimes[i];
                        break;
                    case 5:
                        attendanceRecord.Tin5 = _InTimes[i];
                        break;
                    case 6:
                        attendanceRecord.Tin6 = _InTimes[i];
                        break;
                    case 7:
                        attendanceRecord.Tin7 = _InTimes[i];
                        break;
                    case 8:
                        attendanceRecord.Tin8 = _InTimes[i];
                        break;
                    case 9:
                        attendanceRecord.Tin9 = _InTimes[i];
                        break;
                }
            }
        }
        //Sorting Time Out
        private void SortingOutTime(AttData attendanceRecord)
        {
            List<DateTime> _OutTimes = new List<DateTime>();

            if (attendanceRecord.Tout0 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout0);
            if (attendanceRecord.Tout1 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout1);
            if (attendanceRecord.Tout2 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout2);
            if (attendanceRecord.Tout3 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout3);
            if (attendanceRecord.Tout4 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout4);
            if (attendanceRecord.Tout5 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout5);
            if (attendanceRecord.Tout6 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout6);
            if (attendanceRecord.Tout7 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout7);
            if (attendanceRecord.Tout8 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout8);
            if (attendanceRecord.Tout9 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout9);
            if (attendanceRecord.Tout10 != null)
                _OutTimes.Add((DateTime)attendanceRecord.Tout10);

            var list = _OutTimes.OrderBy(x => x.TimeOfDay.Hours).ToList();
            PlacedSortedOutTime(attendanceRecord, list);


        }

        private void PlacedSortedOutTime(AttData attendanceRecord, List<DateTime> _OutTimes)
        {
            for (int i = 0; i < _OutTimes.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        attendanceRecord.Tout0 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 1:
                        attendanceRecord.Tout1 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 2:
                        attendanceRecord.Tout2 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 3:
                        attendanceRecord.Tout3 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 4:
                        attendanceRecord.Tout4 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 5:
                        attendanceRecord.Tout5 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 6:
                        attendanceRecord.Tout6 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 7:
                        attendanceRecord.Tout7 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 8:
                        attendanceRecord.Tout8 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                    case 9:
                        attendanceRecord.Tout9 = _OutTimes[i];
                        attendanceRecord.TimeOut = _OutTimes[i];
                        break;
                }
            }
        }

        private void CalculateShiftTimes(AttData attendanceRecord, Shift shift)
        {
            try
            {
                //Calculate WorkMin
                attendanceRecord.Remarks.Replace("[LI]", "");
                attendanceRecord.Remarks.Replace("[EI]", "");
                attendanceRecord.Remarks.Replace("[EO]", "");
                attendanceRecord.Remarks.Replace("[LO]", "");
                attendanceRecord.Remarks.Replace("[G-OT]", "");
                attendanceRecord.Remarks.Replace("[R-OT]", "");
                attendanceRecord.Remarks.Replace("[N-OT]", "");
                attendanceRecord.Remarks.Replace("[Manual]", "");
                attendanceRecord.Remarks = attendanceRecord.Remarks + "[Manual]";
                //attendanceRecord.Remarks = "";
                TimeSpan mins = (TimeSpan)(attendanceRecord.TimeOut - attendanceRecord.TimeIn);
                //Check if GZ holiday then place all WorkMin in GZOTMin
                if (attendanceRecord.StatusGZ == true)
                {
                    attendanceRecord.GZOTMin = (short)mins.TotalMinutes;
                    attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                    attendanceRecord.StatusGZOT = true;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[G-OT]";
                }
                //if Rest day then place all WorkMin in OTMin
                else if (attendanceRecord.StatusDO == true)
                {
                    attendanceRecord.OTMin = (short)mins.TotalMinutes;
                    attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                    attendanceRecord.StatusOT = true;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[R-OT]";
                }
                else
                {
                    /////////// to-do -----calculate Margins for those shifts which has break mins 
                    if (shift.HasBreak == true)
                    {
                        attendanceRecord.WorkMin = (short)(mins.TotalMinutes - shift.BreakMin);
                        attendanceRecord.ShifMin = (short)(CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) - (short)shift.BreakMin);
                    }
                    else
                    {
                        //Calculate Late IN, Compare margin with Shift Late In
                        int _lateMin = (int)shift.LateIn;
                        if (attendanceRecord.TimeIn.Value.TimeOfDay > (attendanceRecord.DutyTime + new TimeSpan(0, _lateMin, 0)))
                        {
                            TimeSpan lateMinsSpan = (TimeSpan)(attendanceRecord.TimeIn.Value.TimeOfDay - attendanceRecord.DutyTime);
                            if (lateMinsSpan.TotalMinutes > shift.LateIn)
                            {
                                attendanceRecord.LateIn = (short)lateMinsSpan.TotalMinutes;
                                attendanceRecord.StatusLI = true;
                                attendanceRecord.EarlyIn = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[LI]";
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusLI = null;
                            attendanceRecord.LateIn = null;
                            attendanceRecord.Remarks.Replace("[LI]", "");
                        }

                        //Calculate Early In, Compare margin with Shift Early In
                        if (attendanceRecord.TimeIn.Value.TimeOfDay < attendanceRecord.DutyTime)
                        {
                            TimeSpan EarlyInMinsSpan = (TimeSpan)(attendanceRecord.DutyTime - attendanceRecord.TimeIn.Value.TimeOfDay);
                            if (EarlyInMinsSpan.TotalMinutes > shift.EarlyIn)
                            {
                                attendanceRecord.EarlyIn = (short)EarlyInMinsSpan.TotalMinutes;
                                attendanceRecord.StatusEI = true;
                                attendanceRecord.LateIn = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[EI]";
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusEI = null;
                            attendanceRecord.EarlyIn = null;
                            attendanceRecord.Remarks.Replace("[EI]", "");
                        }

                        // CalculateShiftEndTime = ShiftStart + DutyHours
                        DateTime shiftEnd = CalculateShiftEndTime(shift, attendanceRecord.AttDate.Value,attendanceRecord.DutyTime.Value);

                        //Calculate Early Out, Compare margin with Shift Early Out
                        if (attendanceRecord.TimeOut < shiftEnd)
                        {
                            TimeSpan EarlyOutMinsSpan = (TimeSpan)(shiftEnd - attendanceRecord.TimeOut);
                            if (EarlyOutMinsSpan.TotalMinutes > shift.EarlyOut)
                            {
                                attendanceRecord.EarlyOut = (short)EarlyOutMinsSpan.TotalMinutes;
                                attendanceRecord.StatusEO = true;
                                attendanceRecord.LateOut = null;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[EO]";
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusEO = null;
                            attendanceRecord.EarlyOut = null;
                            attendanceRecord.Remarks.Replace("[EO]", "");
                        }
                        //Calculate Late Out, Compare margin with Shift Late Out
                        if (attendanceRecord.TimeOut.Value > shiftEnd)
                        {
                            TimeSpan LateOutMinsSpan = (TimeSpan)(attendanceRecord.TimeOut.Value- shiftEnd);
                            if (LateOutMinsSpan.TotalMinutes > shift.LateOut)
                            {
                                attendanceRecord.LateOut = (short)LateOutMinsSpan.TotalMinutes;
                                // Late Out cannot have an early out, In case of poll at multiple times before and after shiftend
                                attendanceRecord.EarlyOut = null;
                                attendanceRecord.StatusLO = true;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[LO]";
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusLO = null;
                            attendanceRecord.LateOut = null;
                            attendanceRecord.Remarks.Replace("[LO]", "");
                        }

                        //Subtract EarlyIn and LateOut from Work Minutes
                        //////-------to-do--------- Automate earlyin,lateout from shift setup
                        attendanceRecord.WorkMin = (short)(mins.TotalMinutes);
                        if (attendanceRecord.EarlyIn != null && attendanceRecord.EarlyIn > shift.EarlyIn)
                        {
                            attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.EarlyIn);
                        }
                        if (attendanceRecord.LateOut != null && attendanceRecord.LateOut > shift.LateOut)
                        {
                            attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.LateOut);
                        }
                        if (attendanceRecord.LateOut != null || attendanceRecord.EarlyIn != null)

                            // round off work mins if overtime less than shift.OverTimeMin >
                            if (attendanceRecord.WorkMin > CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) && (attendanceRecord.WorkMin <= (CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) + shift.OverTimeMin)))
                            {
                                attendanceRecord.WorkMin = CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek);
                            }
                        //Calculate OverTime = EI+OT, Compare margin with Shift OverTime
                        //----to-do----- Handle from shift
                        if (attendanceRecord.LateOut > shift.LateOut)
                        {
                            if (attendanceRecord.StatusGZ != true || attendanceRecord.StatusDO != true)
                            {
                                short _EarlyIn;
                                short _LateOut;
                                if (attendanceRecord.EarlyIn == null)
                                    _EarlyIn = 0;
                                else
                                    _EarlyIn = 0;

                                if (attendanceRecord.LateOut == null)
                                    _LateOut = 0;
                                else
                                    _LateOut = (short)attendanceRecord.LateOut;

                                attendanceRecord.OTMin = (short)(_EarlyIn + _LateOut);
                                attendanceRecord.StatusOT = true;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[N-OT]";
                            }
                        }
                    }
                }




            }
            catch (Exception ex)
            {

            }
        }

        private void CalculateRosterTimes(AttData attendanceRecord, Models.Roster roster, Shift _shift)
        {
            try
            {
                TimeSpan mins = (TimeSpan)(attendanceRecord.TimeOut - attendanceRecord.TimeIn);
                attendanceRecord.Remarks.Replace("[LI]", "");
                attendanceRecord.Remarks.Replace("[EI]", "");
                attendanceRecord.Remarks.Replace("[EO]", "");
                attendanceRecord.Remarks.Replace("[LO]", "");
                attendanceRecord.Remarks.Replace("[G-OT]", "");
                attendanceRecord.Remarks.Replace("[R-OT]", "");
                attendanceRecord.Remarks.Replace("[N-OT]", "");
                attendanceRecord.Remarks.Replace("[Manual]", "");
                attendanceRecord.Remarks = attendanceRecord.Remarks + "[Manual]";
                if (attendanceRecord.StatusGZ == true)
                {
                    attendanceRecord.GZOTMin = (short)mins.TotalMinutes;
                    attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                    attendanceRecord.StatusGZOT = true;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[GZ-OT]";
                }
                else if (attendanceRecord.StatusDO == true)
                {
                    attendanceRecord.OTMin = (short)mins.TotalMinutes;
                    attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                    attendanceRecord.StatusOT = true;
                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[R-OT]";
                }
                else
                {
                    ////------to-do ----------handle shift break time
                    //Calculate Late IN, Compare margin with Shift Late In
                    if (attendanceRecord.TimeIn.Value.TimeOfDay > roster.DutyTime)
                    {
                        TimeSpan lateMinsSpan = (TimeSpan)(attendanceRecord.TimeIn.Value.TimeOfDay - roster.DutyTime);
                        if (lateMinsSpan.TotalMinutes > _shift.LateIn)
                        {
                            attendanceRecord.LateIn = (short)lateMinsSpan.TotalMinutes;
                            attendanceRecord.StatusLI = true;
                            attendanceRecord.EarlyIn = null;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[LI]";
                        }
                    }
                    else
                    {
                        attendanceRecord.LateIn = null;
                        attendanceRecord.StatusLI = null;
                        attendanceRecord.Remarks.Replace("[LI]", "");
                    }

                    //Calculate Early In, Compare margin with Shift Early In
                    if (attendanceRecord.TimeIn.Value.TimeOfDay < roster.DutyTime)
                    {
                        TimeSpan EarlyInMinsSpan = (TimeSpan)(roster.DutyTime - attendanceRecord.TimeIn.Value.TimeOfDay);
                        if (EarlyInMinsSpan.TotalMinutes > _shift.EarlyIn)
                        {
                            attendanceRecord.EarlyIn = (short)EarlyInMinsSpan.TotalMinutes;
                            attendanceRecord.StatusEI = true;
                            attendanceRecord.LateIn = null;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[EI]";
                        }
                    }
                    else
                    {
                        attendanceRecord.StatusEI = null;
                        attendanceRecord.EarlyIn = null;
                        attendanceRecord.Remarks.Replace("[EI]", "");
                    }

                    // CalculateShiftEndTime = ShiftStart + DutyHours
                    TimeSpan shiftEnd = roster.DutyTime + (new TimeSpan(0, (int)roster.WorkMin, 0));

                    //Calculate Early Out, Compare margin with Shift Early Out
                    if (attendanceRecord.TimeOut.Value.TimeOfDay < shiftEnd)
                    {
                        TimeSpan EarlyOutMinsSpan = (TimeSpan)(shiftEnd - attendanceRecord.TimeOut.Value.TimeOfDay);
                        if (EarlyOutMinsSpan.TotalMinutes > _shift.EarlyOut)
                        {
                            attendanceRecord.EarlyOut = (short)EarlyOutMinsSpan.TotalMinutes;
                            attendanceRecord.StatusEO = true;
                            attendanceRecord.LateOut = null;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[EO]";
                        }
                    }

                    else
                    {
                        attendanceRecord.StatusEO = null;
                        attendanceRecord.EarlyOut = null;
                        attendanceRecord.Remarks.Replace("[EO]", "");
                    }
                    //Calculate Late Out, Compare margin with Shift Late Out
                    if (attendanceRecord.TimeOut.Value.TimeOfDay > shiftEnd)
                    {
                        TimeSpan LateOutMinsSpan = (TimeSpan)(attendanceRecord.TimeOut.Value.TimeOfDay - shiftEnd);
                        if (LateOutMinsSpan.TotalMinutes > _shift.LateOut)
                        {
                            attendanceRecord.LateOut = (short)LateOutMinsSpan.TotalMinutes;
                            // Late Out cannot have an early out, In case of poll at multiple times before and after shiftend
                            attendanceRecord.EarlyOut = null;
                            attendanceRecord.StatusLO = true;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[LO]";
                        }
                    }
                    attendanceRecord.WorkMin = (short)(mins.TotalMinutes);
                    if (attendanceRecord.EarlyIn != null && attendanceRecord.EarlyIn > _shift.EarlyIn)
                    {
                        attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.EarlyIn);
                    }
                    if (attendanceRecord.LateOut != null && attendanceRecord.LateOut > _shift.LateOut)
                    {
                        attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.LateOut);
                    }
                    if (attendanceRecord.EarlyIn == null && attendanceRecord.LateOut == null)
                    {

                    }
                    //Calculate OverTime, Compare margin with Shift OverTime
                    if (attendanceRecord.EarlyIn > _shift.EarlyIn || attendanceRecord.LateOut > _shift.LateOut)
                    {
                        if (attendanceRecord.StatusGZ != true || attendanceRecord.StatusDO != true)
                        {
                            short _EarlyIn;
                            short _LateOut;
                            if (attendanceRecord.EarlyIn == null)
                                _EarlyIn = 0;
                            else
                                _EarlyIn = 0;

                            if (attendanceRecord.LateOut == null)
                                _LateOut = 0;
                            else
                                _LateOut = (short)attendanceRecord.LateOut;

                            attendanceRecord.OTMin = (short)(_EarlyIn + _LateOut);
                            attendanceRecord.StatusOT = true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                string _error = "";
                if (ex.InnerException.Message != null)
                    _error = ex.InnerException.Message;
                else
                    _error = ex.Message;
                //_myHelperClass.WriteToLogFile("Attendance Processing Roster Times" + _error);
            }
        }

        private string ReturnDayOfWeek(DayOfWeek dayOfWeek)
        {
            string _DayName = "";
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    _DayName = "Monday";
                    break;
                case DayOfWeek.Tuesday:
                    _DayName = "Tuesday";
                    break;
                case DayOfWeek.Wednesday:
                    _DayName = "Wednesday";
                    break;
                case DayOfWeek.Thursday:
                    _DayName = "Thursday";
                    break;
                case DayOfWeek.Friday:
                    _DayName = "Friday";
                    break;
                case DayOfWeek.Saturday:
                    _DayName = "Saturday";
                    break;
                case DayOfWeek.Sunday:
                    _DayName = "Sunday";
                    break;
            }
            return _DayName;
        }

        private TimeSpan CalculateShiftEndTime(Shift shift, DayOfWeek dayOfWeek)
        {
            Int16 workMins = 0;
            try
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        workMins = shift.MonMin;
                        break;
                    case DayOfWeek.Tuesday:
                        workMins = shift.TueMin;
                        break;
                    case DayOfWeek.Wednesday:
                        workMins = shift.WedMin;
                        break;
                    case DayOfWeek.Thursday:
                        workMins = shift.ThuMin;
                        break;
                    case DayOfWeek.Friday:
                        workMins = shift.FriMin;
                        break;
                    case DayOfWeek.Saturday:
                        workMins = shift.SatMin;
                        break;
                    case DayOfWeek.Sunday:
                        workMins = shift.SunMin;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return shift.StartTime + (new TimeSpan(0, workMins, 0));
        }
        private DateTime CalculateShiftEndTime(Shift shift, DateTime _AttDate, TimeSpan _DutyTime)
        {
            Int16 workMins = 0;
            try
            {
                switch (_AttDate.Date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        workMins = shift.MonMin;
                        break;
                    case DayOfWeek.Tuesday:
                        workMins = shift.TueMin;
                        break;
                    case DayOfWeek.Wednesday:
                        workMins = shift.WedMin;
                        break;
                    case DayOfWeek.Thursday:
                        workMins = shift.ThuMin;
                        break;
                    case DayOfWeek.Friday:
                        workMins = shift.FriMin;
                        break;
                    case DayOfWeek.Saturday:
                        workMins = shift.SatMin;
                        break;
                    case DayOfWeek.Sunday:
                        workMins = shift.SunMin;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            DateTime _datetime = new DateTime();
            TimeSpan _Time = new TimeSpan(0, workMins, 0);
            _datetime = _AttDate.Date.Add(_DutyTime);
            _datetime = _datetime.Add(_Time);
            return _datetime;
        }

        private Int16 CalculateShiftMinutes(Shift shift, DayOfWeek dayOfWeek)
        {
            Int16 workMins = 0;
            try
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        workMins = shift.MonMin;
                        break;
                    case DayOfWeek.Tuesday:
                        workMins = shift.TueMin;
                        break;
                    case DayOfWeek.Wednesday:
                        workMins = shift.WedMin;
                        break;
                    case DayOfWeek.Thursday:
                        workMins = shift.ThuMin;
                        break;
                    case DayOfWeek.Friday:
                        workMins = shift.FriMin;
                        break;
                    case DayOfWeek.Saturday:
                        workMins = shift.SatMin;
                        break;
                    case DayOfWeek.Sunday:
                        workMins = shift.SunMin;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return workMins;
        }
        private void CalculateOpenShiftTimes(AttData attendanceRecord, Shift shift)
        {
            try
            {
                //Calculate WorkMin
                if (attendanceRecord != null)
                {
                    if (attendanceRecord.TimeOut != null && attendanceRecord.TimeIn != null)
                    {
                        attendanceRecord.Remarks = "";
                        TimeSpan mins = (TimeSpan)(attendanceRecord.TimeOut - attendanceRecord.TimeIn);
                        //Check if GZ holiday then place all WorkMin in GZOTMin
                        if (attendanceRecord.StatusGZ == true)
                        {
                            attendanceRecord.GZOTMin = (short)mins.TotalMinutes;
                            attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                            attendanceRecord.StatusGZOT = true;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[GZ-OT]";
                        }
                        else if (attendanceRecord.StatusDO == true)
                        {
                            attendanceRecord.OTMin = (short)mins.TotalMinutes;
                            attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                            attendanceRecord.StatusOT = true;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[R-OT]";
                        }
                        else
                        {
                            if (shift.HasBreak == true)
                            {
                                attendanceRecord.WorkMin = (short)(mins.TotalMinutes - shift.BreakMin);
                                attendanceRecord.ShifMin = (short)(CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) - (short)shift.BreakMin);
                            }
                            else
                            {
                                // CalculateShiftEndTime = ShiftStart + DutyHours
                                TimeSpan shiftEnd = CalculateShiftEndTime(shift, attendanceRecord.AttDate.Value.DayOfWeek);
                                attendanceRecord.WorkMin = (short)(mins.TotalMinutes);
                                //Calculate OverTIme, 
                                if (mins.TotalMinutes > (CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) + shift.OverTimeMin))
                                {
                                    attendanceRecord.OTMin = (Int16)(Convert.ToInt16(mins.TotalMinutes) - CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek));
                                    attendanceRecord.WorkMin = (short)((mins.TotalMinutes) - attendanceRecord.OTMin);
                                    attendanceRecord.StatusOT = true;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[N-OT]";
                                }
                                //Calculate Early Out
                                if (mins.TotalMinutes < (CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) + shift.EarlyOut))
                                {
                                    Int16 EarlyoutMin = (Int16)(Convert.ToInt16(mins.TotalMinutes) - CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek));
                                    if (EarlyoutMin > shift.EarlyOut)
                                    {
                                        attendanceRecord.EarlyOut = EarlyoutMin;
                                        attendanceRecord.StatusEO = true;
                                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[EO]";
                                    }
                                }
                                // round off work mins if overtime less than shift.OverTimeMin >
                                if (attendanceRecord.WorkMin > CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) && (attendanceRecord.WorkMin <= (CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek) + shift.OverTimeMin)))
                                {
                                    attendanceRecord.WorkMin = CalculateShiftMinutes(shift, attendanceRecord.AttDate.Value.DayOfWeek);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //private void CreateAttendance(DateTime dateTime)
        //{
        //    using (var ctx = new TAS2013Entities())
        //    {
        //        List<Emp> _emp = new List<Emp>();
        //        _emp = ctx.Emps.Where(aa => aa.ProcessAtt == true && aa.Status == true).ToList();
        //        List<Roster> _Roster = new List<Roster>();
        //        _Roster = context.Rosters.Where(aa => aa.RosterDate == dateTime).ToList();
        //        List<LvData> _LvData = new List<LvData>();
        //        _LvData = context.LvDatas.ToList();
        //        List<LvShort> _lvShort = new List<LvShort>();
        //        _lvShort = context.LvShorts.ToList();
        //        foreach (var emp in _emp)
        //        {
        //            try
        //            {
        //                /////////////////////////////////////////////////////
        //                //  Mark Everyone Absent while creating Attendance //
        //                /////////////////////////////////////////////////////
        //                //Set DUTYCODE = D, StatusAB = true, and Remarks = [Absent]
        //                NewDatabase.AttData att = new NewDatabase.AttData();
        //                att.AttDate = dateTime.Date;
        //                att.DutyCode = "D";
        //                att.StatusAB = true;
        //                att.Remarks = "[Absent]";
        //                if (emp.Shift != null)
        //                    att.DutyTime = emp.Shift.StartTime;
        //                else
        //                    att.DutyTime = new TimeSpan(07, 45, 00);
        //                att.EmpID = emp.EmpID;
        //                att.EmpNo = emp.EmpNo;
        //                att.EmpDate = emp.EmpID + dateTime.ToString("yyMMdd");
        //                att.ShifMin = CalculateShiftMinutes(emp.Shift, dateTime.DayOfWeek);
        //                //////////////////////////
        //                //  Check for Rest Day //
        //                ////////////////////////
        //                //Set DutyCode = R, StatusAB=false, StatusDO = true, and Remarks=[DO]
        //                //Check for 1st Day Off of Shift
        //                if (emp.Shift.DaysName.Name == ReturnDayOfWeek(dateTime.DayOfWeek))
        //                {
        //                    att.DutyCode = "R";
        //                    att.StatusAB = false;
        //                    att.StatusDO = true;
        //                    att.Remarks = "[DO]";
        //                }
        //                //Check for 2nd Day Off of shift
        //                if (emp.Shift.DaysName1.Name == ReturnDayOfWeek(dateTime.DayOfWeek))
        //                {
        //                    att.DutyCode = "R";
        //                    att.StatusAB = false;
        //                    att.StatusDO = true;
        //                    att.Remarks = "[DO]";
        //                }

        //                //////////////////////////
        //                //  Check for GZ Day //
        //                ////////////////////////
        //                //Set DutyCode = R, StatusAB=false, StatusGZ = true, and Remarks=[GZ]
        //                if (emp.Shift.GZDays == true)
        //                {
        //                    foreach (var holiday in context.Holidays)
        //                    {
        //                        if (context.Holidays.Where(hol => hol.HolDate.Month == att.AttDate.Value.Month && hol.HolDate.Day == att.AttDate.Value.Day).Count() > 0)
        //                        {
        //                            att.DutyCode = "G";
        //                            att.StatusAB = false;
        //                            att.StatusGZ = true;
        //                            att.Remarks = "[GZ]";
        //                            att.ShifMin = 0;
        //                        }
        //                    }
        //                }

        //                //////////////////////////
        //                //  Check for Roster   //
        //                ////////////////////////
        //                //If Roster DutyCode is Rest then change the StatusAB and StatusDO
        //                foreach (var roster in _Roster.Where(aa => aa.EmpDate == att.EmpDate))
        //                {
        //                    att.DutyCode = roster.DutyCode.Trim();
        //                    if (att.DutyCode == "R")
        //                    {
        //                        att.StatusAB = false;
        //                        att.StatusDO = true;
        //                        att.Remarks = "[DO]";
        //                    }

        //                    att.ShifMin = roster.WorkMin;
        //                    att.DutyTime = roster.DutyTime;
        //                }

        //                ////////////////////////////
        //                //TODO Check for Official Duty//
        //                //////////////////////////



        //                ////////////////////////////
        //                //  Check for Short Leave//
        //                //////////////////////////
        //                foreach (var sLeave in _lvShort)
        //                {
        //                    if (_lvShort.Where(lv => lv.EmpDate == att.EmpDate).Count() > 0)
        //                    {
        //                        att.StatusSL = true;
        //                        att.StatusAB = null;
        //                        att.DutyCode = "D";
        //                        att.Remarks = "[SL]";
        //                    }
        //                }

        //                //////////////////////////
        //                //   Check for Leave   //
        //                ////////////////////////
        //                //Set DutyCode = R, StatusAB=false, StatusGZ = true, and Remarks=[GZ]
        //                foreach (var Leave in _LvData)
        //                {
        //                    if (_LvData.Where(lv => lv.EmpDate == att.EmpDate).Count() > 0)
        //                    {
        //                        att.StatusLeave = true;
        //                        att.StatusAB = false;
        //                        att.DutyCode = "L";
        //                        att.Remarks = "[Leave]";
        //                    }
        //                    else
        //                    {
        //                        att.StatusLeave = false;
        //                    }
        //                }
        //                ctx.AttDatas.AddObject(att);

        //            }
        //            catch (Exception ex)
        //            {
        //                _myHelperClass.WriteToLogFile("Attendance Creating Error Level 1");
        //            }
        //            ctx.SaveChanges();
        //        }
        //        AttProcess attp = new AttProcess();
        //        attp.ProcessDate = dateTime;
        //        ctx.AttProcesses.AddObject(attp);
        //        ctx.SaveChanges();
        //    }
        //}
        #endregion
    }
}