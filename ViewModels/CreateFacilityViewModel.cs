using HRViabilityPortal.Controllers;
using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRViabilityPortal.ViewModels
{
    public class CreateFacilityViewModel : ViewModelBase
    {
        public CreateFacilityViewModel()
         : base()
        {
            // Initialize other variables
            EventCommand = "FacilityList";
            SearchEntity = new Facility();
            NewFacilityReq = new Facility();
            Entity = new Facility();
            MessageBody = string.Empty;
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public Facility Entity { get; set; }
        public Facility SearchEntity { get; set; }
        public string MessageBody { get; set; }
        public Facility NewFacilityReq { get; set; }

        public bool IsStep1 { get; set; }
        public bool IsStep2 { get; set; }
        public bool IsStep3 { get; set; }
        public bool IsStep4 { get; set; }
        public bool IsStep5 { get; set; }
        public bool IsStep6 { get; set; }

        protected override void Init()
        {
            EventCommand = "FacilityList";
            SearchEntity = new Facility();
            NewFacilityReq = new Facility();
            Entity = new Facility();
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

        //     base.HandleRequest();

        protected override void Save()
        {
            if (Mode == "Add")
            {
                Insert(NewFacilityReq);
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

        public bool Validate(Facility entity)
        {
            ValidationErrors.Clear();

            if (string.IsNullOrEmpty(entity.facilityName))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Please Input a Facility Name!."));
            }

            if (entity.facilityName != "Murabaha" && entity.facilityName != "Ijara Service" && entity.facilityName != "Home Finance" && entity.facilityName != "Bai Muajjal" && !entity.facilityName.Contains("Branch") && !entity.facilityName.Contains("branch"))
            {
                if (string.IsNullOrEmpty(entity.customField1) && string.IsNullOrEmpty(entity.customField2) && string.IsNullOrEmpty(entity.customField3) && string.IsNullOrEmpty(entity.customField4) && string.IsNullOrEmpty(entity.customField5) && string.IsNullOrEmpty(entity.customField6) && string.IsNullOrEmpty(entity.customField7) && string.IsNullOrEmpty(entity.customField8) && string.IsNullOrEmpty(entity.customField9) && string.IsNullOrEmpty(entity.customField10))
                {
                    ValidationErrors.Add(new
                        KeyValuePair<string, string>("",
                            "Please Select at least one Field Name!."));
                }
            }

            return (ValidationErrors.Count == 0);
        }

        public bool Insert(Facility entity)
        {
            bool ret = false;
            bool checkFacility = false;
            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    Facility newreq = new Facility();

                    try
                    {
                        checkFacility = ValidateFacilityName(entity);

                        if (checkFacility == false)
                        {

                            if (entity.facilityName != "Murabaha" && entity.facilityName != "Ijara Service" && entity.facilityName != "Home Finance" && entity.facilityName != "Bai Muajjal" && !entity.facilityName.Contains("Branch") && !entity.facilityName.Contains("branch"))
                            {

                                newreq.facilityName = entity.facilityName;

                                newreq.customField1 = entity.customField1;
                                newreq.customField2 = entity.customField2;
                                newreq.customField3 = entity.customField3;
                                newreq.customField4 = entity.customField4;
                                newreq.customField5 = entity.customField5;
                                newreq.customField6 = entity.customField6;
                                newreq.customField7 = entity.customField7;
                                newreq.customField8 = entity.customField8;
                                newreq.customField9 = entity.customField9;
                                newreq.customField10 = entity.customField10;

                                newreq.active = "Y";
                                newreq.facilityRulesSet = "N";

                                string op = "New Facility: " + newreq.facilityName + " Created Successfully!";

                                //HttpRequest req = base.Request;
                                ReqHistory history = new ReqHistory();
                                history.ActionDateTime = DateTime.Now;
                                history.Initiator = UserId;
                                history.ActionPerformed = "New Facility Request Submission";
                                history.ReqId = "";

                                db.ReqHistories.Add(history);
                                db.Facilities.Add(newreq);

                                db.SaveChanges();
                                Msg = op;                                
                            }
                            else
                            {
                                newreq.facilityName = entity.facilityName;
                                newreq.active = "Y";
                                newreq.facilityRulesSet = "N";

                                string op = "New Facility: " + newreq.facilityName + " Created Successfully!";

                                //HttpRequest req = base.Request;
                                ReqHistory history = new ReqHistory();
                                history.ActionDateTime = DateTime.Now;
                                history.Initiator = UserId;
                                history.ActionPerformed = "New Facility Request Submission";
                                history.ReqId = "";

                                db.ReqHistories.Add(history);
                                db.Facilities.Add(newreq);

                                db.SaveChanges();
                                Msg = op;
                                
                            }
                        }
                        else
                        {
                            Msg = "Submission Exception:  Facility Has been created before!";
                        }

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

        public bool ValidateFacilityName(Facility entity)
        {
            ValidationErrors.Clear();


            if (checkFacilityName(entity.facilityName.Trim()))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Facility Has Been Created Already!."));

                return true;
            }

            return false;
            // return (ValidationErrors.Count == 0);
        }
        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public bool checkFacilityName(string facilityname)
        {
            bool ret = false;
            string gr = "";
            string query = "select facilityName from Facility where facilityName = '" + facilityname + "'";
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
                                gr = reader["facilityName"].ToString();
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
    }
}