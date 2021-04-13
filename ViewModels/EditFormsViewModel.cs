using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data.Entity;

namespace HRViabilityPortal.ViewModels
{
    public class EditFormsViewModel : ViewModelBase
    {
        public EditFormsViewModel()
        {
            Init();
        }

        public List<FacilityForm> FacilityForms { get; set; }
        public FacilityForm SearchEntity { get; set; }
        public FacilityForm Entity { get; set; }
        public FacilityForm FacilityForm { get; set; }

        public List<SelectList> List { set; get; }

        protected override void Init()
        {
            FacilityForms = new List<FacilityForm>();
            FacilityForm = new FacilityForm();
            Entity = new FacilityForm();
            EventCommand = "reviewersearch";
            SearchEntity = new FacilityForm();
            List = new List<SelectList>();

            base.Init();
        }

        protected override void Get()
        {
            FacilityForms = Get(SearchEntity);

            GetDropDownItems();

            base.Get();
        }

        public List<FacilityForm> Get(FacilityForm entity)
        {
            List<FacilityForm> ret = new List<FacilityForm>();
            // TODO: Add your own data access method here
            ret = CreateData();

            return ret;
        }

        protected List<FacilityForm> CreateData()
        {
            List<FacilityForm> ret = new List<FacilityForm>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.FacilityForms.Where(x => x.active == "Y" || x.active == "N").ToList();
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

        protected override void Edit()
        {

            IsValid = true;
            Entity = GetFormsData(Convert.ToInt32(EventArgument));

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

        public bool Insert(FacilityForm entity)
        {
            bool ret = false;
            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    //TODO: Create Insert Code here
                    db.FacilityForms.Add(entity);
                    db.SaveChanges();
                }
            }
            return ret;
        }

        public bool Update(FacilityForm entity)
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
                        var rs = (from info in db.FacilityForms where info.id == entity.id select info).FirstOrDefault();

                        rs.comments = entity.comments;

                        if (entity.active == "2")
                        {

                            rs.active = "N";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Form Disabled with reason:" + rs.comments;
                            history.ReqId = entity.id.ToString();
                            db.ReqHistories.Add(history);
                            Msg = "Form: " + entity.formName + "  Disabled!";
                            IsValid = true;
                        }
                        else
                        {

                            rs.active = "Y";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Form Enabled with reason:" + rs.comments;
                            history.ReqId = entity.id.ToString();
                            db.ReqHistories.Add(history);
                            Msg = "Form: " + entity.formName + "  Enabled!";

                            IsValid = true;
                        }
                        db.Entry(rs).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, UserId);
                }

            }
            return ret;
        }

        public bool Validate(FacilityForm entity)
        {
            ValidationErrors.Clear();
            if (string.IsNullOrEmpty(entity.comments))
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

        protected FacilityForm GetFormsData(int id)
        {
            FacilityForm ret = new FacilityForm();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.FacilityForms.Where(x => x.id == id).SingleOrDefault();

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
    }
}
