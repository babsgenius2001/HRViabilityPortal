using MySql.Data.MySqlClient;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using HRViabilityPortal.ViewModels;
using RestSharp;
using Newtonsoft.Json;
using NLog;

namespace HRViabilityPortal.Controllers
{
    public class HomeController : Controller
    {
        public string userid;
        public string userName;
        public string loginName;
        //public string Msg { get; set; }
        public static Logger _jobLogger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            LogOnModel vm = new LogOnModel();

            return View();
        }

       // public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public static string[] explode(string separator, string source)
        {
            return source.Split(new string[] { separator }, StringSplitOptions.None);
        }


        [HttpPost]
        public ActionResult Index(LogOnModel model, string returnUrl)
        {
            // string ip = System.Web.HttpContext.Current.Request.UserHostAddress;       
            model.IsValid = ModelState.IsValid;

            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (ip != null || ip != String.Empty)
            {
                ip = Request.ServerVariables["REMOTE_ADDR"];
            }
                        
                if (ModelState.IsValid)
                {
                var svc = new ADDetails.Service();

                var ss = svc.ADValidateUser(model.UserName, model.Password);
                string[] resultsArray = explode("|", ss);

                var t = resultsArray[0];
               // var t = "true";
              // Session["name"] = "Salihu Isa";

                  Session["name"] = resultsArray[1];
                // Session["email"] = "Salihu.Isa@jaizbankplc.com";
                  Session["email"] = resultsArray[2];

                    userid = model.UserName.Trim();
                    userName = model.UserName.Trim() + "@jaizbankplc.com";
                    loginName = Session["name"].ToString();

                    _jobLogger.Info("svc.ADValidateUser(model.UserName, model.Password) method Called::  " +userid + " || "+userName+ " || " + loginName);

                    if (t == "true")
                    {
                        _jobLogger.Info("Entering ValidateUser Method:: ");
                        if (ValidateUser(model.UserName.Trim(), model.Password))
                            {
                                if (CheckConfirmation(model.UserName.Trim()))
                                {
                                    SetupFormsAuthTicket(model.UserName, model.RememberMe);

                                    var log = new AllLogs();
                                    log.writeAuditlog(userid, userName, "Login Access Granted to: " + loginName, ip);
                                    _jobLogger.Info("=== Login Access Granted Successfully To: ==== " + loginName);

                                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                                    {
                                        return Redirect(returnUrl);
                                    }

                                if (Session["Role"].ToString() == "AdminHRV")
                                {

                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["Role"].ToString() == "UserHRV")
                                {

                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["role"].ToString() == "HR_Reviewer")
                                {

                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["role"].ToString() == "HR_Approver")
                                {

                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["role"].ToString() == "DH_Approver")
                                {

                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["role"].ToString() == "MD_Approver")
                                {

                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["role"].ToString() == "BMHRV")
                                {
                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["role"].ToString() == "InternalControlHRV")
                                {
                                    return RedirectToAction("index", "Admin");
                                }

                                if (Session["role"].ToString() == "AuditHRV")
                                {
                                    return RedirectToAction("index", "Admin");
                                }

                                else
                                {
                                        return RedirectToAction("index", "Home");
                                }
                                }
                                else
                                {
                                        string op = "Ineligibility: You Have Not Been Confirmed Yet!";

                                        model.Msg = op;
                                        model.IsValid = false;

                                        TempData["Msg"] = model.Msg;
                                        // NOTE: Must clear the model state in order to bind
                                        //       the @Html helpers to the new model values
                                        ModelState.Clear();

                                        var log = new AllLogs();
                                        log.writeAuditlog(userid, userName, "Ineligibility: You Have Not Been Confirmed!: " + userid, ip);

                                    _jobLogger.Info("=== Unsuccessful Login Access To: ==== " + userid + " due to " +op);

                                    var mail = new AllLogs();
                                        mail.ErrorLog("Ineligibility: You Have Not Been Confirmed!: ", model.UserName.Trim());

                                }
                            }
                            else
                            {                            
                                var log = new AllLogs();
                                log.writeAuditlog(userid, userName, "Invalid Username or Password or You have not been profiled on the Portal!. Kindly contact the Administrator: " + userid, ip);

                                var mail = new AllLogs();
                                mail.ErrorLog("Invalid Username/Password or You have not been profiled on the Portal!. Kindly contact the Administrator for: ", model.UserName.Trim());                                               
                            
                                string op = "Invalid Username/Password or You have not been profiled on the Portal!. Kindly contact the Administrator";

                                _jobLogger.Info("=== Invalid Username/Password or You have not been profiled on the Portal!. Kindly contact the Administrator Error For: ==== " + userid);


                                model.Msg = op;
                                model.IsValid = false;

                                TempData["Msg"] = model.Msg;
                                // NOTE: Must clear the model state in order to bind
                                //       the @Html helpers to the new model values
                                ModelState.Clear();

                        }
                    }
                    else
                    {
                    var mail = new AllLogs();
                    mail.ErrorLog("Username or password incorrect error! ", model.UserName.Trim());

                    var log = new AllLogs();
                    log.writeAuditlog(userid, userName, "Username or password incorrect for: " + userid, ip);

                    string op = "Username or Password Incorrect!. Kindly contact the Administrator";
                    _jobLogger.Info("=== Username or Password Incorrect!. Kindly contact the Administrator For: ==== " + userid);
                    model.Msg = op;
                    model.IsValid = false;

                    TempData["Msg"] = model.Msg;
                    // NOTE: Must clear the model state in order to bind
                    //       the @Html helpers to the new model values
                    ModelState.Clear();

                }
            }                      


            return View(model);
        }

        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session.Abandon();

            System.Web.HttpCookie cookie1 = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            System.Web.HttpCookie cookie2 = new System.Web.HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("index", "Home");
        }

        private void SetupFormsAuthTicket(string userName, bool rememberMe)
        {
            var userId = userName;
            var userData = userId.ToString(CultureInfo.InvariantCulture);
            var authTicket = new FormsAuthenticationTicket(1, //version
                                                        userName, // user name
                                                        DateTime.Now, //creation
                                                        DateTime.Now.AddMinutes(30), //Expiration
                                                        rememberMe, //persistanceFlag, //Persistent
                                                        userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            Response.Cookies.Add(new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            // return user;
        }

        public bool ValidateUser(string username, string password)
        {

            JaizRoleManager.JaizRoleManagerService uservalidation = new JaizRoleManager.JaizRoleManagerService();
            JaizRoleManager.LogonModel logModel = new JaizRoleManager.LogonModel() { username = username, password = password, appID = 6, appIDSpecified = true };
            JaizRoleManager.LoginResult result = new JaizRoleManager.LoginResult();
            try
            {
                _jobLogger.Info("=== ValidateUser Method==== ");
                result = uservalidation.ValidateADUser(logModel);

                _jobLogger.Info("=== ValidateUser Result==== " +result.ToString());

                bool loggedon = result.loggedIn;
                string role = "";

                //role = "UserHRV";
                //Session["Role"] = "UserHRV";
               // bool loggedon = true;

                string URL = ConfigurationManager.AppSettings["url"].ToString();

                var client2 = new RestClient(URL + "/GetUserDetails");
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("ContentType", "application/json");
                request2.RequestFormat = DataFormat.Json;
                request2.AddJsonBody(new { UserName = username.Trim() });
                var s2 = client2.Execute(request2);
                var Xxc2 = JsonConvert.DeserializeObject<dynamic>(s2.Content);
                var respon2 = JsonConvert.SerializeObject(Xxc2);
                var json2 = JsonConvert.DeserializeObject<Rootobject2>(respon2);

                if (json2.Status)
                {
                    Session["branchCode"] = json2.Data.branch;
                    Session["branch"] = json2.Data.department;
                }

                Session["userid"] = result.username;
                Session["userImage"] = Session["userID"].ToString() + ".jpg";
                string userimageurl = "http://jaizportal.jaiz.local/portal/pictures/" + Session["userImage"].ToString();
                Session["userImageURL"] = userimageurl;
                userid = Session["userID"].ToString();
                userName = Session["name"].ToString();

                if (loggedon)
                {
                    foreach (string mappedRole in result.roles)
                    {
                        role = mappedRole.Trim();

                        if (result.roles.Contains("AdminHRV"))
                        {
                            Session["Role"] = "AdminHRV";
                            break;
                        }
                        else if (result.roles.Contains("HR_Reviewer"))
                        {
                            Session["Role"] = "HR_Reviewer";
                            break;
                        }                        
                        else if (result.roles.Contains("HR_Approver"))
                        {
                            Session["Role"] = "HR_Approver";
                            break;
                        }
                        else if (result.roles.Contains("DH_Approver"))
                        {
                            Session["Role"] = "DH_Approver";
                            break;
                        }
                        else if (result.roles.Contains("MD_Approver"))
                        {
                            Session["Role"] = "MD_Approver";
                            break;
                        }
                        else if (result.roles.Contains("BMHRV"))
                        {
                            Session["Role"] = "BMHRV";
                            break;
                        }
                        else if (result.roles.Contains("InternalControlHRV"))
                        {
                            Session["Role"] = "InternalControlHRV";
                            break;
                        }
                        else if (result.roles.Contains("AuditHRV"))
                        {
                            Session["Role"] = "AuditHRV";
                            break;
                        }
                        else
                        {
                            Session["Role"] = "UserHRV";
                            break;
                        }

                        
                    }                    
                  
                    //classifying view rights starts here//

                    //string USERID = userid.ToUpper();

                    

                    //if (ConfigurationManager.AppSettings["HRReviewGroup"].ToString().Contains(USERID.Trim()))
                    //{
                    //    Session["Role"] = "HR_Reviewer";
                    //}
                    //else if (ConfigurationManager.AppSettings["HRApproverGroup"].ToString().Contains(USERID.Trim()))
                    //{
                    //    Session["Role"] = "HR_Approver";
                    //}
                    //else if (ConfigurationManager.AppSettings["DHApproverGroup"].ToString().Contains(USERID.Trim()))
                    //{
                    //    Session["Role"] = "DH_Approver";
                    //}
                    //else if (ConfigurationManager.AppSettings["MDApproverGroup"].ToString().Contains(USERID.Trim()))
                    //{
                    //    Session["Role"] = "MD_Approver";
                    //}
                    //else if (ConfigurationManager.AppSettings["BMApproverGroup"].ToString().Contains(USERID.Trim()))
                    //{
                        
                    //    Session["Role"] = "BMHRV";
                    //    //Session["Role"] = "BM";
                    //}
                    //else if (ConfigurationManager.AppSettings["InternalControlApproverGroup"].ToString().Contains(USERID.Trim()))
                    //{
                    //    Session["Role"] = "InternalControlHRV";
                    //    //Session["Role"] = "Internal Control";
                    //}
                    //else if (ConfigurationManager.AppSettings["AuditApproverGroup"].ToString().Contains(USERID.Trim()))
                    //{
                        
                    //    Session["Role"] = "AuditHRV";
                    //    //Session["Role"] = "Audit";
                    //}
                    //else if (ConfigurationManager.AppSettings["AdminGroup"].ToString().Contains(USERID.Trim()))
                    //{
                    //    //Session["Role"] = "Admin"; 
                    //    Session["Role"] = "AdminHRV";
                    //}
                    //else
                    //{
                    //    Session["Role"] = "UserHRV";
                    //}
                    //classifying view rights ends here//                    

                }
                else
                {
                    Session["Role"] = "UserHRV";
                }
                
                if (ConfigurationManager.AppSettings["ExceptionalHRGroup"].ToString().Contains(username.ToUpper().Trim()))
                {
                    Session["Role"] = "HR_Reviewer";
                }

                if (ConfigurationManager.AppSettings["ExceptionalBMGroup"].ToString().Contains(username.ToUpper().Trim()))
                {
                    Session["Role"] = "BMHRV";
                }

              // Session["Role"] = "UserHRV";
                /////---------
               string myRole = Session["Role"].ToString();

                _jobLogger.Info("=== Role Obtained Method ==== " + myRole);
            }
            catch (Exception ex)
            {
                
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, username);

                _jobLogger.Error("=== Validate User Error ==== " + ErrorDescription);

            }

            //return result.loggedIn;
            
           return true;
        }

        public bool CheckConfirmation(string username)
        {
            bool returnval = false;
            string conf = "";
            try
            {
                _jobLogger.Info("=== Checking If Staff IS Confirmed Method ==== ");

                using (MySqlConnection con = new MySqlConnection())
                {
                    con.ConnectionString = ConfigurationManager.AppSettings["MySQLCon"];
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT confirmation FROM staff_personnel where personnel_id = @username";
                    cmd.Parameters.Add("username", MySqlDbType.VarChar).Value = username;

                    _jobLogger.Info("=== Checking If Staff IS Confirmed Query ==== " + cmd.CommandText);

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        conf = rdr["confirmation"].ToString();
                    }

                    rdr.Close();
                    //close Connection

                    if (conf == "Yes")
                    {
                        returnval = true;
                    }
                    else
                    {
                        if (ConfigurationManager.AppSettings["ExceptionalHRGroup"].ToString().Contains(username.ToUpper().Trim()))
                        {
                            returnval = true;
                        }
                        else
                        {
                            returnval = false;
                        }                        
                    }
                    
                }
            }
            catch (Exception ex)
            {

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, username + " || Confirmation Check Error!|| Home Controller.cs || Line 404");

                returnval = false;

                _jobLogger.Error("=== Checking If Staff IS Confirmed Method Error ==== " +ErrorDescription);
            }
            finally
            {

            }

            _jobLogger.Info("=== Checking If Staff IS Confirmed Response ==== " +returnval);
            return returnval;

            
            //return true;
        }

        public class Rootobject2
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public UsersDetails Data { get; set; }
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


    }

}