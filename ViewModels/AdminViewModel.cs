using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRViabilityPortal.ViewModels
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            Init();
            
            EventCommand = "add";
            EventCommand2 = "show";
            EventCommand3 = "search";
            EventCommand4 = "myupload";
            ValidationErrors = new List<KeyValuePair<string, string>>();
            EventArgument = string.Empty;
            IsDisplayBulkEntryVisible = false;
            IsDisplayBulkForPrintAreaVisible = false;
            IsUploadRecordAreaVisible = true;
        }

        
        public string EventCommand { get; set; }
        public string EventCommand2 { get; set; }
        public string EventCommand3 { get; set; }
        public string EventCommand4 { get; set; }
        public string EventArgument { get; set; }
        public string EventArgument2 { get; set; }
        public string EventArgument3 { get; set; }       
        public string CreatedOn { get; set; }
        public bool IsValid { get; set; }
        public bool IsAddTransactionAreaVisible { get; set; }
        public bool IsSearchPilgrimsAreaVisible { get; set; }
        public bool IsDisplayBulkEntryVisible { get; set; }
        public bool IsDisplayTransactionAreaVisible { get; set; }
        public bool IsEditTransactionAreaVisible { get; set; }
        public bool IsDisplayBulkForPrintAreaVisible { get; set; }
        public bool IsUploadRecordAreaVisible { get; set; }
        public string Mode { get; set; }
        public string UserID { get; set; }
        public string UserRole { get; set; }        
        public int TotalNoOfRequest { get; set; }
        public int ApprovedRequests { get; set; }
        public int BranchRequests { get; set; }
        public int RejectedRequests { get; set; }
        public bool IsAllData { get; set; }
        public string branchCode { get; set; }

        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

       public void HandleRequest()
        {
            switch (EventCommand.ToLower())
            {
                case "add":
                    // Add();
                    IsAllData = true;
                    break;
                case "display":
                    //Display();
                    break;
                case "save":
                    //  Save();
                    if (IsValid)
                    {
                        if (Mode == "Edit")
                        {
                            //  DisplayAfterEdit();
                        }
                        else
                        {
                            // Display();
                        }
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            //  Edit();
                            IsValid = false;
                        }
                        else
                        {
                            //  Add();
                            IsValid = false;
                        }

                    }
                    break;
                case "edit":
                    // Edit();
                    break;
                case "commit":
                    //Commit();
                    break;
                case "cancel":
                    // Add();
                    break;
                case "download":
                    //  GetForExport();
                    // Add();
                    break;
                case "print":
                    //  GetForExport();
                    // Add();
                    break;
                case "generate":
                    //  GetForGenerate();
                    IsSearchPilgrimsAreaVisible = true;
                    break;
                case "generateexport":
                    //GetForGenerate();
                    break;

                default:
                    IsAllData = true;
                    break;
            }
        }

        public void AddMode()
        {
            IsAddTransactionAreaVisible = true;
            IsDisplayTransactionAreaVisible = false;
            IsEditTransactionAreaVisible = false;
            Mode = "add";
        }
        public void DisplayMode()
        {
            IsAddTransactionAreaVisible = false;
            IsDisplayTransactionAreaVisible = true;
            IsEditTransactionAreaVisible = false;
            Mode = "display";
        }
        public void EditMode()
        {
            IsAddTransactionAreaVisible = false;
            IsDisplayTransactionAreaVisible = false;
            IsEditTransactionAreaVisible = true;
            Mode = "Edit";
        }
      
        public void DisplayBulk()
        {
            IsDisplayBulkEntryVisible = true;
        }
        
        public void Init()
        {
            IsValid = true;
            IsSearchPilgrimsAreaVisible = true;
            EventCommand = "add";
            EventCommand2 = "show";
            EventCommand3 = "search";
            EventCommand4 = "myupload";
            ValidationErrors = new List<KeyValuePair<string, string>>();
            EventArgument = string.Empty;
            AddMode();
        }        

        public class SelectList
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public bool Selected { get; set; }
        }
    }
}