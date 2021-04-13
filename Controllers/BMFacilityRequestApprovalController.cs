using HRViabilityPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRViabilityPortal.Controllers
{
    public class BMFacilityRequestApprovalController : Controller
    {
        // GET: BMFacilityRequestApproval
        public ActionResult ApproveBMRequest(int? page)
        {
            BMFacilityRequestApprovalViewModel vm = new BMFacilityRequestApprovalViewModel();
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
        public ActionResult ApproveBMRequest(BMFacilityRequestApprovalViewModel vm)

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


    //    public ActionResult 
    }
}