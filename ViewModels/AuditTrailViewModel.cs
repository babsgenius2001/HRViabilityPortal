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
    public class AuditTrailViewModel : ViewModelBase
    {
        public AuditTrailViewModel()
        {
            Init();
        }

        public List<AuditLog> AuditLogs { get; set; }
        public AuditLog SearchEntity { get; set; }
        public AuditLog Entity { get; set; }
        public AuditLog Entity2 { get; set; }
        public AuditLog AuditLog { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        protected override void Init()
        {
            AuditLog = new AuditLog();
            AuditLogs = new List<AuditLog>();
            SearchEntity = new AuditLog();
            Entity = new AuditLog();
            Entity2 = new AuditLog();
            EventCommand = "auditsearch";
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        protected override void Get()
        {
            AuditLogs = Get(SearchEntity);

            base.Get();
        }

        public List<AuditLog> Get(AuditLog entity)
        {
            List<AuditLog> ret = new List<AuditLog>();

            ret = CreateData(entity);

            if (!string.IsNullOrEmpty(Convert.ToString(startDate)) && !string.IsNullOrEmpty(Convert.ToString(endDate)))
            {

                ret = ret.FindAll(p => p.DateOfActivity >= startDate && p.DateOfActivity <= endDate);

            }

            return ret;
        }

        protected List<AuditLog> CreateData(AuditLog entity)
        {
            List<AuditLog> ret = new List<AuditLog>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.AuditLogs.ToList();


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
                mail.ErrorLog(ErrorDescription, entity.UserId);
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
        }

    }
}