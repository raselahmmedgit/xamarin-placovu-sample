using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class PatientProgressReportViewModel
    {
        public PatientProgressReportViewModel()
        {
            PatientProgressReportGraphHistoryViews = new ObservableCollection<PatientProgressReportGraphHistoryViewModel>();
        }
        public Guid PatientProgressReportHistoryId { get; set; }
        public string PatientEmailTemplateSalutation { get; set; }
        public string PatientEmailTemplateIntroduction { get; set; }
        public string PatientEmailTemplateConclusion { get; set; }
        public long PatientProfileId { get; set; }
        public string PatientName { get; set; }
        public Guid PatientProcedureDetailId { get; set; }
        public string ProcedureName { get; set; }
        public ObservableCollection<PatientProgressReportGraphHistoryViewModel> PatientProgressReportGraphHistoryViews { get; set; }
        public ObservableCollection<PatientProgressReportResourceHistoryView> PatientProgressReportResourceHistoryViews { get; set; }
        public string PatientEmailTemplateGraphHtml { get; set; }
    }
}
