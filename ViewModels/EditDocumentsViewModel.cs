using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data.Entity;
using HRViabilityPortal.Database_References;

namespace HRViabilityPortal.ViewModels
{
    public class EditDocumentsViewModel : ViewModelBase
    {
        public EditDocumentsViewModel()
        {
            Init();
        }

        public List<FacilityDocument> FacilityDocs { get; set; }
        public FacilityDocument SearchEntity { get; set; }
        public FacilityDocument Entity { get; set; }
        public FacilityDocument FacilityDoc { get; set; }

        public List<SelectList> List { set; get; }

        protected override void Init()
        {
            FacilityDocs = new List<FacilityDocument>();
            FacilityDoc = new FacilityDocument();
            Entity = new FacilityDocument();
            EventCommand = "reviewersearch";
            SearchEntity = new FacilityDocument();
            List = new List<SelectList>();

            base.Init();
        }

        protected override void Get()
        {
            FacilityDocs = Get(SearchEntity);

            GetDropDownItems();

            base.Get();
        }

        public List<FacilityDocument> Get(FacilityDocument entity)
        {
            List<FacilityDocument> ret = new List<FacilityDocument>();
            // TODO: Add your own data access method here
            ret = CreateData();

            return ret;
        }

        protected List<FacilityDocument> CreateData()
        {
            List<FacilityDocument> ret = new List<FacilityDocument>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.FacilityDocuments.Where(x => x.active == "Y" || x.active == "N").ToList();
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
            Entity = GetDocumentsData(Convert.ToInt32(EventArgument));

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

        public bool Insert(FacilityDocument entity)
        {
            bool ret = false;
            ret = Validate(entity);
            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    //TODO: Create Insert Code here
                    db.FacilityDocuments.Add(entity);
                    db.SaveChanges();
                }
            }
            return ret;
        }

        public bool Update(FacilityDocument entity)
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
                        var rs = (from info in db.FacilityDocuments where info.id == entity.id select info).FirstOrDefault();

                        rs.comments = entity.comments;

                        if (entity.active == "2")
                        {


                            rs.active = "N";
                            rs.documentName = entity.documentName;

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Documnent Disabled with reason:" + rs.comments;
                            history.ReqId = entity.id.ToString();
                            db.ReqHistories.Add(history);
                            Msg = "Documnent: " + entity.documentName + "  Disabled!";
                            IsValid = true;
                        }
                        else
                        {

                            rs.active = "Y";
                            rs.documentName = entity.documentName;

                            // op = "Vehicle Facility Request with Reference No :" + entity.requestReferenceNumber + "  Reviewed and Approved by HR";

                            ReqHistory history = new ReqHistory();
                            history.ActionDateTime = DateTime.Now;
                            history.Initiator = UserId;
                            history.ActionPerformed = "Documnent Enabled with reason:" + rs.comments;
                            history.ReqId = entity.id.ToString();
                            db.ReqHistories.Add(history);
                            Msg = "Documnent: " + entity.documentName + "  Enabled!";

                            IsValid = true;
                        }
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

        public bool Validate(FacilityDocument entity)
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

        protected FacilityDocument GetDocumentsData(int id)
        {
            FacilityDocument ret = new FacilityDocument();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.FacilityDocuments.Where(x => x.id == id).SingleOrDefault();

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