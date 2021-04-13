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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using System.Web.Mvc;
using System.Net;
using System.Net.Http.Headers;
using NLog;

namespace HRViabilityPortal.ViewModels
{
    public class NewRequestViewModel : ViewModelBase
    {
        public NewRequestViewModel()
        {
            Init();

        }

        public static Logger _jobLogger = LogManager.GetCurrentClassLogger();
        private static readonly string ImalConn_ = ConfigurationManager.ConnectionStrings["OracConn_"].ConnectionString.ToString();
        public HRFacilityMaster Entity { get; set; }
        public Calculator docalc { get; set; }
        public LogOnModel logon { get; set; }
        public HRFacilityMaster SearchEntity { get; set; }
        public List<Facility> FacilityRules { get; set; }
        public List<Facility> FacilityReqs { get; set; }
        public Facility SearchEntity2 { get; set; }
        public Facility Entity2 { get; set; }
        public Facility Entity3 { get; set; }
        public Facility FacilityRule { get; set; }
        public string MessageBody { get; set; }
        public string staffId { get; set; }
        public string url { get; set; }
        public string Userbranch { get; set; }
        public string ipAddress { get; set; }
        public List<SelectList> State { get; set; }
        public List<SelectList> Branches { get; set; }
        public List<SelectList> ListOfFacilities { get; set; }
        public List<SelectDocumentsList> DocumentsList { get; set; }
        public List<SelectFormsList> FormsList { get; set; }
        public List<FacilityDetailsDAO> MyFacilities { get; set; }
        public HttpPostedFileBase uploadedDocument { get; set; }
        public HttpPostedFileBase uploadedDocument1 { get; set; }
        public HttpPostedFileBase uploadedDocument2 { get; set; }
        public HttpPostedFileBase uploadedDocument3 { get; set; }
        public HttpPostedFileBase uploadedDocument4 { get; set; }
        public HttpPostedFileBase uploadedDocument5 { get; set; }
        public HttpPostedFileBase uploadedDocument6 { get; set; }
        public HttpPostedFileBase uploadedDocument7 { get; set; }
        public HttpPostedFileBase uploadedDocument8 { get; set; }
        public HttpPostedFileBase uploadedDocument9 { get; set; }
        public HttpPostedFileBase uploadedDocument10 { get; set; }
        public IEnumerable<HttpPostedFileBase> FileBases { get; set; }
        public HttpServerUtilityBase Server { get; set; }
        public HRFacilityMaster FacilityReq { get; set; }

        private static int m_Counter = 0;
        public decimal totalRepaymentPerMonth { get; set; }
        public decimal remainingPaymentperMonth { get; set; }
        public bool IsStep1 { get; set; }
        public bool IsStep2A { get; set; }
        public bool IsStep2B { get; set; }
        public bool IsStep2 { get; set; }
        public bool IsStep3 { get; set; }
        public bool IsStep4 { get; set; }
        public bool IsStep4A { get; set; }
        public bool IsStep4B { get; set; }
        public bool IsStep5 { get; set; }
        public bool IsStep6 { get; set; }
        public bool IsMurabaha { get; set; }
        public bool IsHomeFinance { get; set; }
        public bool IsIjaraService { get; set; }
        public bool IsBaiMuajjal { get; set; }
        public bool OtherFacilities { get; set; }
        public bool IsMainUnderTakingForm { get; set; }
        public bool IsSupervisoryEndorsementForm { get; set; }
        public bool IsUnderTakingForm { get; set; }
        public bool IsEmployerUnderTakingForm { get; set; }
        public bool consentToUnderTaking { get; set; }
        public bool showLoanHistory { get; set; }
        public bool showConditionError { get; set; }
        public bool showConditionProceed { get; set; }
        public string document1 { get; set; }
        public string document2 { get; set; }
        public string document3 { get; set; }
        public string document4 { get; set; }
        public string document5 { get; set; }
        public string document6 { get; set; }
        public string document7 { get; set; }
        public string assetDesM { get; set; }
        public string assetDesB { get; set; }
        public string mobileNumber { get; set; }
        public string maritalStatus { get; set; }
        public string stateNew { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string amountRequested { get; set; }
        public string downPaymentContribution { get; set; }
        public List<DocumentsDAO> MyDocuments { get; set; }
        public string activeStatus { get; set; }
        public string deliveryModeM { get; set; }
        public string deliveryModeI { get; set; }
        public string invoiceM { get; set; }
        public string invoiceI { get; set; }
        public string columnName { get; set; }
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
        public string salaryGrade { get; set; }
        public decimal paye { get; set; }
        public decimal nhf { get; set; }
        public decimal pensionsContribution { get; set; }
        public decimal totalMonthlyDeductions { get; set; }
        public string netMSal { get; set; }
        public string grossASal { get; set; }
        public string annHSal { get; set; }
        public string payeTax { get; set; }
        public string housingFund { get; set; }
        public string pensions { get; set; }
        public string totalMC { get; set; }
        public string repaymentAmount { get; set; }
        public decimal totalLoansRepaymentMonthly { get; set; }
        public string totalLoansRepaymentMonthlyD { get; set; }
        public string totmaxpm { get; set; }
        public string totdedpm { get; set; }
        public string totalmaxdedpmonsalary { get; set; }
        public string maxLimitUsed { get; set; }
        public string maxLimitOpt { get; set; }
        public decimal rateUsedToCalculate { get; set; }
        public int gradeLevel { get; set; }
        public int noOfMonthsFromAssumptionDate { get; set; }


        protected override void Init()
        {
            EventCommand = "FacilityList";
            logon = new LogOnModel();
            State = new List<SelectList>();
            Branches = new List<SelectList>();
            SearchEntity = new HRFacilityMaster();
            FacilityReq = new HRFacilityMaster();
            Entity = new HRFacilityMaster();
            MessageBody = string.Empty;
            ListOfFacilities = new List<SelectList>();
            DocumentsList = new List<SelectDocumentsList>();
            FormsList = new List<SelectFormsList>();
            docalc = new Calculator();
            ValidationErrors = new List<KeyValuePair<string, string>>();

            MyDocuments = new List<DocumentsDAO>();
            MyFacilities = new List<FacilityDetailsDAO>();

            base.Init();
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
        }

        public override void HandleRequest()
        {
            _jobLogger.Info("===========Enetered the NewRequestViewModel=================");
            _jobLogger.Info("===========EventCommand Called================= " + EventCommand.ToLower());
            //// This is an example of adding on a new command
            switch (EventCommand.ToLower())
            {
                
                case "validate":
                    if (validateStep1Items())

                        IsStep2 = true;
                    else
                        IsStep1 = true;

                    GetDropDown();
                    break;
                case "step3":
                    if (validateStep2Items())

                        IsStep3 = true;

                    else
                        IsStep1 = true;
                    GetDropDown();
                    break;
                case "step4":
                    if (confirmFieldItems())
                    {
                        if (CalculateEligibility())
                        {
                            if (showConditionError == true)
                            {
                                showConditionError = true;
                                break;
                            }

                            if (FacilityReq.facilityName == "Murabaha" || FacilityReq.facilityName.Contains("Murabaha") || FacilityReq.facilityName.Contains("murabaha"))
                            {
                                IsMurabaha = true;
                            }

                            else if (FacilityReq.facilityName == "Home Finance" || FacilityReq.facilityName.Contains("Home"))
                            {
                                IsHomeFinance = true;
                            }

                            else if (FacilityReq.facilityName == "Ijara Service" || FacilityReq.facilityName.Contains("Ijara") || FacilityReq.facilityName.Contains("ijara"))
                            {
                                IsIjaraService = true;
                            }

                            else if (FacilityReq.facilityName == "Bai Muajjal")
                            {
                                IsBaiMuajjal = true;
                            }

                            else if (FacilityReq.facilityName.Contains("Branch") || FacilityReq.facilityName.Contains("branch"))
                            {
                                GetDocumentNames();
                                Get();
                                IsStep5 = true;
                            }
                            else
                            {
                                OtherFacilities = true;
                                Get();
                            }
                        }
                        else
                        {
                            IsStep3 = true;
                            MyFacilities = GetAllFacilityDetails(Convert.ToInt32(FacilityReq.cif));

                            GetDropDown();
                        }
                    }
                    else
                    {
                        IsStep3 = true;
                        MyFacilities = GetAllFacilityDetails(Convert.ToInt32(FacilityReq.cif));

                        GetDropDown();
                    }

                    break;               

                case "step5":
                    bool ret = false;
                    ret = ValidateAllMenus();
                    if (ret == true)
                    {
                        GetDocumentNames();
                        Get();
                        IsStep5 = true;
                    }
                    else
                     if (ret == false)
                    {
                        if (FacilityReq.facilityName == "Murabaha" || FacilityReq.facilityName.Contains("Murabaha") || FacilityReq.facilityName.Contains("murabaha"))
                        {
                            IsMurabaha = true;
                        }

                        else if (FacilityReq.facilityName == "Home Finance" || FacilityReq.facilityName.Contains("Home"))
                        {
                            IsHomeFinance = true;
                        }

                        else if (FacilityReq.facilityName == "Ijara Service" || FacilityReq.facilityName.Contains("Ijara") || FacilityReq.facilityName.Contains("ijara"))
                        {
                            IsIjaraService = true;
                        }

                        else if (FacilityReq.facilityName == "Bai Muajjal")
                        {
                            IsBaiMuajjal = true;
                        }

                        else if (FacilityReq.facilityName.Contains("Branch") || FacilityReq.facilityName.Contains("branch"))

                            IsStep5 = true;

                        else

                            OtherFacilities = true;
                    }
                    Get();
                    break;
                case "step6":
                    Mode = "Add";
                    //if (checkCheckBox())                 
                    Save();
                    if (ValidationErrors.Count == 0)
                    {
                        IsStep1 = true;
                        IsDetailAreaVisible = true;
                        GetDropDown();
                        //IsMainUnderTakingForm = true;
                    }
                    else
                        IsStep5 = true;

                    break;
                default:
                    IsStep1 = true;
                    IsDetailAreaVisible = true;
                    GetDropDown();
                    break;
            }

            base.HandleRequest();
        }


        private bool CalculateEligibility()
        {
            decimal ratePercent, ratePercentOutstanding = 0;
            decimal totalMaxDeductionPerMonth = 0;
            decimal amntDiff, valA, valB, valC, valD, mainValToBeUsed = 0;
            bool ret = false;
            bool ret3 = false;
            decimal ratePercentMaxLimit, allowanceGauge = 0;
            decimal compare = 0;
            decimal TotalDeductableMonthlyDeductions = 0;
            bool hasHomeFinanceBefore = false;

            _jobLogger.Info("===========CalculateEligibility Method=================");

            FacilityReq.amountRequested = Convert.ToDecimal(amountRequested);
            FacilityReq.downPaymentContribution = Convert.ToDecimal(downPaymentContribution);

            Entity2 = GetFacilityRulesData(FacilityReq.facilityName);

            MyFacilities = GetAllFacilityDetails(Convert.ToInt32(FacilityReq.cif));
            //List<FacilityDetailsDAO> container =  new List<FacilityDetailsDAO>();
            if (MyFacilities.Count > 0)
            {
                foreach(var fac in MyFacilities)
                {
                    if(fac.facilityType.Contains("Ijara Wa-Iqtina Ret New"))
                    {
                        hasHomeFinanceBefore = true;
                    }
                }
            }
            

            if (Entity2.totalDeductionFromSalary == "1/2")
            {               
                totalMaxDeductionPerMonth = (FacilityReq.netMonthlySalary / 2);       
               
            }

            if (Entity2.totalDeductionFromSalary == "1/3")
            {
                if (hasHomeFinanceBefore)
                {
                    totalMaxDeductionPerMonth = (FacilityReq.netMonthlySalary / 2);
                }
                else
                {
                    totalMaxDeductionPerMonth = (FacilityReq.netMonthlySalary / 3);
                }                   
            }

            if (Entity2.totalDeductionFromSalary == "2/3")
            {
                if (hasHomeFinanceBefore)
                {
                    totalMaxDeductionPerMonth = (FacilityReq.netMonthlySalary / 2);
                }
                else
                {
                    totalMaxDeductionPerMonth = ((FacilityReq.netMonthlySalary * 2) / 3);
                }                   
            }

            ret3 = ValidateTenor();

            if (ret3)
            {
                if (Entity2.maximumAmountLimitOption == "% Of Gross Package")
                {
                    ratePercentMaxLimit = (decimal)Entity2.maximumAmountLimit / 100;

                    allowanceGauge = ratePercentMaxLimit * FacilityReq.grossAnnualSalary;

                }

                if (Entity2.maximumAmountLimitOption == "x Annual Housing Allowance")
                {
                    allowanceGauge = (decimal)(Entity2.maximumAmountLimit * FacilityReq.annualHousingAllowance);

                }

                if (Entity2.maximumAmountLimitOption == "x Gross Package")
                {
                    allowanceGauge = (decimal)(Entity2.maximumAmountLimit * FacilityReq.grossAnnualSalary);

                }

                if (allowanceGauge < FacilityReq.amountRequested)
                {
                    amntDiff = (decimal)(FacilityReq.amountRequested - allowanceGauge);

                    valA = (decimal)(allowanceGauge * (FacilityReq.percentageRate / 100));
                    valB = (decimal)(amntDiff * (Entity2.staffBranchPercentageRate / 100));

                    valC = (decimal)(valA + valB);
                    mainValToBeUsed = (decimal)(allowanceGauge + amntDiff);

                    valD = (valC / mainValToBeUsed);

                    ratePercentOutstanding = Math.Round((valD * 100), 2);

                    try
                    {

                        Request req = new Request();
                        req.Amount = (double)FacilityReq.amountRequested;
                        req.DownPayment = (double)FacilityReq.downPaymentContribution;
                        req.Rate = (double)ratePercentOutstanding / 100;
                        req.Type = 0;
                        req.No_of_Months = FacilityReq.tenor;
                        req.StartDate = DateTime.Today;

                        Calculator C = new Calculator();
                        List<Calculator> L = C.GetCalculationDetails(req);

                        foreach (Calculator stq in L)

                            FacilityReq.repaymentAmount = System.Convert.ToDecimal(stq.AveragePayment);
                        FacilityReq.requestDate = DateTime.Now;

                        rateUsedToCalculate = ratePercentOutstanding;

                    }
                    catch (Exception ex)
                    {
                        string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                        var mail = new AllLogs();
                        mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                        ValidationErrors.Add(new
                              KeyValuePair<string, string>("Error Message", ex.Message));
                        IsValid = false;
                        ret = false;
                    }

                }
                else
                {
                    try
                    {
                        ratePercent = (decimal)FacilityReq.percentageRate / 100;

                        Request req = new Request();
                        req.Amount = (double)FacilityReq.amountRequested;
                        req.DownPayment = (double)FacilityReq.downPaymentContribution;
                        req.Rate = (double)ratePercent;
                        req.Type = 0;
                        req.No_of_Months = FacilityReq.tenor;
                        req.StartDate = DateTime.Today;

                        Calculator C = new Calculator();
                        List<Calculator> L = C.GetCalculationDetails(req);

                        foreach (Calculator stq in L)

                        FacilityReq.repaymentAmount = System.Convert.ToDecimal(stq.AveragePayment);
                        FacilityReq.requestDate = DateTime.Now;

                        rateUsedToCalculate = (decimal)FacilityReq.percentageRate;

                    }
                    catch (Exception ex)
                    {
                        string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                        var mail = new AllLogs();
                        mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                        mail.ErrorLog(ErrorDescription, FacilityReq.staffId + " ||CalculateEligibility Error|| NewRequestViewModel.cs|| Line 1910");

                        ValidationErrors.Add(new
                              KeyValuePair<string, string>("Error Message", ex.Message));
                        IsValid = false;
                        ret = false;
                    }
                }

                TotalDeductableMonthlyDeductions = (decimal)FacilityReq.repaymentAmount + totalLoansRepaymentMonthly;

                repaymentAmount = String.Format("{0:n}", FacilityReq.repaymentAmount);

                compare = Decimal.Compare(totalMaxDeductionPerMonth, TotalDeductableMonthlyDeductions);

                if (compare == 1)
                {
                    IsValid = true;
                    ret = true;

                    totmaxpm = String.Format("{0:n}", totalMaxDeductionPerMonth);
                    totdedpm = String.Format("{0:n}", TotalDeductableMonthlyDeductions);
                    totalmaxdedpmonsalary = Entity2.totalDeductionFromSalary;
                    maxLimitUsed = Convert.ToString(Entity2.maximumAmountLimit);
                    maxLimitOpt = Entity2.maximumAmountLimitOption;

                    showConditionProceed = true;
                }
                else
                {
                    totmaxpm = String.Format("{0:n}", totalMaxDeductionPerMonth);
                    totdedpm = String.Format("{0:n}", TotalDeductableMonthlyDeductions);
                    totalmaxdedpmonsalary = Entity2.totalDeductionFromSalary;
                    maxLimitUsed = Convert.ToString(Entity2.maximumAmountLimit);
                    maxLimitOpt = Entity2.maximumAmountLimitOption;

                    //ValidationErrors.Add(new
                    //        KeyValuePair<string, string>("Invalid",
                    //        "Requested Amount Too High. Monthly Deduction Surpassed Limit: Total Monthly Deductable Amount - " + totdedpm + " passed Total Monthly Deduction Limit - " + totmaxpm + ";  Maximum Total Deduction from Salary Option Used: " + totalmaxdedpmonsalary + ".  Maximum Limit Option Used: "+ maxLimitUsed+ "  "+ maxLimitOpt));

                    showConditionError = true;
                    //IsValid = false;
                    //ret = false;

                    IsValid = true;
                    ret = true;
                }
            }

            return ret;
        }

        protected bool ValidateAllMenus()
        {
            bool ret = true;

            if (FacilityReq.facilityName == "Murabaha" || FacilityReq.facilityName.Contains("Murabaha") || FacilityReq.facilityName.Contains("murabaha"))
            {

                if ((string.IsNullOrEmpty(FacilityReq.assetDescription)) || (string.IsNullOrEmpty(FacilityReq.vendorsName)) || (string.IsNullOrEmpty(FacilityReq.vendorsAddress)) || (string.IsNullOrEmpty(FacilityReq.vendorsPhoneNumber)) || (string.IsNullOrEmpty(FacilityReq.locationOfAsset)) || (string.IsNullOrEmpty(FacilityReq.hasExistingVehicleLoan)) || (string.IsNullOrEmpty(FacilityReq.modeOfDelivery)) || (string.IsNullOrEmpty(FacilityReq.quotationEnclosed)))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "All Relevant Fields Must be Filled!"));

                    IsValid = false;
                    ret = false;
                }
            }

            else if (FacilityReq.facilityName == "Home Finance" || FacilityReq.facilityName.Contains("Home"))
            {

                if ((string.IsNullOrEmpty(FacilityReq.typeOfProperty)) || (string.IsNullOrEmpty(FacilityReq.propertyDescription)) || (string.IsNullOrEmpty(FacilityReq.titleDocumentType)) || (string.IsNullOrEmpty(FacilityReq.locationOfProperty)) || (string.IsNullOrEmpty(FacilityReq.nameOfDeveloper)) || (string.IsNullOrEmpty(FacilityReq.addressOfDeveloper)) || (string.IsNullOrEmpty(FacilityReq.phoneNumberOfDeveloper)) || (string.IsNullOrEmpty(FacilityReq.hasExistingMortgageLoan)))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "All Relevant Fields Must be Filled!"));

                    IsValid = false;
                    ret = false;
                }
            }

            else if (FacilityReq.facilityName == "Ijara Service" || FacilityReq.facilityName.Contains("Ijara") || FacilityReq.facilityName.Contains("ijara"))
            {

                if ((string.IsNullOrEmpty(FacilityReq.serviceDescription)) || (string.IsNullOrEmpty(FacilityReq.serviceProviderName)) || (string.IsNullOrEmpty(FacilityReq.serviceProviderAddress)) || (string.IsNullOrEmpty(FacilityReq.servicePhoneNumber)) || (string.IsNullOrEmpty(FacilityReq.quotationEnclosed)) || (string.IsNullOrEmpty(FacilityReq.modeOfDelivery)))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "All Relevant Fields Must be Filled!"));

                    IsValid = false;
                    ret = false;
                }
            }

            else if (FacilityReq.facilityName == "Bai Muajjal")
            {

                if ((string.IsNullOrEmpty(FacilityReq.assetDescription)) || (string.IsNullOrEmpty(FacilityReq.modeOfDelivery)))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "All Relevant Fields Must be Filled!"));

                    IsValid = false;
                    ret = false;
                }

            }
            else
            {
                if (string.IsNullOrEmpty(FacilityReq.customField1Data) && string.IsNullOrEmpty(FacilityReq.customField2Data) && string.IsNullOrEmpty(FacilityReq.customField3Data) && string.IsNullOrEmpty(FacilityReq.customField4Data) && string.IsNullOrEmpty(FacilityReq.customField5Data) && string.IsNullOrEmpty(FacilityReq.customField6Data) && string.IsNullOrEmpty(FacilityReq.customField7Data) && string.IsNullOrEmpty(FacilityReq.customField8Data) && string.IsNullOrEmpty(FacilityReq.customField9Data) && string.IsNullOrEmpty(FacilityReq.customField10Data))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "All Fields Must be Filled!"));

                    IsValid = false;
                    ret = false;
                }
            }

            return ret;
        }

        protected bool ValidateDocumentExtensionAndSize()
        {
            bool ret = true;

            if (string.IsNullOrEmpty(FacilityReq.fileName1) && string.IsNullOrEmpty(FacilityReq.fileName2) && string.IsNullOrEmpty(FacilityReq.fileName3))
            {               

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "You must attach documents!"));

                    IsValid = false;
                    ret = false;
                
            }

           if (!string.IsNullOrEmpty(FacilityReq.fileName1))
           {
                if (!FacilityReq.fileName1.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }

                if(uploadedDocument1.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }
            if (!string.IsNullOrEmpty(FacilityReq.fileName2))
            {
                if (!FacilityReq.fileName2.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }

                if (uploadedDocument2.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }
            if (!string.IsNullOrEmpty(FacilityReq.fileName3))
            {
                if (!FacilityReq.fileName3.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument3.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }
            if (!string.IsNullOrEmpty(FacilityReq.fileName4))
            {
                if (!FacilityReq.fileName4.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument4.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }
            if (!string.IsNullOrEmpty(FacilityReq.fileName5))
            {
                if (!FacilityReq.fileName5.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument5.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }
            if (!string.IsNullOrEmpty(FacilityReq.fileName6))
            {
                if (!FacilityReq.fileName6.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument6.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }

            if (!string.IsNullOrEmpty(FacilityReq.fileName7))
            {
                if (!FacilityReq.fileName7.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument7.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }

            if (!string.IsNullOrEmpty(FacilityReq.fileName8))
            {
                if (!FacilityReq.fileName8.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument8.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }

            if (!string.IsNullOrEmpty(FacilityReq.fileName9))
            {
                if (!FacilityReq.fileName9.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument9.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }

            if (!string.IsNullOrEmpty(FacilityReq.fileName10))
            {
                if (!FacilityReq.fileName10.EndsWith(".pdf"))
                {

                    ValidationErrors.Add(new
                           KeyValuePair<string, string>("Invalid",
                           "Document must be in .pdf format!"));

                    IsValid = false;
                    ret = false;
                }
                if (uploadedDocument10.ContentLength > 2100000)
                {
                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Document Size Must not be more than 2MB!"));

                    IsValid = false;
                    ret = false;
                }
            }

            return ret;
        }
       public List<Facility> Get()
        {

            // FacilityReqs = GetFacilityRulesData(facName);

            Entity2 = GetFacilityData();
            Entity3 = GetFacilityRulesData(Entity2.facilityName);


            if (Entity3.customField1 != null)
            {
                FacilityReq.customField1 = Entity3.customField1;
                customField1 = Entity3.customField1;
            }
            if (Entity3.customField2 != null)
            {
                FacilityReq.customField2 = Entity3.customField2;
                customField2 = Entity3.customField2;
            }
            if (Entity3.customField3 != null)
            {
                FacilityReq.customField3 = Entity3.customField3;
                customField3 = Entity3.customField3;
            }
            if (Entity3.customField4 != null)
            {
                FacilityReq.customField4 = Entity3.customField4;
                customField4 = Entity3.customField4;
            }

            if (Entity3.customField5 != null)
            {
                FacilityReq.customField5 = Entity3.customField5;
                customField5 = Entity3.customField5;
            }
            if (Entity3.customField6 != null)
            {
                FacilityReq.customField6 = Entity3.customField6;
                customField6 = Entity3.customField6;
            }
            if (Entity3.customField7 != null)
            {
                FacilityReq.customField7 = Entity3.customField7;
                customField7 = Entity3.customField7;
            }
            if (Entity3.customField8 != null)
            {
                FacilityReq.customField8 = Entity3.customField8;
                customField8 = Entity3.customField8;
            }
            if (Entity3.customField9 != null)
            {
                FacilityReq.customField9 = Entity3.customField9;
                customField9 = Entity3.customField9;
            }
            if (Entity3.customField10 != null)
            {
                FacilityReq.customField10 = Entity3.customField10;
                customField10 = Entity3.customField10;
            }
            //MyDocuments = GetAllDocuments(activeStatus);

            return FacilityReqs;
        }

        protected Facility GetFacilityData()
        {
            Facility ret = new Facility();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {                    
                    ret = db.Facilities.Where(x => x.facilityName == FacilityReq.facilityName).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRFacility",
                      ex.Message));
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
        }

        protected List<Facility> CreateData(string facName)
        {
            List<Facility> ret = new List<Facility>();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.Facilities.Where(x => x.facilityName == facName && x.active == "Y").ToList();
                    if (PageNumber > 0 && PageSize > 0)
                    {
                        PagedList = ret.ToPagedList(PageNumber, PageSize);
                        ret = ret.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));
            }
            finally
            {
                //db.Configuration.Close();
            }
            return ret;
        }


        public bool ValidateTenor()
        {
            int tenorObtained = 0;
            bool ret = true;
            tenorObtained = getTenor(FacilityReq.facilityName);

            if (FacilityReq.tenor > tenorObtained)
            {
                ValidationErrors.Add(new
                         KeyValuePair<string, string>("", "Request Cannot be processed: Tenor greater than maximum tenor for Facility!"));
                IsValid = false;
                ret = false;
            }

            return ret;
        }

        protected Facility GetFacilityRulesData(string facilityname)
        {
            Facility ret = new Facility();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.Facilities.Where(x => x.facilityName == facilityname.Trim() && x.active == "Y").SingleOrDefault();

                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("StaffSalaryGrades",
                      ex.Message));
            }
            finally
            {
               
            }
            return ret;
        }

        protected Facility GetFacilityRulesData2(string grade)
        {
            Facility ret = new Facility();
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    ret = db.Facilities.Where(x => x.facilityName == grade.Trim() && x.active == "Y").SingleOrDefault();

                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("facilityRules",
                      ex.Message));
            }
            finally
            {
               
            }
            return ret;
        }

        protected bool validateStep1Items()
        {
            //bool ret = false;
            bool ret = true;
            string obtainedGrade = "";

            Entity2 = GetFacilityRulesData(FacilityReq.facilityName);

            //FacilityReq.dateOfEmployment

            obtainedGrade = confirmGrade(FacilityReq.salaryGrade, FacilityReq.facilityName);

            if(FacilityReq.facilityName == "Home Finance")
            {
                if(Entity2.expectedLengthOfService > noOfMonthsFromAssumptionDate)
                {
                    ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Ineligibility: Lower Length Of Service!"));

                    IsValid = false;
                    ret = false;
                }
            }

            if (!FacilityReq.facilityName.Equals("Home Finance"))
            {
                if(Entity2.expectedLengthOfService > FacilityReq.numberOfMonthsInService)
                {
                    ValidationErrors.Add(new
                    KeyValuePair<string, string>("Invalid",
                    "Ineligibility: Lower Length Of Service!"));

                    IsValid = false;
                    ret = false;
                }               
            }

            if (obtainedGrade == "NO")
            {

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Ineligibility: Grade Not Qualified!"));

                IsValid = false;
                ret = false;
            }

           // else
           if(obtainedGrade == "YES" && ret == true)
            {
                try
                {
                    var client = new RestClient(url + "/AccountByNuban");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("ContentType", "application/json");
                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(new { Nuban = FacilityReq.accountNumber.Trim() });
                    var s = client.Execute(request);

                    var Xxc = JsonConvert.DeserializeObject<dynamic>(s.Content);
                    var respon = JsonConvert.SerializeObject(Xxc);
                    var json = JsonConvert.DeserializeObject<Rootobject>(respon);

                    if (json.Status)
                    {

                        FacilityReq.accountName = json.Data.CustomerName;
                        FacilityReq.branch = json.Data.BranchName;
                        //FacilityReq.branchCode = json.Data.BranchCode;
                        FacilityReq.accountNumber = json.Data.AccountNo;
                        
                        var client2 = new RestClient(url + "/GetUserDetails");
                        var request2 = new RestRequest(Method.POST);
                        request2.AddHeader("ContentType", "application/json");
                        request2.RequestFormat = DataFormat.Json;
                        request2.AddJsonBody(new { UserName = FacilityReq.staffId.Trim() });
                        var s2 = client2.Execute(request2);
                        var Xxc2 = JsonConvert.DeserializeObject<dynamic>(s2.Content);
                        var respon2 = JsonConvert.SerializeObject(Xxc2);
                        var json2 = JsonConvert.DeserializeObject<Rootobject2>(respon2);

                        if (json.Status)
                        {
                            FacilityReq.department = json2.Data.department;
                            FacilityReq.staffSupervisor = json2.Data.supervisor_username;
                            FacilityReq.jobFunction = json2.Data.unit;
                            FacilityReq.branchCode = json2.Data.branch;
                        }

                        _jobLogger.Info("===========VaklidateStep1Items Method=================");
                        //This line will be reedited later//
                        string query = "select * FROM Employee_PaySlip where Emp_Id = (select MAX(Emp_Id) Emp_Id from tblEmployee where Emp_Payroll_No = '" + UserId + "') "
                            + " and EmployeePaySlipId = (select MAX(EmployeePaySlipId) FROM Employee_PaySlip "
                            + " where Emp_Id = (select MAX(Emp_Id) Emp_Id from tblEmployee where Emp_Payroll_No = '" + UserId + "')) ";


                        _jobLogger.Info("===========Fetching Emplotee Payslip Details from PayMaster================ " + query);
                        // string query = "select * from HRPayroll where GradeName = '" + salaryGrade + "'";
                        using (SqlConnection Connect = new SqlConnection(_HRMasterConnectionString()))
                        {
                            using (SqlCommand Cmd = new SqlCommand(query, Connect))
                            {
                                Connect.Open();

                                using (SqlDataReader reader = Cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {                                       
                                        FacilityReq.netMonthlySalary = Math.Round(Convert.ToDecimal(reader["NetPay"]), 2);
                                       // FacilityReq.grossAnnualSalary = Math.Round(Convert.ToDecimal(reader["GrossSalary"]) * 12, 2);                                      

                                        paye = Math.Round(Convert.ToDecimal(reader["TaxAmount"]), 2);
                                        nhf = Math.Round(Convert.ToDecimal(reader["NHFEmp"]), 2);
                                        pensionsContribution = Math.Round(Convert.ToDecimal(reader["PensionEmp"]), 2);
                                        totalMonthlyDeductions = Math.Round(Convert.ToDecimal(reader["TotalDeductions"]), 2);
                                    }

                                    Connect.Close();
                                }
                            }
                        }

                        netMSal = String.Format("{0:n}", FacilityReq.netMonthlySalary);
                        //-----------
                        FacilityReq.grossAnnualSalary = Math.Round(Convert.ToDecimal(fetchMyAnnualGrossPackage(gradeLevel, FacilityReq.branchCode)), 2);
                        grossASal = String.Format("{0:n}", FacilityReq.grossAnnualSalary);
                        //-----------------
                        //------------
                        FacilityReq.annualHousingAllowance = Math.Round(Convert.ToDecimal(fetchMyHousingAllowance(gradeLevel, FacilityReq.branchCode)), 2);
                        annHSal = String.Format("{0:n}", FacilityReq.annualHousingAllowance);
                        //-----------
                        payeTax = String.Format("{0:n}", paye);
                        housingFund = String.Format("{0:n}", nhf);
                        pensions = String.Format("{0:n}", pensionsContribution);
                        totalMC = String.Format("{0:n}", totalMonthlyDeductions);

                        IsValid = true;
                        ret = true;
                    }
                    else
                    {
                        string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

                        var log = new AllLogs();
                        log.writeAuditlog(FacilityReq.staffId, FacilityReq.accountName, "Internal Server Error: Unable to Fetch Details! ", ip);

                        string ErrorDescription = "Internal Server Error: Unable to Fetch Details!";
                        var mail = new AllLogs();
                        mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                        _jobLogger.Info("===========Fetching Employee Payslip Details from PayMaster Error ================ " + ErrorDescription);

                        ValidationErrors.Add(new
                          KeyValuePair<string, string>("Details Fetch",
                          "Internal Server Error: Unable to Fetch Details!"));
                        IsValid = false;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

                    var log = new AllLogs();
                    log.writeAuditlog(FacilityReq.staffId, FacilityReq.accountName, "Exception Error: " + ex.ToString(), ip);

                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                    _jobLogger.Info("===========Fetching Employee Payslip Details from PayMaster Exception error ================ " + ErrorDescription);

                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Nuban", ex.Message));
                    IsValid = false;
                    ret = false;
                }
            }

            return ret;
        }

        protected bool validateStep2Items()
        {           
            bool ret = false;           
            string natureEmployment = "";
            string natEmp = "";

            if (FacilityReq.natureOfEmployment == "Staff")
            {
                natEmp = "permanentStaff";
            }
            if (FacilityReq.natureOfEmployment == "Outsourced El-Jaiz" || FacilityReq.natureOfEmployment == "Outsourced" || FacilityReq.natureOfEmployment == "Contract Staff" || FacilityReq.natureOfEmployment == "Outsourced Caranda" || FacilityReq.natureOfEmployment == "Direct Sales Agent" || FacilityReq.natureOfEmployment == "Intern")
            {
                natEmp = "contractStaff";
            }

            Entity2 = GetFacilityRulesData(FacilityReq.facilityName);

            natureEmployment = confirmEmploymentNature(natEmp, FacilityReq.facilityName);

            if (natureEmployment == "NO")
            {

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Ineligibility: Employment Nature Not Qualified!"));

                IsValid = false;
                return false;
            }

            else
            {
                try
                {

                    FacilityReq.percentageRate = getPercentageRate(FacilityReq.facilityName);
                    MyFacilities = GetAllFacilityDetails(Convert.ToInt32(FacilityReq.cif));
                    totalLoansRepaymentMonthly = fetchAllLoansTotal(Convert.ToInt32(FacilityReq.cif));
                    totalLoansRepaymentMonthlyD = String.Format("{0:n}", totalLoansRepaymentMonthly);
                    IsValid = true;
                    ret = true;

                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Nuban", ex.Message));
                    IsValid = false;
                    return false;
                }
            }

            return ret;
        }

        public class FacilityDetailsDAO
        {
            public string cif { get; set; }
            public string staffname { get; set; }
            public string amountRequested { get; set; }
            public string maturityDate { get; set; }
            public string facilityType { get; set; }
            public string annualRepayment { get; set; }
            public string monthlyRepayment { get; set; }

        }

        public decimal fetchAllLoansTotal(int cif)
        {
            decimal totalrep = 0;
            string query = "";

            query = "select SUM(round((sum(p.capital_amt) + sum(NVL(p.profit_amt, 0) + NVL(p.profit_amt_new, 0))) / count(NVL(p.profit_amt, 0)), 2)) totalMonthlyRepayment  "                     
                      + "from "
                      + "(select D.cif_no, A.long_name_eng, C.code as Class,C.category  , C.long_name_eng as facility_type, Deal_amount, serial_no, D.branch_code, D.MATURITY_DATE, "
                      + "PP.COMP_CODE, PP.BRANCH, "
                      + "PP.PLAN_NBR, PP.PLAN_SEQ "
                      + "from imal.trsdeal D , imal.trsclass C, imal.cif  A, "
                      + "(select * from imal.trspayplan  where (COMP_CODE, BRANCH, PLAN_NBR, PLAN_SEQ)  in (select comp_code, branch, plan_nbr, max(plan_seq) as plan_seq "
                      + "from imal.trspayplan where status = 'P' "
                      + "group by comp_code, branch, plan_nbr)) PP where D.COMP_CODE = PP.COMP_CODE(+) AND D.BRANCH_CODE = PP.BRANCH(+) AND D.SERIAL_NO = PP.TRX_NBR(+)  "
                      + "AND D.STATUS = 'P' AND PP.STATUS = 'P'  and D.cif_no = A.cif_no  and D.class = C.code and C.CODE != 363 and D.cif_no = " + cif + " "
                      + "and D.status = 'P') A, imal.trspayplandet P, imal.TRSdet T  "
                      + "where P.COMP_CODE = A.COMP_CODE AND P.BRANCH = A.BRANCH AND P.PLAN_NBR = A.PLAN_NBR AND P.PLAN_SEQ = A.PLAN_SEQ  "
                      + "and P.payment_type <> 'C' "
                      + "and get_Facility_Balance ( A.plan_nbr ,A.branch,A.plan_seq, A.category,A.Class) > 0 "
                      + "and to_char(P.value_date,'yyyy') = to_char(sysdate, 'yyyy') and MATURITY_DATE > sysdate  "
                      + "and T.line_no = (select max(line_no) from imal.TRSdet where branch_code = T.branch_code and serial_no = T.serial_no) "
                      + "AND A.BRANCH_CODE = T.BRANCH_CODE AND A.SERIAL_NO = T.SERIAL_NO and T.MATR_AC_GL <> 210111 "
                      + "group by cif_no,long_name_eng,A.Deal_amount,MATURITY_DATE,facility_type  "
                      + "order by long_name_eng desc ";

            try
            {
                using (OracleConnection connection = new OracleConnection(ImalConn_))
                {

                    OracleCommand command = new OracleCommand(query, connection);
                    connection.Open();
                    OracleDataReader reader;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        totalrep = Convert.ToDecimal(reader["TOTALMONTHLYREPAYMENT"]);

                    }

                    reader.Close();
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                //put error log here
            }
            finally
            {
                //db.Configuration.Close();
            }

            return totalrep;
        }

        public List<FacilityDetailsDAO> GetAllFacilityDetails(int cif)
        {
            List<FacilityDetailsDAO> ret = new List<FacilityDetailsDAO>();
            FacilityDetailsDAO mert = null;
            string query = "";
            int count = 0;
            _jobLogger.Info("===========GetAllFacilityDetails Method ================ ");

            query = "select A.cif_no,long_name_eng,A.Deal_amount,MATURITY_DATE ,facility_type, sum(p.capital_amt) + sum(NVL(p.profit_amt, 0) + NVL(p.profit_amt_new, 0)) as Annual_payment, "
                     + "round((sum(p.capital_amt) + sum(NVL(p.profit_amt, 0) + NVL(p.profit_amt_new, 0))) / count(NVL(p.profit_amt, 0)), 2) monthly_payment "
                     + "from "
                     + "(select D.cif_no, A.long_name_eng, C.code as Class,C.category  , C.long_name_eng as facility_type, Deal_amount, serial_no, D.branch_code, D.MATURITY_DATE, "
                     + "PP.COMP_CODE, PP.BRANCH, "
                     + "PP.PLAN_NBR, PP.PLAN_SEQ "
                     + "from imal.trsdeal D , imal.trsclass C, imal.cif  A, "
                     + "(select * from imal.trspayplan  where (COMP_CODE, BRANCH, PLAN_NBR, PLAN_SEQ)  in (select comp_code, branch, plan_nbr, max(plan_seq) as plan_seq "
                     + "from imal.trspayplan where status = 'P' "
                     + "group by comp_code, branch, plan_nbr)) PP where D.COMP_CODE = PP.COMP_CODE(+) AND D.BRANCH_CODE = PP.BRANCH(+) AND D.SERIAL_NO = PP.TRX_NBR(+)  "
                     + "AND D.STATUS = 'P' AND PP.STATUS = 'P'  and D.cif_no = A.cif_no  and D.class = C.code and C.CODE != 363 and D.cif_no = " + cif + " "
                     + "and D.status = 'P') A, imal.trspayplandet P, imal.TRSdet T  "
                     + "where P.COMP_CODE = A.COMP_CODE AND P.BRANCH = A.BRANCH AND P.PLAN_NBR = A.PLAN_NBR AND P.PLAN_SEQ = A.PLAN_SEQ  " 
                     + "and P.payment_type <> 'C' "
                     + "and get_Facility_Balance ( A.plan_nbr ,A.branch,A.plan_seq, A.category,A.Class) > 0 "
                     + "and to_char(P.value_date,'yyyy') = to_char(sysdate, 'yyyy') and MATURITY_DATE > sysdate  "
                     + "and T.line_no = (select max(line_no) from imal.TRSdet where branch_code = T.branch_code and serial_no = T.serial_no) "
                     + "AND A.BRANCH_CODE = T.BRANCH_CODE AND A.SERIAL_NO = T.SERIAL_NO and T.MATR_AC_GL <> 210111 "
                     + "group by cif_no,long_name_eng,A.Deal_amount,MATURITY_DATE,facility_type  "
                     + "order by long_name_eng desc ";

            try
            {
                _jobLogger.Info("===========GetAllFacilityDetails Query ================ " +query);
                using (OracleConnection connection = new OracleConnection(ImalConn_))
                {

                    OracleCommand command = new OracleCommand(query, connection);
                    connection.Open();
                    OracleDataReader reader;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        mert = new FacilityDetailsDAO();
                        count++;

                        decimal amnt = Convert.ToDecimal(reader["DEAL_AMOUNT"]);
                        decimal annualP = Convert.ToDecimal(reader["ANNUAL_PAYMENT"]);
                        decimal monthlyP = Convert.ToDecimal(reader["MONTHLY_PAYMENT"]);
                        DateTime matDate = Convert.ToDateTime(reader["MATURITY_DATE"]);

                        mert.amountRequested = String.Format("{0:n}", amnt);
                        mert.maturityDate = String.Format("{0:MM/dd/yyyy}", matDate);
                        mert.facilityType = reader["FACILITY_TYPE"].ToString();
                        mert.annualRepayment = String.Format("{0:n}", annualP);
                        mert.monthlyRepayment = String.Format("{0:n}", monthlyP);

                        string facDetails = JsonConvert.SerializeObject(mert);

                        _jobLogger.Info("===========GetAllFacilityDetails Query Results================ " + facDetails);

                        ret.Add(mert);
                          
                        showLoanHistory = true;
                    }                    

                    reader.Close();
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                _jobLogger.Error("===========GetAllFacilityDetails Query Error================ " + ErrorDescription);

                //put error log here
            }
            finally
            {
                //db.Configuration.Close();
            }

            return ret;
        }


        protected bool confirmFieldItems()
        {
            bool ret = true;
            bool check = false;
            string aboLimit = "";
            string boLimit = "";
            string sboLimit = "";

            aboLimit = ConfigurationManager.AppSettings["ABOLimit"];
            boLimit = ConfigurationManager.AppSettings["BOLimit"];
            sboLimit = ConfigurationManager.AppSettings["SBOLimit"];

            FacilityReq.downPaymentContribution = Convert.ToDecimal(downPaymentContribution);
            FacilityReq.amountRequested = Convert.ToDecimal(amountRequested);

            check = checkDownPayment(FacilityReq.facilityName);

            if (check == true)
            {
                if (string.IsNullOrEmpty(Convert.ToString(FacilityReq.downPaymentContribution)))
                {

                    ValidationErrors.Add(new
                          KeyValuePair<string, string>("Invalid",
                          "Down Payment Cannot be Empty/Null For This Facility!"));

                    IsValid = false;
                    ret = false;
                }


            }

            if (string.IsNullOrEmpty(Convert.ToString(FacilityReq.tenor)) || FacilityReq.tenor == 0 || FacilityReq.tenor < 0)
            {

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Invalid: Tenor Cannot be Null/Zero/Negative!"));

                IsValid = false;
                ret = false;
            }

            if (string.IsNullOrEmpty(Convert.ToString(FacilityReq.amountRequested)) || FacilityReq.amountRequested == 0 || FacilityReq.amountRequested < 0)
            {

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                       "Invalid: Amount Cannot be Null/Zero/Negative!"));

                IsValid = false;
                ret = false;
            }

            if (string.IsNullOrEmpty(FacilityReq.loanPurpose))
            {

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Loan Purpose Cannot be Null or Empty!"));

                IsValid = false;
                ret = false;
            }

            if (FacilityReq.facilityName == "Home Finance" || FacilityReq.facilityName.Contains("Home"))
            {
                if(FacilityReq.salaryGrade == "Assistant Banking Officer")
                {
                    if (FacilityReq.amountRequested > Convert.ToDecimal(aboLimit))
                    {
                        ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Requested Amount Cannot Be Accessed For Your Grade!"));

                        IsValid = false;
                        ret = false;
                    }
                }

                if (FacilityReq.salaryGrade == "Banking Officer")
                {
                    if (FacilityReq.amountRequested > Convert.ToDecimal(boLimit))
                    {
                        ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Requested Amount Cannot Be Accessed For Your Grade!"));

                        IsValid = false;
                        ret = false;
                    }
                }

                if (FacilityReq.salaryGrade == "Senior Banking Officer")
                {
                    if (FacilityReq.amountRequested > Convert.ToDecimal(sboLimit))
                    {
                        ValidationErrors.Add(new
                      KeyValuePair<string, string>("Invalid",
                      "Requested Amount Cannot Be Accessed For Your Grade!"));

                        IsValid = false;
                        ret = false;
                    }
                }
            }

            return ret;
        }

       
        public bool checkDownPayment(string facilityName)
        {
            string resp = "";
            bool ret = false;

            string query = "select downPaymentRequired from Facility where facilityName = '" + facilityName + "'";
            try
            {
                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {
                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resp = reader["downPaymentRequired"].ToString();
                            }

                            Connect.Close();
                        }
                    }

                    if (resp == "YES")
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);

                ValidationErrors.Add(new
                      KeyValuePair<string, string>("HRViability",
                      ex.Message));

                //put error log here
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
                Insert(FacilityReq, Entity3);
            }
            else
            {
                Update(FacilityReq, Entity3);
            }
            // Set any validation errors
            ValidationErrors = ValidationErrors;
            // Set mode based on validation errors
            base.Save();
        }
        public bool Update(HRFacilityMaster entity, Facility entity3)
        {
            bool ret = false;
           
                using (var db = new HRViabilityPortalEntities())
                {
                    db.Entry(entity).State = EntityState.Modified;
                    //db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
           
            return ret;
        }        

        public bool Insert(HRFacilityMaster entity, Facility entity3)
        {
            bool ret = false;
            string otherFacilityValues = "";
            _jobLogger.Info("===========Entered The Insert/Submit Method================ ");
            ret = ValidateDocumentExtensionAndSize();

            if (ret)
            {
                using (var db = new HRViabilityPortalEntities())
                {

                    HRFacilityMaster newreq = new HRFacilityMaster();

                    string refNo = "HRREF" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    try
                    {

                        newreq.requestReferenceNumber = refNo;
                        newreq.facilityName = entity.facilityName;
                        newreq.salaryGrade = entity.salaryGrade;
                        newreq.accountNumber = entity.accountNumber;
                        newreq.cif = entity.cif;
                        newreq.accountName = entity.accountName;
                        newreq.staffId = entity.staffId;
                        newreq.numberOfMonthsInService = entity.numberOfMonthsInService;
                        newreq.jobFunction = entity.jobFunction;
                        newreq.branch = entity.branch;
                        //changes made here on 19/08/2020-----
                        if(entity.branchCode == "1")
                        {
                            newreq.branchCode = "2";
                        }
                        else
                        {
                            newreq.branchCode = entity.branchCode;
                        }                       
                        //newreq.branchCode = "2";
                        //-------------------
                        newreq.department = entity.department;
                        newreq.staffSupervisor = entity.staffSupervisor + "@jaizbankplc.com";

                        newreq.mobileNumber = entity.mobileNumber;
                        newreq.maritalStatus = entity.maritalStatus;
                        newreq.state = entity.state;
                        newreq.dateOfBirth = entity.dateOfBirth;
                        newreq.dateOfEmployment = entity.dateOfEmployment;

                        newreq.natureOfEmployment = entity.natureOfEmployment;
                        newreq.grossAnnualSalary = entity.grossAnnualSalary;
                        newreq.netMonthlySalary = entity.netMonthlySalary;

                        newreq.jobFunction = entity.jobFunction;
                        newreq.amountRequested = entity.amountRequested;

                        newreq.downPaymentContribution = entity.downPaymentContribution;
                        newreq.tenor = entity.tenor;
                        newreq.percentageRate = entity.percentageRate;
                        newreq.loanPurpose = entity.loanPurpose;
                        newreq.repaymentAmount = entity.repaymentAmount;

                        newreq.assetDescription = entity.assetDescription;
                        newreq.vendorsName = entity.vendorsName;
                        newreq.vendorsAddress = entity.vendorsAddress;
                        newreq.vendorsPhoneNumber = entity.vendorsPhoneNumber;
                        newreq.locationOfAsset = entity.locationOfAsset;
                        newreq.vehicleModel = entity.vehicleModel;
                        newreq.yearOfManufacture = entity.yearOfManufacture;
                        newreq.modeOfDelivery = entity.modeOfDelivery;
                        newreq.quotationEnclosed = entity.quotationEnclosed;

                        newreq.typeOfProperty = entity.typeOfProperty;
                        newreq.propertyDescription = entity.propertyDescription;
                        newreq.titleDocumentType = entity.titleDocumentType;
                        newreq.nameOfDeveloper = entity.nameOfDeveloper;
                        newreq.addressOfDeveloper = entity.addressOfDeveloper;
                        newreq.phoneNumberOfDeveloper = entity.phoneNumberOfDeveloper;
                        newreq.locationOfProperty = entity.locationOfProperty;
                        newreq.annualHousingAllowance = entity.annualHousingAllowance;

                        newreq.serviceDescription = entity.serviceDescription;
                        newreq.serviceProviderName = entity.serviceProviderName;
                        newreq.serviceProviderAddress = entity.serviceProviderAddress;
                        newreq.servicePhoneNumber = entity.servicePhoneNumber;

                        newreq.hasExistingMortgageLoan = entity.hasExistingMortgageLoan;
                        newreq.hasExistingVehicleLoan = entity.hasExistingVehicleLoan;

                        newreq.requestDate = DateTime.Now;
                        newreq.requestStatus = "Awaiting_HR_Review";

                        newreq.initiator = entity.staffId + "@jaizbankplc.com";
                        newreq.staffEmailAddress = entity.staffId + "@jaizbankplc.com";
                        newreq.initiatorTimestamp = DateTime.Now;

                        // Assign FileName to db field
                        if (!String.IsNullOrEmpty(entity.fileName1))
                        {
                            newreq.fileName1 = entity.staffId + "_" + entity.fileName1;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName2))
                        {
                            newreq.fileName2 = entity.staffId + "_" + entity.fileName2;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName3))
                        {
                            newreq.fileName3 = entity.staffId + "_" + entity.fileName3;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName4))
                        {
                            newreq.fileName4 = entity.staffId + "_" + entity.fileName4;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName5))
                        {
                            newreq.fileName5 = entity.staffId + "_" + entity.fileName5;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName6))
                        {
                            newreq.fileName6 = entity.staffId + "_" + entity.fileName6;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName7))
                        {
                            newreq.fileName7 = entity.staffId + "_" + entity.fileName7;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName8))
                        {
                            newreq.fileName8 = entity.staffId + "_" + entity.fileName8;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName9))
                        {
                            newreq.fileName9 = entity.staffId + "_" + entity.fileName9;
                        }
                        if (!String.IsNullOrEmpty(entity.fileName10))
                        {
                            newreq.fileName10 = entity.staffId + "_" + entity.fileName10;
                        }
                       
                        newreq.fileContentType1 = entity.fileContentType1;
                        newreq.fileContentType2 = entity.fileContentType2;
                        newreq.fileContentType3 = entity.fileContentType3;
                        newreq.fileContentType4 = entity.fileContentType4;
                        newreq.fileContentType5 = entity.fileContentType5;
                        newreq.fileContentType6 = entity.fileContentType6;
                        newreq.fileContentType7 = entity.fileContentType7;
                        newreq.fileContentType8 = entity.fileContentType8;
                        newreq.fileContentType9 = entity.fileContentType9;
                        newreq.fileContentType10 = entity.fileContentType10;

                        string path1 = "";
                        string path2 = "";
                        string path3 = "";
                        string path4 = "";
                        string path5 = "";
                        string path6 = "";
                        string path7 = "";
                        string path8 = "";
                        string path9 = "";
                        string path10 = "";

                        if (!String.IsNullOrEmpty(entity.fileName1))
                        {
                            
                             path1 = "~//upload//" + entity.staffId + "_" + entity.fileName1;
                        }
                        else
                        {
                             path1 = "~//upload//" + entity.fileName1;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName2))
                        {

                             path2 = "~//upload//" + entity.staffId + "_" + entity.fileName2;
                        }
                        else
                        {
                             path2 = "~//upload//" + entity.fileName2;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName3))
                        {

                            path3 = "~//upload//" + entity.staffId + "_" + entity.fileName3;
                        }
                        else
                        {
                            path3 = "~//upload//" + entity.fileName3;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName4))
                        {

                            path4 = "~//upload//" + entity.staffId + "_" + entity.fileName4;
                        }
                        else
                        {
                            path4 = "~//upload//" + entity.fileName4;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName5))
                        {

                            path5 = "~//upload//" + entity.staffId + "_" + entity.fileName5;
                        }
                        else
                        {
                            path5 = "~//upload//" + entity.fileName5;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName6))
                        {

                            path6 = "~//upload//" + entity.staffId + "_" + entity.fileName6;
                        }
                        else
                        {
                            path6 = "~//upload//" + entity.fileName6;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName7))
                        {

                            path7 = "~//upload//" + entity.staffId + "_" + entity.fileName7;
                        }
                        else
                        {
                            path7 = "~//upload//" + entity.fileName7;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName8))
                        {

                            path8 = "~//upload//" + entity.staffId + "_" + entity.fileName8;
                        }
                        else
                        {
                            path8 = "~//upload//" + entity.fileName8;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName9))
                        {

                            path9 = "~//upload//" + entity.staffId + "_" + entity.fileName9;
                        }
                        else
                        {
                            path9 = "~//upload//" + entity.fileName9;
                        }

                        if (!String.IsNullOrEmpty(entity.fileName10))
                        {

                            path10 = "~//upload//" + entity.staffId + "_" + entity.fileName10;
                        }
                        else
                        {
                            path10 = "~//upload//" + entity.fileName10;
                        }                                             

                        newreq.fileData1 = path1;
                        newreq.fileData2 = path2;
                        newreq.fileData3 = path3;
                        newreq.fileData4 = path4;
                        newreq.fileData5 = path5;
                        newreq.fileData6 = path6;
                        newreq.fileData7 = path7;
                        newreq.fileData8 = path8;
                        newreq.fileData9 = path9;
                        newreq.fileData10 = path10;

                        newreq.customField1 = entity.customField1;
                        newreq.customField2 = entity.customField2;
                        newreq.customField3 = entity.customField3;
                        newreq.customField4 = entity.customField4;
                        newreq.customField5 = entity.customField5;
                        newreq.customField6 = entity.customField6;
                        newreq.customField7 = entity.customField7;
                        newreq.customField8 = entity.customField8;
                        newreq.customField9 = entity.customField9;
                        newreq.customField10 = entity.customField10;

                        newreq.customField1Data = entity.customField1Data;
                        newreq.customField2Data = entity.customField2Data;
                        newreq.customField3Data = entity.customField3Data;
                        newreq.customField4Data = entity.customField4Data;
                        newreq.customField5Data = entity.customField5Data;
                        newreq.customField6Data = entity.customField6Data;
                        newreq.customField7Data = entity.customField7Data;
                        newreq.customField8Data = entity.customField8Data;
                        newreq.customField9Data = entity.customField9Data;
                        newreq.customField10Data = entity.customField10Data;
                        newreq.documentEmailStatus = "N";

                        //generating various documents and send via mail//
                        var mail = new EmailSender();

                        if (entity.facilityName == "Murabaha")
                        {
                            assetDesM = entity.assetDescription;
                            assetDesB = "";
                        }
                        else if (entity.facilityName == "Bai Muajjal")
                        {
                            assetDesB = entity.assetDescription;
                            assetDesM = "";
                        }
                        else
                        {
                            assetDesM = "";
                            assetDesB = "";
                        }

                        if (entity.facilityName == "Murabaha")
                        {
                            deliveryModeM = entity.modeOfDelivery;
                            deliveryModeI = "";
                        }
                        else if (entity.facilityName == "Ijara Service")
                        {
                            deliveryModeI = entity.modeOfDelivery;
                            deliveryModeM = "";
                        }
                        else
                        {
                            deliveryModeM = "";
                            deliveryModeI = "";
                        }

                        if (entity.facilityName == "Murabaha")
                        {
                            invoiceM = entity.quotationEnclosed;
                            invoiceI = "";
                        }
                        else if (entity.facilityName == "Ijara Service")
                        {
                            invoiceI = entity.quotationEnclosed;
                            invoiceM = "";
                        }
                        else
                        {
                            invoiceM = "";
                            invoiceI = "";
                        }

                        if (string.IsNullOrEmpty(entity.vendorsName) && string.IsNullOrEmpty(entity.vendorsAddress) && string.IsNullOrEmpty(entity.vendorsPhoneNumber) && string.IsNullOrEmpty(entity.locationOfAsset))
                        {
                            entity.vendorsName = "";
                            entity.vendorsAddress = "";
                            entity.vendorsPhoneNumber = "";
                            entity.locationOfAsset = "";

                        }

                        if (string.IsNullOrEmpty(entity.vehicleModel) && string.IsNullOrEmpty(entity.yearOfManufacture))
                        {
                            entity.vehicleModel = "";
                            entity.yearOfManufacture = "";
                        }


                        if (string.IsNullOrEmpty(entity.typeOfProperty) && string.IsNullOrEmpty(entity.propertyDescription) && string.IsNullOrEmpty(entity.titleDocumentType) && string.IsNullOrEmpty(entity.locationOfProperty) && string.IsNullOrEmpty(entity.nameOfDeveloper) && string.IsNullOrEmpty(entity.addressOfDeveloper) || string.IsNullOrEmpty(entity.phoneNumberOfDeveloper))
                        {
                            entity.typeOfProperty = "";
                            entity.propertyDescription = "";
                            entity.titleDocumentType = "";
                            entity.locationOfProperty = "";
                            entity.nameOfDeveloper = "";
                            entity.addressOfDeveloper = "";
                            entity.phoneNumberOfDeveloper = "";
                        }

                        if (string.IsNullOrEmpty(entity.serviceDescription) && string.IsNullOrEmpty(entity.serviceProviderName) && string.IsNullOrEmpty(entity.serviceProviderAddress) && string.IsNullOrEmpty(entity.servicePhoneNumber))
                        {
                            entity.serviceDescription = "";
                            entity.serviceProviderName = "";
                            entity.serviceProviderAddress = "";
                            entity.servicePhoneNumber = "";
                        }

                        if (entity.facilityName != "Murabaha" && entity.facilityName != "Home Finance" && entity.facilityName != "Ijara Service" && entity.facilityName != "Bai Muajjal" && (!entity.facilityName.Contains("Branch")) && (!entity.facilityName.Contains("branch")))
                        {
                            otherFacilityValues = "Financing-  " + entity.facilityName;
                        }
                        else
                        {
                            otherFacilityValues = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField1Data))
                        {
                            customField1 = entity.customField1;
                            customFieldData1 = entity.customField1Data;
                        }
                        else
                        {
                            customField1 = "";
                            customFieldData1 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField2Data))
                        {
                            customField2 = entity.customField2;
                            customFieldData2 = entity.customField2Data;
                        }
                        else
                        {
                            customField2 = "";
                            customFieldData2 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField3Data))
                        {
                            customField3 = entity.customField3;
                            customFieldData3 = entity.customField3Data;
                        }
                        else
                        {
                            customField3 = "";
                            customFieldData3 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField4Data))
                        {
                            customField4 = entity.customField4;
                            customFieldData4 = entity.customField4Data;
                        }
                        else
                        {
                            customField4 = "";
                            customFieldData4 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField5Data))
                        {
                            customField5 = entity.customField5;
                            customFieldData5 = entity.customField5Data;
                        }
                        else
                        {
                            customField5 = "";
                            customFieldData5 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField6Data))
                        {
                            customField6 = entity.customField6;
                            customFieldData6 = entity.customField6Data;
                        }
                        else
                        {
                            customField6 = "";
                            customFieldData6 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField7Data))
                        {
                            customField7 = entity.customField7;
                            customFieldData7 = entity.customField7Data;
                        }
                        else
                        {
                            customField7 = "";
                            customFieldData7 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField8Data))
                        {
                            customField8 = entity.customField8;
                            customFieldData8 = entity.customField8Data;
                        }
                        else
                        {
                            customField8 = "";
                            customFieldData8 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField9Data))
                        {
                            customField9 = entity.customField9;
                            customFieldData9 = entity.customField9Data;
                        }
                        else
                        {
                            customField9 = "";
                            customFieldData9 = "";
                        }

                        if (!string.IsNullOrEmpty(entity.customField10Data))
                        {
                            customField10 = entity.customField10;
                            customFieldData10 = entity.customField10Data;
                        }
                        else
                        {
                            customField10 = "";
                            customFieldData10 = "";
                        }

                        String amntRequested = String.Format("{0:n}", entity.amountRequested);
                        String downPayment = String.Format("{0:n}", entity.downPaymentContribution);
                        String grosspay = String.Format("{0:n}", entity.grossAnnualSalary);
                        String netpay = String.Format("{0:n}", entity.netMonthlySalary);

                        //mail.DocumentGeneratorHelper(entity.staffId, entity.mobileNumber, entity.accountNumber, entity.accountName, entity.natureOfEmployment, entity.department, netpay, grosspay, entity.facilityName, entity.dateOfEmployment, entity.dateOfBirth, entity.jobFunction, amntRequested, downPayment, entity.maritalStatus, Convert.ToInt32(entity.numberOfMonthsInService), entity.tenor, entity.loanPurpose);

                        //mail.ApplicationFormGeneratorHelper(assetDesM, assetDesB, entity.vendorsName, entity.vendorsAddress, entity.vendorsPhoneNumber, entity.locationOfAsset, entity.vehicleModel, entity.yearOfManufacture, deliveryModeM, deliveryModeI, entity.typeOfProperty, entity.propertyDescription, entity.titleDocumentType, entity.locationOfProperty, entity.nameOfDeveloper, entity.addressOfDeveloper, entity.phoneNumberOfDeveloper, entity.serviceDescription,
                        //entity.serviceProviderName, entity.serviceProviderAddress, entity.servicePhoneNumber, invoiceM, invoiceI, entity.accountNumber, entity.accountName, entity.staffId, entity.mobileNumber,  entity.natureOfEmployment, entity.department, netpay, grosspay, entity.facilityName, entity.dateOfEmployment, entity.dateOfBirth, entity.jobFunction, amntRequested, downPayment,
                        //entity.maritalStatus, Convert.ToInt32(entity.numberOfMonthsInService), entity.tenor, entity.loanPurpose, customField1, customField2, customField3, customField4, customField5, customField6, customField7, customField8, customField9, customField10, customFieldData1, customFieldData2, customFieldData3, customFieldData4, customFieldData5,
                        //customFieldData6, customFieldData7, customFieldData8, customFieldData9, customFieldData10, otherFacilityValues);

                        ReqHistory history = new ReqHistory();
                        history.ActionDateTime = DateTime.Now;
                        history.Initiator = UserId;
                        history.ActionPerformed = "New Facility Request Submission";
                        history.ReqId = refNo;

                        db.ReqHistories.Add(history);

                        string submission = JsonConvert.SerializeObject(newreq);
                        _jobLogger.Info("===========The Insert/Submit Method Values To Submit Into DB================  " + submission);

                        db.HRFacilityMasters.Add(newreq);

                        db.SaveChanges();

                        string op = "New Request with Reference Number: " + refNo + " Submitted Successfully!";
                        _jobLogger.Info("===========The Insert/Submit Submission Successful==================");
                        //send mail
                        sendmail(refNo);
                        sendmailInitiator(refNo);
                        //SendEmailWithAllDocuments(entity.accountName, entity.staffId);

                        Msg = op;

                        string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                        var log = new AllLogs();
                        log.writeAuditlog(newreq.staffId, newreq.accountName, "New Facility Request Submitted Successfully By: " + newreq.staffId, ip);

                    }
                    catch (Exception ex)
                    {
                        Msg = "Submission Exception:  " + ex.ToString();

                        string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                        var mail = new AllLogs();
                        
                        mail.ErrorLog(ErrorDescription, FacilityReq.staffId + " ||New Request Submission Error|| NewRequestViewModel.cs|| Line 1910");

                        string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

                        _jobLogger.Info("=========== Submission Unsuccessful================== " + ErrorDescription);

                        var log = new AllLogs();
                        log.writeAuditlog(newreq.staffId, newreq.accountName, "New Facility Request Submission Exception Error By: " + newreq.staffId + "  on  " + DateTime.Now, ip);
                    }
                    finally
                    {

                    }
                }
            }            
                
            //}
            return ret;
        }

        public int SendEmailWithAllDocuments(string acctName, string staffId)
        {
            int res = 0;
            string mailSubject = "Facility Documents For " + acctName;
            string mailBody = "Hello, Kindly find attached Facility Document For " + acctName;
            //Path.GetFullPath(@"\\172.13.21.160\\HRViabilityGeneratedDocuments\\" + referenceNo + ".pdf");
            string docPath1 = Path.GetFullPath(@"C:\\Users\BA07190\Documents\HRViabilityDocuments\GeneratedDocuments\UndertakingAuthorization_" + staffId + ".pdf");
            string docPath2 = Path.GetFullPath(@"C:\\Users\BA07190\Documents\HRViabilityDocuments\GeneratedDocuments\EmployeeUndertaking_" + staffId + ".pdf");
            string docPath3 = Path.GetFullPath(@"C:\\Users\BA07190\Documents\HRViabilityDocuments\GeneratedDocuments\EmployerUndertaking_" + staffId + ".pdf");
            string docPath4 = Path.GetFullPath(@"C:\\Users\BA07190\Documents\HRViabilityDocuments\GeneratedDocuments\RetailFacilityApplication_" + staffId + ".pdf");

            //string docPath1 = Path.GetFullPath(@"\\172.13.21.160\\HRViabilityGeneratedDocuments\\UndertakingAuthorization_" + staffId + ".pdf");
            //string docPath2 = Path.GetFullPath(@"\\172.13.21.160\\HRViabilityGeneratedDocuments\\EmployeeUndertaking_" + staffId + ".pdf");
            //string docPath3 = Path.GetFullPath(@"\\172.13.21.160\\HRViabilityGeneratedDocuments\\EmployerUndertaking_" + staffId + ".pdf");
            //string docPath4 = Path.GetFullPath(@"\\172.13.21.160\\HRViabilityGeneratedDocuments\\RetailFacilityApplication" + staffId + ".pdf");

            ////string docPath1 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\UndertakingAuthorization.pdf";
            //string docPath11 = new Uri(docPath1).LocalPath;

            ////string docPath2 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\EmployeeUndertaking.pdf";
            //string docPath22 = new Uri(docPath2).LocalPath;

            ////string docPath3 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\EmployerUndertaking.pdf";
            //string docPath33 = new Uri(docPath3).LocalPath;

            ////string docPath4 = "file:\\C:\\Users\\BA07190\\Documents\\My Projects\\My .NET Projects\\HRViabilityPortal\\CompletedDocuments\\RetailFacilityApplication.pdf";
            //string docPath44 = new Uri(docPath4).LocalPath;


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://1jr2x.api.infobip.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", getAuthorizationHeaderString());

            var request = new MultipartFormDataContent();
            request.Add(new StreamContent(File.OpenRead(docPath1)), "attachment", new FileInfo(docPath1).Name);
            request.Add(new StreamContent(File.OpenRead(docPath2)), "attachment", new FileInfo(docPath2).Name);
            request.Add(new StreamContent(File.OpenRead(docPath3)), "attachment", new FileInfo(docPath3).Name);
            request.Add(new StreamContent(File.OpenRead(docPath4)), "attachment", new FileInfo(docPath4).Name);
            request.Add(new StringContent("Jaiz Bank Plc <notification@alerts.jaizbankplc.com>"), "from");
            request.Add(new StringContent(FacilityReq.staffId+"@jaizbankplc.com"), "to");          
            request.Add(new StringContent(mailSubject), "subject");
            request.Add(new StringContent(mailBody), "html");          
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
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
            finally
            {

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
      
        public class Data
        {
            public string BranchAddress { get; set; }
            public string BranchName { get; set; }
            public string BranchCode { get; set; }
            public object AccountID { get; set; }
            public object AccountNo { get; set; }
            public object AccountStatus { get; set; }
            public object AccountType { get; set; }
            public object BVN { get; set; }
            public object Currency { get; set; }
            public object CustomerAccountBalance { get; set; }
            public object CustomerName { get; set; }
            public object CustomerType { get; set; }
            public object Email { get; set; }
            public object GlCode { get; set; }
            public object MaturityDate { get; set; }
            public object PhoneNo { get; set; }
            public object RelationShipOfficer { get; set; }
            public object RelationShipOfficerID { get; set; }
            public object ResponseCode { get; set; }
        }
        public class Rootobject
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public Data Data { get; set; }
        }
        public class Rootobject2
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public UsersDetails Data { get; set; }
        }

        private static readonly Random GetSmallRandom = new Random();

        public string GetRefNo()
        {
            return GetSmallRandom.Next(100000, 999999).ToString("000000");

        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }
        public string _HRMasterConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRMasterConn"].ToString();
        }

        public string getNextSequencevalue()
        {
            string nextSeq = "";

            string query = "select HRVIABILITYPORTALSEQ.nextval as nextvalue from dual";
            try
            {
                using (OracleConnection connection = new OracleConnection(ImalConn_))
                {
                    OracleCommand command = new OracleCommand(query, connection);
                    connection.Open();
                    OracleDataReader reader;
                    reader = command.ExecuteReader();

                    // Always call Read before accessing data.
                    while (reader.Read())
                    {
                        nextSeq = reader["nextvalue"].ToString();
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {               
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
            finally
            {

            }

            return nextSeq;
        }

        public string confirmGrade(string grade, string facilityname)
        {
            string realGrade = "";
            string query = "";
            string resp = "";

            if (facilityname == "Murabaha")
            {
                query = "select distinct(gradeName) from MurabahaGradesTable where active = 'Y' and gradeName = '" + grade.Trim() + "'";
                try
                {
                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {
                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            Connect.Open();

                            using (SqlDataReader reader = Cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    realGrade = reader["gradeName"].ToString();
                                }

                                Connect.Close();
                            }


                        }
                    }

                    if (string.IsNullOrEmpty(realGrade))
                    {
                        resp = "NO";
                    }
                    else
                    {
                        resp = "YES";
                    }
                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }
                finally
                {

                }
            }
            else if (facilityname == "Home Finance")
            {
                query = "select distinct(gradeName) from HomeFinanceGradesTable where active = 'Y' and gradeName = '" + grade.Trim() + "'";
                try
                {
                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {
                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            Connect.Open();

                            using (SqlDataReader reader = Cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    realGrade = reader["gradeName"].ToString();
                                }

                                Connect.Close();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(realGrade))
                    {
                        resp = "NO";
                    }
                    else
                    {
                        resp = "YES";
                    }
                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }
                finally
                {

                }
            }
            else if (facilityname == "Ijara Service")
            {
                query = "select distinct(gradeName) from IjaraServiceGradesTable where active = 'Y' and gradeName = '" + grade.Trim() + "'";

                try
                {
                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {
                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            Connect.Open();

                            using (SqlDataReader reader = Cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    realGrade = reader["gradeName"].ToString();
                                }

                                Connect.Close();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(realGrade))
                    {
                        resp = "NO";
                    }
                    else
                    {
                        resp = "YES";
                    }
                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }
                finally
                {

                }
            }
            else if (facilityname == "Bai Muajjal")
            {
                query = "select distinct(gradeName) from BaiMuajjalGradesTable where active = 'Y' and gradeName = '" + grade.Trim() + "'";
                try
                {
                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {
                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            Connect.Open();

                            using (SqlDataReader reader = Cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    realGrade = reader["gradeName"].ToString();
                                }

                                Connect.Close();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(realGrade))
                    {
                        resp = "NO";
                    }
                    else
                    {
                        resp = "YES";
                    }
                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }
                finally
                {

                }
            }
            else if (facilityname.Contains("Branch") || facilityname.Contains("branch"))
            {
                query = "select distinct(gradeName) from BranchGradesTable where active = 'Y' and gradeName = '" + grade.Trim() + "'";
                try
                {
                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {
                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            Connect.Open();

                            using (SqlDataReader reader = Cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    realGrade = reader["gradeName"].ToString();
                                }

                                Connect.Close();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(realGrade))
                    {
                        resp = "NO";
                    }
                    else
                    {
                        resp = "YES";
                    }
                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }
                finally
                {

                }
            }
            else
            {
                String facility = facilityname.Replace(" ", "");

                query = "select distinct(gradeName) from " + facility.Trim() + "GradesTable where active = 'Y' and gradeName = '" + grade.Trim() + "'";
                try
                {
                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {
                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            Connect.Open();

                            using (SqlDataReader reader = Cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    realGrade = reader["gradeName"].ToString();
                                }

                                Connect.Close();
                            }


                        }
                    }

                    if (string.IsNullOrEmpty(realGrade))
                    {
                        resp = "NO";
                    }
                    else
                    {
                        resp = "YES";
                    }

                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }
                finally
                {

                }
            }
            return resp;
        }

        public string confirmEmploymentNature(string employnature, string facilityname)
        {
            string realEmp = "";          

            string query = "select " + employnature + " from Facility where facilityName = '" + facilityname + "'";
            try { 
                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {
                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                realEmp = reader["" + employnature + ""].ToString();
                            }

                            Connect.Close();
                        }


                    }
                    }
                }
                catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
                finally
                {

                }
            return realEmp;
        }

        public decimal getPercentageRate(string facilityName)
        {
            decimal rate = 0;

            if (facilityName.Contains("Branch") || facilityName.Contains("branch"))
            {
                string query = "select staffBranchPercentageRate from Facility where facilityName = '" + facilityName + "'";
                try { 
                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {
                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rate = (decimal)reader["staffBranchPercentageRate"];
                            }

                            Connect.Close();
                        }
                    }
                }

                }
                    catch (Exception ex)
                {
                    string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                    var mail = new AllLogs();
                    mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                }
                finally
                {

                }
        }
            else
            {
                string query = "select percentageRate from Facility where facilityName = '" + facilityName + "'";
                try { 
                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {
                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        Connect.Open();

                        using (SqlDataReader reader = Cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rate = (decimal)reader["percentageRate"];
                            }

                            Connect.Close();
                        }
                    }
                }

            }
             catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
            finally
            {

            }

        }            

            return rate;
        }

        public int getTenor(string facilityName)
        {
            int ten = 0;

            string query = "select maximumTenor from Facility where facilityName = '" + facilityName + "'";
            try { 
                    using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                    {
                        using (SqlCommand Cmd = new SqlCommand(query, Connect))
                        {
                            Connect.Open();

                            using (SqlDataReader reader = Cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ten = (int)reader["maximumTenor"];
                                }

                                Connect.Close();
                            }


                        }
                    }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
            finally
            {

            }

            return ten;
        }
        public void sendmailInitiator(string refNo)
        {
            try
            {
                string subject = "New Facility Request Submitted";
                string dt = DateTime.Now.ToString();
              
                StringBuilder sb = new StringBuilder();
                StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlDoc/NewRequest.html"));
                string line = sr.ReadToEnd();
                sb.Append(line);
                sb.Replace("{refNo}", refNo);
                sb.Replace("{date}", dt);              
                string body = sb.ToString();
                string emailReceipient = UserId + "@jaizbankplc.com";
                var mail = new EmailSender();

                //SendEmailMainHelper(string receiver, string mailContent, string sender, string mailSubject)
                mail.SendEmailMainHelper(emailReceipient, body, subject);

            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
        }


        public void sendmail(string refNo)
        {
            try
            {
                string subject = "New Facility Request Awaiting Your Attention!";
                string dt = DateTime.Now.ToString();
                string initiator = UserId+"@jaizbankplc.com";

                StringBuilder sb = new StringBuilder();
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/HtmlDoc/HRReviewTemplate.html"));
                string line = sr.ReadToEnd();
                sb.Append(line);
                sb.Replace("{refNo}", refNo);
                sb.Replace("{date}", dt);
                sb.Replace("{initiator}", initiator);
                string body = sb.ToString();

                var mail = new EmailSender();                
               // mail.sendmail(emailReceipient, body, subject);
                mail.SendEmailMainHelperHR(body, subject);

            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
            }
        }

        public class SelectList
        {
            public string Select { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }
        }

        protected void GetDropDown()
         {

            using (var db = new HRViabilityPortalEntities())
            {
                
                var facilityType = (from f in db.Facilities.Where(x => x.active == "Y" && x.facilityRulesSet == "Y") orderby f.facilityName ascending select f.facilityName).Distinct();

                foreach (var bn in facilityType)
                {
                    
                    SelectList item = new SelectList();
                    item.Select = "==Select==";
                    item.Text = bn;
                    item.Value = bn;
                    ListOfFacilities.Add(item);
                }

                //var docType = (from f in db.FacilityDocuments.Where(x => x.active == "Y") orderby f.documentName ascending select f.documentName).Distinct();
                //foreach (var bn in docType)
                //{
                //    SelectDocumentsList item = new SelectDocumentsList();
                   
                //    item.Text = bn;
                //    item.Value = bn;
                //    DocumentsList.Add(item);
                //}

                //var formType = (from f in db.FacilityForms.Where(x => x.active == "Y") orderby f.formName ascending select f.formName).Distinct();
                //foreach (var bn in formType)
                //{
                //    SelectFormsList item = new SelectFormsList();
                   
                //    item.Text = bn;
                //    item.Value = bn;
                //    FormsList.Add(item);
                //}

            }
        }
            
        protected void GetDocumentNames()
        {
            String query = "";
            using (var db = new HRViabilityPortalEntities())
            {
                if (FacilityReq.facilityName == "Murabaha")
                {
                    var docType = (from f in db.MurabahaDocumentsTables.Where(x => x.active == "Y" && x.documentName != "Not Applicable") orderby f.documentName ascending select f.documentName).Distinct();
                    foreach (var bn in docType)
                    {
                        SelectDocumentsList item = new SelectDocumentsList();
                       
                        item.Text = bn;
                        item.Value = bn;
                        DocumentsList.Add(item);
                    }
                }

                else if (FacilityReq.facilityName == "Home Finance")
                {
                    var docType = (from f in db.HomeFinanceDocumentsTables.Where(x => x.active == "Y" && x.documentName != "Not Applicable") orderby f.documentName ascending select f.documentName).Distinct();
                    foreach (var bn in docType)
                    {
                        SelectDocumentsList item = new SelectDocumentsList();
                     
                        item.Text = bn;
                        item.Value = bn;
                        DocumentsList.Add(item);
                    }
                }

                else if (FacilityReq.facilityName == "Ijara Service")
                {
                    var docType = (from f in db.IjaraServiceDocumentsTables.Where(x => x.active == "Y" && x.documentName != "Not Applicable") orderby f.documentName ascending select f.documentName).Distinct();
                    foreach (var bn in docType)
                    {
                        SelectDocumentsList item = new SelectDocumentsList();
                        
                        item.Text = bn;
                        item.Value = bn;
                        DocumentsList.Add(item);
                    }
                }

                else if (FacilityReq.facilityName == "Bai Muajjal")
                {
                    var docType = (from f in db.BaiMuajjalDocumentsTables.Where(x => x.active == "Y" && x.documentName != "Not Applicable") orderby f.documentName ascending select f.documentName).Distinct();
                    foreach (var bn in docType)
                    {
                        SelectDocumentsList item = new SelectDocumentsList();
                       
                        item.Text = bn;
                        item.Value = bn;
                        DocumentsList.Add(item);
                    }
                }
                else if (FacilityReq.facilityName.Contains("Branch") || FacilityReq.facilityName.Contains("branch"))
                {
                    //var docType = (from f in db.BranchDocumentsTables.Where(x => x.active == "Y") orderby f.documentName ascending select f.documentName).Distinct();
                    //foreach (var bn in docType)
                    //{
                    //    SelectDocumentsList item = new SelectDocumentsList();
                    //    item.Select = "==Select==";
                    //    item.Text = bn;
                    //    item.Value = bn;
                    //    DocumentsList.Add(item);
                    //}

                    query = "select distinct(documentName) from BranchDocumentsTable where active = 'Y' and documentName != 'Not Applicable' order by documentName asc";
                    int count = 0;
                    try
                    {
                        using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                        {

                            using (SqlCommand Cmd = new SqlCommand(query, Connect))
                            {
                                Connect.Open();

                                using (SqlDataReader reader = Cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        SelectDocumentsList item = new SelectDocumentsList();
                                       
                                        item.Text = reader.GetString(reader.GetOrdinal("documentName"));
                                        item.Value = reader.GetString(reader.GetOrdinal("documentName"));

                                        DocumentsList.Add(item);

                                        count++;
                                    }

                                    Connect.Close();
                                }
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
                        mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                    }
                    finally
                    {
                        //db.Configuration.Close();
                    }


                }

                else
                {
                    String facility = FacilityReq.facilityName.Replace(" ", "");
                    query = "select distinct(documentName) from " + facility+ "DocumentsTable where active = 'Y' and documentName != 'Not Applicable' order by documentName asc";
                    int count = 0;
                    try
                    {
                        using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                        {

                            using (SqlCommand Cmd = new SqlCommand(query, Connect))
                            {
                                Connect.Open();

                                using (SqlDataReader reader = Cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        SelectDocumentsList item = new SelectDocumentsList();
                                       
                                        item.Text = reader.GetString(reader.GetOrdinal("documentName")); 
                                        item.Value = reader.GetString(reader.GetOrdinal("documentName"));                                       

                                        DocumentsList.Add(item);

                                        count++;
                                    }

                                    Connect.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ValidationErrors.Add(new
                              KeyValuePair<string, string>("HRViability",
                              ex.Message));

                        //put error log here
                        string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                        var mail = new AllLogs();
                        mail.ErrorLog(ErrorDescription, FacilityReq.staffId);
                    }
                    finally
                    {
                        //db.Configuration.Close();
                    }
                }

            }
        }

        public string fetchMyHousingAllowance(int gradeLevel, string branchCode)
        {
            string ret = "";

            using (HRViabilityPortalEntities d = new HRViabilityPortalEntities())
            {
                var rr = d.StaffGradesAllowances.Where(a => a.gradeLevelCode == gradeLevel).SingleOrDefault();

                if (ConfigurationManager.AppSettings["AbujaLagosPHCBranches"].ToString().Contains(branchCode.Trim()))
                {
                    ret = rr.abjLagPhcBranchesHousing;
                }
                else
                {
                    ret = rr.otherBranchesHousing;
                }                              
            }

            return ret;
        }

        public string fetchMyAnnualGrossPackage(int gradeLevel, string branchCode)
        {
            string ret = "";

            using (HRViabilityPortalEntities d = new HRViabilityPortalEntities())
            {
                var rr = d.StaffGradesAllowances.Where(a => a.gradeLevelCode == gradeLevel).SingleOrDefault();

                if (ConfigurationManager.AppSettings["AbujaLagosPHCBranches"].ToString().Contains(branchCode.Trim()))
                {
                    ret = rr.abjLagPhcBranchesAnnualGross;
                }
                else
                {
                    ret = rr.otherBranchesAnnualGrosss;
                }
            }

            return ret;
        }
        public class SelectFormsList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class SelectDocumentsList
        {           
            public string Text { get; set; }
            public string Value { get; set; }
            //public HttpPostedFileBase uploadedDoc { get; set; }
        }

        public class DocumentsDAO
        {
            public int id { get; set; }
            public string documentName { get; set; }
            public string space { get; set; }
            public bool isSelected { get; set; }
            public HttpPostedFileBase uploadedDocument { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }

        }
    }
}