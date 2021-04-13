using HRViabilityPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRViabilityPortal.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }
        public ActionResult AddRow(int? page)

        {
            TestViewModel vm = new TestViewModel();
            try
            {
                vm.PageNumber = (page ?? 1);
                vm.PageSize = 10;

                vm.IsRow = true;                

                vm.HandleRequest();
            }
            catch (Exception ex)
            {
                return RedirectToAction("index", "Home");
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddRow(TestViewModel vm)

        {

            vm.IsValid = ModelState.IsValid;

            vm.UserId = HttpContext.Session["userID"].ToString();

            int k = 0;
            string Msg;

            //if (vm.FileBases != null)
            //{
            //    string _FileNameA = Path.GetFileName(vm.FileBases.Count.);
            //}

                if (vm.DocumentsList != null & vm.DocumentsList.Count != 0)
                {

                String query = "INSERT INTO tbl_AttachmentsMaster (requestReferenceNumber, fileName, fileContentType, fileData) "
                    + " VALUES (@requestReferenceNumber, @fileName, @fileContentType, @fileData)";

                using (SqlConnection Connect = new SqlConnection(_ConnectionString()))
                {

                    using (SqlCommand Cmd = new SqlCommand(query, Connect))
                    {
                        try
                        {

                            while (k < vm.DocumentsList.Count)
                            {
                                string _FileName = Path.GetFileName(vm.uploadedDocument1.FileName);
                                string _FileName1 = Path.GetFileName(vm.uploadedDocument2.FileName);
                                string _FileName2 = Path.GetFileName(vm.uploadedDocument3.FileName);
                                //string _FileName = Path.GetFileName(vm.uploadedDocument1.FileName);
                                //string _FileName = Path.GetFileName(vm.uploadedDocument1.FileName);

                                //string _path = Path.Combine(Server.MapPath("~/Upload"), _FileName);
                                string _path = "~//upload//" + _FileName;
                                string _ref = "ATT" + Convert.ToString(Guid.NewGuid());

                                Cmd.Parameters.AddWithValue("@requestReferenceNumber", _ref);
                                //Cmd.Parameters.AddWithValue("@fileName", _FileName);
                                Cmd.Parameters.AddWithValue("@fileName", _FileName);
                                Cmd.Parameters.AddWithValue("@fileContentType", vm.DocumentsList[k].Value);
                                Cmd.Parameters.AddWithValue("@fileData", _path);

                                Connect.Open();

                                Cmd.ExecuteNonQuery();

                                Connect.Close();

                                Cmd.Parameters.Clear();

                                k++;

                            }

                        }
                        catch (Exception ex)
                        {
                            Msg = "submission exception:  " + ex.ToString();

                        }

                    }
                }
            }


            //    for (int k = 0; k < vm.DocumentsList.Count; k++)
            //    {                    

            //        if (vm.uploadedDocument != null && vm.uploadedDocument.ContentLength > 0)
            //        {
            //            string _FileName = Path.GetFileName(vm.uploadedDocument.FileName);
            //            string _path = Path.Combine(Server.MapPath("~/Upload"), _FileName);
            //            string contentType = vm.AttachmentReq.fileContentType;



            //            vm.uploadedDocument.SaveAs(_path);
            //            vm.AttachmentReq.fileName = _FileName;
            //        }
            //    }

            //}


            //if (vm.uploadedDocument1 != null && vm.uploadedDocument1.ContentLength > 0)
            //{
            //    string _FileName = Path.GetFileName(vm.uploadedDocument1.FileName);
            //    string _path = Path.Combine(Server.MapPath("~/Upload"), _FileName);
            //    vm.uploadedDocument1.SaveAs(_path);
            //    vm.FacilityReq.fileName1 = _FileName;
            //}

            //if (vm.uploadedDocument2 != null && vm.uploadedDocument2.ContentLength > 0)
            //{
            //    string _FileName = Path.GetFileName(vm.uploadedDocument2.FileName);
            //    string _path = Path.Combine(Server.MapPath("~/Upload"), _FileName);
            //    vm.uploadedDocument2.SaveAs(_path);
            //    vm.FacilityReq.fileName2 = _FileName;
            //}

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