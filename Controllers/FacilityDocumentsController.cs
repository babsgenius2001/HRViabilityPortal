//using CrystalDecisions.CrystalReports.Engine;
using HRViabilityPortal.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRViabilityPortal.Controllers
{
    public class FacilityDocumentsController : Controller
    {
        // GET: FacilityDocuments

        public static Logger _jobLogger = LogManager.GetCurrentClassLogger();
        private static SqlCommand command = null;
        private static SqlConnection conn = null;
        private static SqlTransaction tran = null;

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public ActionResult GenerateDocuments(int? page)

        {
            FacilityDocumentsViewModel vm = new FacilityDocumentsViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                //String username = HttpContext.Session["name"].ToString(); 
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;

                vm.IsSearchAreaVisible = true;
                
                vm.FacilityReq.staffId = vm.UserId;
                //vm.FacilityReq.accountName = username;
                vm.loggedInUser = vm.UserId;

                vm.HandleRequest();
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult GenerateDocuments(FacilityDocumentsViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = HttpContext.Session["userID"].ToString();
            vm.loggedInUser = vm.UserId;
            String username = HttpContext.Session["name"].ToString();

            vm.FacilityReq.staffId = vm.UserId;
            vm.FacilityReq.accountName = username;

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 10;
                vm.PageNumber = 1;
            }

            vm.HandleRequest();

            if (String.IsNullOrEmpty(vm.mainRefNo))
            {
                vm.mainRefNo = "";
            }
            else
            {
                Session["refNum"] = vm.mainRefNo;
            }

            if (vm.IsValid)
            {
                TempData["Msg"] = vm.Msg;

                vm.IsSearchAreaVisible = true;
                vm.IsDetailAreaVisible = true;
                                
                ModelState.Clear();              
            }
            else
            {
                foreach (KeyValuePair<string, string> item in vm.ValidationErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }
            }
            return View(vm);
        }

        //public ActionResult PrintReceipt()
        //{
        //    _jobLogger.Info("======Entered the PrintReceipt() method inside the FacilityDocumentsController ======");
        //    //FacilityDocumentsViewModel dbh = new FacilityDocumentsViewModel();
        //    ReportDocument rd = new ReportDocument();
        //    string reference = "";
        //    string receiptName = "";

        //    reference = Session["refNum"].ToString();
        //    _jobLogger.Info("======Reference Number To Be Used Is====== " +reference);
        //    _jobLogger.Info("======About Entering the fetchReportData(reference) method====== ");
        //    DataTable dt_report = fetchReportData(reference);

        //    _jobLogger.Info("======Records Obtained from Procedure ====== " + dt_report.Rows.Count);
        //    if (dt_report.Rows.Count > 0)
        //    {
        //        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "FacilityReceipt.rpt"));
        //        rd.SetDataSource(dt_report);

        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();

        //        rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
        //        rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
        //        rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

        //        //Printer
        //        // rd.PrintToPrinter(1, true, 0, 0);

        //        _jobLogger.Info("======Crystal Report Generated For this Receipt ====== FacilityReceipt.rpt");
        //    }
                       
        //    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    stream.Seek(0, SeekOrigin.Begin);
        //    ////JaizNibssBiometricsEntities context = new JaizNibssBiometricsEntities();

        //    ////var query = context.customerBioDatas.Where(x => x.requestReference == reference).FirstOrDefault();
        //    ////var queryList = new List<customerBioData> { query };

        //    ////rd.SetDataSource(queryList);        
        //    ///
        //    receiptName = reference + "_FacilityReceipt.pdf";
        //    _jobLogger.Info("======Out of the Stream to convert to FacilityReceipt.pdf====== " + stream.ToString());

        //    return File(stream, "application/pdf", receiptName);
        //}

        public DataTable fetchReportData(string requestReference)
        {
            _jobLogger.Info("======Inside the fetchReportData(reference) method with ReferenceNo:  " + requestReference + " ====== ");
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(_ConnectionString());
                conn.Open();

                command = new SqlCommand("SELECT_REPORT_TRANS", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@requestReference", SqlDbType.VarChar).Value = requestReference;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);

                _jobLogger.Info("======Records Obtained from the SELECT_REPORT_TRANS Procedure====== ");
            }
            catch (Exception ex)
            {
                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = "Error Selecting Data from HRFacilityMaster Table: " + ex.InnerException.ToString(),
                    ErrorTime = DateTime.Now,
                    ModulePointer = "fetchReportData",
                    StackTrace = ex.Message
                };
                LogUtility.ActivityLogger.WriteErrorLog(err2);

                _jobLogger.Error("======Error Selecting Data SELECT_REPORT_TRANS Proceduree====== " + ex.InnerException.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return dt;
        }

    }
}