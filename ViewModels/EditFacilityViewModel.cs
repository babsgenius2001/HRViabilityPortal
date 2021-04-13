using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data.Entity;
using HRViabilityPortal.Database_References;

namespace HRViabilityPortal.ViewModels
{
    public class EditFacilityViewModel : ViewModelBase
    {
        public EditFacilityViewModel()
        {
            Init();
        }

        public List<Facility> FacilityReqs { get; set; }
        public Facility SearchEntity { get; set; }
        public Facility Entity { get; set; }
        public Facility FacilityReq { get; set; }

        public List<SelectList> List { set; get; }

        protected override void Init()
        {
            FacilityReqs = new List<Facility>();
            FacilityReq = new Facility();
            Entity = new Facility();
            EventCommand = "reviewersearch";
            SearchEntity = new Facility();
            List = new List<SelectList>();

            base.Init();
        }

        protected override void Get()
        {
            FacilityReqs = Get(SearchEntity);

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

        protected List<Facility> CreateData()
        {
            List<Facility> ret = new List<Facility>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.Facilities.Where(x => x.active == "Y" || x.active == "N").ToList();
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


        public class SelectList
        {
            public string Text { get; set; }
            public string Value { get; set; }
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
            Entity = GetFacilityData(Convert.ToInt32(EventArgument));

            GetDropDownItems();

            base.Edit();
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

                        rs.facilityComments = entity.facilityComments;
                        rs.facilityName = entity.facilityName;

                        if (entity.active == "2")
                        {
                            rs.active = "N";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Facility Disabled with reason:" + rs.facilityComments;
                            history.ReqId = entity.id.ToString();
                            db.ReqHistories.Add(history);
                            Msg = "Facility: " + entity.facilityName + "  Disabled Successfully!";
                            IsValid = true;
                        }
                        else
                        {

                            rs.active = "Y";

                            // op = "Vehicle Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Approved by HR";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Facility Enabled with reason:" + rs.facilityComments;
                            history.ReqId = entity.id.ToString();
                            db.ReqHistories.Add(history);
                            Msg = "Facility: " + entity.facilityName + "  Updated Successfully!";

                            IsValid = true;
                        }
                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }                   
                catch(Exception ex)
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
            if (string.IsNullOrEmpty(entity.facilityComments))
            {
                ValidationErrors.Add(new
                  KeyValuePair<string, string>("Comment",
                  "Please Supply your Comment."));
                IsValid = false;
            }
            if (string.IsNullOrEmpty(entity.active))
            {
                ValidationErrors.Add(new
                  KeyValuePair<string, string>("Action",
                  "Please select action."));
                IsValid = false;
            }
            return (ValidationErrors.Count == 0);
        }

        protected Facility GetFacilityData(int id)
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
    }
}