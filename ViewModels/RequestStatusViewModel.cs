using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;
using HRViabilityPortal.Database_References;
using HRViabilityPortal.Controllers;
using System.Data.SqlClient;
using System.Configuration;

namespace HRViabilityPortal.ViewModels
{
    public class RequestStatusViewModel : ViewModelBase
    {
        public RequestStatusViewModel()
        {
            Init();
        }

        public List<HRFacilityMaster> FacilityReqs { get; set; }
        public List<FacilityRequestsDAO> HistoricalReqs { get; set; }
        public HRFacilityMaster SearchEntity { get; set; }
        public AuditLog AuditSearch { get; set; }
        public HRFacilityMaster Entity { get; set; }
        public HRFacilityMaster FacilityReq { get; set; }
        public string loggedInUser { get; set; }

        protected override void Init()
        {
            FacilityReq = new HRFacilityMaster();
            FacilityReqs = new List<HRFacilityMaster>();
            HistoricalReqs = new List<FacilityRequestsDAO>();
            SearchEntity = new HRFacilityMaster();
            AuditSearch = new AuditLog();
            Entity = new HRFacilityMaster();
            EventCommand = "historical";
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        protected override void Get()
        {
            FacilityReqs = Get(SearchEntity);
                       
            base.Get();
        }

        protected override void GetHistory()
        {
            HistoricalReqs = GetHistoricalData(loggedInUser);
           // HistoricalReqs = Get(SearchEntity);

            base.GetHistory();
        } 

        public bool checkRefNo(HRFacilityMaster entity)
        {
            bool ret = false;

            if (string.IsNullOrEmpty(entity.requestReferenceNumber))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Kindly Provide The Reference Number!"));

                IsValid = false;
                return false;
            }

            else
            {
                IsValid = true;
                ret = true;
            }

            return ret;
        }

        public List<HRFacilityMaster> Get(HRFacilityMaster entity)
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            //if (checkRefNo(entity))
            //{
                ret = CreateData();

                //if (ret.Count() == 0)
                //{
                //    //var util = new Utilities();
                //    //util.AlertPopup =
                //    //  "ERROR <br/><br/>No Record Found!. Provide A Correct Request ID!";

                //    //string op = "No Record Found!. Provide A Correct Request ID!";
                //    //Msg = op;
                //    ValidationErrors.Add(new
                //  KeyValuePair<string, string>("",
                //      "No Record Found!. Provide A Valid/Correct Reference Number!"));

                //    //IsSearchAreaVisible = false;
                //    IsValid = false;

                //    IsSearchAreaVisible = true;
                //    IsListAreaVisible = true;
                //    IsHistoricalListAreaVisible = true;

                //    string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

                //    var log = new AllLogs();
                //    log.writeAuditlog(FacilityReq.staffId, UserId + "@jaizbankplc.com", "No Record Found!. Provide A Valid/Correct Reference Number!", ip);
                //}
                //else
                //{
                //    ret = ret.FindAll(p => p.requestReferenceNumber.Trim().StartsWith(entity.requestReferenceNumber.Trim()));

                //    string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                  
                //    var log = new AllLogs();
                //    log.writeAuditlog(FacilityReq.staffId, UserId + "@jaizbankplc.com", "Checked the Status Of Facility With Reference Number: " + entity.requestReferenceNumber, ip);
                //}
           // }
                    


            //if (!string.IsNullOrEmpty(entity.requestReferenceNumber))
            //{
                
                

            //}

            return ret;
        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public List<FacilityRequestsDAO> GetHistoricalData(string loggeduser)
        {
            List<FacilityRequestsDAO> ret = new List<FacilityRequestsDAO>();
            FacilityRequestsDAO mert = null;
            string query = "";
            int count = 0;

            query = "select requestReferenceNumber, facilityName, amountRequested, requestDate, requestStatus";
            query+= " FROM HRFacilityMaster where staffId = '"+loggeduser+"'";
          
            try
            {
                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {

                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mert = new FacilityRequestsDAO();
                                count++;

                                mert.refNumber = reader.GetString(reader.GetOrdinal("requestReferenceNumber"));
                                mert.facilityName = reader.GetString(reader.GetOrdinal("facilityName"));
                                mert.amountRequestedFor = reader.GetDecimal(reader.GetOrdinal("amountRequested"));
                                mert.dateChecked = reader.GetDateTime(reader.GetOrdinal("requestDate"));
                                mert.statusOfFacility = reader.GetString(reader.GetOrdinal("requestStatus"));

                                ret.Add(mert);
                            }

                            Connect.Close();

                            if (PageNumber > 0 && PageSize > 0)
                            {
                                PagedList = ret.ToPagedList(PageNumber, PageSize);
                                ret = ret.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                //put error log here
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, loggeduser);
            }
            finally
            {
                //db.Configuration.Close();
            }

            return ret;
        }

        public string GetBMDetails(string brnCode)
        {
            string bmName = "";

            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    var ret = db.BMDetails.Where(x => x.branchCode == brnCode).SingleOrDefault();

                    bmName = ret.emailAddress;
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, Entity.staffId);
            }
            finally
            {
                //db.Configuration.Close();
            }

            return bmName;
        }


        public string GetRequestStatusDetails(string requestStatus)
        {
                    string status = "";          

                    if(requestStatus == "Added_To_Queue")
                    {
                        status = "Request On Queue";
                    }
                    if (requestStatus == "Approved_For_Disbursement")
                    {
                        status = "Request Approved For Disbursement";
                    }
                    if (requestStatus == "Awaiting_BM_Approval")
                    {
                        status = "Request Pending On Branch Manager";
                    }
                    if (requestStatus == "Awaiting_HeadHR_Approval")
                    {
                        status = "Request Pending On Head, HRD";
                    }
                    if (requestStatus == "Awaiting_HR_Review")
                    {
                        status = "Request Pending On HRD Reviewer";
                    }
                    if (requestStatus == "Request_Rejected")
                    {
                        status = "Request Has Been Rejected";
                    }
              

            return status;
        }

        public string GetRejectionReason(string refNo)
        {
            string reason = "";

            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    var ret = db.HRFacilityMasters.Where(x => x.requestReferenceNumber == refNo).SingleOrDefault();
                                       
                    if (!String.IsNullOrEmpty(ret.hrApprover1RejectComment))
                    {
                        reason = ret.hrApprover1RejectComment;
                    }
                    else if (!String.IsNullOrEmpty(ret.hrApprover2RejectComment))
                    {
                        reason = ret.hrApprover2RejectComment;
                    }
                   else if (!String.IsNullOrEmpty(ret.mdApproverRejectComment))
                    {
                        reason = ret.mdApproverRejectComment;
                    }
                    else if (!String.IsNullOrEmpty(ret.bmApproverRejectComment))
                    {
                        reason = ret.bmApproverRejectComment;
                    }
                    else if (!String.IsNullOrEmpty(ret.dhApproverRejectComment))
                    {
                        reason = ret.dhApproverRejectComment;
                    }
                    else
                    {
                        reason = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, Entity.staffId);
            }
            finally
            {
                //db.Configuration.Close();
            }

            return reason;
        }

        protected List<HRFacilityMaster> CreateData()
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.HRFacilityMasters.Where(x => x.staffId == loggedInUser).OrderByDescending(x=> x.requestDate).ToList();
                    //ret = db.HRFacilityMasters.Where(x => x.requestStatus == "Awaiting_HR_Review" || x.requestStatus == "Awaiting_HeadHR_Approval" || x.requestStatus == "Awaiting_MD_Approval" || x.requestStatus == "Awaiting_BM_Approval" || x.requestStatus == "Request_Rejected" || x.requestStatus == "Approved_For_Disbursement").ToList();


                    if (PageNumber > 0 && PageSize > 0)
                    {
                        PagedList = ret.ToPagedList(PageNumber, PageSize);
                        ret = ret.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, Entity.staffId);
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
        }    

        public class FacilityRequestsDAO
        {
            public string refNumber { get; set; }
            public string facilityName { get; set; }
            public decimal amountRequestedFor { get; set; }
            public DateTime dateChecked { get; set; }
            public string statusOfFacility { get; set; }
        }

    }

}