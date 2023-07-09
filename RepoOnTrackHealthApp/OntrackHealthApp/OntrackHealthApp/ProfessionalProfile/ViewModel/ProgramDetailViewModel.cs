using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.Model;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProgramDetailViewModel
    {
        public ProgramDetailViewModel()
        {
            ProcedureNotification = new ProcedureNotification();
            ProcedureNotifications = new List<ProcedureNotification>();
            ProcedureNotificationDetails = new List<ProcedureNotificationDetail>();
            ProcedureNotificationSurveys = new List<ProcedureNotificationSurvey>();
            SurveyQuestionSets = new List<SurveyQuestionSet>();
            SurveyQuestions = new List<SurveyQuestion>();
            SurveyQuestionDetails = new List<SurveyQuestionDetail>();
        }

        public IEnumerable<ProcedureNotification> ProcedureNotifications { get; set; }

        public ProcedureNotification ProcedureNotification { get; set; }

        public IEnumerable<ProcedureNotificationDetail> ProcedureNotificationDetails { get; set; }
        public IEnumerable<ProcedureNotificationSurvey> ProcedureNotificationSurveys { get; set; }
        public IEnumerable<SurveyQuestionSet> SurveyQuestionSets { get; set; }
        public IEnumerable<SurveyQuestion> SurveyQuestions { get; set; }
        public IEnumerable<SurveyQuestionDetail> SurveyQuestionDetails { get; set; }

    }
}
