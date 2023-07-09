using OntrackHealthApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using Acr.UserDialogs;

namespace OntrackHealthApp.ViewModel
{
    public class NotificationListPageViewModel : ViewModelBase
    {
        public ObservableCollection<PatientNotification> PatientNotifications { get; set; }

        public ObservableCollection<PatientSurveyPatientNotificationDetailViewModel> PatientSurveyPatientNotificationDetailViewModels { get; set; }

        private List<PSProcedureNotificationDetail> PSProcedureNotificationDetails { get; set; }

        public Command LoadItemsCommand { get; set; }

        private readonly ITokenContainer _iTokenContainer;
        private readonly IScheduleClient _iScheduleClient;

        public NotificationListPageViewModel()
        {
            PatientNotifications = new ObservableCollection<PatientNotification>();
            PSProcedureNotificationDetails = new List<PSProcedureNotificationDetail>();
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iScheduleClient = new ScheduleClient(apiClient);
        }

        public async Task ExecuteLoadCommandAsync()
        {
            if (IsBusy)
            { return; }

            try
            {
                IsBusy = true;

                PatientNotifications.Clear();

                var response = await _iScheduleClient.SchedulePage();
                if (response.StatusIsSuccessful)
                {
                    var data = response.Data;

                    if (data != null)
                    {

                        #region PatientScheduleHomePageViewModel

                        PatientScheduleHomePageViewModel patientScheduleHomePageViewModel = data;

                        if (patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels.Any())
                        {
                            PatientSurveyPatientNotificationDetailViewModels = new ObservableCollection<PatientSurveyPatientNotificationDetailViewModel>(patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels);

                            foreach (var patientSurveyPatientNotificationDetailViewModel in patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels)
                            {
                                PSProcedureNotificationDetails = patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails;
                                PatientNotification patientNotification = new PatientNotification();
                                patientNotification.NotificationId = patientSurveyPatientNotificationDetailViewModel.NotificationId;
                                patientNotification.NotificationTitle = (patientSurveyPatientNotificationDetailViewModel.NotificationMonth + " " + patientSurveyPatientNotificationDetailViewModel.NotificationDay.ToString() + " - " + patientSurveyPatientNotificationDetailViewModel.NotificationTitle).ToUpper();
                                PatientNotifications.Add(patientNotification);
                            }
                        }
                        #endregion
                    }

                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }

            }
            catch (Exception)
            {
                //Debug.WriteLine(ex);
                IsSuccess = false;
                ErrorMessage = AppConstant.ErrorCommon;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadCommandAsync(string patientProcedureDetailId)
        {
            if (IsBusy)
            { return; }

            try
            {
                IsBusy = true;

                PatientNotifications.Clear();

                var response = await _iScheduleClient.SchedulePage(patientProcedureDetailId);
                if (response.StatusIsSuccessful)
                {
                    var data = response.Data;

                    if (data != null)
                    {

                        #region PatientScheduleHomePageViewModel

                        PatientScheduleHomePageViewModel patientScheduleHomePageViewModel = data;

                        if (patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels.Any())
                        {
                            PatientSurveyPatientNotificationDetailViewModels = new ObservableCollection<PatientSurveyPatientNotificationDetailViewModel>(patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels);

                            foreach (var patientSurveyPatientNotificationDetailViewModel in patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels)
                            {
                                PSProcedureNotificationDetails = patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails;

                                PatientNotification patientNotification = new PatientNotification();
                                patientNotification.NotificationId = patientSurveyPatientNotificationDetailViewModel.NotificationId;
                                patientNotification.NotificationTitle = (patientSurveyPatientNotificationDetailViewModel.NotificationMonth + " " + patientSurveyPatientNotificationDetailViewModel.NotificationDay.ToString() + " - " + patientSurveyPatientNotificationDetailViewModel.NotificationTitle);
                                PatientNotifications.Add(patientNotification);
                            }
                        }

                        #endregion

                    }

                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }

            }
            catch (Exception)
            {
                //Debug.WriteLine(ex);
                IsSuccess = false;
                ErrorMessage = AppConstant.ErrorCommon;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
