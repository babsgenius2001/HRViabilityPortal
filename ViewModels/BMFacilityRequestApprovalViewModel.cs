using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;
using RestSharp;
using Newtonsoft.Json;
using HRViabilityPortal.Database_References;
using HRViabilityPortal.Controllers;
using System.Configuration;
using System.Data.SqlClient;
//using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
//using CrystalDecisions.Shared;

namespace HRViabilityPortal.ViewModels
{
    public class BMFacilityRequestApprovalViewModel : ViewModelBase
    {
        public BMFacilityRequestApprovalViewModel()
        {
            Init();
        }

        public List<HRFacilityMaster> FacilityReqs { get; set; }
        public HRFacilityMaster SearchEntity { get; set; }
        public HRFacilityMaster Entity { get; set; }
        public HRFacilityMaster FacilityReq { get; set; }
        public List<GradesDAO> SalaryGrade { get; set; }
        public List<DocumentsDAO> DocumentsNeeded { get; set; }
        public List<Facility> FacilityReqs2 { get; set; }
        public Facility SearchEntity2 { get; set; }
        public Facility Entity2 { get; set; }
        public Facility FacilityReq2 { get; set; }

        public string ActionTypeId { get; set; }
        public string Comment { get; set; }
        public string branchcode { get; set; }
        public List<SelectList> List { set; get; }
        public IPagedList PagedList { get; set; }

        private static SqlCommand command = null;
        private static SqlConnection conn = null;
        private static SqlTransaction tran = null;

        public HttpServerUtilityBase Server { get; }
        public HttpResponseBase Response { get; }

        protected override void Init()
        {
            FacilityReqs = new List<HRFacilityMaster>();
            FacilityReq = new HRFacilityMaster();
            Entity = new HRFacilityMaster();
            EventCommand = "reviewersearch";
            SearchEntity = new HRFacilityMaster();
            List = new List<SelectList>();
            SalaryGrade = new List<GradesDAO>();
            DocumentsNeeded = new List<DocumentsDAO>();

            FacilityReqs2 = new List<Facility>();
            FacilityReq2 = new Facility();
            Entity2 = new Facility();
            SearchEntity2 = new Facility();

            base.Init();
        }

        protected override void Get()
        {
            FacilityReqs = Get(SearchEntity);

            GetDropDownItems();

            base.Get();
        }

        protected override void Save()
        {
            if (Mode == "Add")
            {
                Insert(Entity);
            }
            else
            {
                Update(Entity);
            }
            // Set any validation errors
            ValidationErrors = ValidationErrors;
            // Set mode based on validation errors
            base.Save();
        }

        public List<HRFacilityMaster> Get(HRFacilityMaster entity)
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            // TODO: Add your own data access method here
            ret = CreateData();

            return ret;
        }


        protected List<HRFacilityMaster> CreateData()
        {
            //string url = "http://172.13.21.160:8013/JaizService/Api/JaizHelper";
            //var client = new RestClient(url + "/GetUserDetails");
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("ContentType", "application/json");
            //request.RequestFormat = DataFormat.Json;
            //request.AddJsonBody(new { UserName = UserId });
            //var s = client.Execute(request);
            //var Xxc = JsonConvert.DeserializeObject<dynamic>(s.Content);
            //var respon = JsonConvert.SerializeObject(Xxc);
            //var json = JsonConvert.DeserializeObject<Rootobject2>(respon);

            //if (json.Status)
            //{
            //    branchcode = json.Data.branch;

            //}

            //sendApprove("HRREF20210209143335");

            string branchC = fetchBMBranchCode(UserId);

            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    // ret = db.HRFacilityMasters.Where(x => x.requestReferenceNumber == "HRREF20210121164753").ToList();
                    ret = db.HRFacilityMasters.Where(x => x.requestStatus == "Awaiting_BM_Approval" && x.branchCode.Trim() == branchC.Trim()).ToList();
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

        public string fetchBMBranchCode(string userId)
        {
            string brnCode = "";
            string query1 = "";

            query1 = "select branchCode from BMDetails where staffId = '" + userId + "'";

            try
            {
                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {

                    using (SqlCommand Cmd1 = new SqlCommand(query1, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                brnCode = reader["branchCode"].ToString();
                            }

                            Connect.Close();
                        }
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

            return brnCode;
        }
        public bool Insert(HRFacilityMaster entity)
        {
            bool ret = false;
            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    //TODO: Create Insert Code here
                    db.HRFacilityMasters.Add(entity);
                    db.SaveChanges();
                }
            }
            return ret;
        }

        public bool Validate(HRFacilityMaster entity)
        {
            ValidationErrors.Clear();
            if (string.IsNullOrEmpty(entity.bmBranchOfRequestComment))
            {
                ValidationErrors.Add(new
                  KeyValuePair<string, string>("Comment",
                  "Please Supply your Comment."));
                IsValid = false;
            }
            if (string.IsNullOrEmpty(entity.bmBranchOfRequestApproveReject))
            {
                ValidationErrors.Add(new
                  KeyValuePair<string, string>("Comment",
                  "Please select action."));
                IsValid = false;
            }
            return (ValidationErrors.Count == 0);
        }

        public bool Update(HRFacilityMaster entity)
        {
            bool ret = false;
            ret = Validate(entity);
            //ret = true;
            GetDropDownItems();
            string op = "";

            if (ret)
            {
                try
                {
                    using (var db = new HRViabilityPortalEntities())
                    {
                        var rs = (from info in db.HRFacilityMasters where info.requestReferenceNumber == entity.requestReferenceNumber select info).FirstOrDefault();
                        rs.bmBranchOfRequestComment = entity.bmBranchOfRequestComment;

                        if (entity.bmBranchOfRequestApproveReject == "2")
                        {

                            rs.bmBranchOfRequest = UserId + "@jaizbankplc.com";
                            rs.bmBranchOfRequestTimestamp = DateTime.Now;
                            rs.bmBranchOfRequestApproveReject = "Rejected";
                            rs.requestStatus = "Request_Rejected";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Facility Request Rejected with reason:" + rs.bmBranchOfRequestComment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Rejected!";
                            IsValid = true;
                        }

                        else
                        {

                            rs.bmBranchOfRequest = UserId + "@jaizbankplc.com";
                            rs.bmBranchOfRequestTimestamp = DateTime.Now;
                            rs.bmBranchOfRequestApproveReject = "Approved";
                            rs.requestStatus = "Approved_For_Disbursement";

                            op = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Approved by the BM of your branch";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId + "@jaizbankplc.com";
                            history.ActionPerformed = "Facility Request Approved with Reason:" + rs.bmBranchOfRequestComment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request Approved with Reference No: " + entity.requestReferenceNumber;
                            IsValid = true;
                        }
                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();

                    }

                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {

                }
                if (entity.bmBranchOfRequestApproveReject == "2")
                {
                    sendReject(entity.requestReferenceNumber);
                }
                else
                {
                    ///sendDisbursementReceipt(entity.requestReferenceNumber);
                    sendApprove(entity.requestReferenceNumber);
                }

                // sendApprove("HRREF20210209143335");
            }
            return ret;
        }

        public void sendReject(string refNo)
        {
            try
            {

                string subject = "Facility Request Rejected!";
                string dt = DateTime.Now.ToString();

                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();
                    string to = rs.initiator;                    
                    string initiator = rs.initiator;
                    string processor = UserId + "@jaizbankplc.com";
                    string reason = rs.bmApproverRejectComment;
                    string hrreviewer = rs.hrApprover1;
                    string hrapprover = rs.hrApprover2;
                    string dh = rs.dhApprover;
                    string md = rs.mdApprover;
                    string bm = rs.bmBranchOfRequest;

                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/RejectedBM.html"));
                    string line = sr.ReadToEnd();
                    sb.Append(line);
                    sb.Replace("{refNo}", refNo);
                    sb.Replace("{date}", dt);
                    sb.Replace("{initiator}", initiator);
                    sb.Replace("{hrreviewer}", hrreviewer);
                    sb.Replace("{hrapprover}", hrapprover);
                    sb.Replace("{dh}", dh);
                    sb.Replace("{md}", md);
                    sb.Replace("{bm}", bm);
                    sb.Replace("{Reason}", reason);
                    string body = sb.ToString();
                    var mail = new EmailSender();

                    mail.SendEmailMainHelper(to, body, subject);
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, Entity.staffId);
            }
        }

        public void sendApprove(string refNo)
        {
            try
            {
                string subject = "New Facility Request Approved!";
                string dt = DateTime.Now.ToString();

                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();
                    string to = rs.initiator;
                    //string to = "BA07190@jaizbankplc.com";
                    string initiator = rs.initiator;
                    string reason = rs.bmBranchOfRequestComment;
                    string hrreviewer = rs.hrApprover1;
                    string hrapprover = rs.hrApprover2;
                    string dh = rs.dhApprover;
                    string md = rs.mdApprover;
                    string bm = rs.bmBranchOfRequest;

                    string amount = String.Format("{0:n}", rs.amountRequested);
                    string tenor = rs.tenor.ToString();
                    string rate = rs.percentageRate.ToString();
                    string repayment = String.Format("{0:n}", rs.repaymentAmount);

                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/BMApproverTemplate.html"));
                    string line = sr.ReadToEnd();
                    sb.Append(line);
                    sb.Replace("{refNo}", refNo);
                    sb.Replace("{amount}", amount);
                    sb.Replace("{tenor}", tenor);
                    sb.Replace("{rate}", rate);
                    sb.Replace("{repayment}", repayment);
                    sb.Replace("{date}", dt);
                    sb.Replace("{initiator}", initiator);
                    //sb.Replace("{hrreviewer}", hrreviewer);
                    //sb.Replace("{hrapprover}", hrapprover);
                    //sb.Replace("{dh}", dh);
                    //sb.Replace("{md}", md);
                    //sb.Replace("{bm}", bm);
                    //sb.Replace("{Reason}", reason);
                    string body = sb.ToString();
                    var mail = new EmailSender();
                    mail.SendEmailMainHelper(to, body, subject);
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, Entity.staffId);
            }
        }


        //public void sendDisbursementReceipt(string refNo)
        //{
        //    ReportDocument rd = new ReportDocument();
        //   // int res = 0;

        //    DataTable dt_report = fetchReportData(refNo);

        //    if (dt_report.Rows.Count > 0)
        //    {
        //        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "FacilityReceipt.rpt"));
        //        rd.SetDataSource(dt_report);

        //        string accountName = "";
        //        string staffId = "";

        //        using (var db = new HRViabilityPortalEntities())
        //        {
        //            var rspF = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();

        //            accountName = rspF.accountName;
        //            staffId = rspF.staffId;
        //        }
               
        //        string mailSubject = "Facility Request Receipt For " + accountName;
        //        string mailBody = "Hello, Kindly find attached your Facility Request Receipt For " + accountName;

        //        HttpClient client = new HttpClient();
        //        client.BaseAddress = new Uri("https://1jr2x.api.infobip.com/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", getAuthorizationHeaderString());

        //        var request = new MultipartFormDataContent();
        //        request.Add(new StreamContent(rd.ExportToStream(ExportFormatType.PortableDocFormat)), "attachment", "Receipt_"+staffId+".pdf");               
        //        request.Add(new StringContent("Jaiz Bank Plc <notification@alerts.jaizbankplc.com>"), "from");
        //        request.Add(new StringContent(FacilityReq.staffId + "@jaizbankplc.com"), "to");
        //        request.Add(new StringContent(mailSubject), "subject");
        //        request.Add(new StringContent(mailBody), "html");
        //        request.Add(new StringContent("true"), "intermediateReport");
        //        request.Add(new StringContent("application/json"), "notifyContentType");
        //        request.Add(new StringContent("DLR callback data"), "callbackData");


        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();

        //    //    rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
        //    //    rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
        //    //    rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

        //    //    //Printer
        //    //    rd.PrintToPrinter(1, true, 0, 0);
        //    }

        //}

        //public DataTable fetchReportData(string requestReference)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        conn = new SqlConnection(_ConnectionString());
        //        conn.Open();

        //        command = new SqlCommand("SELECT_REPORT_TRANS", conn);
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.Parameters.Add("@requestReference", SqlDbType.VarChar).Value = requestReference;

        //        SqlDataAdapter adapter = new SqlDataAdapter(command);

        //        adapter.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        var err2 = new LogUtility.Error()
        //        {
        //            ErrorDescription = "Error Selecting Data from HRFacilityMaster Table: " + ex.InnerException.ToString(),
        //            ErrorTime = DateTime.Now,
        //            ModulePointer = "fetchReportData",
        //            StackTrace = ex.Message
        //        };
        //        LogUtility.ActivityLogger.WriteErrorLog(err2);
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //            conn.Close();
        //    }

        //    return dt;
        //}
               

        public string getAuthorizationHeaderString()
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
            string header = System.Convert.ToBase64String(concatenated);

            return header;
        }





        protected override void Edit()
        {

            IsValid = true;
            Entity = GetFacilityData(EventArgument);
            Entity2 = GetFacilityRulesData(Entity.facilityName);
            SalaryGrade = GetAllApplicableGrades();
            DocumentsNeeded = GetAllRequiredDocuments();

            GetDropDownItems();

            base.Edit();
        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public List<GradesDAO> GetAllApplicableGrades()
        {
            List<GradesDAO> ret = new List<GradesDAO>();
            GradesDAO mert = null;
            string query = "";
            int count = 0;

            if (Entity.facilityName == "Murabaha")
            {
                query = "select distinct(gradeName) from MurabahaGradesTable where active = 'Y' order by gradeName asc";

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
                                    mert = new GradesDAO();
                                    count++;

                                    mert.gradeName = reader.GetString(reader.GetOrdinal("gradeName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName == "Home Finance")
            {
                query = "select distinct(gradeName) from HomeFinanceGradesTable where active = 'Y' order by gradeName asc";

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
                                    mert = new GradesDAO();
                                    count++;

                                    mert.gradeName = reader.GetString(reader.GetOrdinal("gradeName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName == "Ijara Service")
            {
                query = "select distinct(gradeName) from IjaraServiceGradesTable where active = 'Y' order by gradeName asc";

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
                                    mert = new GradesDAO();
                                    count++;

                                    mert.gradeName = reader.GetString(reader.GetOrdinal("gradeName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName == "Bai Muajjal")
            {
                query = "select distinct(gradeName) from BaiMuajjalGradesTable where active = 'Y' order by gradeName asc";

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
                                    mert = new GradesDAO();
                                    count++;

                                    mert.gradeName = reader.GetString(reader.GetOrdinal("gradeName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName.Contains("Branch") || Entity.facilityName.Contains("branch"))
            {
                query = "select distinct(gradeName) from BranchGradesTable where active = 'Y' order by gradeName asc";

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
                                    mert = new GradesDAO();
                                    count++;

                                    mert.gradeName = reader.GetString(reader.GetOrdinal("gradeName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }

                IsBranch = true;
            }

            else
            {
                String facility = Entity.facilityName.Replace(" ", "");
                query = "select distinct(gradeName) from " + facility + "GradesTable where active = 'Y' order by gradeName asc";

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
                                    mert = new GradesDAO();
                                    count++;

                                    mert.gradeName = reader.GetString(reader.GetOrdinal("gradeName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }

                IsOthers = true;
            }



            return ret;
        }

        public List<DocumentsDAO> GetAllRequiredDocuments()
        {
            List<DocumentsDAO> ret = new List<DocumentsDAO>();
            DocumentsDAO mert = null;
            string query = "";
            int count = 0;

            if (Entity.facilityName == "Murabaha")
            {
                query = "select distinct(documentName) from MurabahaDocumentsTable where active = 'Y'  and documentName != 'Not Applicable' order by documentName asc";

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
                                    mert = new DocumentsDAO();
                                    count++;

                                    mert.documentName = reader.GetString(reader.GetOrdinal("documentName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName == "Home Finance")
            {
                query = "select distinct(documentName) from HomeFinanceDocumentsTable where active = 'Y'  and documentName != 'Not Applicable' order by documentName asc";

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
                                    mert = new DocumentsDAO();
                                    count++;

                                    mert.documentName = reader.GetString(reader.GetOrdinal("documentName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName == "Ijara Service")
            {
                query = "select distinct(documentName) from IjaraServiceDocumentsTable where active = 'Y'  and documentName != 'Not Applicable' order by documentName asc";

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
                                    mert = new DocumentsDAO();
                                    count++;

                                    mert.documentName = reader.GetString(reader.GetOrdinal("documentName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName == "Bai Muajjal")
            {
                query = "select distinct(documentName) from BaiMuajjalDocumentsTable where active = 'Y'  and documentName != 'Not Applicable' order by documentName asc";

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
                                    mert = new DocumentsDAO();
                                    count++;

                                    mert.documentName = reader.GetString(reader.GetOrdinal("documentName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }
            }

            else if (Entity.facilityName.Contains("Branch") || Entity.facilityName.Contains("branch"))
            {
                query = "select distinct(documentName) from BranchDocumentsTable where active = 'Y'  and documentName != 'Not Applicable' order by documentName asc";

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
                                    mert = new DocumentsDAO();
                                    count++;

                                    mert.documentName = reader.GetString(reader.GetOrdinal("documentName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }

                IsBranch = true;
            }

            else
            {

                String facility = Entity.facilityName.Replace(" ", "");
                query = "select distinct(documentName) from " + facility + "DocumentsTable where active = 'Y'  and documentName != 'Not Applicable' order by documentName asc";

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
                                    mert = new DocumentsDAO();
                                    count++;

                                    mert.documentName = reader.GetString(reader.GetOrdinal("documentName"));
                                    mert.space = "";

                                    ret.Add(mert);

                                }

                                Connect.Close();
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
                    mail.ErrorLog(ErrorDescription, Entity.staffId);
                }
                finally
                {
                    //db.Configuration.Close();
                }

                IsOthers = true;
            }



            return ret;
        }

        protected Facility GetFacilityRulesData(string facilityName)
        {
            Facility ret = new Facility();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    ret = db.Facilities.Where(x => x.facilityName == facilityName && x.active == "Y").SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRFacility",
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
        protected HRFacilityMaster GetFacilityData(string refNo)
        {
            HRFacilityMaster ret = new HRFacilityMaster();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.HRFacilityMasters.Where(x => x.requestReferenceNumber == refNo).SingleOrDefault();

                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRFacility",
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
        protected void GetDropDownItems()
        {
            SelectList ItemE2 = new SelectList();
            ItemE2.Text = "Approve"; ItemE2.Value = "4";
            List.Add(ItemE2);
            SelectList ItemE = new SelectList();
            ItemE.Text = "Reject"; ItemE.Value = "2";
            List.Add(ItemE);
        }
        public class SelectList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class Rootobject2
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public UsersDetails Data { get; set; }
        }

        public class UsersDetails
        {
            public string username { get; set; }
            public string email { get; set; }
            public string fullname { get; set; }
            public string surname { get; set; }
            public string first_name { get; set; }
            public string middle_name { get; set; }
            public string branch { get; set; }
            public string department { get; set; }
            public string unit { get; set; }
            public string supervisor_username { get; set; }
        }

        public class GradesDAO
        {
            public int id { get; set; }
            public string gradeName { get; set; }
            public string space { get; set; }
            public bool isSelected { get; set; }

        }

        public class DocumentsDAO
        {
            public int id { get; set; }
            public string documentName { get; set; }
            public string space { get; set; }
            public bool isSelected { get; set; }

        }
    }
}