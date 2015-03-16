using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using WMS.CustomClass;
using WMS.Models;

namespace WMS.Reports
{
    public partial class EmpAttSummary : System.Web.UI.Page
    {
        string PathString = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                ReportViewer1.Visible = true;
                ReportViewer1.Width = 1050;
                ReportViewer1.Height = 700;
                DateTime dt = DateTime.Today.Date.AddDays(-30);
                if (GlobalVariables.DeploymentType == false)
                {
                    PathString = "/Reports/RDLC/EmpAttSummary.rdlc";
                }
                else
                    PathString = "/WMS/Reports/RDLC/EmpAttSummary.rdlc";
                LoadCBOSections();
                LoadReport(PathString, context.ViewPresentEmps.Where(aa => aa.AttDate >= dt && aa.EmpID == 1).ToList());

            }
        }
        public bool ChkEmpNo()
        {
            if (chkEmpNo.Checked == true)
                return true;
            else
                return false;
        }
        public bool ChkSection()
        {
            if (chkSection.Checked == true)
                return true;
            else
                return false;
        }
        public string Section
        {
            get { return cboSection.SelectedItem.Text; }
            set { }
        }
        public string EmpNo
        {
            get { return tbEmpNo.Text; }
            set { }
        }
        private void LoadCBOSections()
        {
            cboSection.DataSource = context.Sections.ToList();
            cboSection.DataTextField = "SectionName";
            cboSection.DataValueField = "SectionID";
            cboSection.DataBind();
        }
        TAS2013Entities context = new TAS2013Entities();
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {

            using (var ctx = new TAS2013Entities())
            {
                User _loggedUser = HttpContext.Current.Session["LoggedUser"] as User;

                List<ViewPresentEmp> _ViewList = new List<ViewPresentEmp>();
                if (ChkEmpNo())
                {
                    _ViewList = ctx.ViewPresentEmps.Where(aa => aa.EmpNo == EmpNo && aa.AttDate >= DateFrom && aa.AttDate <= DateTo).ToList();
                }
                if (ChkSection())
                {
                    User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
                    QueryBuilder qb = new QueryBuilder();
                    string query = qb.MakeCustomizeQuery(LoggedInUser);
                    string _dateTo = "'" + DateTo.Date.Year.ToString() + "-" + DateTo.Date.Month.ToString() + "-" + DateTo.Date.Day.ToString() + "'";
                    string _dateFrom = "'" + DateFrom.Date.Year.ToString() + "-" + DateFrom.Date.Month.ToString() + "-" + DateFrom.Date.Day.ToString() + "'";
                    DataTable dt = qb.GetValuesfromDB("select * from ViewPresentEmp " + query + " and (AttDate >= " + _dateFrom + " and AttDate <= " + _dateTo + " )");
                }
                if (GlobalVariables.DeploymentType == false)
                {
                    PathString = "/Reports/RDLC/EmpAttSummary.rdlc";
                }
                else
                    PathString = "/WMS/Reports/RDLC/EmpAttSummary.rdlc";
                LoadReport(PathString, _ViewList); 
                ctx.Dispose();
            }
        }

        public DateTime DateFrom
        {
            get
            {
                if (dateFrom.Value == "")
                    return DateTime.Today.Date.AddDays(-1);
                else
                    return DateTime.Parse(dateFrom.Value);
            }
        }

        public DateTime DateTo
        {
            get
            {
                if (dateTo.Value == "")
                    return DateTime.Today.Date.AddDays(-1);
                else
                    return DateTime.Parse(dateTo.Value);
            }
        }

        private void LoadReport(string path, List<ViewPresentEmp> _Employee)
        {
            string DateToFor = "";
            if (DateFrom.Date.ToString("d") == DateTo.ToString("d"))
            {
                DateToFor = "Date : " + DateFrom.Date.ToString("dd-MMM-yyyy");
            }
            else
            {
                DateToFor = "From : " + DateFrom.Date.ToString("d") + " To: " + DateTo.Date.ToString("dd-MMM-yyyy");
            }

            string _Header = context.Options.FirstOrDefault().CompanyName + " - Employee Detail Attendance Report";
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewPresentEmp> ie;
            ie = _Employee.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Date", DateToFor, false);
            ReportParameter rp1 = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            ReportViewer1.LocalReport.Refresh();
        }
    }
}