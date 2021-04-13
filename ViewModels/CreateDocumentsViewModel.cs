
using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRViabilityPortal.ViewModels
{
    public class CreateDocumentsViewModel : ViewModelBase
    {
        public CreateDocumentsViewModel()
        : base()
        {
            // Initialize other variables
            EventCommand = "FacilityDocumentsList";
            SearchEntity = new FacilityDocument();
            NewDocumentsReq = new FacilityDocument();
            Entity = new FacilityDocument();
            MessageBody = string.Empty;
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public FacilityDocument Entity { get; set; }
        public FacilityDocument SearchEntity { get; set; }
        public string MessageBody { get; set; }
        public FacilityDocument NewDocumentsReq { get; set; }

        public bool IsStep1 { get; set; }
        public bool IsStep2 { get; set; }
        public bool IsStep3 { get; set; }
        public bool IsStep4 { get; set; }
        public bool IsStep5 { get; set; }
        public bool IsStep6 { get; set; }

        protected override void Init()
        {
            EventCommand = "FacilityList";
            SearchEntity = new FacilityDocument();
            NewDocumentsReq = new FacilityDocument();
            Entity = new FacilityDocument();
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
                Insert(NewDocumentsReq);
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

        public bool Validate(FacilityDocument entity)
        {
            ValidationErrors.Clear();
            if (string.IsNullOrEmpty(entity.documentName))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Please Input a Document Name!."));
            }

            return (ValidationErrors.Count == 0);
        }

        public bool ValidateDocumentName(FacilityDocument entity)
        {
            ValidationErrors.Clear();


            if (checkDocumentName(entity.documentName.Trim()))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Document Has Been Created Already!."));

                return true;
            }

            return false;

        }
        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public bool checkDocumentName(string docname)
        {
            bool ret = false;
            string gr = "";
            string query = "select documentName from FacilityDocuments where documentName = '" + docname + "'";
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
                                gr = reader["documentName"].ToString();
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
            }catch(Exception ex) 
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }

            return ret;
        }

        public bool Insert(FacilityDocument entity)
        {
            bool ret = false;
            bool checkdoc = false;

            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    FacilityDocument newreq = new FacilityDocument();

                    try
                    {
                        newreq.documentName = entity.documentName;
                        newreq.isSelected = false;

                        checkdoc = ValidateDocumentName(entity);
                        if (checkdoc == false)
                        {
                            newreq.active = "Y";

                            string op = "New Document: " + newreq.documentName + " Created Successfully!";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "New Document Creation Request Submission";
                            history.ReqId = "";

                            db.ReqHistories.Add(history);
                            db.FacilityDocuments.Add(newreq);

                            db.SaveChanges();
                            Msg = op;
                        }
                        else
                        {
                            Msg = "Submission Exception:  Document Has been created before!";
                        }

                    }
                    catch (Exception ex)
                    {
                        Msg = "Submission Exception:  " + ex.ToString();
                        
                        var mail = new AllLogs();
                        mail.ErrorLog(Msg, UserId);
                    }
                }
            }
            return ret;
        }

    }
}