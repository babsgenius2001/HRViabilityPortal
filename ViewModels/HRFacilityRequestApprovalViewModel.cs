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
    public class HRFacilityRequestApprovalViewModel : ViewModelBase
    {
        public HRFacilityRequestApprovalViewModel()
        {
            Init();
        }

        public List<HRFacilityMaster> FacilityReqs { get; set; }
        public HRFacilityMaster SearchEntity { get; set; }
        public HRFacilityMaster Entity { get; set; }
        public HRFacilityMaster FacilityReq { get; set; }

        public List<Facility> FacilityReqs2 { get; set; }
        public Facility SearchEntity2 { get; set; }
        public Facility Entity2 { get; set; }
        public Facility FacilityReq2 { get; set; }
        public List<GradesDAO> SalaryGrade { get; set; }
        public List<DocumentsDAO> DocumentsNeeded { get; set; }

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

            FacilityReqs2 = new List<Facility>();
            FacilityReq2 = new Facility();
            Entity2 = new Facility();
            SearchEntity2 = new Facility();
            SalaryGrade = new List<GradesDAO>();
            DocumentsNeeded = new List<DocumentsDAO>();

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
            Entity2 = GetFacilityRulesData();          

            return ret;
        }


        protected List<HRFacilityMaster> CreateData()
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.HRFacilityMasters.Where(x => x.requestStatus == "Awaiting_HeadHR_Approval").ToList();
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
                mail.ErrorLog(ErrorDescription, UserId);
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
        }

        protected Facility GetFacilityRulesData()
        {
            Facility ret = new Facility();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.Facilities.SingleOrDefault();

                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("Facility",
                      ex.Message));

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
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
            //if (string.IsNullOrEmpty(entity.hrApprover2Comment))
            //{
            //    ValidationErrors.Add(new
            //      KeyValuePair<string, string>("Comment",
            //      "Please Supply your Comment."));
            //    IsValid = false;
            //}
            if (string.IsNullOrEmpty(entity.hrApprover2ApproveReject))
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

            string repDhApproval = checkdhApproval(entity.facilityName);
            string repMdApproval = checkMdApproval(entity.facilityName);

            if (ret)
            {
                try { 

                    using (var db = new HRViabilityPortalEntities())
                    {
                    var rs = (from info in db.HRFacilityMasters where info.requestReferenceNumber == entity.requestReferenceNumber select info).FirstOrDefault();

                    if (repMdApproval == "YES")
                    {
                        
                        if (entity.hrApprover2ApproveReject == "2")
                        {

                            rs.hrApprover2 = UserId + "@jaizbankplc.com";
                            rs.hrApprover2Timestamp = DateTime.Now;
                            rs.hrApprover2ApproveReject = "Rejected";
                            rs.requestStatus = "Request_Rejected";
                            rs.hrApprover2RejectComment = entity.hrApprover2Comment;

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Facility Request Rejected with reason:" + rs.hrApprover2RejectComment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Rejected!";
                            IsValid = true;
                        }

                        else
                        {

                            rs.hrApprover2 = UserId + "@jaizbankplc.com";
                            rs.hrApprover2Timestamp = DateTime.Now;
                            rs.hrApprover2ApproveReject = "Approved";
                            rs.requestStatus = "Awaiting_MD_Approval";
                            rs.hrApprover2Comment = entity.hrApprover2Comment;

                            op = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Approved by HR";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId + "@jaizbankplc.com";
                            history.ActionPerformed = "Facility Request Approved with Reason:" + rs.hrApprover2Comment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request Approved with Reference No: " + entity.requestReferenceNumber;
                            IsValid = true;


                        }
                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    //if (Entity2.dhApproval == "YES")
                    if (repDhApproval == "YES")
                    {
                        //if(entity.facilityName == "Home Finance")
                        //{
                        if (entity.hrApprover2ApproveReject == "2")
                        {

                            rs.hrApprover2 = UserId + "@jaizbankplc.com";
                            rs.hrApprover2Timestamp = DateTime.Now;
                            rs.hrApprover2ApproveReject = "Rejected";
                            rs.requestStatus = "Request_Rejected";
                            rs.hrApprover2RejectComment = entity.hrApprover2RejectComment;

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Facility Request Rejected with reason:" + rs.hrApprover2RejectComment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Rejected!";
                            IsValid = true;
                        }

                        else
                        {

                            rs.hrApprover2 = UserId + "@jaizbankplc.com";
                            rs.hrApprover2Timestamp = DateTime.Now;
                            rs.hrApprover2ApproveReject = "Approved";
                            rs.requestStatus = "Awaiting_DH_Approval";
                            rs.hrApprover2Comment = entity.hrApprover2Comment;

                            op = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Approved by HR";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId + "@jaizbankplc.com";
                            history.ActionPerformed = "Facility Request Approved with Reason:" + rs.hrApprover2Comment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);

                            Msg = entity.facilityName + " Facility Request Approved with Reference No: " + entity.requestReferenceNumber;
                            IsValid = true;


                        }
                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    else
                    {
                        if (entity.hrApprover2ApproveReject == "2")
                        {

                            rs.hrApprover2 = UserId + "@jaizbankplc.com";
                            rs.hrApprover2Timestamp = DateTime.Now;
                            rs.hrApprover2ApproveReject = "Rejected";
                            rs.requestStatus = "Request_Rejected";
                            rs.hrApprover2RejectComment = entity.hrApprover2Comment;

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Facility Request Rejected with reason:" + rs.hrApprover2RejectComment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Rejected!";
                            IsValid = true;
                        }

                        else
                        {

                            rs.hrApprover2 = UserId + "@jaizbankplc.com";
                            rs.hrApprover2Timestamp = DateTime.Now;
                            rs.hrApprover2ApproveReject = "Approved";
                            rs.requestStatus = "Awaiting_BM_Approval";
                            rs.hrApprover2Comment = entity.hrApprover2Comment;

                            op = entity.facilityName + " Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Approved by HR";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId + "@jaizbankplc.com";
                            history.ActionPerformed = "Facility Request Approved with Reason:" + rs.hrApprover2Comment;
                            history.ReqId = entity.requestReferenceNumber;
                            db.ReqHistories.Add(history);
                            Msg = entity.facilityName + " Facility Request Approved with Reference No: " + entity.requestReferenceNumber;
                            IsValid = true;
                        }
                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                }catch(Exception ex)
                {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                 mail.ErrorLog(ErrorDescription, UserId);
                }
                    
                if (entity.hrApprover2ApproveReject == "2")
                {
                    sendReject(entity.requestReferenceNumber);
                }
                else if (entity.hrApprover2ApproveReject == "1" && repDhApproval == "YES")
                {
                    sendApprove1(entity.requestReferenceNumber);
                }
                else if (entity.hrApprover2ApproveReject == "1" && repMdApproval == "YES")
                {
                    sendApprove2(entity.requestReferenceNumber);
                }
            }
            return ret;
        }

        public string checkMdApproval(string facilityName)
        {
            string resp = "";          

            string query = "select mdApproval from Facility where facilityName = '" + facilityName + "'";

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
                                resp = reader["mdApproval"].ToString();
                            }

                            Connect.Close();
                        }
                    }
                }
            }catch(Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }             
                       
            return resp;
        }

        public string checkdhApproval(string facilityName)
        {
            string resp = "";

            string query = "select dhApproval from Facility where facilityName = '" + facilityName + "'";

            try { 

                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {
                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resp = reader["dhApproval"].ToString();
                            }

                            Connect.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }
            return resp;
        }

        public void sendReject(string refNo)
        {
            try
            {

                string subject = "Facility Request Has Been Rejected By Head HR!";
                string dt = DateTime.Now.ToString();

                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();
                    string emailreceipient = rs.initiator;
                    string initiator = rs.initiator;
                    string processor = UserId + "@jaizbankplc.com";
                    string reason = rs.hrApprover2RejectComment;
                    string hrreviewer = rs.hrApprover1;

                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/RejectedHRApprover.html"));
                    string line = sr.ReadToEnd();
                    sb.Append(line);
                    sb.Replace("{refNo}", refNo);
                    sb.Replace("{date}", dt);
                    sb.Replace("{initiator}", initiator);
                    sb.Replace("{hrreviewer}", hrreviewer);
                    sb.Replace("{headHR}", processor);                              
                    sb.Replace("{Reason}", reason);
                    
                    string body = sb.ToString();
                    var mail = new EmailSender();
                    mail.SendEmailMainHelper(emailreceipient, body, subject);
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }           
        }

        public void sendApprove1(string refNo)
        {
            try
            {
                string subject = "Facility Request Has Been Approved By Head HR And Awaiting Your Approval!";
                string dt = DateTime.Now.ToString();

                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();
                    string to = rs.initiator;                   
                    string hrReviewer = rs.hrApprover1;
                    string initiator = rs.initiator;
                    string receipient = ConfigurationManager.AppSettings["DH"].ToString();
                    string comments = rs.hrApprover2Comment;
                    string processor = rs.hrApprover2 + "@jaizbankplc.com";
                   
                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/HRApproverTemplate.html"));
                    string line = sr.ReadToEnd();
                    sb.Append(line);
                    sb.Replace("{refNo}", refNo);
                    sb.Replace("{initiator}", initiator);
                    sb.Replace("{hrreviewer}", hrReviewer);                  
                    sb.Replace("{date}", dt);
                    sb.Replace("{hrapprover}", processor);
                    sb.Replace("{comments}", comments);
                    string body = sb.ToString();
                    var mail = new EmailSender();
                    mail.SendEmailMainHelper(receipient, body, subject);
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }
        }

        public void sendApprove2(string refNo)
        {
            try
            {
                string subject = "Facility Request Has Been Approved By Head HR And Awaiting Your Approval!";
                string dt = DateTime.Now.ToString();

                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();
                    string to = rs.initiator;
                    string hrReviewer = rs.hrApprover1;
                    string initiator = rs.initiator;
                    string receipient = ConfigurationManager.AppSettings["MD"].ToString();
                    string comments = rs.hrApprover2Comment;
                    string processor = rs.hrApprover2 + "@jaizbankplc.com";

                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/HRApproverTemplate.html"));
                    string line = sr.ReadToEnd();
                    sb.Append(line);
                    sb.Replace("{refNo}", refNo);
                    sb.Replace("{initiator}", initiator);
                    sb.Replace("{hrreviewer}", hrReviewer);
                    sb.Replace("{date}", dt);
                    sb.Replace("{hrapprover}", processor);
                    sb.Replace("{comments}", comments);
                    string body = sb.ToString();
                    var mail = new EmailSender();
                    mail.SendEmailMainHelper(receipient, body, subject);
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }
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
                mail.ErrorLog(ErrorDescription, UserId);
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
                mail.ErrorLog(ErrorDescription, UserId);
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