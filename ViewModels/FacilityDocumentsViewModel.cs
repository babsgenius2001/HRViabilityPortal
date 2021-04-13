//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using HRViabilityPortal.Controllers;
using HRViabilityPortal.Database_References;
using NLog;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HRViabilityPortal.ViewModels
{
    public class FacilityDocumentsViewModel : ViewModelBase
    {
        public FacilityDocumentsViewModel()
        {
            Init();
        }

        private static SqlCommand command = null;
        private static SqlConnection conn = null;
        private static SqlTransaction tran = null;
        public static Logger _jobLogger = LogManager.GetCurrentClassLogger();

        public HttpServerUtilityBase Server { get; }
        public HttpResponseBase Response { get; }

        public List<HRFacilityMaster> FacilityReqs { get; set; }
        public List<FacilityRequestsDAO> HistoricalReqs { get; set; }
        public HRFacilityMaster SearchEntity { get; set; }
        public AuditLog AuditSearch { get; set; }
        public HRFacilityMaster Entity { get; set; }
        public HRFacilityMaster FacilityReq { get; set; }
        public string loggedInUser { get; set; }
        public string assetDesM { get; set; }
        public string assetDesB { get; set; }
        public string deliveryModeM { get; set; }
        public string deliveryModeI { get; set; }
        public string invoiceM { get; set; }
        public string invoiceI { get; set; }

        public string customField1 { get; set; }
        public string customField2 { get; set; }
        public string customField3 { get; set; }
        public string customField4 { get; set; }
        public string customField5 { get; set; }
        public string customField6 { get; set; }
        public string customField7 { get; set; }
        public string customField8 { get; set; }
        public string customField9 { get; set; }
        public string customField10 { get; set; }

        public string customFieldData1 { get; set; }
        public string customFieldData2 { get; set; }
        public string customFieldData3 { get; set; }
        public string customFieldData4 { get; set; }
        public string customFieldData5 { get; set; }
        public string customFieldData6 { get; set; }
        public string customFieldData7 { get; set; }
        public string customFieldData8 { get; set; }
        public string customFieldData9 { get; set; }
        public string customFieldData10 { get; set; }
        public string mainRefNo { get; set; }
        public DateTime dateReq { get; set; }

        protected override void Init()
        {
            FacilityReq = new HRFacilityMaster();
            FacilityReqs = new List<HRFacilityMaster>();
            HistoricalReqs = new List<FacilityRequestsDAO>();
            SearchEntity = new HRFacilityMaster();
            AuditSearch = new AuditLog();
            Entity = new HRFacilityMaster();
            EventCommand = "historical";
            ValidationErrors = new List<KeyValuePair<string, string>>();

            base.Init();
        }

        public override void HandleRequest()
        {
            //// This is an example of adding on a new command
            switch (EventCommand.ToLower())
            {
                case "search":
                    Get();
                    break;
                case "save":

                    // GenerateDocuments();
                    if (sendReceipt(SearchEntity.requestReferenceNumber))
                    {
                        IsDetailAreaVisible = false;
                    }                    
                    //sendDisbursementReceipt(SearchEntity.requestReferenceNumber);                   

                    break;

                default:

                    break;
            }

            base.HandleRequest();
        }

        protected override void Get()
        {
            FacilityReqs = Get(SearchEntity);

            mainRefNo = SearchEntity.requestReferenceNumber;

            base.Get();
        }

        protected HRFacilityMaster GetFacilityData(HRFacilityMaster ent)
        {
            HRFacilityMaster ret = new HRFacilityMaster();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    //ret = db.HRVehicleMasters.Where(x => x.requestReferenceNumber == FacilityVR.requestReferenceNumber).SingleOrDefault();
                    ret = db.HRFacilityMasters.Where(x => x.requestReferenceNumber == ent.requestReferenceNumber).SingleOrDefault();

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

        //public bool GenerateDocuments()
        //{
        //    bool resp = false;
        //    string otherFacilityValues = "";
        //    ReportDocument crystalReport = new ReportDocument();

        //    try
        //    {
        //        Entity = GetFacilityData(SearchEntity);
        //        HRViabilityPortalEntities context = new HRViabilityPortalEntities();

        //        string pdfFile = "E:\\"+ SearchEntity.requestReferenceNumber+".pdf";
                
        //        //string _path = Path.Combine(Server.MapPath("~/Upload"), Entity.staffId+ "_" + Entity.requestReferenceNumber);
        //        //pdfFile.SaveAs(_path);

        //        crystalReport.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport1.rpt"));
        //        crystalReport.SetDataSource(context.HRFacilityMasters.Where(x => x.requestReferenceNumber == SearchEntity.requestReferenceNumber).FirstOrDefault());

        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();


        //        crystalReport.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
        //        crystalReport.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
        //        crystalReport.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

        //        crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, pdfFile);

        //        Stream stream = crystalReport.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);               

        //        //crystalReport.Load(Path.Combine(Server.MapPath("~/Report"), "CustomerVerification.rpt"));
        //        //crystalReport.SetDatabaseLogon("username of Sql", "word of sql", "server name", "Database name");
        //        //crystalReport.SetParameterValue("empid", ddEmpcode.Text);
        //        //CrystalReportViewer1.ReportSource = crystalReport;
        //        //crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, pdfFile);

        //        var mail = new EmailSender();

        //        //String amountRequested = String.Format("{0:n}", Entity.amountRequested);
        //        //String downPaymentContribution = String.Format("{0:n}", Entity.downPaymentContribution);
        //        //String grosspay = String.Format("{0:n}", Entity.grossAnnualSalary);
        //        //String netpay = String.Format("{0:n}", Entity.netMonthlySalary);               

        //        //String amntRequested = String.Format("{0:n}", Entity.amountRequested);
        //        //String downPayment = String.Format("{0:n}", Entity.downPaymentContribution);

        //        //mail.DocumentGeneratorHelper(Entity.staffId, Entity.mobileNumber, Entity.accountNumber, Entity.accountName, Entity.natureOfEmployment, Entity.department, netpay, grosspay, Entity.facilityName, Entity.dateOfEmployment, Entity.dateOfBirth, Entity.jobFunction, amntRequested, downPayment, Entity.maritalStatus, Convert.ToInt32(Entity.numberOfMonthsInService), Entity.tenor, Entity.loanPurpose);

        //        //mail.ApplicationFormGeneratorHelper(assetDesM, assetDesB, Entity.vendorsName, Entity.vendorsAddress, Entity.vendorsPhoneNumber, Entity.locationOfAsset, Entity.vehicleModel, Entity.yearOfManufacture, deliveryModeM, deliveryModeI, Entity.typeOfProperty, Entity.propertyDescription, Entity.titleDocumentType, Entity.locationOfProperty, Entity.nameOfDeveloper, Entity.addressOfDeveloper, Entity.phoneNumberOfDeveloper, Entity.serviceDescription,
        //        //Entity.serviceProviderName, Entity.serviceProviderAddress, Entity.servicePhoneNumber, invoiceM, invoiceI, Entity.staffId, Entity.mobileNumber, Entity.accountNumber, Entity.accountName, Entity.natureOfEmployment, Entity.department, netpay, grosspay, Entity.facilityName, Entity.dateOfEmployment, Entity.dateOfBirth, Entity.jobFunction, amntRequested, downPayment,
        //        //Entity.maritalStatus, Convert.ToInt32(Entity.numberOfMonthsInService), Entity.tenor, Entity.loanPurpose, customField1, customField2, customField3, customField4, customField5, customField6, customField7, customField8, customField9, customField10, customFieldData1, customFieldData2, customFieldData3, customFieldData4, customFieldData5,
        //        //customFieldData6, customFieldData7, customFieldData8, customFieldData9, customFieldData10, otherFacilityValues);

        //        string op = "Facility Documents For Reference Number: " + Entity.requestReferenceNumber+ " Generated Successfully!";
               
        //        SendEmailWithAllDocuments(Entity.accountName);

        //        Msg = op;

        //        string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
        //        var log = new AllLogs();
        //        log.writeAuditlog(Entity.staffId, Entity.accountName, "Facility Documents Generated Successfully By: " + Entity.staffId, ip);

        //        //return File(stream, "application/pdf", "CustomerSlip.pdf");

        //    }
        //    catch (Exception ex)
        //    {
        //        Msg = "Submission Exception:  " + ex.ToString();

        //        var mail = new AllLogs();
        //        mail.ErrorLog(Msg, UserId);

        //        string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

        //        var log = new AllLogs();
        //        log.writeAuditlog(Entity.staffId, Entity.accountName, "Facility Documents Generation Exception Error By: " + Entity.staffId + "  on  " + DateTime.Now, ip);
        //    }

        //    return resp;

        //}

        public int SendEmailWithAllDocuments(string acctName)
        {
            int res = 0;
            string mailSubject = "Facility Documents For " + acctName;
            string mailBody = "Hello, Kindly find attached Facility Document For " + acctName;
            //string docPath1 = ConfigurationManager.AppSettings["docPath1"];
            //string docPath2 = ConfigurationManager.AppSettings["docPath2"];
            //string docPath3 = ConfigurationManager.AppSettings["docPath3"];
            //string docPath4 = ConfigurationManager.AppSettings["docPath4"];

            //string docPath1 = "file:///C:/Users/BA07190/Documents/My Projects/My .NET Projects/HRViabilityPortal/CompletedDocuments/UndertakingAuthorization.pdf";

            string docPath11 = Path.GetFullPath(@"C:\inetpub\wwwroot\HRViabilityPortal\CompletedDocuments\UndertakingAuthorization.pdf");
            string docPath22 = Path.GetFullPath(@"C:\inetpub\wwwroot\HRViabilityPortal\CompletedDocuments\EmployeeUndertaking.pdf");
            string docPath33 = Path.GetFullPath(@"C:\inetpub\wwwroot\HRViabilityPortal\CompletedDocuments\EmployerUndertaking.pdf");
            string docPath44 = Path.GetFullPath(@"C:\inetpub\wwwroot\HRViabilityPortal\CompletedDocuments\RetailFacilityApplication.pdf");


            ////string docPath1 = "file:///C:/Users/appsoluser/Documents/HRViabilityDocuments/CompletedDocuments/UndertakingAuthorization.pdf";
            //string docPath1 = "file:\\C:\\Users\\appsoluser\\Documents\\HRViabilityDocuments\\CompletedDocuments\\UndertakingAuthorization.pdf";
            //// string docPath1 = "file:///C:/inetpub/wwwroot/HRViabilityPortal/CompletedDocuments/UndertakingAuthorization.pdf";
            //string docPath11 = new Uri(docPath1).LocalPath;

            //string docPath2 = "file:\\C:\\Users\\appsoluser\\Documents\\HRViabilityDocuments\\CompletedDocuments\\EmployeeUndertaking.pdf";
            //string docPath22 = new Uri(docPath2).LocalPath;

            //string docPath3 = "file:\\C:\\Users\\appsoluser\\Documents\\HRViabilityDocuments\\CompletedDocuments\\EmployerUndertaking.pdf";
            //string docPath33 = new Uri(docPath3).LocalPath;

            //string docPath4 = "file:\\C:\\Users\\appsoluser\\Documents\\HRViabilityDocuments\\CompletedDocuments\\RetailFacilityApplication.pdf";
            //string docPath44 = new Uri(docPath4).LocalPath;

            //string docPath1 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\UndertakingAuthorization.pdf";
            //string docPath11 = new Uri(docPath1).LocalPath;

            //string docPath2 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\EmployeeUndertaking.pdf";
            //string docPath22 = new Uri(docPath2).LocalPath;

            //string docPath3 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\EmployerUndertaking.pdf";
            //string docPath33 = new Uri(docPath3).LocalPath;

            //string docPath4 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\RetailFacilityApplication.pdf";
            //string docPath44 = new Uri(docPath4).LocalPath;


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://1jr2x.api.infobip.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", getAuthorizationHeaderString());

            var request = new MultipartFormDataContent();
            request.Add(new StreamContent(File.OpenRead(docPath11)), "attachment", new FileInfo(docPath11).Name);
            request.Add(new StreamContent(File.OpenRead(docPath22)), "attachment", new FileInfo(docPath22).Name);
            request.Add(new StreamContent(File.OpenRead(docPath33)), "attachment", new FileInfo(docPath33).Name);
            request.Add(new StreamContent(File.OpenRead(docPath44)), "attachment", new FileInfo(docPath44).Name);
            request.Add(new StringContent("Jaiz Bank Plc <notification@alerts.jaizbankplc.com>"), "from");
            request.Add(new StringContent(FacilityReq.staffId + "@jaizbankplc.com"), "to");
            request.Add(new StringContent(mailSubject), "subject");
            request.Add(new StringContent(mailBody), "text");
            request.Add(new StringContent("true"), "intermediateReport");
            request.Add(new StringContent("application/json"), "notifyContentType");
            request.Add(new StringContent("DLR callback data"), "callbackData");

            HttpResponseMessage response = null;
            try
            {
                response = client.PostAsync("email/1/send", request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    res = 1;

                }
            }
            catch (Exception e)
            {
                res = 0;
                Console.WriteLine(e.InnerException);
                string exc = e.ToString();

                string ErrorDescription = e.Message + Environment.NewLine + e.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);
            }

            return res;
        }

        public string getAuthorizationHeaderString()
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
            string header = System.Convert.ToBase64String(concatenated);

            return header;
        }

        public bool checkRefNo(HRFacilityMaster entity)
        {
            bool ret = false;

            if (entity.requestReferenceNumber == null)
            {
                ValidationErrors.Add(new
                   KeyValuePair<string, string>("",
                       "Kindly Provide The Reference Number!"));

                IsValid = false;
                return false;
            }

            else
            {
                IsValid = true;
                ret = true;
            }

            return ret;
        }

        public List<HRFacilityMaster> Get(HRFacilityMaster entity)
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();

            mainRefNo = entity.requestReferenceNumber;

            ret = CreateData(entity);

            if(ret.Count == 0)
            {
                ValidationErrors.Add(new
                     KeyValuePair<string, string>("HRViability",
                     "No records found for this Reference Number!"));

                ret = null;
            }

            if (!string.IsNullOrEmpty(entity.requestReferenceNumber))
            {

                ret = ret.FindAll(p => p.requestReferenceNumber.Trim().StartsWith(entity.requestReferenceNumber.Trim()));
                  
                string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                //writeAuditlog(string userid, string username, string op, string ipAdd)

                var log = new AllLogs();
                log.writeAuditlog(FacilityReq.staffId, FacilityReq.accountName, "Checked: " + entity.requestReferenceNumber, ip);

            }
            else
            {
                ret = null;
            }

            return ret;
        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

     
        protected List<HRFacilityMaster> CreateData(HRFacilityMaster entity)
        {
            List<HRFacilityMaster> ret = new List<HRFacilityMaster>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.HRFacilityMasters.Where(x => x.requestStatus == "Approved_For_Disbursement" && x.requestReferenceNumber == entity.requestReferenceNumber).ToList();


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
                mail.ErrorLog(ErrorDescription, UserId);
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
        }


        public bool sendReceipt(string refNo)
        {
            string staffId = "";
            string staffName = "";
            string acctNo = "";
            string dept = "";
            string grade = "";
            string facilityType = "";
            string amountReq = "";
            string salary = "";
            string repayment = "";
            string downPayment = "";
            string rate = "";
            string tenor = "";
            string datereq = "";
            string staffEmail = "";
            int res = 0;
            bool ret = false;
            //String.Format("{0:n}", totalLoansRepaymentMonthly);

            using (var db = new HRViabilityPortalEntities())
            {
                var receipt = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo.Trim() select m).FirstOrDefault();

                staffId = receipt.staffId;
                staffName = receipt.accountName;
                acctNo = receipt.accountNumber;
                dept = receipt.department;
                grade = receipt.salaryGrade;
                facilityType = receipt.facilityName;
                amountReq = String.Format("{0:n}", receipt.amountRequested);
                salary = String.Format("{0:n}", receipt.netMonthlySalary);
                repayment = String.Format("{0:n}", receipt.repaymentAmount);
                downPayment = String.Format("{0:n}", receipt.downPaymentContribution);
                rate = String.Format("{0:n}", receipt.percentageRate);
                tenor = receipt.tenor.ToString();

                dateReq = Convert.ToDateTime(receipt.requestDate);
                datereq = dateReq.ToString("dd-MM-yyyy hh:mm:ss");

                staffEmail = receipt.staffEmailAddress;
            }

            //TestDocGenerator.DocumentGenerator generateDoc = new TestDocGenerator.DocumentGenerator();
            DocumentGenerator.DocumentGenerator generateDoc = new DocumentGenerator.DocumentGenerator();

            generateDoc.GenerateFacilityReceipt(refNo, staffId, staffName, acctNo, dept, grade, facilityType, amountReq, salary, repayment, downPayment, rate, tenor, datereq);

            string mailSubject = "Facility Receipt For " + staffId;
            string dt = DateTime.Now.ToString();
            string footerDt = DateTime.Now.Year.ToString();

            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/FacilityReceiptMailTemplate.html"));
            string line = sr.ReadToEnd();
            sb.Append(line);
            sb.Replace("{staffName}", staffName);           
            sb.Replace("{footerDt}", footerDt);
            string mailBody = sb.ToString();
                        
            string docPath = Path.GetFullPath(@"D:\\HRViabilityDocuments\GeneratedDocuments\FacilityReceipt_" + staffId + ".pdf");
            _jobLogger.Info("|SendEmailWithDocument|========== Path to pick Image from ================== " + docPath);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://1jr2x.api.infobip.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", getAuthorizationHeaderString());

            var request = new MultipartFormDataContent();
            request.Add(new StreamContent(File.OpenRead(docPath)), "attachment", new FileInfo(docPath).Name);
            request.Add(new StringContent("Jaiz Bank Plc <notification@alerts.jaizbankplc.com>"), "from");
            request.Add(new StringContent(staffEmail), "to");            
            request.Add(new StringContent(mailSubject), "subject");
            request.Add(new StringContent(mailBody), "html");
            request.Add(new StringContent("true"), "intermediateReport");
            request.Add(new StringContent("application/json"), "notifyContentType");
            request.Add(new StringContent("DLR callback data"), "callbackData");

            _jobLogger.Info("|SendEmailWithDocument|========== Request to send for email is ================== " + request.ToString());
            HttpResponseMessage response = null;
            try
            {
                response = client.PostAsync("email/1/send", request).Result;
                _jobLogger.Info("|SendEmailWithDocument|========== Response for sending email is ================== " + response.Content.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    res = 1;

                    _jobLogger.Info("|SendEmailWithDocument|========== Mail sent successfully ================== ");

                    ret = true;

                    string op = "Facility Document Generated And Sent To Your Email Successfully!";                   

                    Msg = op;
                }
            }
            catch (Exception e)
            {
                res = 0;
                _jobLogger.Info("|SendEmailWithDocument|========== Mail not sent successfully ================== ");
                Console.WriteLine(e.InnerException);
                string exc = e.ToString();

                string ErrorDescription = e.Message + Environment.NewLine + e.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, UserId);

                ret = false;


                string op = "Error Generating Facility Document!";

                Msg = op;
            }
            finally
            {

            }

            return ret;
        }

        

        //public void sendDisbursementReceipt(string refNo)
        //{
        //    ReportDocument rd = new ReportDocument();
        //    // int res = 0;

        //    string doc = Path.GetFullPath(@"C:\\Users\BA07190\Documents\MyProjects\My_NET_Projects\HRViabilityPortal\Reports");

        //    DataTable dt_report = fetchReportData(refNo);

        //    if (dt_report.Rows.Count > 0)
        //    {
        //        rd.Load(Path.Combine(Server.MapPath(doc), "FacilityReceipt.rpt"));
        //        //rd.Load(doc, "FacilityReceipt.rpt");
        //        //rd.Load(Path.Combine(Server.MapPath("~/Reports"), "FacilityReceipt.rpt"));
        //        rd.SetDataSource(dt_report);

        //        string accountName = "";
        //        string staffId = "";

        //        using (var db = new HRViabilityPortalEntities())
        //        {
        //            var rspF = (from m in db.HRFacilityMasters where m.requestReferenceNumber == refNo select m).FirstOrDefault();

        //            accountName = rspF.accountName;
        //            staffId = rspF.staffId;
        //        }

        //        string mailSubject = "Facility Request Receipt For " + accountName;
        //        string mailBody = "Hello, Kindly find attached your Facility Request Receipt For " + accountName;

        //        HttpClient client = new HttpClient();
        //        client.BaseAddress = new Uri("https://1jr2x.api.infobip.com/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", getAuthorizationHeaderString());

        //        var request = new MultipartFormDataContent();
        //        request.Add(new StreamContent(rd.ExportToStream(ExportFormatType.PortableDocFormat)), "attachment", "Receipt_" + staffId + ".pdf");
        //        request.Add(new StringContent("Jaiz Bank Plc <notification@alerts.jaizbankplc.com>"), "from");
        //        request.Add(new StringContent("BA07190@jaizbankplc.com"), "to");
        //        //request.Add(new StringContent(staffId + "@jaizbankplc.com"), "to");
        //        request.Add(new StringContent(mailSubject), "subject");
        //        request.Add(new StringContent(mailBody), "html");
        //        request.Add(new StringContent("true"), "intermediateReport");
        //        request.Add(new StringContent("application/json"), "notifyContentType");
        //        request.Add(new StringContent("DLR callback data"), "callbackData");


        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();

        //        //    rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
        //        //    rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
        //        //    rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

        //        //    //Printer
        //        //    rd.PrintToPrinter(1, true, 0, 0);
        //    }

        //}

        public DataTable fetchReportData(string requestReference)
        {
            _jobLogger.Info("======Inside the  dbh.fetchReportData(reference) method with ReferenceNo:  " + requestReference +  " ====== ");
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



        public class FacilityRequestsDAO
        {
            public string refNumber { get; set; }
            public string facilityName { get; set; }
            public decimal amountRequestedFor { get; set; }
            public DateTime dateChecked { get; set; }
            public string statusOfFacility { get; set; }
        }

    }
}