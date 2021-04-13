using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;

namespace HRViabilityPortal.ViewModels
{
    public class EditFacilityRuleViewModel : ViewModelBase
    {
        public EditFacilityRuleViewModel()
        {
            Init();
        }

        public List<Facility> FacilityRules { get; set; }
        public Facility SearchEntity { get; set; }
        public Facility Entity { get; set; }
        public Facility FacilityRule { get; set; }
        public List<DocumentsDAO> DocumentsNeeded { get; set; }
        public List<SelectList> List { set; get; }

        protected override void Init()
        {
            FacilityRules = new List<Facility>();
            FacilityRule = new Facility();
            Entity = new Facility();
            EventCommand = "reviewersearch";
            SearchEntity = new Facility();
            List = new List<SelectList>();
            DocumentsNeeded = new List<DocumentsDAO>();

            base.Init();
        }

        protected override void Get()
        {
            FacilityRules = Get(SearchEntity);

            GetDropDownItems();

            base.Get();
        }

        public List<Facility> Get(Facility entity)
        {
            List<Facility> ret = new List<Facility>();
            // TODO: Add your own data access method here
            ret = CreateData();

            return ret;
        }

        protected void GetDropDownItems()
        {
            SelectList ItemE2 = new SelectList();
            ItemE2.Text = "Enable"; ItemE2.Value = "4";
            List.Add(ItemE2);
            SelectList ItemE = new SelectList();
            ItemE.Text = "Disable"; ItemE.Value = "2";
            List.Add(ItemE);
        }

        protected override void Edit()
        {

            IsValid = true;
            Entity = GetFacilityRulesData(Convert.ToInt32(EventArgument));
            DocumentsNeeded = GetAllRequiredDocuments();

            GetDropDownItems();

            base.Edit();
        }

        public bool Insert(Facility entity)
        {
            bool ret = false;
            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    //TODO: Create Insert Code here
                    db.Facilities.Add(entity);
                    db.SaveChanges();
                }
            }
            return ret;
        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
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
                    mail.ErrorLog(ErrorDescription, Entity.facilityName+" Rule ");
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
                    mail.ErrorLog(ErrorDescription, Entity.facilityName + " Rule ");
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
                    mail.ErrorLog(ErrorDescription, Entity.facilityName + " Rule ");
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
                    mail.ErrorLog(ErrorDescription, Entity.facilityName + " Rule ");
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
                    mail.ErrorLog(ErrorDescription, Entity.facilityName + " Rule ");
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
                    mail.ErrorLog(ErrorDescription, Entity.facilityName + " Rule ");
                }
                finally
                {
                    //db.Configuration.Close();
                }

                IsOthers = true;
            }



            return ret;
        }

        public bool Update(Facility entity)
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
                        var rs = (from info in db.Facilities where info.id == entity.id select info).FirstOrDefault();

                        rs.facilityRulesComments = entity.facilityRulesComments;
                        rs.confirmedStaff = entity.confirmedStaff;
                        rs.unconfirmedStaff = entity.unconfirmedStaff;
                        rs.permanentStaff = entity.permanentStaff;
                        rs.contractStaff = entity.contractStaff;
                        rs.expectedLengthOfService = entity.expectedLengthOfService;
                        rs.percentageRate = entity.percentageRate;
                        rs.staffBranchPercentageRate = entity.staffBranchPercentageRate;
                        rs.minimumAnnualGrossAllowance = entity.minimumAnnualGrossAllowance;
                        rs.minimumAnnualHousingAllowance = entity.minimumAnnualHousingAllowance;
                        rs.maximumAmountLimit = entity.maximumAmountLimit;
                        rs.totalDeductionFromSalary = entity.totalDeductionFromSalary;
                        rs.minimumTenor = entity.minimumTenor;
                        rs.maximumTenor = entity.maximumTenor;
                        rs.undertakingFormNeeded = entity.undertakingFormNeeded;
                        rs.supervisorEndorsementNeeded = entity.supervisorEndorsementNeeded;
                        rs.searchAuthorityNeeded = entity.searchAuthorityNeeded;
                        rs.maximumAmountLimitOption = entity.maximumAmountLimitOption;
                        rs.mdApproval = entity.mdApproval;

                        ReqHistory history = new ReqHistory();
                        history.ActionDateTime = DateTime.Now;
                        history.Initiator = UserId;
                        history.ActionPerformed = "Facility: " + entity.facilityName + "'s Rules Updated Successfully with reason:" + rs.facilityRulesComments;
                        history.ReqId = entity.id.ToString();
                        db.ReqHistories.Add(history);

                        Msg = "Facility: " + entity.facilityName + "'s Rules Updated Successfully!";

                        IsValid = true;

                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }catch(Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, UserId);
                }

            }
            return ret;
        }

        public bool Validate(Facility entity)
        {
            ValidationErrors.Clear();
            if (string.IsNullOrEmpty(entity.facilityRulesComments))
            {
                ValidationErrors.Add(new
                  KeyValuePair<string, string>("Comment",
                  "Please Supply your Comment."));
                IsValid = false;
            }

            return (ValidationErrors.Count == 0);
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
        protected List<Facility> CreateData()
        {
            List<Facility> ret = new List<Facility>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.Facilities.Where(x => x.active == "Y").ToList();
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
                      KeyValuePair<string, string>("HRViabilityAdminPortal",
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

        protected Facility GetFacilityRulesData(int id)
        {
            Facility ret = new Facility();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.Facilities.Where(x => x.id == id).SingleOrDefault();

                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("StaffSalaryGrades",
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

        public class SelectList
        {
            public string Text { get; set; }
            public string Value { get; set; }
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