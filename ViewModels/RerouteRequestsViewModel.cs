using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HRViabilityPortal.Database_References;
using MySql.Data.MySqlClient;

namespace HRViabilityPortal.ViewModels
{
    public class RerouteRequestsViewModel: ViewModelBase
    {
        public RerouteRequestsViewModel()
        {
            Init();
        }

        public HRFacilityMaster Entity { get; set; }
        public HRFacilityMaster SearchEntity { get; set; }
        public HRFacilityMaster facilityRequest { get; set; }
        public string loggedInUser { get; set; }
        public string requestRef { get; set; }
        public bool IsStep1 { get; set; }       
        public bool IsStep2 { get; set; }
        public List<SelectList> Branches { get; set; }
        public string newBranch { get; set; }

        protected override void Init()
        {            
            Entity = new HRFacilityMaster();
            SearchEntity = new HRFacilityMaster();
            facilityRequest = new HRFacilityMaster();
            Branches = new List<SelectList>();

            ValidationErrors = new List<KeyValuePair<string, string>>();

            base.Init();
        }

        public class SelectList
        {
            public string Select { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public override void HandleRequest()
        {
            switch (EventCommand.ToLower())
            {
                case "proceed":
                    if (Validate())
                    {
                        Get();
                        GetBranches();
                        IsStep2 = true;
                        IsStep1 = true;                      
                    }
                    else
                    {
                        IsStep1 = true;
                        IsStep2 = false;                       
                    }

                    break;
                case "submit":                  
                        Save();                    

                    if (ValidationErrors.Count == 0)
                    {
                        IsStep1 = true;
                        IsStep2 = false;
                        GetBranches();

                        SearchEntity.requestReferenceNumber = "";
                    }
                    else
                        IsStep1 = true;
                        IsStep2 = false;
                        GetBranches();

                    break;
                default:
                    IsStep1 = true;
                    GetBranches();
                    break;
            }

            base.HandleRequest();
        }

        protected override void Save()
        {
            Update(Entity);
            // Set any validation errors
            ValidationErrors = ValidationErrors;
            // Set mode based on validation errors
            base.Save();
        }

        public bool Validate(string brn)
        {
            bool ret = true;

            ValidationErrors.Clear();
          
            if (string.IsNullOrEmpty(newBranch))
            {
                ValidationErrors.Add(new
                  KeyValuePair<string, string>("Error",  "Please Select A Branch."));
                IsValid = false;
                ret = false;
            }

            return ret;
        }

        public bool Update(HRFacilityMaster entity)
        {
            bool ret = false;

            ret = Validate(newBranch);

            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    var rs = (from info in db.HRFacilityMasters where info.requestReferenceNumber == entity.requestReferenceNumber select info).FirstOrDefault();

                    rs.branchCode = newBranch;                  

                    Msg = "Request Reference: " + entity.requestReferenceNumber + " Has Been Reassigned to Branch : " + newBranch + " Successfully!";
                    IsValid = true;

                    db.Entry(rs).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);

                IsValid = false;
            }

            return ret;
        }

        protected bool Validate()
        {
            bool ret = true;

            if (string.IsNullOrEmpty(SearchEntity.requestReferenceNumber))
            {

                ValidationErrors.Add(new
                       KeyValuePair<string, string>("Invalid",
                       "Kindly Enter The Request Reference Number!"));

                IsValid = false;
                ret = false;
            }

            return ret;
        }

        protected override void Get()
        {
            Entity = GetRequestDetails(SearchEntity);            

            base.Get();
        }

        protected HRFacilityMaster GetRequestDetails(HRFacilityMaster entity)
        {
            HRFacilityMaster ret = new HRFacilityMaster();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    ret = db.HRFacilityMasters.Where(x => x.requestReferenceNumber == entity.requestReferenceNumber).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRabilityPortal",
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

        protected void GetBranches()
        {
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    var branchList = (from f in db.BMDetails select f).OrderBy(x => x.branchName).ToList();

                    foreach (var bn in branchList)
                    {

                        SelectList item = new SelectList();
                        item.Select = "==Select==";
                        item.Text = bn.branchName;
                        item.Value = bn.branchCode;

                        Branches.Add(item);
                    }                   

                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }
            finally
            {

            }
        }

    }
}