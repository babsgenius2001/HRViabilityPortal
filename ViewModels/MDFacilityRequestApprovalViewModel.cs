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
using System.Configuration;
using System.Data.SqlClient;

namespace HRViabilityPortal.ViewModels
{
    public class MDFacilityRequestApprovalViewModel : ViewModelBase
    {

        public MDFacilityRequestApprovalViewModel()
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
        public string brnCode { get; set; }
        public string ActionTypeId { get; set; }
        public string Comment { get; set; }
        public List<SelectList> List { set; get; }
        public IPagedList PagedList { get; set; }

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
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.HRFacilityMasters.Where(x => x.requestStatus == "Awaiting_MD_Approval").ToList();
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
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
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
            if (string.IsNullOrEmpty(entity.mdApproverComment))
            {
                ValidationErrors.Add(new
                  KeyValuePair<string, string>("Comment",
                  "Please Supply your Comment."));
                IsValid = false;
            }
            if (string.IsNullOrEmpty(entity.mdApproverApproveReject))
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
            GetDropDownItems();
            string op = "";

            if (ret)
            {
                try
                {

                    using (var db = new HRViabilityPortalEntities())
                    {
                        var rs = (from info in db.HRFacilityMasters where info.requestReferenceNumber == entity.requestReferenceNumber select info).FirstOrDefault();
                        rs.mdApproverComment = entity.mdApproverComment;


                        if (entity.mdApproverApproveReject == "2")
                        {

                            rs.mdApprover = UserId + "@jaizbankplc.com";
                            rs.mdApproverTimestamp = DateTime.Now;
                            rs.mdApproverApproveReject = "Rejected";
                            rs.requestStatus = "Request_Rejected";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Facility Request Rejected with reason:" + rs.mdApproverComment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Rejected!";
                            IsValid = true;
                        }

                        else
                        {

                            rs.mdApprover = UserId + "@jaizbankplc.com";
                            rs.mdApproverTimestamp = DateTime.Now;
                            rs.mdApproverApproveReject = "Approved";
                            rs.requestStatus = "Awaiting_BM_Approval";

                            op = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Approved by HR";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId + "@jaizbankplc.com";
                            history.ActionPerformed = "Facility Request Approved with Reason:" + rs.mdApproverComment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request Approved with Reference No: " + entity.requestReferenceNumber;
                            IsValid = true;
                        }
                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();

                    }

                }catch(Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }

                if (entity.hrApprover2ApproveReject == "2")
                {
                    sendReject(entity.requestReferenceNumber);
                }
                else
                {
                    sendApprove(entity.requestReferenceNumber);
                }
            }
            return ret;
        }
        public void sendReject(string refNo)
        {
            try
            {

                string subject = "New Facility Request!";
                string dt = DateTime.Now.ToString();

                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();
                    string to = rs.initiator;
                    string initiator = rs.initiator;
                    string md = UserId + "@jaizbankplc.com";
                    string reason = rs.mdApproverRejectComment;
                    string hrreviewer = rs.hrApprover1;
                    string hrapprover = rs.hrApprover2;
                    string dh = rs.dhApprover;
                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/RejectedMD.html"));
                    string line = sr.ReadToEnd();
                    sb.Append(line);
                    sb.Replace("{refNo}", refNo);
                    sb.Replace("{date}", dt);                   
                    sb.Replace("{initiator}", initiator);
                    sb.Replace("{hrreviewer}", hrreviewer);
                    sb.Replace("{hrapprover}", hrapprover);
                    sb.Replace("{dh}", dh);
                    sb.Replace("{md}", md);
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
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
        }
        public void sendApprove(string refNo)
        {
            try
            {
                string subject = "New Facility Request for your Approval!";
                string dt = DateTime.Now.ToString();

                string bmEmail = fetchBMEmail(refNo);

                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();
                    string to = rs.initiator;
                    string initiator = rs.initiator;                    
                    string reason = rs.mdApproverComment;
                    string hrreviewer = rs.hrApprover1;
                    string hrapprover = rs.hrApprover2;                 
                    string dh = rs.dhApprover;
                    string processor = UserId + "@jaizbankplc.com";

                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/MDApproverTemplate.html"));
                    string line = sr.ReadToEnd();
                    sb.Append(line);
                    sb.Replace("{refNo}", refNo);
                    sb.Replace("{date}", dt);
                    sb.Replace("{initiator}", initiator);
                    sb.Replace("{hrreviewer}", hrreviewer);
                    sb.Replace("{hrapprover}", hrapprover);
                    sb.Replace("{md}", UserId + "@jaizbankplc.com");
                    sb.Replace("{dh}", dh);
                    sb.Replace("{reason}", reason);
                    string body = sb.ToString();
                    var mail = new EmailSender();
                    mail.SendEmailMainHelper(bmEmail, body, subject);
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
        }

        public string fetchBMEmail(string refNo)
        {
            string bmemail = "";
            string query1, query2 = "";

            query1 = "select branchCode from HRFacilityMaster where requestReferenceNumber = '" + refNo + "'";

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


                    query2 = "select emailAddress from BMDetails where branchCode = '" + brnCode + "'";

                    using (SqlCommand Cmd = new SqlCommand(query2, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bmemail = reader["emailAddress"].ToString();
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
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
            finally
            {
                //db.Configuration.Close();
            }

            return bmemail;
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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
                    //ret = db.HRVehicleMasters.Where(x => x.requestReferenceNumber == FacilityVR.requestReferenceNumber).SingleOrDefault();
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
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
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