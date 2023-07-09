using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.ViewModel
{
    public class PatientSurveyPageViewModel : ViewModelBase
    {
        public PatientSurveyPatientNotificationDetailViewModel PatientSurveyPatientNotificationDetailViewModel { get; set; }
        public PatientSurveyQuestionSetViewModel PatientSurveyQuestionSetViewModel { get; set; }
        public PatientSurveyProcedureNotificationViewModel PatientSurveyProcedureNotificationViewModel { get; set; }

        public long NotificationId { get; set; }
        public string NotificationTitle { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private readonly IScheduleClient _iScheduleClient;
        public Command LoadPatientSurveyNotificationDetailCommand { get; set; }

        public PatientSurveyPageViewModel()
        {
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iScheduleClient = new ScheduleClient(apiClient);

            PatientSurveyPatientNotificationDetailViewModel = new PatientSurveyPatientNotificationDetailViewModel();
            PatientSurveyQuestionSetViewModel = new PatientSurveyQuestionSetViewModel();
            PatientSurveyProcedureNotificationViewModel = new PatientSurveyProcedureNotificationViewModel();
            LoadPatientSurveyNotificationDetailCommand = new Command(async () => { await ExecuteLoadPatientSurveyNotificationDetailCommandAsync(); });
        }

        public async Task ExecuteLoadPatientSurveyNotificationDetailCommandAsync()
        {
            if (IsBusy) { return; }
            App.ShowUserDialogAsync();
            try
            {
                IsBusy = true;

                var response = await _iScheduleClient.GetScheduleDetailWithSurvey(NotificationId, _iTokenContainer.CurrentPatientProcedureDetailId);

                if (response.StatusIsSuccessful)
                {
                    PatientSurveyPatientNotificationDetailViewModel = response.Data;
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }
            }
            catch (Exception)
            {
                IsSuccess = false;
                ErrorMessage = AppConstant.ErrorCommon;
                App.HideUserDialogAsync();
            }
            finally
            {
                IsBusy = false;
                App.HideUserDialogAsync();
            }
        }
    }
}
