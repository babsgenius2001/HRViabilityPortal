using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRViabilityPortal.ViewModels
{
    public class RemoveDocumentsViewModel : ViewModelBase
    {
        public RemoveDocumentsViewModel()
        {
            Init();

        }
        
        public HRFacilityMaster Entity { get; set; }              
        public Facility SearchEntity2 { get; set; }
        public Facility Entity2 { get; set; }
        public Facility Entity3 { get; set; }
        public Facility FacilityRule { get; set; }
        public List<SelectList> ListOfFacilities { get; set; }
        public List<SelectDocumentsList> DocumentsList { get; set; }       
        public Facility FacilityReq { get; set; }
        public bool IsStep1 { get; set; }       
        public string documentName { get; set; }

        protected override void Init()
        {
            EventCommand = "FacilityList";
           
            FacilityReq = new Facility();
            Entity = new HRFacilityMaster();           
            ListOfFacilities = new List<SelectList>();
            DocumentsList = new List<SelectDocumentsList>();           
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
                    GetDropDown();
                    break;

                default:
                    IsStep1 = true;
                    IsDetailAreaVisible = true;
                    GetDropDown();
                    break;
            }

        }

        protected override void Save()
        {
            if (Mode == "Add")
            {
                Update(FacilityReq);
            }
           
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
                        "Please Select a Facility Name!."));
            }

            if (string.IsNullOrEmpty(documentName))
            {
                ValidationErrors.Add(new
                    KeyValuePair<string, string>("",
                        "Please Select a Document Name!."));
            }


            return (ValidationErrors.Count == 0);
        }


        public bool Update(Facility entity)
        {
            bool ret = false;
            string fac, query = "";
            ret = Validate(entity);
            if (ret)
            {

                fac = entity.facilityName.Replace(" ", "");
                query = "UPDATE " + fac.Trim() + "DocumentsTable SET active = 'N' where documentName = '"+documentName.Trim()+"'";

                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {

                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        try
                        {
                            Connect.Open();

                            Cmd.ExecuteNonQuery();

                            Connect.Close();

                            IsValid = true;
                            ret = true;

                            string op = "Document Removed Successfully for Facility: " + entity.facilityName + " !";
                            Msg = op;
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
            return ret;
        }

        protected void GetDropDown()
        {

            using (var db = new HRViabilityPortalEntities())
            {

                var facilityType = (from f in db.Facilities.Where(x => x.active == "Y") orderby f.facilityName ascending select f.facilityName).Distinct();
                foreach (var bn in facilityType)
                {
                    SelectList item = new SelectList();                   
                    item.Text = "==Select==";
                    item.Value = " ";
                    ListOfFacilities.Add(item);

                    SelectList item2 = new SelectList();                   
                    item2.Text = bn;
                    item2.Value = bn;
                    ListOfFacilities.Add(item2);
                }

                var docType = (from f in db.FacilityDocuments.Where(x => x.active == "Y") orderby f.documentName ascending select f.documentName).Distinct();
                foreach (var bn in docType)
                {
                    SelectDocumentsList item = new SelectDocumentsList();
                    item.Text = "==Select==";
                    item.Value = " ";
                    DocumentsList.Add(item);

                    SelectDocumentsList item2 = new SelectDocumentsList();
                    item2.Text = bn;
                    item2.Value = bn;
                    DocumentsList.Add(item2);
                }
            }
        }

       
        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public class SelectDocumentsList
        {
            public string Text { get; set; }
            public string Value { get; set; }
            //public HttpPostedFileBase uploadedDoc { get; set; }
        }

        public class SelectList
        {
            public string Select { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}