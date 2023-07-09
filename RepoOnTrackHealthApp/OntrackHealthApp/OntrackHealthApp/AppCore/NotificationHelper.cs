using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OntrackHealthApp.AppCore
{
    public class NotificationHelper
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IScheduleClient _iScheduleClient;

        public NotificationHelper()
        {
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iScheduleClient = new ScheduleClient(apiClient);
        }

        public async Task PullNotificationAsync()
        {
            if (_iTokenContainer.IsApiToken())
            {
                var patientEmailNotificationHistoryViewModel = await GetPatientEmailNotificationHistoryViewModel();
                if(patientEmailNotificationHistoryViewModel != null)
                {
                    if (!string.IsNullOrEmpty(patientEmailNotificationHistoryViewModel.MobileTemplateTitle) && !string.IsNullOrEmpty(patientEmailNotificationHistoryViewModel.MobileTemplateDetail))
                    {
                        ToastHelper.ShowPushNotification(patientEmailNotificationHistoryViewModel.MobileTemplateTitle, patientEmailNotificationHistoryViewModel.MobileTemplateDetail, patientEmailNotificationHistoryViewModel.NotificationId.ToString(), patientEmailNotificationHistoryViewModel.PatientProcedureDetailId.ToString());
                    }
                }

                _iTokenContainer.HasSendToastNotification = true;
            }
        }

        public async Task PullNotificationListAsync()
        {
            if (_iTokenContainer.IsApiToken())
            {
                var patientEmailNotificationHistoryViewModelList = await GetPatientEmailNotificationHistoryViewModels();

                foreach (var patientEmailNotificationHistoryViewModel in patientEmailNotificationHistoryViewModelList)
                {
                    if (patientEmailNotificationHistoryViewModel != null)
                    {
                        if (!string.IsNullOrEmpty(patientEmailNotificationHistoryViewModel.MobileTemplateTitle) && !string.IsNullOrEmpty(patientEmailNotificationHistoryViewModel.MobileTemplateDetail))
                        {
                            ToastHelper.ShowPushNotification(patientEmailNotificationHistoryViewModel.MobileTemplateTitle, patientEmailNotificationHistoryViewModel.MobileTemplateDetail, patientEmailNotificationHistoryViewModel.NotificationId.ToString(), patientEmailNotificationHistoryViewModel.PatientProcedureDetailId.ToString());
                        }
                    }
                }

                _iTokenContainer.HasSendToastNotification = true;
            }
        }

        private async Task<PatientEmailNotificationHistoryViewModel> GetPatientEmailNotificationHistoryViewModel()
        {
            PatientEmailNotificationHistoryViewModel patientEmailNotificationHistoryViewModel = new PatientEmailNotificationHistoryViewModel();
            try
            {
                #region Api Call

                var response = await _iScheduleClient.PatientNotification();
                if (response.StatusIsSuccessful)
                {
                    patientEmailNotificationHistoryViewModel = response.Data;
                }
                else
                {
                }

                #endregion
            }
            catch
            {

            }

            return patientEmailNotificationHistoryViewModel;
        }

        private async Task<List<PatientEmailNotificationHistoryViewModel>> GetPatientEmailNotificationHistoryViewModels()
        {
            List<PatientEmailNotificationHistoryViewModel> patientEmailNotificationHistoryViewModelList = new List<PatientEmailNotificationHistoryViewModel>();
            try
            {
                #region Api Call

                var response = await _iScheduleClient.PatientNotifications();
                if (response.StatusIsSuccessful)
                {
                    patientEmailNotificationHistoryViewModelList = response.DataList;
                }
                else
                {
                }

                #endregion
            }
            catch
            {

            }

            return patientEmailNotificationHistoryViewModelList;
        }
    }
}
