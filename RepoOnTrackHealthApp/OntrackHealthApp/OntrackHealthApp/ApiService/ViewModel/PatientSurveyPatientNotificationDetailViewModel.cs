using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public partial class PatientSurveyPatientNotificationDetailViewModel
    {
        public PatientSurveyPatientNotificationDetailViewModel()
        {
            this.PSProcedureNotificationDetails = new List<PSProcedureNotificationDetail>();
        }

        public long PatientNotificationDetailId { get; set; }

        public long NotificationId { get; set; }

        public long? PracticeProfileId { get; set; }

        public long? ProfessionalProfileId { get; set; }

        public long? PatientProfileId { get; set; }

        public long? ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public Guid? PracticeLocationId { get; set; }

        public DateTime? NotificationSchedule { get; set; }

        public int NotificationStatusId { get; set; }

        public bool? IsEmailSent { get; set; }

        public DateTime? EmailSentDateTime { get; set; }

        public DateTime? NotificationDate { get; set; }

        public int NotificationDay { get; set; }

        public string NotificationMonth { get; set; }

        public string FullAddress { get; set; }

        public string NotificationTitle { get; set; }

        public string NotificationHeader { get; set; }

        public bool IsActiveSchedule { get; set; }

        public bool IsLastNotification { get; set; }

        public int NotificationOrder { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public int NotificationTypeId { set; get; }

        public bool HasSurveyQuestion { set; get; }

        public virtual List<PSProcedureNotificationDetail> PSProcedureNotificationDetails { get; set; }

        public virtual PatientSurveyProcedureNotificationViewModel PatientSurveyProcedureNotificationViewModel { get; set; }

        public virtual PatientSurveyPracticeLocationViewModel PatientSurveyPracticeLocationViewModels { get; set; }

        public virtual PatientSurveyQuestionSetViewModel PatientSurveyQuestionSetViewModel { get; set; }
    }
}
