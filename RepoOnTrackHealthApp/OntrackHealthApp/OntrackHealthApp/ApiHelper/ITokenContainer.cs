using OntrackHealthApp.ApiService.Model;

namespace OntrackHealthApp.ApiHelper
{
    public interface ITokenContainer
    {
        object ApiToken { get; set; }

        bool IsApiToken();

        void ClearApiToken();

        #region User Identity

        string ApiUserId { get; set; }

        string ApiUserRole { get; set; }

        string ApiUserNameDisplay { get; set; }

        string ApiUserEmail { get; set; }

        long? ApiPracticeProfileId { get; set; }

        long? ApiPatientProfileId { get; set; }

        bool ApiIsSystemAdmin { get; set; }

        bool ApiIsAdmin { get; set; }

        string ApiPracticeName { get; set; }

        string ApiPracticeLocationName { get; set; }

        string ApiProcedureName { get; set; }

        string ApiMobileNotificationIntervalMinute { get; set; }

        string ApiMobileNotificationElapsedMinute { get; set; }

        bool ApiIsAgreePatientMonitoringConsent { get; set; }

        bool ApiIsDoNotAgreePatientMonitoringConsent { get; set; }

        long CurrentProcedureId { get; set; }

        string CurrentProcedureName { get; set; }

        string CurrentPatientProcedureDetailId { get; set; }

        bool HasSendToastNotification { get; set; }

        bool HasStartByClickToastNotification { get; set; }

        long? ApiProfessionalProfileId { get; set; }

        string ApiProfessionalProfileName { get; set; }

        UserIdentityModel GetUserIdentityModel();

        void SetUserIdentityModel(UserIdentityModel model);

        ApplicationSettingModel GetApplicationSettingModel();

        void SetApplicationSettingModel(ApplicationSettingModel model);

        #endregion


        bool IsUsingLatestVersion { get; set; }

        string GooglePlayStoreAppUrl { get; set; }

        string AppStoreAppUrl { get; set; }

        string AppStoreAppName { get; set; }

        string AndroidVersionCode { get; set; }
    }
}
