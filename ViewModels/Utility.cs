using HRViabilityPortal.Database_References;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Globalization;

namespace HRViabilityPortal.Controllers
{
    public class Utility
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class SelectList
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class IDP
    {
        public string Uniqueid()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssFFF");
        }

        public string nextchar(string ch)
        {
            string nxt = "";
            string[] chars = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            int len = chars.Length;
            for (int i = 0; i < chars.Length; i++)
            {
                if (ch == chars[i])
                {
                    nxt = chars[i + 1];
                    break;
                }
            }
            return nxt;
        }
        public bool isValidNumber(int k)
        {
            bool flag = false;
            try
            {
                if (Convert.ToInt32(k) > 0)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }

            return flag;
        }
        public string GetUniqueKey(string basevalue)
        {
            string result = "";
            string tchr = basevalue.Substring(2, 2);  //last two xters
            int lasttwo = Convert.ToInt32(tchr);   //READING LAST TWO CHARACTERS
            string thdigit = basevalue.Substring(1, 1); //read 2nd Xchater of the four digit
            string fdigit = basevalue.Substring(0, 1);  // first xter of the 4 digit
            if (lasttwo <= 99)
            {
                lasttwo += 1;   // add 1 to last two digit if less than 99
                tchr = lasttwo.ToString(); // change the value of last two digit to string
                if (Convert.ToInt32(tchr) <= 9)
                {
                    tchr = "0" + Convert.ToInt32(tchr);
                }
            }
            if (lasttwo > 99)
            {
                tchr = "00";     //reset the last two characters to 00

                if (thdigit == "Z")  // last value in the xter array
                {

                    thdigit = "A";
                    fdigit = nextchar(fdigit);  //read next character

                }
                else
                {
                    thdigit = nextchar(thdigit);
                }
            }
            result = fdigit.ToString() + thdigit.ToString() + tchr.ToString();

            return result;
        }

    }

    public class EmailSender
    {
        public int sendEmail(string mailto, string Body, string subject)
        {

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(mailto));
            msg.From = new MailAddress("no-reply@jaizbankplc.com", "Jaiz Bank Plc");
            msg.Subject = subject;
           // msg.CC.Add(new MailAddress(copy));
            msg.Body = Body;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("eplatform@jaizbankplc.com", "@ppS0l*12");
            client.Credentials = new NetworkCredential("9a2517fe4b6ec021281ba2c799acf74f", "c068a5db94d7c5b82ccb11243792689d");
            //client.Credentials = new NetworkCredential("cexception@jaizbankplc.com", "Abcd1234");
            //client.Credentials = new NetworkCredential("d9c251a31f99b3bc5a49b1c3d804f7a8", "74e6e3cb82178b5b5483e25ad19c3fe9");
            client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
                               //client.Host = "in-v3.mailjet.com";
            client.Host = "in-v3.mailjet.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            //client.Timeout = 200000;

            try
            {
                client.Send(msg);
                var mail = new AllLogs();
                mail.ErrorLog("Email sent successfully!!!", "sendEmail|| No Webservice|| Utility.cs|| Line 139");
                return 1;

            }
            catch (Exception ex)
            {
                client.Dispose();
                IDP s = new IDP();
                // s.ErrorLog(ex.Message);
                //var err2 = new LogUtility.Error()
                //{
                //    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                //    ErrorTime = DateTime.Now,
                //    ModulePointer = "Email Blast",
                //    StackTrace = ex.StackTrace
                //};
                //LogUtility.ActivityLogger.WriteErrorLog(err2);
                //Console.WriteLine("Error Sending to email: " + mailto + " " + ex.ToString());
                //throw ex;

                var mail = new AllLogs();
                mail.ErrorLog("Email Not Sent successfully!!!", "sendEmail|| No Webservice Error|| Utility.cs|| Line 161");
                return 0;

            }


        }

        public void SendEmailMainHelperHR(string mailContent, string mailSubject)

        {

            InternetBankingHelper.JaizHelper service = new InternetBankingHelper.JaizHelper();
            InternetBankingHelper.EmailObject EObj = new InternetBankingHelper.EmailObject();
            InternetBankingHelper.EmailResponse Response = new InternetBankingHelper.EmailResponse();


            try
            {

                var emailReceipient = ConfigurationManager.AppSettings["HRReviewer"].Split(',');
                string[] HRReviewerEmailGroup = emailReceipient;

                foreach (var m in HRReviewerEmailGroup)
                {
                    EObj.EmailAddress = m.Trim();
                    EObj.EmailContent = mailContent;
                    EObj.FromAddress = "info@jaizbankplc.com";
                    EObj.Subject = mailSubject;
                    EObj.SenderId = "INTBK";

                    bool T = true;
                    service.SendEmailViaHelper(EObj, out Response, out T);
                }

            }
            catch (Exception ex)
            {
                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                    ErrorTime = DateTime.Now,
                    ModulePointer = "HR Viability Portal:Validating ADUser Error",
                    StackTrace = ex.StackTrace
                };

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, "INTBK");

                //  new LogUtility.ActivityLogger().WriteErrorLog(err2);

            }

        }

        public void SendEmailMainHelper(string receiver, string mailContent, string mailSubject)

        {

            InternetBankingHelper.JaizHelper service = new InternetBankingHelper.JaizHelper();
            InternetBankingHelper.EmailObject EObj = new InternetBankingHelper.EmailObject();
            InternetBankingHelper.EmailResponse Response = new InternetBankingHelper.EmailResponse();


            try
            {
                
                //EObj.EmailAddress = "BA07190@jaizbankplc.com";
                EObj.EmailAddress = receiver;
                EObj.EmailContent = mailContent;
                EObj.FromAddress = "info@jaizbankplc.com";
                EObj.Subject = mailSubject;
                EObj.SenderId = "INTBK";
                
                bool T = true;
                service.SendEmailViaHelper(EObj, out Response, out T);

            }
            catch (Exception ex)
            {
                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                    ErrorTime = DateTime.Now,
                    ModulePointer = "HR Viability Portal:Validating ADUser Error",
                    StackTrace = ex.StackTrace
                };

                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, "INTBK");

                //  new LogUtility.ActivityLogger().WriteErrorLog(err2);

            }

        }
        public void SendEmailDocuments(string receiver, string mailContent, string mailSubject, string attachmentPath)

        {

            InternetBankingHelper.JaizHelper service = new InternetBankingHelper.JaizHelper();
            InternetBankingHelper.EmailObject EObj = new InternetBankingHelper.EmailObject();
            InternetBankingHelper.EmailResponse Response = new InternetBankingHelper.EmailResponse();


            try
            {
                EObj.EmailAddress = receiver;
                EObj.EmailContent = mailContent;
                EObj.FromAddress = "info@jaizbankplc.com";
                EObj.Subject = mailSubject;
                EObj.SenderId = "INTBK";
                EObj.Attachment = attachmentPath;
                
                bool T = true;
                service.SendEmailViaHelper(EObj, out Response, out T);

            }
            catch (Exception ex)
            {
                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                    ErrorTime = DateTime.Now,
                    ModulePointer = "HR Viability Portal:Validating ADUser Error",
                    StackTrace = ex.StackTrace
                };
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, "INTBK|| Email Webservice Error|| Utility.cs|| Line 240");

                //  new LogUtility.ActivityLogger().WriteErrorLog(err2);

            }

        }

        public void DocumentGeneratorHelper(String staffId, String phoneNumber, String accountNum, String staffName, String natureOfEmployment,
            String employerAddress, String netMonthlySalary, String annualGrossPackage, String facilityType, String dateOfResumption, String dateOfBirth, String jobFunction, String amountRequested, String downPayment,
            String maritalStatus, int lengthOfservice, int tenor, String loanPurpose)

        {
            bool tenorD = true;
            bool lengthD = true;

            DocumentGenerator.DocumentGenerator service = new DocumentGenerator.DocumentGenerator();            

            try
            {

                service.CreateDocuments(staffId, phoneNumber, accountNum, staffName, natureOfEmployment, employerAddress, netMonthlySalary, annualGrossPackage, facilityType, dateOfResumption, dateOfBirth, jobFunction, amountRequested, downPayment,
            maritalStatus, lengthOfservice, lengthD, tenor, tenorD, loanPurpose);

            }
            catch (Exception ex)
            {
                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                    ErrorTime = DateTime.Now,
                    ModulePointer = "HR Viability Portal:CreateDocuments Error",
                    StackTrace = ex.StackTrace
                };

                //  new LogUtility.ActivityLogger().WriteErrorLog(err2);
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, staffId+ "||DocumentGeneratorHelper Error|| Utility.cs|| Line 278");

               
            }

        }

        public void ApplicationFormGeneratorHelper(String assetDesM, string assetDesB, String vendorName, String vendorAdd, String vendorPhone, String assetLoc, String vehicleModel, String vehicleYear, String deliveryModeM, String deliveryModeI, String typeProperty, String propDesc, String docTitle, String propLoc, String developerName, String developerAdd, String developerNo, String serviceDesc,
       String serviceProvName, String serviceProvAdd, String serviceProvNo, String invoiceM, String invoiceI, String accountNum, String staffName, String staffId, String phoneNumber,  String natureOfEmployment, String employerAddress, String netMonthlySalary, String annualGrossPackage, String facilityType, String dateOfResumption, String dateOfBirth, String jobFunction, String amountRequested, String downPayment, String maritalStatus, int lengthOfservice, int tenor, String loanPurpose,
       String customF1, String customF2, String customF3, String customF4, String customF5, String customF6, String customF7, String customF8, String customF9, String customF10, String customFData1, String customFData2,
                    String customFData3, String customFData4, String customFData5, String customFData6, String customFData7, String customFData8, String customFData9, String customFData10, String otherFacilities)

        {
            DocumentGenerator.DocumentGenerator service = new DocumentGenerator.DocumentGenerator();

            try
            {

                service.CreateApplicationForm(assetDesM,assetDesB, vendorName, vendorAdd, vendorPhone, assetLoc, vehicleModel, vehicleYear, 
                deliveryModeM, deliveryModeI, typeProperty, propDesc, docTitle, propLoc, developerName, developerAdd, developerNo, serviceDesc,
                serviceProvName, serviceProvAdd, serviceProvNo, invoiceM, invoiceI, accountNum, staffName, staffId, phoneNumber, natureOfEmployment, 
                employerAddress, netMonthlySalary, annualGrossPackage, facilityType, dateOfResumption, dateOfBirth, jobFunction,  amountRequested, 
                downPayment, maritalStatus, lengthOfservice, true, tenor, true, loanPurpose, customF1, customF2, customF3, customF4, customF5, 
                customF6, customF7, customF8, customF9, customF10, customFData1, customFData2, customFData3, customFData4, customFData5, 
                customFData6, customFData7, customFData8, customFData9, customFData10, otherFacilities);

            }
            catch (Exception ex)
            {
                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                    ErrorTime = DateTime.Now,
                    ModulePointer = "HR Viability Portal:CreateApplicationForm Error",
                    StackTrace = ex.StackTrace
                };

                //  new LogUtility.ActivityLogger().WriteErrorLog(err2);
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                
                mail.ErrorLog(ErrorDescription, staffName + "||ApplicationFormGeneratorHelper Error|| Utility.cs|| Line 319");

            }

        }
    }

   
}

public class AllLogs
    {
        public string getcurrentpage()  //This method returns Current Page Name
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }
        public void writeAuditlog(string userid, string username, string op, string ipAdd)
        {
        // string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            try
            {
                using (var db = new HRViabilityPortalEntities())
                {
                    AuditLog dsl = new AuditLog();
                    dsl.UserId = userid;
                    dsl.ActivityPerformed = op;
                    dsl.IpAddress = ipAdd;
                    dsl.UserName = username;
                    dsl.DateOfActivity = DateTime.Now;
                    db.AuditLogs.Add(dsl);
                    db.SaveChanges();
                }

            } catch(Exception ex)
            {
                string ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException;
                var mail = new AllLogs();
                mail.ErrorLog(ErrorDescription, userid);
        }

        }
        public void DataLog(string dataToLog)
        {
            try
            {
                string appLocation = AppDomain.CurrentDomain.BaseDirectory;
                string file = appLocation + "\\ErrorLog.txt";
                if (!File.Exists(file))
                {
                    File.CreateText(file);
                }
                using (StreamWriter writer = File.AppendText(file))
                {
                    string data = "\r\nLog Written at : " + DateTime.Now.ToString() + "\n" + dataToLog;
                    writer.WriteLine(data);
                    writer.WriteLine("==========================================");
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public void ErrorLog(string ex, string userId)
        {
            try
            {
                string pth = ConfigurationManager.AppSettings["ErrorLog"];
                //string pth =  AppDomain.CurrentDomain.BaseDirectory + "ErrorLog\\" ;
                string err = ex;
                DateTime dt = DateTime.Now;
                string fld = dt.ToString("yyyy") + "_" + dt.ToString("MM") + "_";
                pth += fld + dt.ToString("dd") + ".txt";
                if (!File.Exists(pth))
                {
                    using (StreamWriter sw = File.CreateText(pth))
                    {
                        sw.WriteLine(dt + " : " + err + " || " +userId);
                        sw.WriteLine(" ");
                        sw.Close();
                        sw.Dispose();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(pth))
                    {
                        sw.WriteLine(dt + " : " + err + " || " + userId);
                        sw.WriteLine(" ");
                        sw.Close();
                        sw.Dispose();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public void ErrorLog2(string dataToLog, string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    File.CreateText(filename);
                }
                using (StreamWriter writer = File.AppendText(filename))
                {
                    string data = "\r\nLog Written at : " + DateTime.Now.ToString() + "\n" + dataToLog;
                    writer.WriteLine(data);
                    writer.WriteLine("==========================================");
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        internal void writeAuditlog(object p1, object p2, object p3, object ip)
        {
            throw new NotImplementedException();
        }
    }

