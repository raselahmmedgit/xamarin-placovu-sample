using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientEmailNotificationHistoryViewModel
    {
        public long PatientEmailNotificationHistoryId { get; set; }

        public long PatientNotificationDetailId { get; set; }

        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public string PatientProfileName { get; set; }

        public string PracticeProfileName { get; set; }

        public string ProfessionalProfileName { get; set; }

        public DateTime? NotificationSchedule { get; set; }

        public long NotificationId { get; set; }

        public string NotificationTitle { get; set; }

        public DateTime? NotificationDate { get; set; }

        public int NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public Guid? PatientEmailNotificationHistoryKey { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public string SmsTemplateId { get; set; }

        public string SmsTemplateTypeId { get; set; }

        public string SmsTemplateTitle { get; set; }

        public string SmsTemplateDetail { get; set; }

        public int MobileSentStatus { get; set; }

        public int MobileFailedCount { get; set; }

        public string MobileSentStatusMessage { get; set; }

        public string MobileTemplateTitle { get; set; }

        public string MobileTemplateDetail { get; set; }

    }
}
