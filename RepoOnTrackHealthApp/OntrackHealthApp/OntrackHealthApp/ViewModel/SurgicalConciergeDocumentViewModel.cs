using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class SurgicalConciergeDocumentViewModel
    {
        public IEnumerable<SurgicalResourceDocumentViewModel> SurgicalResourceDocumentList { set; get; }
        public string ProcedureName { set; get; }
        public string PracticeLocationName { set; get; }
        public string SurgicalResourceDocumentDetailHtmlWebViewSource { set; get; }
        public PracticeInformationViewModel PracticeInformationViewModel { get; set; }
    }
}
