using HRViabilityPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace HRViabilityPortal.Controllers
{
    public class RequestStatusController : Controller
    {
        // GET: RequestStatus
        public ActionResult ViewStatus(int? page)

        {
            RequestStatusViewModel vm = new RequestStatusViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                //String username = HttpContext.Session["name"].ToString();
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;
                vm.IsHistoricalListAreaVisible = true;

                vm.EventCommand = "search";

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
        public ActionResult ViewStatus(RequestStatusViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = HttpContext.Session["userID"].ToString();
            vm.loggedInUser = vm.UserId;
            //String username = HttpContext.Session["name"].ToString();

            vm.FacilityReq.staffId = vm.UserId;
            //vm.FacilityReq.accountName = username;

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 10;
                vm.PageNumber = 1;
            }
            vm.HandleRequest();

            if (vm.IsValid)
            {
                TempData["Msg"] = vm.Msg;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;
                vm.IsHistoricalListAreaVisible = true;

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