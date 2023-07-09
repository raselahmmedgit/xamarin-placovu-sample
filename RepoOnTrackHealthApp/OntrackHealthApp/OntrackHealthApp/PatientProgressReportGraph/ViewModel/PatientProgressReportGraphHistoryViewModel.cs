using OntrackHealthApp.ChartHelper.Models;
using System;

namespace OntrackHealthApp.ViewModel
{
    public class PatientProgressReportGraphHistoryViewModel
    {
        public Guid PatientProgressReportHistoryId { get; set; }

        public long PatientEmailTemplateGraphId { get; set; }

        public string GraphName { get; set; }

        public string GraphController { get; set; }

        public string GraphAction { get; set; }

        public long? SurveyQuestionSetId { get; set; }

        public string GraphDefaultContent { get; set; }

        public string PatientEmailTemplateGraphHtml { get; set; }

        public ChartDataModel ChartDataModel { get; set; }
    }
}
