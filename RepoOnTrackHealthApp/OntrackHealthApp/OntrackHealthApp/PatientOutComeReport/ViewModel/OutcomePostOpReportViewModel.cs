using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.PatientOutComeReport.ViewModel
{
    public class OutcomePostOpReportViewModel
    {
        public string SurveyQuestionShortText { get; set; }
        public string SurveyQuestionGroupName { get; set; }
        public string Day0 { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day4 { get; set; }
        public string Day4Cath { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
    }
}
