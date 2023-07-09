using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Response;
using OntrackHealthApp.ApiService.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class ScheduleClient : ClientBase, IScheduleClient
    {
        private readonly ITokenContainer _iTokenContainer;
        private string SchedulePageUri = string.Empty;
        private string SchedulePageByIdUri = "api/PatientProfile/Schedule/";
        private string PatientNotificationLatestByIdUri = "api/PatientProfile/GetLatestPatientNotification/";
        private string PatientNotificationByIdUri = string.Empty;
        private string PatientNotificationsByIdUri = string.Empty;
        private string PatientNotificationCurrentByIdUri = "api/PatientProfile/CurrentNotification";

        public ScheduleClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
            SchedulePageUri = "api/PatientProfile/Schedule/" + _iTokenContainer.CurrentPatientProcedureDetailId;
            PatientNotificationByIdUri = "api/PatientProfile/Notification/" + _iTokenContainer.CurrentPatientProcedureDetailId;
            PatientNotificationsByIdUri = "api/PatientProfile/Notifications/" + _iTokenContainer.CurrentPatientProcedureDetailId;
        }

        public async Task<SchedulePageResponse> SchedulePage()
        {
            return await GetJsonDecodedContent<SchedulePageResponse, PatientScheduleHomePageViewModel>(SchedulePageUri);
        }

        public async Task<SchedulePageResponse> SchedulePage(string patientProcedureDetailId)
        {
            string url = SchedulePageByIdUri + patientProcedureDetailId;
            return await GetJsonDecodedContent<SchedulePageResponse, PatientScheduleHomePageViewModel>(url);
        }

        public async Task<SchedulePageResponse> GetLatestPatientNotification(string patientProcedureDetailId)
        {
            string url = PatientNotificationLatestByIdUri + patientProcedureDetailId;
            return await GetJsonDecodedContent<SchedulePageResponse, PatientScheduleHomePageViewModel>(url);
        }

        public async Task<PatientEmailNotificationHistoryResponse> PatientNotification()
        {
            return await GetJsonDecodedContent<PatientEmailNotificationHistoryResponse, PatientEmailNotificationHistoryViewModel>(PatientNotificationByIdUri);
        }

        public async Task<PatientEmailNotificationHistoryListResponse> PatientNotifications()
        {
            return await GetJsonListDecodedContent<PatientEmailNotificationHistoryListResponse, List<PatientEmailNotificationHistoryViewModel>>(PatientNotificationsByIdUri);
        }

        public async Task<PatientEmailNotificationHistoryResponse> GetPatientNotification()
        {
            return await GetJsonDecodedContent<PatientEmailNotificationHistoryResponse, PatientEmailNotificationHistoryViewModel>(PatientNotificationCurrentByIdUri);
        }

        public async Task<PatientSchedulesUpToDateApiResponse> GetPatientSchedulesUpToDate(string patientProcedureDetailId)
        {
            string url = $"api/PatientSchedule/ScheduleUpToDate/{patientProcedureDetailId}";
            return await GetJsonListDecodedContent<PatientSchedulesUpToDateApiResponse, List<PatientNotificationShortViewFromApi>>(url);
        }
        public async Task<SchedulePageNResponse> ScheduleByNotificationAndProcedureDetail(long notificationId, string patientProcedureDetailId)
        {
            string url = "";
            if (notificationId == 0)
            {
                url = $"api/PatientSchedule/LatestScheduleByProcedureDetail/{patientProcedureDetailId}";
            }
            else
            {
                url = $"api/PatientSchedule/ScheduleByNotificationAndProcedureDetail/{notificationId}/{patientProcedureDetailId}";
            }
            return await GetJsonDecodedContent<SchedulePageNResponse, PatientSurveyPatientNotificationDetailViewModel>(url);
        }
        public async Task<NotificationDetailPageResponse> GetScheduleDetailContent(long notificationDetailId)
        {
            string url = $"api/PatientSchedule/ScheduleDetailContent/{notificationDetailId}";
            return await GetJsonDecodedContent<NotificationDetailPageResponse, PSProcedureNotificationDetail>(url);
        }
        public async Task<ScheduleDetailWithSurveyPageResponse> GetScheduleDetailWithSurvey(long notificationId, string patientProcedureDetailId)
        {
            string url = $"api/PatientSchedule/ScheduleDetailWithSurvey/{notificationId}/{patientProcedureDetailId}";
            return await GetJsonDecodedContent<ScheduleDetailWithSurveyPageResponse, PatientSurveyPatientNotificationDetailViewModel>(url);
        }
    }

    public interface IScheduleClient
    {
        Task<SchedulePageResponse> SchedulePage();
        Task<SchedulePageResponse> SchedulePage(string patientProcedureDetailId);
        Task<SchedulePageResponse> GetLatestPatientNotification(string patientProcedureDetailId);
        Task<PatientEmailNotificationHistoryResponse> PatientNotification();
        Task<PatientEmailNotificationHistoryListResponse> PatientNotifications();
        Task<PatientEmailNotificationHistoryResponse> GetPatientNotification();
        Task<PatientSchedulesUpToDateApiResponse> GetPatientSchedulesUpToDate(string patientProcedureDetailId);
        Task<SchedulePageNResponse> ScheduleByNotificationAndProcedureDetail(long notificationId, string patientProcedureDetailId);
        Task<NotificationDetailPageResponse> GetScheduleDetailContent(long notificationDetailId);
        Task<ScheduleDetailWithSurveyPageResponse> GetScheduleDetailWithSurvey(long notificationId, string patientProcedureDetailId);
    }
}
