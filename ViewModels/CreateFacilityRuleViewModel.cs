using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace HRViabilityPortal.ViewModels
{
    public class CreateFacilityRuleViewModel : ViewModelBase
    {
        public CreateFacilityRuleViewModel()
         : base()
        {
            // Initialize other variables
            EventCommand = "getallgrades";
            ListOfFacilities = new List<SelectList>();
            Grades = new List<SelectListItem>();
            Dict = new List<Facility>();
            DocumentsList = new List<SelectDocumentsList>();
            FormsList = new List<SelectFormsList>();
            SearchEntity = new Facility();
           
            NewFacilityRule = new Facility();
            Entity = new Facility();
            MessageBody = string.Empty;
           
            SalaryGrade = new List<GradesDAO>();
            MyDocuments = new List<DocumentsDAO>();
            MyForms = new List<FormsDAO>();

            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public Facility Entity { get; set; }
        public List<GradesDAO> SalaryGrade { get; set; }
        public List<DocumentsDAO> MyDocuments { get; set; }
        public List<FormsDAO> MyForms { get; set; }
        public List<SelectListItem> Grades { get; set; }
        public List<SelectDocumentsList> DocumentsList { get; set; }
        public List<SelectFormsList> FormsList { get; set; }
        public List<SelectList> ListOfFacilities { get; set; }
        public List<Facility> Dict { set; get; }
        public Facility SearchEntity { get; set; }
        public string MessageBody { get; set; }
        public Facility NewFacilityRule { get; set; }

        public bool IsStep1 { get; set; }
        public bool IsStep2 { get; set; }
        public bool IsStep21 { get; set; }
        public bool IsStep22 { get; set; }
        public bool IsStep3 { get; set; }
        public bool IsStep4 { get; set; }
        public bool IsStep5 { get; set; }
        public bool IsStep6 { get; set; }
        public bool IsChecked { get; set; }
        public bool IsCheckAll { get; set; }
        public bool authorityNeeded { get; set; }
        public bool confirmedStaff { get; set; }
        public bool unconfirmedStaff { get; set; }
        public bool permanentStaff { get; set; }
        public bool contractStaff { get; set; }
        public bool undertakingNeeded { get; set; }
        public bool supervisorEndorsement { get; set; }
        public bool executiveTrainee { get; set; }
        public bool assistantBankingOfficer { get; set; }
        public bool bankingOfficer { get; set; }
        public bool seniorBankingOfficer { get; set; }
        public bool assistantManager { get; set; }
        public bool deputyManager { get; set; }
        public bool manager { get; set; }
        public bool seniorManager { get; set; }
        public bool principalManager { get; set; }
        public bool assistantGeneralManager { get; set; }
        public bool deputyGeneralManager { get; set; }
        public bool generalManager { get; set; }
        public bool executiveDirector { get; set; }
        public bool deputyManagingDirector { get; set; }
        public bool managingDirector { get; set; }
        public string document1 { get; set; }
        public string document2 { get; set; }
        public string document3 { get; set; }
        public string document4 { get; set; }
        public string document5 { get; set; }
        public string document6 { get; set; }
        public string form1 { get; set; }
        public string form2 { get; set; }
        public string form3 { get; set; }
        public string form4 { get; set; }
        public string form5 { get; set; }
        public string form6 { get; set; }
        public List<String> Document { set; get; }
        public List<String> Form { set; get; }
        public string activeStatus { get; set; }


        protected override void Init()
        {
            EventCommand = "FacilityRuleList";
            SearchEntity = new Facility();
            NewFacilityRule = new Facility();
            ListOfFacilities = new List<SelectList>();
            Entity = new Facility();
            MessageBody = string.Empty;
            ValidationErrors = new List<KeyValuePair<string, string>>();
            Grades = new List<SelectListItem>();
            DocumentsList = new List<SelectDocumentsList>();
            FormsList = new List<SelectFormsList>();
           
            SalaryGrade = new List<GradesDAO>();

            MyDocuments = new List<DocumentsDAO>();
            MyForms = new List<FormsDAO>();

            base.Init();
        }

        public override void HandleRequest()
        {

            switch (EventCommand.ToLower())
            {
                case "step2":
                    if (validateStep1Items())

                        IsStep2 = true;

                    else
                        IsStep1 = true;
                    IsDetailAreaVisible = true;
                    GetDropDown();

                    break;
                case "step3":
                    if (validateStep2Items())

                        IsStep3 = true;

                    else
                        IsStep2 = true;

                    IsDetailAreaVisible = true;
                    GetGrades();

                    break;

                case "step4":
                    if (validateDocuments())

                        IsStep4 = true;
                    else
                        IsStep3 = true;

                    IsDetailAreaVisible = true;
                    GetGrades();
                    break;

                case "submit":
                    Mode = "Add";
                    validateForms();
                    Save();
                    IsStep1 = true;
                    IsDetailAreaVisible = true;
                    GetDropDown();
                    GetGrades();
                    NewFacilityRule.expectedLengthOfService = 0;
                    break;
                default:
                    IsStep1 = true;
                    IsDetailAreaVisible = true;
                    GetDropDown();
                    GetGrades();

                    break;
            }

            base.HandleRequest();
        }

        protected override void Save()
        {
            if (Mode == "Add")
            {
                Update(NewFacilityRule);
            }

            ValidationErrors = ValidationErrors;

            base.Save();
        }

        protected override void GetGrades()
        {
            SalaryGrade = GetAllGrades(activeStatus);
            MyDocuments = GetAllDocuments(activeStatus);
            MyForms = GetAllForms(activeStatus);

            base.GetGrades();
        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public List<GradesDAO> GetAllGrades(string activeStatus)
        {
            List<GradesDAO> ret = new List<GradesDAO>();
            GradesDAO mert = null;
            string query = "";
            int count = 0;

            query = "select id, gradeName from StaffGradesSalary where active = 'Y' order by gradeName asc";

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

                                mert.id = reader.GetInt32(reader.GetOrdinal("id"));
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
                mail.ErrorLog(ErrorDescription, UserId);
            }
            finally
            {
                //db.Configuration.Close();
            }

            return ret;
        }
        public List<DocumentsDAO> GetAllDocuments(string activeStatus)
        {
            List<DocumentsDAO> ret = new List<DocumentsDAO>();
            DocumentsDAO mert = null;
            string query = "";
            int count = 0;

            query = "select id, documentName from FacilityDocuments where active = 'Y' order by documentName asc";

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

                                mert.id = reader.GetInt32(reader.GetOrdinal("id"));
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
                mail.ErrorLog(ErrorDescription, UserId);
            }
            finally
            {
                //db.Configuration.Close();
            }

            return ret;
        }
        public List<FormsDAO> GetAllForms(string activeStatus)
        {
            List<FormsDAO> ret = new List<FormsDAO>();
            FormsDAO mert = null;
            string query = "";
            int count = 0;

            query = "select id, formName from FacilityForms where active = 'Y' order by formName asc";

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
                                mert = new FormsDAO();
                                count++;

                                mert.id = reader.GetInt32(reader.GetOrdinal("id"));
                                mert.formName = reader.GetString(reader.GetOrdinal("formName"));
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
                mail.ErrorLog(ErrorDescription, UserId);
            }
            finally
            {
                //db.Configuration.Close();
            }

            return ret;
        }
       
        public bool validateStep1Items()
        {
            bool ret = false;

            if (string.IsNullOrEmpty(NewFacilityRule.facilityName))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Input a Facility Name!."));

                IsValid = false;
                ret = false;
            }

            if (string.IsNullOrEmpty(NewFacilityRule.confirmedStaff))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Select An Option for Confirmed Staff!."));

                IsValid = false;
                ret = false;
            }

            if (string.IsNullOrEmpty(NewFacilityRule.unconfirmedStaff))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Select An Option for UnConfirmed Staff!."));

                IsValid = false;
                ret = false;
            }

            if (string.IsNullOrEmpty(NewFacilityRule.permanentStaff))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Select An Option for Permanent Staff!."));

                IsValid = false;
                ret = false;
            }

            if (string.IsNullOrEmpty(NewFacilityRule.contractStaff))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Select An Option for Contract Staff!."));

                IsValid = false;
                ret = false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.expectedLengthOfService)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Enter The Expected Length Of Service!"));

                IsValid = false;
                ret = false;
            }

            if (SalaryGrade.Count(x => x.isSelected) == 0)
            {
                ValidationErrors.Add(new KeyValuePair<string, string>("", "Please Select At least One Grade Option!"));
                IsValid = false;
                ret = false;
            }
            else
            {
                int k = 0;
                int i = 0;

                if (NewFacilityRule.facilityName == "Murabaha")
                {

                    String query = "INSERT INTO MurabahaGradesTable (gradeName, active) VALUES (@gradeName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < SalaryGrade.Count)
                                {
                                    if (SalaryGrade[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@gradeName", SalaryGrade[k].gradeName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;

                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;
                                
                                var mail = new AllLogs();
                                mail.ErrorLog(Msg, UserId);
                            }

                        }
                    }

                }
                else if (NewFacilityRule.facilityName == "Home Finance")
                {

                    String query = "INSERT INTO HomeFinanceGradesTable (gradeName, active) VALUES (@gradeName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < SalaryGrade.Count)
                                {
                                    if (SalaryGrade[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@gradeName", SalaryGrade[k].gradeName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;

                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                var mail = new AllLogs();
                                mail.ErrorLog(Msg, UserId);
                            }

                        }
                    }

                }
                else if (NewFacilityRule.facilityName == "Ijara Service")
                {

                    String query = "INSERT INTO IjaraServiceGradesTable (gradeName, active) VALUES (@gradeName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < SalaryGrade.Count)
                                {
                                    if (SalaryGrade[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@gradeName", SalaryGrade[k].gradeName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;

                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                var mail = new AllLogs();
                                mail.ErrorLog(Msg, UserId);
                            }

                        }
                    }

                }
                else if (NewFacilityRule.facilityName == "Bai Muajjal")
                {

                    String query = "INSERT INTO BaiMuajjalGradesTable (gradeName, active) VALUES (@gradeName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < SalaryGrade.Count)
                                {
                                    if (SalaryGrade[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@gradeName", SalaryGrade[k].gradeName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();

                                    }


                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                var mail = new AllLogs();
                                mail.ErrorLog(Msg, UserId);
                            }

                        }
                    }

                }
                else if (NewFacilityRule.facilityName.Contains("Branch") || NewFacilityRule.facilityName.Contains("branch"))
                {

                    String query = "INSERT INTO BranchGradesTable (gradeName, active) VALUES (@gradeName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {
                                while (k < SalaryGrade.Count)
                                {
                                    if (SalaryGrade[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@gradeName", SalaryGrade[k].gradeName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();

                                    }


                                    k++;
                                }

                                IsValid = true;
                                IsBranch = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                var mail = new AllLogs();
                                mail.ErrorLog(Msg, UserId);
                            }

                        }
                    }

                }

                else
                {
                    String facility = NewFacilityRule.facilityName.Replace(" ", "");
                    string query1 = "CREATE TABLE [dbo].[" + facility + "GradesTable]([id][int] IDENTITY(1, 1) NOT NULL, "
                       + "[gradeName] [varchar] (max) NULL, "
                       + "[active] [varchar] (1) NULL, "
                       + "PRIMARY KEY CLUSTERED([id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] "
                       + " ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query1, Connect))
                        {
                            try
                            {
                                Connect.Open();

                                Cmd.ExecuteNonQuery();

                                Connect.Close();

                            }
                            catch (Exception ex)
                            {
                                Msg = "Table Creation Exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                var mail = new AllLogs();
                                mail.ErrorLog(Msg, UserId);
                            }

                        }
                    }

                    String query = "INSERT INTO " + facility.Trim() + "GradesTable (gradeName, active) VALUES (@gradeName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {
                                while (k < SalaryGrade.Count)
                                {
                                    if (SalaryGrade[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@gradeName", SalaryGrade[k].gradeName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();

                                    }


                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                var mail = new AllLogs();
                                mail.ErrorLog(Msg, UserId);
                            }

                        }
                    }
                }

            }

            return ret;
        }

        public bool validateStep2Items()
        {
            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.percentageRate)))
            {
                if (!(NewFacilityRule.facilityName.Contains("Branch") || NewFacilityRule.facilityName.Contains("branch")))
                {
                    ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Input a Percentage Rate!."));

                    IsValid = false;
                    return false;
                }
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.staffBranchPercentageRate)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Input a Staff Branch Percentage Rate!."));

                IsValid = false;
                return false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.maximumAmountLimit)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Input The Maximum Amount Limit!."));

                IsValid = false;
                return false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.totalDeductionFromSalary)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Select A Total Deduction Option!."));

                IsValid = false;
                return false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.minimumTenor)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Input The Minimum Tenor!."));

                IsValid = false;
                return false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.maximumTenor)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Input The Maximum Tenor!."));

                IsValid = false;
                return false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.undertakingFormNeeded)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Select Option For Undertaking!."));

                IsValid = false;
                return false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(NewFacilityRule.supervisorEndorsementNeeded)))
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Please Select Option For Supervisor Endorsement!."));

                IsValid = false;
                return false;
            }

            IsValid = true;
            return true;
        }
        public bool validateDocuments()
        {
            bool ret = false;

            if (MyDocuments.Count(x => x.isSelected) == 0)
            {
                ValidationErrors.Add(new KeyValuePair<string, string>("", "Please Select At least One Document Option!"));
                IsValid = false;
                ret = false;
            }
            if (MyDocuments.Count(x => x.isSelected) > 10)
            {
                ValidationErrors.Add(new KeyValuePair<string, string>("", "Please Select Maximum Of 10 Options!"));
                IsValid = false;
                ret = false;
            }
            else
            {
                int k = 0;

                if (NewFacilityRule.facilityName == "Murabaha")
                {

                    String query = "INSERT INTO MurabahaDocumentsTable (documentName, active) VALUES (@documentName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyDocuments.Count)
                                {
                                    if (MyDocuments[k].isSelected == true)
                                    {

                                        Cmd.Parameters.AddWithValue("@documentName", MyDocuments[k].documentName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName == "Home Finance")
                {

                    String query = "INSERT INTO HomeFinanceDocumentsTable (documentName, active) VALUES (@documentName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyDocuments.Count)
                                {
                                    if (MyDocuments[k].isSelected == true)
                                    {

                                        Cmd.Parameters.AddWithValue("@documentName", MyDocuments[k].documentName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName == "Ijara Service")
                {

                    String query = "INSERT INTO IjaraServiceDocumentsTable (documentName, active) VALUES (@documentName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyDocuments.Count)
                                {
                                    if (MyDocuments[k].isSelected == true)
                                    {

                                        Cmd.Parameters.AddWithValue("@documentName", MyDocuments[k].documentName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName == "Bai Muajjal")
                {

                    String query = "INSERT INTO BaiMuajjalDocumentsTable (documentName, active) VALUES (@documentName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyDocuments.Count)
                                {
                                    if (MyDocuments[k].isSelected == true)
                                    {

                                        Cmd.Parameters.AddWithValue("@documentName", MyDocuments[k].documentName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName.Contains("Branch") || NewFacilityRule.facilityName.Contains("branch"))
                {

                    String query = "INSERT INTO BranchDocumentsTable (documentName, active) VALUES (@documentName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {
                                while (k < MyDocuments.Count)
                                {
                                    if (MyDocuments[k].isSelected == true)
                                    {

                                        Cmd.Parameters.AddWithValue("@documentName", MyDocuments[k].documentName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();

                                    }


                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else
                {
                    String facility = NewFacilityRule.facilityName.Replace(" ", "");
                    string query1 = "CREATE TABLE [dbo].[" + facility + "DocumentsTable]([id][int] IDENTITY(1, 1) NOT NULL, "
                       + "[documentName] [varchar] (max) NULL, "
                       + "[active] [varchar] (1) NULL, "
                       + "PRIMARY KEY CLUSTERED([id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] "
                       + " ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query1, Connect))
                        {
                            try
                            {
                                Connect.Open();

                                Cmd.ExecuteNonQuery();

                                Connect.Close();

                            }
                            catch (Exception ex)
                            {
                                Msg = "creation exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                    String query = "INSERT INTO " + facility.Trim() + "DocumentsTable (documentName, active) VALUES (@documentName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {
                                while (k < MyDocuments.Count)
                                {
                                    if (MyDocuments[k].isSelected == true)
                                    {

                                        Cmd.Parameters.AddWithValue("@documentName", MyDocuments[k].documentName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();

                                    }


                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }
                }
            }

            return ret;
        }

        public bool validateForms()
        {
            bool ret = false;

            if (MyForms.Count(x => x.isSelected) == 0)
            {
                ValidationErrors.Add(new KeyValuePair<string, string>("", "Please Select At least One Form Option!"));
                IsValid = false;
                ret = false;
            }
            if (MyForms.Count > 6)
            {
                ValidationErrors.Add(new KeyValuePair<string, string>("", "Please Select Maximum Of 6 Options!"));
                IsValid = false;
                ret = false;
            }
            else
            {
                int k = 0;

                if (NewFacilityRule.facilityName == "Murabaha")
                {

                    String query = "INSERT INTO MurabahaFormsTable (formName, active) VALUES (@formName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyForms.Count)
                                {
                                    if (MyForms[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@formName", MyForms[k].formName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();
                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName == "Home Finance")
                {

                    String query = "INSERT INTO HomeFinanceFormsTable (formName, active) VALUES (@formName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyForms.Count)
                                {
                                    if (MyForms[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@formName", MyForms[k].formName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();
                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName == "Ijara Service")
                {

                    String query = "INSERT INTO IjaraServiceFormsTable (formName, active) VALUES (@formName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyForms.Count)
                                {
                                    if (MyForms[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@formName", MyForms[k].formName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();
                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName == "Bai Muajjal")
                {

                    String query = "INSERT INTO BaiMuajjalFormsTable (formName, active) VALUES (@formName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {

                                while (k < MyForms.Count)
                                {
                                    if (MyForms[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@formName", MyForms[k].formName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();
                                        Connect.Close();

                                        Cmd.Parameters.Clear();
                                    }

                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else if (NewFacilityRule.facilityName.Contains("Branch") || NewFacilityRule.facilityName.Contains("branch"))
                {

                    String query = "INSERT INTO BranchFormsTable (formName, active) VALUES (@formName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {
                                while (k < MyForms.Count)
                                {
                                    if (MyForms[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@formName", MyForms[k].formName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();

                                    }


                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                }

                else
                {
                    String facility = NewFacilityRule.facilityName.Replace(" ", "");
                    string query1 = "CREATE TABLE [dbo].[" + facility + "FormsTable]([id][int] IDENTITY(1, 1) NOT NULL, "
                       + "[formName] [varchar] (max) NULL, "
                       + "[active] [varchar] (1) NULL, "
                       + "PRIMARY KEY CLUSTERED([id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] "
                       + " ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query1, Connect))
                        {
                            try
                            {
                                Connect.Open();

                                Cmd.ExecuteNonQuery();

                                Connect.Close();

                            }
                            catch (Exception ex)
                            {
                                Msg = "creation exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }

                    String query = "INSERT INTO " + facility.Trim() + "FormsTable (formName, active) VALUES (@formName, @active)";

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            try
                            {
                                while (k < MyForms.Count)
                                {
                                    if (MyForms[k].isSelected == true)
                                    {
                                        Cmd.Parameters.AddWithValue("@formName", MyForms[k].formName);
                                        Cmd.Parameters.AddWithValue("@active", "Y");

                                        Connect.Open();

                                        Cmd.ExecuteNonQuery();

                                        Connect.Close();

                                        Cmd.Parameters.Clear();

                                    }


                                    k++;
                                }

                                IsValid = true;
                                ret = true;
                            }
                            catch (Exception ex)
                            {
                                Msg = "submission exception:  " + ex.ToString();
                                IsValid = false;
                                ret = false;

                                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                                var mail = new AllLogs();
                                mail.ErrorLog(ErrorDescription, UserId);
                            }

                        }
                    }
                }



            }

            return ret;
        }
        public bool Validate(Facility entity)
        {
            ValidationErrors.Clear();
            if (string.IsNullOrEmpty(entity.facilityName))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Please Input a Facility Name!."));
            }

            return (ValidationErrors.Count == 0);
        }

        public bool Update(Facility entity)
        {
            bool ret = false;
            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from info in db.Facilities where info.facilityName.Trim() == entity.facilityName.Trim() select info).FirstOrDefault();

                    try
                    {
                        rs.facilityName = entity.facilityName;
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
                        rs.facilityRulesSet = "Y";
                        rs.maximumAmountLimitOption = entity.maximumAmountLimitOption;
                        rs.mdApproval = entity.mdApproval;
                        rs.dhApproval = entity.dhApproval;
                        rs.downPaymentRequired = entity.downPaymentRequired;


                        string op = "New Facility Rule Created Successfully for: " + entity.facilityName + " !";

                        //HttpRequest req = base.Request;
                        ReqHistory history = new ReqHistory();
                        history.ActionDateTime = DateTime.Now;
                        history.Initiator = UserId;
                        history.ActionPerformed = op;
                        history.ReqId = "";
                        db.ReqHistories.Add(history);

                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();
                        Msg = op;
                    }
                    catch (Exception ex)
                    {
                        Msg = "Submission Exception:  " + ex.ToString();

                        string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                        var mail = new AllLogs();
                        mail.ErrorLog(ErrorDescription, UserId);
                    }
                }
            }
            return ret;
        }

        public class SelectList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class SelectListItem
        {

            public int? Value { get; set; }
            public string gradeName { get; set; }
            public bool Selected { get; set; }
        }

        public class SelectDocumentsList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class SelectGradeNames
        {
            public string value { get; set; }
        }

        public class SelectFormsList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }


        protected void GetDropDown()
        {

            using (var db = new HRViabilityPortalEntities())
            {
                //ret = db.Facilities.Where(x => x.active == "Y").ToList();
                var facilityType = (from f in db.Facilities.Where(x => x.active == "Y" && x.facilityRulesSet == "N") orderby f.facilityName ascending select f.facilityName).Distinct();
                foreach (var bn in facilityType)
                {
                    SelectList item = new SelectList();
                    item.Text = bn;
                    item.Value = bn;
                    ListOfFacilities.Add(item);
                }

                var docType = (from f in db.FacilityDocuments.Where(x => x.active == "Y") orderby f.documentName ascending select f.documentName).Distinct();
                foreach (var bn in docType)
                {
                    SelectDocumentsList item = new SelectDocumentsList();
                    item.Text = bn;
                    item.Value = bn;
                    DocumentsList.Add(item);
                }

                var formType = (from f in db.FacilityForms.Where(x => x.active == "Y") orderby f.formName ascending select f.formName).Distinct();
                foreach (var bn in formType)
                {
                    SelectFormsList item = new SelectFormsList();
                    item.Text = bn;
                    item.Value = bn;
                    FormsList.Add(item);
                }

                //var gradeType = (from f in db.StaffGradesSalaries.Where(x => x.active == "Y") orderby f.gradeName ascending select f.gradeName).Distinct();
                //foreach(var bn in gradeType)
                //{
                //    SelectGradeNames item = new SelectGradeNames();
                //    item.value = bn;
                //    SalaryGrades.Add(item);
                //}
            }
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

        public class FormsDAO
        {
            public int id { get; set; }
            public string formName { get; set; }
            public string space { get; set; }
            public bool isSelected { get; set; }

        }
    }
}