using Newtonsoft.Json;
using PagedList;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using HRViabilityPortal.Database_References;
using HRViabilityPortal.Controllers;

namespace HRViabilityPortal.ViewModels
{
    public class BranchesRequestsViewModel : ViewModelBase
    {
        public BranchesRequestsViewModel()
        {
            Init();
        }

        public List<HRFacilityMaster> FacilityReqs { get; set; }
        public HRFacilityMaster SearchEntity { get; set; }
        public HRFacilityMaster Entity { get; set; }
        public HRFacilityMaster Entity2 { get; set; }
        public HRFacilityMaster FacilityReq { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string branchCode { get; set; }

        protected override void Init()
        {
            FacilityReq = new HRFacilityMaster();
            FacilityReqs = new List<HRFacilityMaster>();           
            SearchEntity = new HRFacilityMaster();
            Entity = new HRFacilityMaster();
            Entity2 = new HRFacilityMaster();
            EventCommand = "reviewersearch";
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        protected override void Get()
        {
            FacilityReqs = Get(SearchEntity);

            base.Get();
        }

        public List<HRFacilityMaster> Get(HRFacilityMaster entity)
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();

            ret = CreateData();

            if (!string.IsNullOrEmpty(Convert.ToString(startDate)) && !string.IsNullOrEmpty(Convert.ToString(endDate)))
            {

                ret = ret.FindAll(p => p.requestDate >= startDate && p.requestDate <= endDate);

            }
            return ret;
        }

        protected List<HRFacilityMaster> CreateData()
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    //ret = db.HRFacilityMasters.Where(x => x.branchCode == branchCode).ToList();
                    ret = db.HRFacilityMasters.Where(x => x.branchCode == "2").ToList();
                    //ret = db.HRFacilityMasters.ToList();

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
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, Entity.staffId);
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
        }
    }
}