﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TAS2013Entities : DbContext
    {
        public TAS2013Entities()
            : base("name=TAS2013Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AccessEmp> AccessEmps { get; set; }
        public DbSet<AttCode> AttCodes { get; set; }
        public DbSet<AttData> AttDatas { get; set; }
        public DbSet<AttDataManEdit> AttDataManEdits { get; set; }
        public DbSet<AttMnData> AttMnDatas { get; set; }
        public DbSet<AttMnDataPer> AttMnDataPers { get; set; }
        public DbSet<AttProcess> AttProcesses { get; set; }
        public DbSet<AuditForm> AuditForms { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditOperation> AuditOperations { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<DaysName> DaysNames { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<DownloadTime> DownloadTimes { get; set; }
        public DbSet<DutyCode> DutyCodes { get; set; }
        public DbSet<DutyTime> DutyTimes { get; set; }
        public DbSet<Emp> Emps { get; set; }
        public DbSet<EmpAccess> EmpAccesses { get; set; }
        public DbSet<EmpFace> EmpFaces { get; set; }
        public DbSet<EmpFp> EmpFps { get; set; }
        public DbSet<EmpPhoto> EmpPhotoes { get; set; }
        public DbSet<EmpPresence> EmpPresences { get; set; }
        public DbSet<EmpRdr> EmpRdrs { get; set; }
        public DbSet<EmpType> EmpTypes { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<JobCard> JobCards { get; set; }
        public DbSet<JobCardApp> JobCardApps { get; set; }
        public DbSet<JobCardEmp> JobCardEmps { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LvApplication> LvApplications { get; set; }
        public DbSet<LvData> LvDatas { get; set; }
        public DbSet<LvQuota> LvQuotas { get; set; }
        public DbSet<LvShort> LvShorts { get; set; }
        public DbSet<LvType> LvTypes { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<PollData> PollDatas { get; set; }
        public DbSet<RdrDutyCode> RdrDutyCodes { get; set; }
        public DbSet<RdrEventLog> RdrEventLogs { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<ReaderType> ReaderTypes { get; set; }
        public DbSet<ReaderVendor> ReaderVendors { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Roster> Rosters { get; set; }
        public DbSet<RosterDetail> RosterDetails { get; set; }
        public DbSet<RosterType> RosterTypes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SummaryDepartment> SummaryDepartments { get; set; }
        public DbSet<SummarySection> SummarySections { get; set; }
        public DbSet<SummaryShift> SummaryShifts { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<EmpView> EmpViews { get; set; }
        public DbSet<ViewAbsent> ViewAbsents { get; set; }
        public DbSet<ViewAuditLog> ViewAuditLogs { get; set; }
        public DbSet<ViewCard> ViewCards { get; set; }
        public DbSet<ViewEarlyIN> ViewEarlyINs { get; set; }
        public DbSet<ViewEarlyOut> ViewEarlyOuts { get; set; }
        public DbSet<ViewEditAttendance> ViewEditAttendances { get; set; }
        public DbSet<ViewEmpPic> ViewEmpPics { get; set; }
        public DbSet<ViewLateComer> ViewLateComers { get; set; }
        public DbSet<ViewLateOut> ViewLateOuts { get; set; }
        public DbSet<ViewLeave> ViewLeaves { get; set; }
        public DbSet<ViewLeaveData> ViewLeaveDatas { get; set; }
        public DbSet<ViewLeaveQuota> ViewLeaveQuotas { get; set; }
        public DbSet<ViewMissingAtt> ViewMissingAtts { get; set; }
        public DbSet<ViewMonthlyData> ViewMonthlyDatas { get; set; }
        public DbSet<ViewMonthlyDataPer> ViewMonthlyDataPers { get; set; }
        public DbSet<ViewMultipleInOut> ViewMultipleInOuts { get; set; }
        public DbSet<ViewOverTime> ViewOverTimes { get; set; }
        public DbSet<ViewPollData> ViewPollDatas { get; set; }
        public DbSet<ViewPresentEmp> ViewPresentEmps { get; set; }
        public DbSet<ViewSLData> ViewSLDatas { get; set; }
        public DbSet<ViewSummary> ViewSummaries { get; set; }
    }
}
