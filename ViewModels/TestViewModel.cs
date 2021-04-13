using Newtonsoft.Json;
using PagedList;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using HRViabilityPortal.Database_References;
using HRViabilityPortal.Controllers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using System.Web.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace HRViabilityPortal.ViewModels
{
    public class TestViewModel: ViewModelBase
    {
        public TestViewModel()
        {
            Init();

        }

        public List<SelectDocumentsList> DocumentsList { get; set; }
        public List<AttachmentsList> Attach { get; set; }
        public HttpPostedFileBase uploadedDocument1 { get; set; }
        public HttpPostedFileBase uploadedDocument2 { get; set; }
        public HttpPostedFileBase uploadedDocument3 { get; set; }
        public HttpPostedFileBase uploadedDocument4 { get; set; }
        public HttpPostedFileBase uploadedDocument5 { get; set; }
        public HttpPostedFileBase uploadedDocument6 { get; set; }      
        public HRFacilityMaster FacilityReq { get; set; }
    // public IEnumerable<HttpPostedFileBase> FileBases { get; set; }
        public List<HttpPostedFileBase> FileBases { get; set; }
        public List<DocumentsDAO> MyDocuments { get; set; }
      
        public string _ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HRViabilityPortalConn"].ToString();
        }

        protected override void Init()
        {
            EventCommand = "FacilityList";
          
            DocumentsList = new List<SelectDocumentsList>();
            FileBases = new List<HttpPostedFileBase>();
            ValidationErrors = new List<KeyValuePair<string, string>>();
            MyDocuments = new List<DocumentsDAO>();           

            FacilityReq = new HRFacilityMaster();

            base.Init();
        }

        public override void HandleRequest()
        {
            //// This is an example of adding on a new command
            switch (EventCommand.ToLower())
            {
                
                case "step3":
                    
                    break;
               
                default:
                    GetDocumentNames();
                   
                    IsRow = true;

                    break;
            }

            base.HandleRequest();
        }

        protected void GetDocumentNames()
        {
            String query = "";
            using (var db = new HRViabilityPortalEntities())
            {
                
                    var docType = (from f in db.MurabahaDocumentsTables.Where(x => x.active == "Y" && x.documentName != "Not Applicable") orderby f.documentName ascending select f.documentName).Distinct();
                    foreach (var bn in docType)
                    {
                        SelectDocumentsList item = new SelectDocumentsList();

                        item.Text = bn;
                        item.Value = bn;
                        DocumentsList.Add(item);
                    }       
            }            
        }
        public class SelectFormsList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class SelectDocumentsList
        {
            public string Text { get; set; }
            public string Value { get; set; }
            //public HttpPostedFileBase uploadedDoc { get; set; }
        }

        public class AttachmentsList
        {
           public HttpPostedFileBase uploadedDoc { get; set; }
        }


        public class DocumentsDAO
        {
            public int id { get; set; }
            public string documentName { get; set; }
            public string space { get; set; }
            public bool isSelected { get; set; }
            public HttpPostedFileBase uploadedDocument { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }

        }
    }
}