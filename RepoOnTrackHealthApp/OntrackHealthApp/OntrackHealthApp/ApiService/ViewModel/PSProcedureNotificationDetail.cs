namespace OntrackHealthApp.ApiService.ViewModel
{
    public partial class PSProcedureNotificationDetail
    {
        public long NotificationDetailId { get; set; }

        public long? PracticeProfileId { get; set; }

        public long? NotificationId { get; set; }

        public string NotificationDetailHeader { get; set; }

        public string NotificationDetail { get; set; }

        public long ProcedureId { get; set; }

        public int NotificationOrder { set; get; }

        public string NotificationTitle { get; set; }

        public long PatientNotificationDetailId { get; set; }
    }
}
