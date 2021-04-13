using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HRViabilityPortal.ViewModels
{
    public class ViewModelBase
    {
        public ViewModelBase()
        {
            Init();
        }
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public string staffCategory { get; set; }
        public int CIF { get; set; }
        public int noOfMonthsInService { get; set; }   
        public int gradeSalary { get; set; }
        public string StaffName { get; set; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        public string Mode { get; set; }
        public bool IsValid { get; set; }
        public string EventCommand { get; set; }
        public string EventArgument { get; set; }
        public string EventCommand2 { get; set; }
        public string EventArgument2 { get; set; }
        public bool IsDetailAreaVisible { get; set; }
        public bool IsListAreaVisible { get; set; }
        public bool IsSearchAreaVisible { get; set; }
        public bool IsHistoricalListAreaVisible { get; set; }
        public bool IsBranch { get; set; }
        public bool IsOthers { get; set; }
        public bool IsRow { get; set; }
        public string pageInfo { get; set; }
        public string Msg { get; set; }
        public IPagedList PagedList { get; set; }
        public bool IsNextAreaVisible { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int maxRows { get; set; }

        protected virtual void Init()
        {
            EventCommand = "List";
            EventArgument = string.Empty;
            ValidationErrors = new List<KeyValuePair<string, string>>();
          
            ListMode();
        }

        protected virtual void Get()
        {

        }

        protected virtual void GetQueuedItems()
        {

        }

        protected virtual void GetHistory()
        {

        }

        public virtual void HandleRequest()
        {
            switch (EventCommand.ToLower())
            {
                case "list":
                case "search":
                    Get();
                   // GetQueuedItems();
                    break;
                case "historical":
                   
                    GetHistory();
                    break;

                case "getallgrades":
                    GetGrades();
                    break;

                case "add":
                    Add();
                    break;

                case "edit":
                    IsValid = true;
                    Edit();
                    break;

                //case "editqueue":
                //    IsValid = true;
                //    EditQueued();
                //    break;

                case "delete":
                    ResetSearch();
                    Delete();
                    break;

                case "save":
                    Save();
                    Get();
                    break;

                case "cancel":
                    ListMode();
                    Get();
                    break;

                case "resetsearch":
                    ResetSearch();
                    Get();
                    break;
            }
        }

        protected virtual void ListMode()
        {
            IsValid = true;
            IsDetailAreaVisible = false;
            IsListAreaVisible = true;
            IsSearchAreaVisible = true;

            Mode = "List";
        }

        protected virtual void AddMode()
        {
            IsDetailAreaVisible = true;
            IsListAreaVisible = false;
            IsSearchAreaVisible = false;

            Mode = "Add";
        }

        protected virtual void EditMode()
        {
            IsDetailAreaVisible = true;
            IsListAreaVisible = false;
            IsSearchAreaVisible = false;

            Mode = "Edit";
        }
        

        //protected virtual void EditQueueMode()
        //{
        //    IsDetailAreaVisible = true;
        //    IsListAreaVisible = false;
        //    IsSearchAreaVisible = false;

        //    Mode = "EditQueued";
        //}

        protected virtual void Add()
        {

            // Put ViewModel into Add Mode
            AddMode();
        }

        protected virtual void GetGrades()
        {

        }
        protected virtual void Edit()
        {

            // Put View Model into Edit Mode
            EditMode();
        }

        //protected virtual void EditQueued()
        //{

        //    // Put View Model into Edit Mode
        //    EditQueueMode();
        //}

        protected virtual void Delete()
        {

            // Set back to normal mode
            ListMode();
        }

        protected virtual void Save()
        {
            if (ValidationErrors.Count > 0)
            {
                IsValid = false;
            }

            if (!IsValid)
            {
                if (Mode == "Add")
                {
                    AddMode();
                }
                else
                {
                    EditMode();
                }
            }
        }

        protected virtual void ResetSearch()
        {
        }


    }
}