using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.PatientOutComeReport.ViewModel
{
    public class MobileSurveyReportViewModel
    {
        public MobileSurveyReportViewModel()
        {
            graphDataProperty = new GraphDataProperty();
            NotificationLabels = new List<string>();
            QuestionLabels = new List<string>();
            OutcomeReportViewModels = new List<OutcomePostOpReportViewModel>();
            ProfessionalSurveyReportMobileMenus = new List<ProfessionalSurveyReportMobileMenu>();
        }
        public long? PatientProfileId { set; get; }
        public GraphDataProperty graphDataProperty { get; set; }
        public List<string> NotificationLabels { get; set; }
        public List<string> QuestionLabels { get; set; }
        public string PatientProcedureDetailId { get; set; }
        public int? ReportType { get; set; }
        public int? OutcomeReportType { get; set; }
        public string NotificationDate { get; set; }
        public string ReportHeader { set; get; }
        public List<OutcomePostOpReportViewModel> OutcomeReportViewModels { set; get; }
        public List<ProfessionalSurveyReportMobileMenu> ProfessionalSurveyReportMobileMenus { set; get; }
    }
}
