using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class PatientAggregateSurveyReportModel
    {
        public bool IsShowPatientEmailTemplate { get; set; }
        public PatientAggregateSurveyReportModel()
        {
            List<PatientAggregateSurveyReportView> PatientAggregateSurveyReportViewList = new List<PatientAggregateSurveyReportView>();
        }
        public PatientProfileViewModelForAggregateSurvey PatientProfileViewModel { get; set; }
        public string PatientAge { get; set; }
        public PatientProcedureDetailViewModelForAggregateSurvey PatientProcedureDetailViewModel { get; set; }
        public List<PatientAggregateSurveyReportView> PatientAggregateSurveyReportViewList { get; set; }
        public long isActiveNotificationId { get; set; }
        public bool IsOneWeekNotification { get; set; }
    }

    public class PatientProfileViewModelForAggregateSurvey
    {
        public long PatientProfileId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthStr { get; set; }
        public string PreferredName { get; set; }
    }

    public class PatientProcedureDetailViewModelForAggregateSurvey
    {
        public long ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public Guid PatientProcedureDetailId { get; set; }
    }

    public class PatientAggregateSurveyReportView
    {
        public long NotificationId { get; set; }        public string NotificationShortTitle { get; set; }        public string NotificationAbbreviateTitle { get; set; }        public long SurveyQuestionId { get; set; }        public int SurveyQuestionTypeId { get; set; }        public string SurveyQuestionShortText { get; set; }        public int SurveyQuestionDisplayOrder { get; set; }        public long SurveyQuestionSetId { get; set; }        public string SurveyQuestionSetName { get; set; }        public string SurveyQuestionAnswareText { get; set; }        public Guid? PatientProcedureDetailId { get; set; }        public DateTime? NotificationDate { get; set; }        public int NotificationOrder { get; set; }        public string ControllerName { get; set; }        public string ActionName { get; set; }        public bool? IsShowGraphInPatientSurveyActivity { get; set; }        public string SurveyQuestionDetailText { get; set; }        public long? SurveyQuestionGroupId { get; set; }        public string SurveyQuestionGroupName { get; set; }        public string SurveyQuestionReportText { get; set; }
    }
}
