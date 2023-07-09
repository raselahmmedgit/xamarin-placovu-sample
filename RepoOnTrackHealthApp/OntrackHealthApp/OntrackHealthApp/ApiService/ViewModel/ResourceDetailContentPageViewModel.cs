using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ResourceDetailContentPageViewModel
    {
        public int ResourceDetailContentPageViewModelId { get; set; }
        public string ProcedureName { get; set; }
        public Guid PatientProcedureDetailId { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string HtmlDocument { get; set; }
    }
}
