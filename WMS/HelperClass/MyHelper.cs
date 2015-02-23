using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.HelperClass
{
    public static class MyHelper
    {
        public static void SaveAuditLog(int _userID,short _form,short _operation,DateTime _date)
        {
            using (var ctx = new TAS2013Entities())
            {
                AuditLog auditEntry = new AuditLog();
                auditEntry.AuditUserID = _userID;
                auditEntry.FormID = _form;
                auditEntry.OperationID = _operation;
                auditEntry.AuditDateTime = _date;
                ctx.AuditLogs.Add(auditEntry);
                ctx.SaveChanges();
            }
        }
        public static TimeSpan ConvertTime(string p)
        {
            try
            {
                string hour = "";
                string min = "";
                int count = 0;
                int chunkSize = 2;
                int stringLength = 4;

                for (int i = 0; i < stringLength; i += chunkSize)
                {
                    count++;
                    if (count == 1)
                    {
                        hour = p.Substring(i, chunkSize);
                    }
                    if (count == 2)
                    {
                        min = p.Substring(i, chunkSize);
                    }
                    if (i + chunkSize > stringLength)
                    {
                        chunkSize = stringLength - i;
                    }
                }
                TimeSpan _currentTime = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(min), 00);
                return _currentTime;
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay;
            }
        }
        public static bool CheckforPermission(User _User, ReportName _report)
        {
            bool check = false;
            try
            {
                switch (_report)
                {
                    case ReportName.Audit:
                        if (_User.MRAudit == true)
                            check = true;
                        break;
                    case ReportName.Daily:
                        if (_User.MRDailyAtt == true)
                            check = true;
                        break;
                    case ReportName.Detail:
                        if (_User.MRDetail == true)
                            check = true;
                        break;
                    case ReportName.Employee:
                        if (_User.MREmployee == true)
                            check = true;
                        break;
                    case ReportName.Grpah:
                        if (_User.MRGraph == true)
                            check = true;
                        break;
                    case ReportName.Leave:
                        if (_User.MRLeave == true)
                            check = true;
                        break;
                    case ReportName.ManualAtt:
                        if (_User.MRManualEditAtt == true)
                            check = true;
                        break;
                    case ReportName.Monthly:
                        if (_User.MRMonthly == true)
                            check = true;
                        break;
                    case ReportName.Summary:
                        if (_User.MRSummary == true)
                            check = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        public enum ReportName
        {
            Daily = 1,
            Leave,
            Monthly,
            Audit,
            ManualAtt,
            Employee,
            Detail,
            Summary,
            Grpah
        }
    }
}