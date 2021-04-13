using HRViabilityPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRViabilityPortal.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public AdminController()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        // GET: Admin
     
        public ActionResult Index(int? page)
        {
            AdminViewModel vm = new AdminViewModel();
            vm.branchCode = HttpContext.Session["branchCode"].ToString();
            string query1 = "";
            string query2 = "";
            string query3 = "";
            string query4 = "";
            int totalnoofrequests = 0;
            int rejected = 0;
            int approved = 0;
            int branchreqs = 0; 

            query1 = "SELECT COUNT(*) AS TOTALREQUESTS FROM HRFacilityMaster";

            using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand(query1, Connect))
                {
                    Connect.Open();

                    using (SqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            totalnoofrequests = Convert.ToInt32(reader["TOTALREQUESTS"].ToString());
                        }

                        Connect.Close();
                    }
                }
            }

            query2 = "SELECT COUNT(*) AS APPROVEDREQUESTS FROM HRFacilityMaster WHERE REQUESTSTATUS = 'Approved_For_Disbursement' ";

            using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand(query2, Connect))
                {
                    Connect.Open();

                    using (SqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            approved = Convert.ToInt32(reader["APPROVEDREQUESTS"].ToString());
                        }

                        Connect.Close();
                    }
                }
            }

            query3 = "SELECT COUNT(*) AS REJECTEDREQUESTS FROM HRFacilityMaster WHERE REQUESTSTATUS = 'Request_Rejected' ";

            using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand(query3, Connect))
                {
                    Connect.Open();

                    using (SqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rejected = Convert.ToInt32(reader["REJECTEDREQUESTS"].ToString());
                        }

                        Connect.Close();
                    }
                }
            }

            query4 = "SELECT COUNT(*) AS BRANCHREQUESTS FROM HRFacilityMaster WHERE BRANCHCODE = '" + vm.branchCode + "' ";

            using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand(query4, Connect))
                {
                    Connect.Open();

                    using (SqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            branchreqs = Convert.ToInt32(reader["BRANCHREQUESTS"].ToString());
                        }

                        Connect.Close();
                    }
                }
            }



            vm.TotalNoOfRequest = totalnoofrequests;
            vm.ApprovedRequests = approved;
            vm.RejectedRequests = rejected;
            vm.BranchRequests = branchreqs;

            vm.IsAllData = true;

            vm.HandleRequest();
            ModelState.Clear();
            return View(vm);
        }
           
        [HttpPost]
        public ActionResult Index(AdminViewModel vm)
        {
            vm.HandleRequest();
            ModelState.Clear();
            
            return View(vm);
        }
    }
}