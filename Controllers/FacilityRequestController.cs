using HRViabilityPortal.Database_References;
using HRViabilityPortal.ViewModels;
using MySql.Data.MySqlClient;
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
    public class FacilityRequestController : Controller
    {
        // GET: FacilityRequest
        public ActionResult Index()
        {
            return View();
        }

        public static Logger _jobLogger = LogManager.GetCurrentClassLogger();

        IEnumerable<HttpPostedFileBase> files;
        DateTime dob { get; set; }
        DateTime doa { get; set; }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        public ActionResult NewRequest(int? page)
        {
            NewRequestViewModel vm = new NewRequestViewModel();

            vm.UserId = Session["userID"].ToString();
            // vm.StaffName = Session["name"].ToString();

            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (ip != null || ip != String.Empty)
            {
                ip = Request.ServerVariables["REMOTE_ADDR"];
            }

                vm.ipAddress = ip;
                try {

                    _jobLogger.Info("=== Inside Facility Request Controller - GET");
                    using (MySqlConnection con = new MySqlConnection())
                    {
                        con.ConnectionString = ConfigurationManager.AppSettings["MySQLCon"];
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;

                        cmd.CommandText = "SELECT 12 * (YEAR(curdate()) - YEAR(confirmation_date)) +(MONTH(curdate()) - MONTH(confirmation_date)) AS months, cif_number, nuban, grade_level, category, telephone_number, marital_status, state_of_origin, date_of_birth, date_of_assumption, 12 * (YEAR(curdate()) - YEAR(date_of_assumption)) +(MONTH(curdate()) - MONTH(date_of_assumption)) AS monthsFromAssumptionDate from jaiz_db.staff_personnel where personnel_id = @staffId";

                        cmd.Parameters.Add("staffId", MySqlDbType.VarChar).Value = vm.UserId;

                    _jobLogger.Info("=== Calling Query to Fetch Staff Details 1 ==== " + cmd.CommandText);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            vm.CIF = Convert.ToInt32(rdr["cif_number"]);
                            vm.AccountNumber = rdr["nuban"].ToString();
                            vm.noOfMonthsInService = Convert.ToInt32(rdr["months"]);
                            vm.noOfMonthsFromAssumptionDate = Convert.ToInt32(rdr["monthsFromAssumptionDate"]);
                            vm.gradeLevel = Convert.ToInt32(rdr["grade_level"]);
                            vm.staffCategory = rdr["category"].ToString();

                            vm.FacilityReq.mobileNumber = rdr["telephone_number"].ToString();
                            vm.FacilityReq.mobileNumber = vm.FacilityReq.mobileNumber.Replace(" ", String.Empty);
                            vm.FacilityReq.maritalStatus = rdr["marital_status"].ToString();
                            vm.FacilityReq.maritalStatus = vm.FacilityReq.maritalStatus.Replace(" ", String.Empty);
                            vm.FacilityReq.state = rdr["state_of_origin"].ToString();

                            dob = Convert.ToDateTime(rdr["date_of_birth"]).Date;
                            doa = Convert.ToDateTime(rdr["date_of_assumption"]).Date;

                            vm.FacilityReq.dateOfBirth = String.Format("{0:dd-MMM-yyyy}", dob);                            
                            vm.FacilityReq.dateOfEmployment = String.Format("{0:dd-MMM-yyyy}", doa);                           
                        }

                        con.Close();                            
                                        
                        con.ConnectionString = ConfigurationManager.AppSettings["MySQLCon"];
                        con.Open();
                        MySqlCommand cmd2 = new MySqlCommand();
                        cmd2.Connection = con;

                        cmd2.CommandText = "SELECT grade_level from jaiz_db.grades_level where grade_level_code = @grade_level_code";

                        cmd2.Parameters.Add("grade_level_code", MySqlDbType.Int32).Value = vm.gradeLevel;

                    _jobLogger.Info("=== Calling Query to Fetch Staff Details 2 ==== " + cmd2.CommandText);

                    MySqlDataReader rdr2 = cmd2.ExecuteReader();
                        if (rdr2.HasRows)
                        {
                            rdr2.Read();

                            vm.FacilityReq.salaryGrade = rdr2["grade_level"].ToString();
                            vm.salaryGrade = rdr2["grade_level"].ToString();
                        }
                        con.Close();

                }
            }
            catch (Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, vm.UserId);    // }           

                _jobLogger.Info("=== Error Fetching Staff Details ==== " + ErrorDescription);
            }
            finally
            {

            }

            vm.FacilityReq.staffId = vm.UserId;
            vm.FacilityReq.accountNumber = vm.AccountNumber;
            vm.FacilityReq.cif = vm.CIF;
            //vm.FacilityReq.numberOfMonthsInService = 36;
            //----please change back///
            vm.FacilityReq.numberOfMonthsInService = vm.noOfMonthsInService;
            vm.FacilityReq.natureOfEmployment = vm.staffCategory;            

            if (vm.FacilityReq.salaryGrade == "EXECUTIVE TRAINEE" || vm.FacilityReq.salaryGrade == "EXECUTIVE TRAINEE A" || vm.FacilityReq.salaryGrade == "EXECUTIVE TRAINEE B" || vm.FacilityReq.salaryGrade == "EXECUTIVE TRAINEE C")
            {
                vm.FacilityReq.salaryGrade = "Executive Trainee";
            }
            if (vm.FacilityReq.salaryGrade == "SENIOR EXECUTIVE TRAINEE" || vm.FacilityReq.salaryGrade == "SENIOR EXECUTIVE TRAINEE A" || vm.FacilityReq.salaryGrade == "SENIOR EXECUTIVE TRAINEE B" || vm.FacilityReq.salaryGrade == "SENIOR EXECUTIVE TRAINEE C")
            {
                vm.FacilityReq.salaryGrade = "Senior Executive Trainee";
            }
            if (vm.FacilityReq.salaryGrade == "ASSISTANT BANKING OFFICER" || vm.FacilityReq.salaryGrade == "ASSISTANT BANKING OFFICER I" || vm.FacilityReq.salaryGrade == "ASSISTANT BANKING OFFICER II" || vm.FacilityReq.salaryGrade == "ASSISTANT BANKING OFFICER III")
            {
                vm.FacilityReq.salaryGrade = "Assistant Banking Officer";
            }
            if (vm.FacilityReq.salaryGrade == "BANKING OFFICER" || vm.FacilityReq.salaryGrade == "BANKING OFFICER I" || vm.FacilityReq.salaryGrade == "BANKING OFFICER II" || vm.FacilityReq.salaryGrade == "BANKING OFFICER III")
            {
                vm.FacilityReq.salaryGrade = "Banking Officer";
            }
            if (vm.FacilityReq.salaryGrade == "SENIOR BANKING OFFICER" || vm.FacilityReq.salaryGrade == "SENIOR BANKING OFFICER I" || vm.FacilityReq.salaryGrade == "SENIOR BANKING OFFICER II" || vm.FacilityReq.salaryGrade == "SENIOR BANKING OFFICER III")
            {
                vm.FacilityReq.salaryGrade = "Senior Banking Officer";
            }
            if (vm.FacilityReq.salaryGrade == "ASSISTANT MANAGER" || vm.FacilityReq.salaryGrade == "ASSISTANT MANAGER I" || vm.FacilityReq.salaryGrade == "ASSISTANT MANAGER II" || vm.FacilityReq.salaryGrade == "ASSISTANT MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "Assistant Manager";
            }
            if (vm.FacilityReq.salaryGrade == "DEPUTY MANAGER" || vm.FacilityReq.salaryGrade == "DEPUTY MANAGER I" || vm.FacilityReq.salaryGrade == "DEPUTY MANAGER II" || vm.FacilityReq.salaryGrade == "DEPUTY MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "Deputy Manager";
            }
            if (vm.FacilityReq.salaryGrade == "MANAGER" || vm.FacilityReq.salaryGrade == "MANAGER I" || vm.FacilityReq.salaryGrade == "MANAGER II" || vm.FacilityReq.salaryGrade == "MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "Manager";
            }
            if (vm.FacilityReq.salaryGrade == "SENIOR MANAGER" || vm.FacilityReq.salaryGrade == "SENIOR MANAGER I" || vm.FacilityReq.salaryGrade == "SENIOR MANAGER II" || vm.FacilityReq.salaryGrade == "SENIOR MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "Senior Manager";
            }
            if (vm.FacilityReq.salaryGrade == "PRINCIPAL MANAGER" || vm.FacilityReq.salaryGrade == "PRINCIPAL MANAGER I" || vm.FacilityReq.salaryGrade == "PRINCIPAL MANAGER II" || vm.FacilityReq.salaryGrade == "PRINCIPAL MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "Principal Manager";
            }
            if (vm.FacilityReq.salaryGrade == "ASSISTANT GENERAL MANAGER" || vm.FacilityReq.salaryGrade == "ASSISTANT GENERAL MANAGER I" || vm.FacilityReq.salaryGrade == "ASSISTANT GENERAL MANAGER II" || vm.FacilityReq.salaryGrade == "ASSISTANT GENERAL MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "Assistant General Manager";
            }
            if (vm.FacilityReq.salaryGrade == "DEPUTY GENERAL MANAGER" || vm.FacilityReq.salaryGrade == "DEPUTY GENERAL MANAGER I" || vm.FacilityReq.salaryGrade == "DEPUTY GENERAL MANAGER II" || vm.FacilityReq.salaryGrade == "DEPUTY GENERAL MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "Deputy General Manager";
            }
            if (vm.FacilityReq.salaryGrade == "GENERAL MANAGER" || vm.FacilityReq.salaryGrade == "GENERAL MANAGER I" || vm.FacilityReq.salaryGrade == "GENERAL MANAGER II" || vm.FacilityReq.salaryGrade == "GENERAL MANAGER III")
            {
                vm.FacilityReq.salaryGrade = "General Manager";
            }
            if (vm.FacilityReq.salaryGrade == "EXECUTIVE DIRECTOR")
            {
                vm.FacilityReq.salaryGrade = "Executive Director";
            }
            if (vm.FacilityReq.salaryGrade == "DEPUTY MANAGING DIRECTOR")
            {
                vm.FacilityReq.salaryGrade = "Deputy Managing Director";
            }
            if (vm.FacilityReq.salaryGrade == "MANAGING DIRECTOR")
            {
                vm.FacilityReq.salaryGrade = "Managing Director";
            }

            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);

            vm.IsDetailAreaVisible = true;
            vm.IsSearchAreaVisible = false;
            vm.IsListAreaVisible = false;
            vm.IsStep1 = true;
            vm.activeStatus = "Y";

            vm.HandleRequest();
            return View(vm);
        }

        [HttpPost]
        public ActionResult NewRequest(NewRequestViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsDetailAreaVisible = true;
            vm.IsSearchAreaVisible = false;
            vm.IsListAreaVisible = false;
            vm.url = ConfigurationManager.AppSettings["url"];
            vm.Userbranch = Session["branch"].ToString();            
            vm.UserId = Session["userID"].ToString();
            vm.activeStatus = "Y";
            vm.columnName = "Path";

            _jobLogger.Info("=== Inside Facility Request Controller - POST");


            if (vm.DocumentsList != null & vm.DocumentsList.Count != 0)
            {
                if(vm.DocumentsList.Count == 1)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                }
                if (vm.DocumentsList.Count == 2)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                }
                if (vm.DocumentsList.Count == 3)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                }
                if (vm.DocumentsList.Count == 4)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                    vm.FacilityReq.fileContentType4 = vm.DocumentsList[3].Value;
                }
                if (vm.DocumentsList.Count == 5)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                    vm.FacilityReq.fileContentType4 = vm.DocumentsList[3].Value;
                    vm.FacilityReq.fileContentType5 = vm.DocumentsList[4].Value;
                }
                if (vm.DocumentsList.Count == 6)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                    vm.FacilityReq.fileContentType4 = vm.DocumentsList[3].Value;
                    vm.FacilityReq.fileContentType5 = vm.DocumentsList[4].Value;
                    vm.FacilityReq.fileContentType6 = vm.DocumentsList[5].Value;
                }
                if (vm.DocumentsList.Count == 7)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                    vm.FacilityReq.fileContentType4 = vm.DocumentsList[3].Value;
                    vm.FacilityReq.fileContentType5 = vm.DocumentsList[4].Value;
                    vm.FacilityReq.fileContentType6 = vm.DocumentsList[5].Value;
                    vm.FacilityReq.fileContentType7 = vm.DocumentsList[6].Value;
                }

                if (vm.DocumentsList.Count == 8)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                    vm.FacilityReq.fileContentType4 = vm.DocumentsList[3].Value;
                    vm.FacilityReq.fileContentType5 = vm.DocumentsList[4].Value;
                    vm.FacilityReq.fileContentType6 = vm.DocumentsList[5].Value;
                    vm.FacilityReq.fileContentType7 = vm.DocumentsList[6].Value;
                    vm.FacilityReq.fileContentType8 = vm.DocumentsList[7].Value;
                }

                if (vm.DocumentsList.Count == 9)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                    vm.FacilityReq.fileContentType4 = vm.DocumentsList[3].Value;
                    vm.FacilityReq.fileContentType5 = vm.DocumentsList[4].Value;
                    vm.FacilityReq.fileContentType6 = vm.DocumentsList[5].Value;
                    vm.FacilityReq.fileContentType7 = vm.DocumentsList[6].Value;
                    vm.FacilityReq.fileContentType8 = vm.DocumentsList[7].Value;
                    vm.FacilityReq.fileContentType9 = vm.DocumentsList[8].Value;
                }
                if (vm.DocumentsList.Count == 10)
                {
                    vm.FacilityReq.fileContentType1 = vm.DocumentsList[0].Value;
                    vm.FacilityReq.fileContentType2 = vm.DocumentsList[1].Value;
                    vm.FacilityReq.fileContentType3 = vm.DocumentsList[2].Value;
                    vm.FacilityReq.fileContentType4 = vm.DocumentsList[3].Value;
                    vm.FacilityReq.fileContentType5 = vm.DocumentsList[4].Value;
                    vm.FacilityReq.fileContentType6 = vm.DocumentsList[5].Value;
                    vm.FacilityReq.fileContentType7 = vm.DocumentsList[6].Value;
                    vm.FacilityReq.fileContentType8 = vm.DocumentsList[7].Value;
                    vm.FacilityReq.fileContentType9 = vm.DocumentsList[8].Value;
                    vm.FacilityReq.fileContentType10 = vm.DocumentsList[9].Value;
                }

            }          


            if (vm.uploadedDocument1 != null && vm.uploadedDocument1.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument1.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" +_FileName);
                vm.uploadedDocument1.SaveAs(_path);
                vm.FacilityReq.fileName1 = _FileName;
            }

            if (vm.uploadedDocument2 != null && vm.uploadedDocument2.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument2.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument2.SaveAs(_path);
                vm.FacilityReq.fileName2 = _FileName;
            }

            if (vm.uploadedDocument3 != null && vm.uploadedDocument3.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument3.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument3.SaveAs(_path);
                vm.FacilityReq.fileName3 = _FileName;
            }

            if (vm.uploadedDocument4 != null && vm.uploadedDocument4.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument4.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument4.SaveAs(_path);
                vm.FacilityReq.fileName4 = _FileName;
            }

            if (vm.uploadedDocument5 != null && vm.uploadedDocument5.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument5.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument5.SaveAs(_path);
                vm.FacilityReq.fileName5 = _FileName;
            }

            if (vm.uploadedDocument6 != null && vm.uploadedDocument6.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument6.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument6.SaveAs(_path);
                vm.FacilityReq.fileName6 = _FileName;
            }

            if (vm.uploadedDocument7 != null && vm.uploadedDocument7.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument7.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument7.SaveAs(_path);
                vm.FacilityReq.fileName7 = _FileName;
            }

            if (vm.uploadedDocument8 != null && vm.uploadedDocument8.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument8.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument8.SaveAs(_path);
                vm.FacilityReq.fileName8 = _FileName;
            }

            if (vm.uploadedDocument9 != null && vm.uploadedDocument9.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument9.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument9.SaveAs(_path);
                vm.FacilityReq.fileName9 = _FileName;
            }

            if (vm.uploadedDocument10 != null && vm.uploadedDocument10.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(vm.uploadedDocument10.FileName);
                string _path = Path.Combine(Server.MapPath("~/Upload"), vm.UserId + "_" + _FileName);
                vm.uploadedDocument10.SaveAs(_path);
                vm.FacilityReq.fileName10 = _FileName;
            }

                if (vm.customField1 != null)
                {

                    vm.FacilityReq.customField1 = vm.customField1;
                }
                if (vm.customField2 != null)
                {

                    vm.FacilityReq.customField2 = vm.customField2;
                }
                if (vm.customField3 != null)
                {

                    vm.FacilityReq.customField3 = vm.customField3;
                }
                if (vm.customField4 != null)
                {

                    vm.FacilityReq.customField4 = vm.customField4;
                }
                if (vm.customField5 != null)
                {

                    vm.FacilityReq.customField5 = vm.customField5;
                }
                if (vm.customField6 != null)
                {

                    vm.FacilityReq.customField6 = vm.customField6;
                }
                if (vm.customField7 != null)
                {

                    vm.FacilityReq.customField7 = vm.customField7;
                }
                if (vm.customField8 != null)
                {

                    vm.FacilityReq.customField8 = vm.customField8;
                }
                if (vm.customField9 != null)
                {

                    vm.FacilityReq.customField9 = vm.customField9;
                }
                if (vm.customField10 != null)
                {

                    vm.FacilityReq.customField10 = vm.customField10;
                }

                vm.HandleRequest();

                if (vm.IsValid)
                {
                    TempData["Msg"] = vm.Msg;
                    // NOTE: Must clear the model state in order to bind
                    //       the @Html helpers to the new model values
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
        }
    }