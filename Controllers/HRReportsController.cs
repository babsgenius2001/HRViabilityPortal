using HRViabilityPortal.Database_References;
using HRViabilityPortal.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRViabilityPortal.Controllers
{
    public class HRReportsController : Controller
    {
        // GET: HRReports
        public ActionResult ApprovedRequests(int? page)

        {
            ApprovedRequestsViewModel vm = new ApprovedRequestsViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

                vm.HandleRequest();

                if (vm.FacilityReqs.Count() > 0)
                {
                    Session["export"] = vm.FacilityReqs;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult ApprovedRequests(ApprovedRequestsViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = Session["userID"].ToString();

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 10;
                vm.PageNumber = 1;
            }
            vm.HandleRequest();

            if (vm.FacilityReqs.Count() > 0)
            {
                Session["export"] = vm.FacilityReqs;
            }

            if (vm.IsValid)
            {
                TempData["Msg"] = vm.Msg;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

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


        public ActionResult RejectedRequests(int? page)

        {
            RejectedRequestsViewModel vm = new RejectedRequestsViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

                vm.HandleRequest();

                if (vm.FacilityReqs.Count() > 0)
                {
                    Session["export"] = vm.FacilityReqs;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult RejectedRequests(RejectedRequestsViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = Session["userID"].ToString();

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 10;
                vm.PageNumber = 1;
            }
            vm.HandleRequest();

            if (vm.FacilityReqs.Count() > 0)
            {
                Session["export"] = vm.FacilityReqs;
            }

            if (vm.IsValid)
            {
                TempData["Msg"] = vm.Msg;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

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

        public ActionResult BranchesRequests(int? page)

        {
            BranchesRequestsViewModel vm = new BranchesRequestsViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

                using (MySqlConnection con = new MySqlConnection())
                {
                    con.ConnectionString = ConfigurationManager.AppSettings["MySQLCon"];
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "SELECT branch_code from jaiz_db.staff_personnel where personnel_id = @staffId";

                    cmd.Parameters.Add("staffId", MySqlDbType.VarChar).Value = vm.UserId;

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();

                        vm.branchCode = rdr["branch_code"].ToString();

                    }
                    con.Close();
                }
    
                vm.HandleRequest();

                if (vm.FacilityReqs.Count() > 0)
                {
                    Session["export"] = vm.FacilityReqs;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult BranchesRequests(BranchesRequestsViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = Session["userID"].ToString();

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 10;
                vm.PageNumber = 1;
            }
            vm.HandleRequest();

            if (vm.FacilityReqs.Count() > 0)
            {
                Session["export"] = vm.FacilityReqs;
            }

            if (vm.IsValid)
            {
                TempData["Msg"] = vm.Msg;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

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

        public ActionResult ExportToReportsList(string tion)
        {
            var myList = (List<HRFacilityMaster>)Session["export"];
            var gv = new GridView();
            gv.DataSource = myList;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return View();
        }

        public ActionResult QueuedRequests(int? page)

        {
            QueuedRequestsViewModel vm = new QueuedRequestsViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 50;

                vm.EventCommand = "search";

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;
              
                vm.HandleRequest();
              
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult QueuedRequests(QueuedRequestsViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 50;
            vm.PageNumber = 1;

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 50;
                vm.PageNumber = 1;
            }
            vm.HandleRequest();            

            if (vm.IsValid)
            {
                TempData["Msg"] = vm.Msg;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

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