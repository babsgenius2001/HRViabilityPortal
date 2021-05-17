using HRViabilityPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRViabilityPortal.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateFacility(int? page)
        {
            CreateFacilityViewModel vm = new CreateFacilityViewModel();
            vm.IsSearchAreaVisible = false;  
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult CreateFacility(CreateFacilityViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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
                // NOTE: Must clear the model state in order to bind
                //       the @Html helpers to the new model values
                ModelState.Clear();

                vm.NewFacilityReq.facilityName = "";

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

        public ActionResult EditFacility(int? page)
        {
            EditFacilityViewModel vm = new EditFacilityViewModel();
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult EditFacility(EditFacilityViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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

        public ActionResult CreateFacilityRule(int? page)
        {

            CreateFacilityRuleViewModel vm = new CreateFacilityRuleViewModel();

            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();
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
        public ActionResult CreateFacilityRule(CreateFacilityRuleViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            vm.activeStatus = "Y";

            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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
                // NOTE: Must clear the model state in order to bind
                //       the @Html helpers to the new model values
                ModelState.Clear();
                // vm.NewFacilityRule.expectedLengthOfService = 0;
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

        public ActionResult EditFacilityRule(int? page)
        {
            EditFacilityRuleViewModel vm = new EditFacilityRuleViewModel();
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult EditFacilityRule(EditFacilityRuleViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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

        public ActionResult CreateDocuments(int? page)
        {
            CreateDocumentsViewModel vm = new CreateDocumentsViewModel();
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult CreateDocuments(CreateDocumentsViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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
                // NOTE: Must clear the model state in order to bind
                //       the @Html helpers to the new model values
                ModelState.Clear();

                vm.NewDocumentsReq.documentName = "";

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

        public ActionResult EditDocuments(int? page)
        {
            EditDocumentsViewModel vm = new EditDocumentsViewModel();
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult EditDocuments(EditDocumentsViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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

        public ActionResult RemoveDocument(int? page)
        {
            RemoveDocumentsViewModel vm = new RemoveDocumentsViewModel();
            vm.IsSearchAreaVisible = true;
            vm.IsStep1 = true;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult RemoveDocument(RemoveDocumentsViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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

        public ActionResult CreateForms(int? page)
        {
            CreateFormsViewModel vm = new CreateFormsViewModel();
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult CreateForms(CreateFormsViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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
                // NOTE: Must clear the model state in order to bind
                //       the @Html helpers to the new model values
                ModelState.Clear();

                vm.NewFormsReq.formName = "";

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

        public ActionResult EditForms(int? page)
        {
            EditFormsViewModel vm = new EditFormsViewModel();
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult EditForms(EditFormsViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
            //TO DO
            //if (Session["Role"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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

        public ActionResult UpdatePayroll(int? page)
        {
            AdjustPayrollViewModel vm = new AdjustPayrollViewModel();
            vm.IsSearchAreaVisible = false;
         
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult UpdatePayroll(AdjustPayrollViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;
          
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

        public ActionResult UpdateBMDetails(int? page)
        {
            AdjustBMDetailsViewModel vm = new AdjustBMDetailsViewModel();
            vm.IsSearchAreaVisible = false;

            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);
            vm.HandleRequest();
            return View(vm);
        }
        [HttpPost]
        public ActionResult UpdateBMDetails(AdjustBMDetailsViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;

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

        public ActionResult RerouteRequests(int? page)
        {
            RerouteRequestsViewModel vm = new RerouteRequestsViewModel();            
            vm.EventCommand = "rerouteRequest";
            vm.UserId = Session["userID"].ToString();

            vm.PageSize = 10;
            vm.PageNumber = (page ?? 1);

            vm.HandleRequest();

            return View(vm);
        }
        [HttpPost]
        public ActionResult RerouteRequests(RerouteRequestsViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.IsSearchAreaVisible = false;

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