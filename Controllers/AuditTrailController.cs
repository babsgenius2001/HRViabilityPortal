using HRViabilityPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRViabilityPortal.Controllers
{
    public class AuditTrailController : Controller
    {
        // GET: AuditTrail
        public ActionResult ViewLogs(int? page)

        {
            AuditTrailViewModel vm = new AuditTrailViewModel();
            try
            {
                vm.UserId = HttpContext.Session["userID"].ToString();
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;

                vm.IsSearchAreaVisible = true;
                vm.IsListAreaVisible = true;

                vm.HandleRequest();

                if (vm.AuditLogs.Count() > 0)
                {
                    Session["export"] = vm.AuditLogs;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult ViewLogs(AuditTrailViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = Session["userID"].ToString();

            if (vm.EventCommand == "resetsearch" || vm.EventCommand == "save" || vm.EventCommand == "cancel")
            {
                vm.PageSize = 10;
                vm.PageNumber = 1;
            }
            vm.HandleRequest();

            if (vm.AuditLogs.Count() > 0)
            {
                Session["export"] = vm.AuditLogs;
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
    }
}