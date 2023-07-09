using OntrackHealthApp.ApiHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public partial class PatientSurveyProcedureNotificationViewModel : ApiModel
    {
        public PatientSurveyProcedureNotificationViewModel()
        {
            this.PatientSurveyQuestionSetViewModels = new List<PatientSurveyQuestionSetViewModel>();
        }
        public long NotificationId { get; set; }

        public long PatientNotificationDetailId { get; set; }

        public string NotificationIdAuto { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProcedureId { get; set; }

        public string NotificationTitle { get; set; }

        public string NotificationNote { get; set; }

        public int NotificationSendDay { get; set; }

        public bool? IsPostNotification { get; set; }

        public bool? IsPreNotification { get; set; }

        public long? SurveyQuestionSetId { get; set; }

        public string NotificationHeader { get; set; }

        public int NotificationTypeId { get; set; }

        public string EmailTemplateId { get; set; }

        public int? EmailPriorityId { get; set; }

        public bool? IsDeleted { get; set; }

        public string TagOne { get; set; }

        public string TagTwo { get; set; }

        public long ProfessionalProfileId { get; set; }

        public bool IsLastNotification { get; set; }

        public int NotificationOrder { set; get; }

        public Guid PatientProcedureDetailId { get; set; }

        public virtual List<PatientSurveyQuestionSetViewModel> PatientSurveyQuestionSetViewModels { get; set; }

    }

    public partial class PatientSurveyProcedureNotificationCreateModel : ApiModel
    {
        public PatientSurveyProcedureNotificationCreateModel()
        {
            this.PatientSurveyQuestionCreateModels = new List<PatientSurveyQuestionCreateModel>();
        }

        /// <summary>
        /// PatientProfileId
        /// </summary>
        public long PaId { get; set; }

        /// <summary>
        /// NotificationId
        /// </summary>
        public long NoId { get; set; }

        /// <summary>
        /// PatientNotificationDetailId
        /// </summary>
        public long PaNoDtId { get; set; }

        /// <summary>
        /// PracticeProfileId
        /// </summary>
        public long PrId { get; set; }

        /// <summary>
        /// ProcedureId
        /// </summary>
        public long ProcId { get; set; }

        /// <summary>
        /// SurveyQuestionSetId
        /// </summary>
        public long? SetId { get; set; }

        /// <summary>
        /// ProfessionalProfileId
        /// </summary>
        public long ProId { get; set; }

        /// <summary>
        /// IsLastNotification
        /// </summary>
        public bool IsLast { get; set; }

        /// <summary>
        /// PatientProcedureDetailId
        /// </summary>
        public Guid ProcdId { get; set; }

        /// <summary>
        /// NotificationOrder
        /// </summary>
        public int Order { set; get; }

        public virtual List<PatientSurveyQuestionCreateModel> PatientSurveyQuestionCreateModels { get; set; }

    }
}
