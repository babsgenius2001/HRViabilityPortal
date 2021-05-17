using HRViabilityPortal.Database_References;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRViabilityPortal.ViewModels
{
    public class AdjustBMDetailsViewModel: ViewModelBase
    {
        public AdjustBMDetailsViewModel()
        {
            Init();
        }

        public List<BMDetail> BMInfos { get; set; }
        public BMDetail SearchEntity { get; set; }
        public BMDetail Entity { get; set; }
        public BMDetail BMInfo { get; set; }
        public string newBMEmail { get; set; }
        public string newBMName { get; set; }
        public string newStaffID { get; set; }

        public List<SelectList> List { set; get; }

        protected override void Init()
        {
            BMInfos = new List<BMDetail>();
            BMInfo = new BMDetail();
            Entity = new BMDetail();
            EventCommand = "reviewersearch";
            SearchEntity = new BMDetail();
            List = new List<SelectList>();

            base.Init();
        }

        protected override void Get()
        {
            BMInfos = Get(SearchEntity);          

            base.Get();
        }

        public List<BMDetail> Get(BMDetail entity)
        {
            List<BMDetail> ret = new List<BMDetail>();

            ret = CreateData();

            return ret;
        }

        protected List<BMDetail> CreateData()
        {
            List<BMDetail> ret = new List<BMDetail>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    ret = db.BMDetails.OrderBy(x=> x.branchName).ToList();                   
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViabilityPortal",
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

        protected override void Edit()
        {

            IsValid = true;
            Entity = GetBMData(EventArgument);

            base.Edit();
        }

        protected BMDetail GetBMData(string staffId)
        {
            BMDetail ret = new BMDetail();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.BMDetails.Where(x => x.staffId == staffId).SingleOrDefault();

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

        public bool Insert(BMDetail entity)
        {
            bool ret = false;

            using (var db = new HRViabilityPortalEntities())
            {
                //TODO: Create Insert Code here
                db.BMDetails.Add(entity);
                db.SaveChanges();
            }

            return ret;
        }

        protected bool Validate()
        {
            bool ret = true;

            if (string.IsNullOrEmpty(Entity.staffId))
            {

                ValidationErrors.Add(new
                       KeyValuePair<string, string>("Invalid",
                       "Kindly Enter The Staff ID of BM to Reassign!"));

                IsValid = false;
                ret = false;
            }

            if(ValidateStaffID(Entity.staffId) == false)
            {
                ValidationErrors.Add(new
                       KeyValuePair<string, string>("Invalid",
                       "Staff ID Doesnt EXist/Invalid Staff ID Provided!"));

                IsValid = false;
                ret = false;
            }
           

            return ret;
        }

        protected bool ValidateStaffID(string staffID)
        {
            bool ret = true;
            string url = "";

            url = ConfigurationManager.AppSettings["url"].ToString();

            var client = new RestClient(url + "/GetUserDetails");
            var request = new RestRequest(Method.POST);
            request.AddHeader("ContentType", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { UserName = staffID.Trim() });

            var s = client.Execute(request);
            var Xxc = JsonConvert.DeserializeObject<dynamic>(s.Content);
            var respon = JsonConvert.SerializeObject(Xxc);
            var json = JsonConvert.DeserializeObject<Rootobject>(respon);

            if (json.Status)
            {
                if (String.IsNullOrEmpty(json.Data.cif_number))
                {
                    ret = false;
                }
                else
                {
                    newStaffID = staffID;
                    newBMEmail = json.Data.email;
                    newBMName = json.Data.fullname;

                    ret = true;
                }
            }

            return ret;
        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public bool Update(BMDetail entity)
        {
            bool ret = false;         
            ret = Validate();

            if (ret)
            {
                var qry = "UPDATE BMDetails "
               + " SET staffId = '"+ newStaffID.Trim() + "', " 
               + " staffName = '" + newBMName.Trim() + "' ,"
               + " emailAddress = '" + newBMEmail.Trim() + "' "              
               + " WHERE branchCode ='" + entity.branchCode.Trim() + "' ";

                try
                {                   

                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {

                        Connect.Open();

                        SqlCommand command;
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        command = new SqlCommand(qry, Connect);

                        adapter.InsertCommand = new SqlCommand(qry, Connect);
                        adapter.InsertCommand.ExecuteNonQuery();
                        command.Dispose();

                        Connect.Close();

                        Msg = "BM: " + newStaffID + "'s Details Updated Successfully!";
                    }
                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, UserId);

                    IsValid = false;
                }
            }
                    
            return ret;
        }

        public class SelectList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class Rootobject
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
            public string cif_number { get; set; }
        }
    }
}