using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.CustomClass
{
    public class QueryBuilder
    {
        public DataTable GetValuesfromDB(string query)
        {
            DataTable dt = new DataTable();
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TAS2013ConnectionString"].ConnectionString);

            using (SqlCommand cmdd = Conn.CreateCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(cmdd))
            {
                cmdd.CommandText = query;
                cmdd.CommandType = CommandType.Text;
                Conn.Open();
                sda.Fill(dt);
                Conn.Close();
            }
            return dt;
        }
        public string MakeCustomizeQuery(User _user)
        {
            string query = " where ";
            string subQuery = "";
            List<string> _Criteria = new List<string>();
            List<string> _CriteriaForOr = new List<string>();
           if (_user.ViewLocation == true)
            {
                _Criteria.Add(" LocID = " + _user.LocationID.ToString());
            }
            if (_user.ViewContractual == true)
            {
                _CriteriaForOr.Add(" CatID = 3 ");
            }
            if (_user.ViewPermanentMgm == true)
            {
                _CriteriaForOr.Add(" CatID = 2  ");
            }
            if (_user.ViewPermanentStaff == true)
            {
                _CriteriaForOr.Add(" CatID = 4  ");
            }
            _CriteriaForOr.Add(" CatID=1 ");
            
            switch (_user.RoleID)
            {
                case 1:
                    _CriteriaForOr.Add("CatID >= 7 ");
                    break;
                case 2:
                    _Criteria.Add(" CompanyID= 1 or CompanyID = 2 ");
                    _CriteriaForOr.Add("CatID = 7 ");
                    break;
                case 3:
                    _Criteria.Add(" CompanyID>= 3");
                    _CriteriaForOr.Add("CatID >= 8 ");
                    break;
                case 4:
                    _Criteria.Add(" CompanyID = "+_user.CompanyID.ToString());
                    break;
                case 5:
                    break;
            }
            for (int i = 0; i < _Criteria.Count; i++ )
            {
                query = query + _Criteria[i] + " and ";
            }
            //query = query + " ) and (";
            //query = query + _Criteria[_Criteria.Count-1];

            subQuery = " ( ";
            for (int i = 0; i < _CriteriaForOr.Count - 1; i++)
            {
                subQuery = subQuery + _CriteriaForOr[i] + " or ";
            }
            subQuery = subQuery + _CriteriaForOr[_CriteriaForOr.Count - 1];
            subQuery = subQuery + " ) ";
            query = query + subQuery;
            return query;
        }

        public string QueryForCompanySegeration(User _user)
        {
            string query = "";
            switch (_user.RoleID)
            {
                case 1:

                    break;
                case 2:
                    query = " CompanyID= 1 or CompanyID = 2 ";
                    break;
                case 3:
                    query=" CompanyID>= 3";
                    break;
                case 4:
                    query=" CompanyID = " + _user.CompanyID.ToString();
                    break;
                case 5:
                    break;
            }
            if (query != "")
            {
                query = " where " + query;
            }
            return query;
        }
    }
}