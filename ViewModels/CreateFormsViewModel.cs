using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRViabilityPortal.ViewModels
{
    public class CreateFormsViewModel : ViewModelBase
    {
        public CreateFormsViewModel()
      : base()
        {
            // Initialize other variables
            EventCommand = "FacilityFormsList";
            SearchEntity = new FacilityForm();
            NewFormsReq = new FacilityForm();
            Entity = new FacilityForm();
            MessageBody = string.Empty;
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public FacilityForm Entity { get; set; }
        public FacilityForm SearchEntity { get; set; }
        public string MessageBody { get; set; }
        public FacilityForm NewFormsReq { get; set; }

        public bool IsStep1 { get; set; }
        public bool IsStep2 { get; set; }
        public bool IsStep3 { get; set; }
        public bool IsStep4 { get; set; }
        public bool IsStep5 { get; set; }
        public bool IsStep6 { get; set; }

        protected override void Init()
        {
            EventCommand = "FacilityList";
            SearchEntity = new FacilityForm();
            NewFormsReq = new FacilityForm();
            Entity = new FacilityForm();
            MessageBody = string.Empty;
            ValidationErrors = new List<KeyValuePair<string, string>>();
            base.Init();
        }

        public override void HandleRequest()
        {
            //// This is an example of adding on a new command
            switch (EventCommand.ToLower())
            {
                case "submit":
                    Mode = "Add";
                    Save();
                    IsStep1 = true;
                    IsDetailAreaVisible = true;
                    break;

                default:
                    IsStep1 = true;
                    IsDetailAreaVisible = true;
                    break;
            }

        }

        protected override void Save()
        {
            if (Mode == "Add")
            {
                Insert(NewFormsReq);
            }
            //else
            //{
            //    Update(VehicleReq);
            //}
            // Set any validation errors
            ValidationErrors = ValidationErrors;
            // Set mode based on validation errors
            base.Save();
        }

        public bool Validate(FacilityForm entity)
        {
            ValidationErrors.Clear();
            if (string.IsNullOrEmpty(entity.formName))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Please Input a Form Name!."));
            }

            return (ValidationErrors.Count == 0);
        }

        public bool ValidateFormName(FacilityForm entity)
        {
            ValidationErrors.Clear();


            if (checkFormName(entity.formName.Trim()))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Form Has Been Created Already!."));

                return true;
            }

            return false;

        }
        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public bool checkFormName(string formname)
        {
            bool ret = false;
            string gr = "";
            string query = "select formName from FacilityForms where formName = '" + formname + "'";
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
                            gr = reader["formName"].ToString();
                        }

                        Connect.Close();
                    }

                    if (string.IsNullOrEmpty(gr))
                    {
                        ret = false;
                    }
                    else
                    {
                        ret = true;
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

            return ret;
        }

        public bool Insert(FacilityForm entity)
        {
            bool ret = false;
            bool checkdoc = false;

            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    FacilityForm newreq = new FacilityForm();

                    try
                    {
                        newreq.formName = entity.formName;
                        newreq.isSelected = false;

                        checkdoc = ValidateFormName(entity);
                        if (checkdoc == false)
                        {
                            newreq.active = "Y";
                            newreq.isSelected = false;

                            string op = "New Form: " + newreq.formName + " Created Successfully!";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId + "@jaizbankplc.com";
                            history.ActionPerformed = "New Form Creation Request Submission";
                            history.ReqId = "";

                            db.ReqHistories.Add(history);
                            db.FacilityForms.Add(newreq);

                            db.SaveChanges();
                            Msg = op;
                        }
                        else
                        {
                            Msg = "Submission Exception:  Form Has been created before!";
                        }

                    }
                    catch (Exception ex)
                    {
                        Msg = "Submission Exception:  " + ex.ToString();
                    }
                }
            }
            return ret;
        }
    }
}