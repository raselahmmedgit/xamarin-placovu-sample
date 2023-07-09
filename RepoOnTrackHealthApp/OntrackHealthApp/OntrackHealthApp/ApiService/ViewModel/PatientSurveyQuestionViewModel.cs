using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientSurveyQuestionViewModel
    {
        public PatientSurveyQuestionViewModel()
        {
            this.PatientSurveyQuestionDetailViewModels = new List<PatientSurveyQuestionDetailViewModel>();
        }

        public long AutoId { get; set; }

        public long PracticeProfileId { get; set; }

        public long PatientProfileId { get; set; }

        public long SurveyQuestionSetId { get; set; }

        public long SurveyQuestionId { get; set; }

        public long NotificationId { get; set; }

        public string SurveyQuestionText { get; set; }

        public string SurveyQuestionType { get; set; }

        public int? SurveyQuestionTypeId { get; set; }

        public int? QqnDisplayOrder { get; set; }

        public string SelectedAnsware { get; set; }

        public string SelectedAnswareTwo { get; set; }

        public bool DontAskAgain { get; set; }

        public DateTime? NotificationSchedule { get; set; }

        public string QuestionSuggestion { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public virtual List<PatientSurveyQuestionDetailViewModel> PatientSurveyQuestionDetailViewModels { get; set; }
    }

    public class PatientSurveyQuestionCreateModel
    {
        public PatientSurveyQuestionCreateModel()
        {
            this.PatientSurveyQuestionDetailCreateModels = new List<PatientSurveyQuestionDetailCreateModel>();
        }

        /// <summary>
        /// SurveyQuestionSetId
        /// </summary>
        public long SetId { get; set; }

        /// <summary>
        /// SurveyQuestionId
        /// </summary>
        public long QsnId { get; set; }

        /// <summary>
        /// SurveyQuestionTypeId
        /// </summary>
        public int? SqTypeId { get; set; }

        /// <summary>
        /// SelectedAnsware
        /// </summary>
        public string SeAns { get; set; }
        public string SeAns2 { get; set; }

        public virtual List<PatientSurveyQuestionDetailCreateModel> PatientSurveyQuestionDetailCreateModels { get; set; }
    }
}
