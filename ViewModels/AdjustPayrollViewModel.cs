using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace HRViabilityPortal.ViewModels
{
    public class AdjustPayrollViewModel:ViewModelBase
    {
        public AdjustPayrollViewModel()
        {
            Init();
        }

        public List<HRPayroll> PayrollReqs { get; set; }
        public HRPayroll SearchEntity { get; set; }
        public HRPayroll Entity { get; set; }
        public HRPayroll PayrollReq { get; set; }

        public List<SelectList> List { set; get; }

        protected override void Init()
        {
            PayrollReqs = new List<HRPayroll>();
            PayrollReq = new HRPayroll();
            Entity = new HRPayroll();
            EventCommand = "reviewersearch";
            SearchEntity = new HRPayroll();
            List = new List<SelectList>();

            base.Init();
        }

        protected override void Get()
        {
            PayrollReqs = Get(SearchEntity);

            //GetDropDownItems();

            base.Get();
        }

        public List<HRPayroll> Get(HRPayroll entity)
        {
            List<HRPayroll> ret = new List<HRPayroll>();
           
            ret = CreateData();

            return ret;
        }

        protected List<HRPayroll> CreateData()
        {
            List<HRPayroll> ret = new List<HRPayroll>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.HRPayrolls.ToList();
                    //if (PageNumber > 0 && PageSize > 0)
                    //{
                    //    PagedList = ret.ToPagedList(PageNumber, PageSize);
                    //    ret = ret.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
                    //}
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
            Entity = GetPayrollData(Convert.ToInt32(EventArgument));

            base.Edit();
        }

        protected HRPayroll GetPayrollData(int id)
        {
            HRPayroll ret = new HRPayroll();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.HRPayrolls.Where(x => x.Id == id).SingleOrDefault();

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

        public bool Insert(HRPayroll entity)
        {
            bool ret = false;
           
                using (var db = new HRViabilityPortalEntities())
                {
                    //TODO: Create Insert Code here
                    db.HRPayrolls.Add(entity);
                    db.SaveChanges();
                }
         
            return ret;
        }

        public bool Update(HRPayroll entity)
        {
            bool ret = false;           
            string op = "";

            
                try
                {
                    using (var db = new HRViabilityPortalEntities())
                    {
                        var rs = (from info in db.HRPayrolls where info.Id == entity.Id select info).FirstOrDefault();

                        rs.AnnualGrossPackage = entity.AnnualGrossPackage;
                        rs.AnnualHousingAllowance = entity.AnnualHousingAllowance;
                        rs.MonthlyGrossPackage = entity.MonthlyGrossPackage;
                        rs.MonthlyNetPackage = entity.MonthlyNetPackage;
                        rs.PensionsContribution = entity.PensionsContribution;
                        rs.PAYE = entity.PAYE;
                        rs.NHF = entity.NHF;
                        rs.TotalAnnualDeduction = entity.TotalAnnualDeduction;
                        rs.TotalMonthlyDeduction = entity.TotalMonthlyDeduction;

                        Msg = "Grade: " + entity.GradeName + "'s Payroll Details Updated Successfully!";
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

        public class SelectList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}