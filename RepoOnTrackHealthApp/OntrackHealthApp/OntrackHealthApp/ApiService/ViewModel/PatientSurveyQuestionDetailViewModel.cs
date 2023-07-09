using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientSurveyQuestionDetailViewModel
    {
        public long AutoId { get; set; }

        public long PracticeProfileId { get; set; }

        public long PatientProfileId { get; set; }

        public long ProcedureId { get; set; }

        public long SurveyQuestionSetId { get; set; }

        public long? SurveyQuestionDetailId { get; set; }

        public long? SurveyQuestionId { get; set; }

        public string QuestionDetailText { get; set; }

        public bool? QuestionDetailValue { get; set; }

        public long? NextQuestionId { get; set; }

        public long? DisplayOrder { get; set; }

        public string OptionSuggestion { get; set; }

        public string DefaultValue { get; set; }

        public string DefaultValueType { get; set; }

        public DateTime? NotificationSchedule { get; set; }

        public long NotificationId { get; set; }

        public long? DoNotShowNextSetId { get; set; }

        public string SubmitButtonValue { get; set; }

        public string SelectedAnsware { get; set; }

        public bool IsSelected { get; set; }

        public bool DoNotShowNextTime { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

    }

    public class PatientSurveyQuestionDetailCreateModel
    {
        /// <summary>
        /// SurveyQuestionSetId
        /// </summary>
        public long SetId { get; set; }

        /// <summary>
        /// SurveyQuestionDetailId
        /// </summary>
        public long? SqdId { get; set; }

        /// <summary>
        /// SurveyQuestionId
        /// </summary>
        public long? SqId { get; set; }

        /// <summary>
        /// QuestionDetailValue
        /// </summary>
        public bool? Qdv { get; set; }

        /// <summary>
        /// DefaultValue
        /// </summary>
        public string Dv { get; set; }

        /// <summary>
        /// DoNotShowNextSetId
        /// </summary>
        public long? DnnSetId { get; set; }

        /// <summary>
        /// SelectedAnsware
        /// </summary>
        public string Sa { get; set; }


        /// <summary>
        /// IsSelected
        /// </summary>
        public bool Sel { get; set; }


        /// <summary>
        /// DoNotShowNextTime
        /// </summary>
        public bool DnnTime { get; set; }
    }
}
