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
    public class HRFacilityRequestApprovalController : Controller
    {
        // GET: HRFacilityRequestApprover
        public ActionResult ApproveRequest(int? page)

        {
            HRFacilityRequestApprovalViewModel vm = new HRFacilityRequestApprovalViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;
                vm.HandleRequest();
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult ApproveRequest(HRFacilityRequestApprovalViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = Session["userID"].ToString();

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 10;
                vm.PageNumber = 1;
            }
            vm.HandleRequest();

            if (vm.IsValid)
            {
                TempData["Msg"] = vm.Msg;

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