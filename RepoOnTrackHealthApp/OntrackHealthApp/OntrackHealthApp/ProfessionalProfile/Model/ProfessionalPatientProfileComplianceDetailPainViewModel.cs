using System;
using System.Collections.Generic;
using System.Text;
using static OntrackHealthApp.AppCore.Enums;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class ProfessionalPatientProfileComplianceDetailPainViewModel
    {
        public long SurveyQuestionId { get; set; }

        public string SurveyQuestionAnswareText { get; set; }

        public string SurveyQuestionText
        {
            get
            {
                string surveyQuestionText = "";
                if (SurveyQuestionId == (long)SurveyQuestionIdForEnum.Pain)
                {
                    surveyQuestionText = "Pain level 5";
                }
                else if (SurveyQuestionId == (long)SurveyQuestionIdForEnum.PainPills)
                {
                    surveyQuestionText = "More than 10 pain pills a day";
                }
                return surveyQuestionText;
            }
        }
    }
}
