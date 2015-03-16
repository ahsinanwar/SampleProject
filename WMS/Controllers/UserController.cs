using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using WMS.Controllers.Filters;

namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class UserController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /User/
        public ActionResult Index()
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            var users = db.Users.Where(aa=>aa.CompanyID==LoggedInUser.CompanyID).Include(u => u.UserRole).Include(u => u.Location).Include(u => u.Company);
            return View(users.ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        public ActionResult ListOfADUser()
        {

            return View(GetADUsers());
        }
        public ActionResult Create()
        {
            //for (int i = 0; i < 7; i++)
            //{
            //    string Time = Request.Form["StudentList[" + i.ToString() + "].Date"].ToString();
            //}

            ViewBag.CompanyID = new SelectList(db.Companies, "CompID", "CompName");
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo");
            ViewBag.LocationID = new SelectList(db.Locations, "LocID", "LocName");
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName");
            return View();
        }


        private ADUsersModel GetADUsers()
        {
            ADUsersModel _objstudentmodel = new ADUsersModel();
            _objstudentmodel._ADUsersAttributes = new List<ADUsersAttributes>();
            using (var context = new PrincipalContext(ContextType.Domain, "fatima-group.com", "ffl.ithelpdesk@fatima-group.com", "fatima@0202"))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    int i = 1;
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        string name = result.Name;
                        string displayName = result.DisplayName;
                        string userPrincipleName = result.UserPrincipalName;
                        string samAccountName = result.SamAccountName;
                        string distinguishedName = result.DistinguishedName;
                        //label1.Text += "Name:    " + result.Name;
                        //label1.Text += "      account name   :    " + result.UserPrincipalName;
                        //label1.Text += "      Server:    " + result.Context.ConnectedServer + "\r";
                        _objstudentmodel._ADUsersAttributes.Add(new ADUsersAttributes
                        {
                            ID = i,
                            UserName = name,
                            DisplayName = displayName,
                            PrincipleName = userPrincipleName,
                            DistingushedName = distinguishedName,
                            SAMName = samAccountName
                        });
                        i++;
                    }
                }
            }
            return _objstudentmodel;
        }
        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Password,EmpID,DateCreated,Name,Status,Department,CanEdit,CanDelete,CanAdd,CanView,CompanyID,RoleID,MHR,MDevice,MLeave,MDesktop,MEditAtt,MUser,MOption,MRoster,MRDailyAtt,MRLeave,MRMonthly,MRAudit,MRManualEditAtt,MREmployee,MRDetail,MRSummary,MRGraph,ViewPermanentStaff,ViewPermanentMgm,ViewContractual,ViewLocation,LocationID")] User user)
        {

            bool check = false;
            string _EmpNo = Request.Form["EmpNo"].ToString();
            List<Emp> _emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo).ToList();
            if (_emp.Count == 0)
            {
                check = true;
            }
            if (user.UserName == null)
                check = true;

            if (Request.Form["Status"] == "1")
                user.Status = true;
            else
                user.Status = false;

            if (Request.Form["CanEdit"] == "1")
                user.CanEdit = true;
            else
                user.CanEdit = false;

            if (Request.Form["CanDelete"] == "1")
                user.CanDelete = true;
            else
                user.CanDelete = false;

            if (Request.Form["CanAdd"] == "1")
                user.CanAdd = true;
            else
                user.CanAdd = false;

            if (Request.Form["CanView"] == "1")
                user.CanView = true;
            else
                user.CanView = false;
            if (Request.Form["MUser"] == "1")
                user.MUser = true;
            else
                user.MUser = false;
            if (Request.Form["MUser"] == "1")
                user.MUser = true;
            else
                user.MUser = false;
            if (Request.Form["MHR"] == "1")
                user.MHR = true;
            else
                user.MHR = false;
            if (Request.Form["MOption"] == "1")
                user.MOption = true;
            else
                user.MOption = false;
            if (Request.Form["MDevice"] == "1")
                user.MDevice = true;
            else
                user.MDevice = false;
            if (Request.Form["MDesktop"] == "1")
                user.MDesktop = true;
            else
                user.MDesktop = false;
            if (Request.Form["MEditAtt"] == "1")
                user.MEditAtt = true;
            else
                user.MEditAtt = false;
            if (Request.Form["MLeave"] == "1")
                user.MLeave = true;
            else
                user.MLeave = false;
            if (Request.Form["MRoster"] == "1")
                user.MRoster = true;
            else
                user.MRoster = false;
            if (Request.Form["MRLeave"] == "1")
                user.MRLeave = true;
            else
                user.MRLeave = false;
            if (Request.Form["MRDailyAtt"] == "1")
                user.MRDailyAtt = true;
            else
                user.MRDailyAtt = false;
            if (Request.Form["MRMonthly"] == "1")
                user.MRMonthly = true;
            else
                user.MRMonthly = false;
            if (Request.Form["MRAudit"] == "1")
                user.MRAudit = true;
            else
                user.MRAudit = false;
            if (Request.Form["MRManualEditAtt"] == "1")
                user.MRManualEditAtt = true;
            else
                user.MRManualEditAtt = false;
            if (Request.Form["MREmployee"] == "1")
                user.MREmployee = true;
            else
                user.MREmployee = false;
            if (Request.Form["MRDetail"] == "1")
                user.MRDetail = true;
            else
                user.MRDetail = false;
            if (Request.Form["MRSummary"] == "1")
                user.MRSummary = true;
            else
                user.MRSummary = false;
            if (Request.Form["MRGraph"] == "1")
                user.MRGraph = true;
            else
                user.MRGraph = false;

            if (Request.Form["ViewPermanentStaff"] == "1")
                user.ViewPermanentStaff = true;
            else
                user.ViewPermanentStaff = false;
            if (Request.Form["ViewPermanentMgm"] == "1")
                user.ViewPermanentMgm = true;
            else
                user.ViewPermanentMgm = false;
            if (Request.Form["ViewContractual"] == "1")
                user.ViewContractual = true;
            else
                user.ViewContractual = false;
            if (Request.Form["ViewLocation"] == "1")
                user.ViewLocation = true;
            else
                user.ViewLocation = false;

            if (check == false)
            {
                string _dpName = FindADUser(user.UserName);
                if (_dpName != "No")
                {
                    user.Name = _dpName;
                    user.DateCreated = DateTime.Today;
                    user.EmpID = _emp.FirstOrDefault().EmpID;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CompanyID = new SelectList(db.Companies, "CompID", "CompName", user.CompanyID);
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", user.EmpID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocID", "LocName", user.LocationID);
            return View(user);
        }

        private string FindADUser(string adUserName)
        {
            string displayName = "No";
            ADUsersModel adModel = GetADUsers();
            foreach (var item in adModel._ADUsersAttributes)
            {
                if (item.SAMName.ToUpper() == adUserName.ToUpper())
                {
                    displayName = item.DisplayName;
                }
            }
            return displayName;
        }


        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompID", "CompName", user.CompanyID);
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", user.EmpID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocID", "LocName", user.LocationID);
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password,EmpID,DateCreated,Name,Status,Department,CanEdit,CanDelete,CanAdd,CanView,CompanyID,RoleID,MHR,MDevice,MLeave,MDesktop,MEditAtt,MUser,MOption,MRDailyAtt,MRLeave,MRMonthly,MRAudit,MRManualEditAtt,MREmployee,MRDetail,MRSummary,MRGraph,ViewPermanentStaff,ViewPermanentMgm,ViewContractual,ViewLocation,LocationID")] User user)
        {
            bool check = false;
            user.RoleID = Convert.ToByte(Request.Form["RoleID"].ToString());
            if (Request.Form["Status"].ToString() == "true")
                user.Status = true;
            else
                user.Status = false;

            if (Request.Form["CanEdit"].ToString() == "true")
                user.CanEdit = true;
            else
                user.CanEdit = false;

            if (Request.Form["CanDelete"].ToString() == "true")
                user.CanDelete = true;
            else
                user.CanDelete = false;

            if (Request.Form["CanAdd"].ToString() == "true")
                user.CanAdd = true;
            else
                user.CanAdd = false;

            if (Request.Form["CanView"].ToString() == "true")
                user.CanView = true;
            else
                user.CanView = false;
            if (Request.Form["MUser"].ToString() == "true")
                user.MUser = true;
            else
                user.MUser = false;
            if (Request.Form["MUser"].ToString() == "true")
                user.MUser = true;
            else
                user.MUser = false;
            if (Request.Form["MHR"].ToString() == "true")
                user.MHR = true;
            else
                user.MHR = false;
            //if (Request.Form["MOption"].ToString() == "true")
            //    user.MOption = true;
            //else
            //    user.MOption = false;
            if (Request.Form["MDevice"].ToString() == "true")
                user.MDevice = true;
            else
                user.MDevice = false;
            if (Request.Form["MDesktop"].ToString() == "true")
                user.MDesktop = true;
            else
                user.MDesktop = false;
            if (Request.Form["MEditAtt"].ToString() == "true")
                user.MEditAtt = true;
            else
                user.MEditAtt = false;
            if (Request.Form["MLeave"].ToString() == "true")
                user.MLeave = true;
            else
                user.MLeave = false;
            if (Request.Form["MRLeave"].ToString() == "true")
                user.MRLeave = true;
            else
                user.MRLeave = false;
            if (Request.Form["MRDailyAtt"].ToString() == "true")
                user.MRDailyAtt = true;
            else
                user.MRDailyAtt = false;
            if (Request.Form["MRMonthly"].ToString() == "true")
                user.MRMonthly = true;
            else
                user.MRMonthly = false;
            if (Request.Form["MRAudit"].ToString() == "true")
                user.MRAudit = true;
            else
                user.MRAudit = false;
            if (Request.Form["MRManualEditAtt"].ToString() == "true")
                user.MRManualEditAtt = true;
            else
                user.MRManualEditAtt = false;
            if (Request.Form["MREmployee"].ToString() == "true")
                user.MREmployee = true;
            else
                user.MREmployee = false;
            if (Request.Form["MRDetail"].ToString() == "true")
                user.MRDetail = true;
            else
                user.MRDetail = false;
            if (Request.Form["MRSummary"].ToString() == "true")
                user.MRSummary = true;
            else
                user.MRSummary = false;
            if (Request.Form["MRGraph"].ToString() == "true")
                user.MRGraph = true;
            else
                user.MRGraph = false;

            if (check == false)
            {

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            ViewBag.CompanyID = new SelectList(db.Companies, "CompID", "CompName", user.CompanyID);
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", user.EmpID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocID", "LocName", user.LocationID);
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
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

    public class ADUsersAttributes
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string PrincipleName { get; set; }
        public string SAMName { get; set; }
        public string DistingushedName { get; set; }
        public bool Checked { get; set; }

    }
    public class ADUsersModel
    {
        public List<ADUsersAttributes> _ADUsersAttributes { get; set; }
    }

}
