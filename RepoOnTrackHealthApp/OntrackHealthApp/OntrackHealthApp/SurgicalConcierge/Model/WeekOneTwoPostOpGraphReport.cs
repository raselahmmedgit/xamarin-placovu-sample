using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class WeekOneTwoPostOpGraphReport
    {
        public long SurveyQuestionId { get; set; }
        public string SurveyQuestionShortText { get; set; }

        public string TwoDayPatientSurvey { get; set; }
        public string FourDayPatientSurvey { get; set; }
        public string SixDayPatientSurvey { get; set; }

        public string PatientSurvey { get; set; }
        public string ProfessionalSurvey { get; set; }
        public string PracticeSurvey { get; set; }
        public string AllSurvey { get; set; }

        public decimal? PatientSurveyAverage { get; set; }
        public decimal? ProfessionalSurveyAverage { get; set; }
        public decimal? PracticeSurveyAverage { get; set; }
        public decimal? AllSurveyAverage { get; set; }
    }
}
