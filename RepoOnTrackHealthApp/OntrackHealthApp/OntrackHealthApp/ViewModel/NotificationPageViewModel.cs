using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using OntrackHealthApp.Model;
using System.Threading.Tasks;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using System.Linq;

namespace OntrackHealthApp.ViewModel
{
    public class NotificationPageViewModel : ViewModelBase
    {
        public long _patientNotificationDetailId;
        private string _notificationTitle = "";
        private string _notificationHeader = "";
        private string _checkinbuttonText = "";
        private bool _showCheckInButton = false;
        private bool _showNotificationButton = false;
        public readonly ITokenContainer _iTokenContainer;
        public readonly IScheduleClient _iScheduleClient;

        private WebViewSource _notificationHeaderHtmlSrc = null;

        public long PatientNotificationDetailId
        {
            set { SetProperty(ref _patientNotificationDetailId, value); }
            get
            {
                return _patientNotificationDetailId;
            }
        }

        public WebViewSource NotificationHeaderHtmlSrc
        {
            set { SetProperty(ref _notificationHeaderHtmlSrc, value); }
            get
            {
                return _notificationHeaderHtmlSrc;
            }
        }

        public string CheckinbuttonText
        {
            set { SetProperty(ref _checkinbuttonText, value); }
            get
            {
                return _checkinbuttonText;
            }
        }

        public bool ShowCheckInButton
        {
            set { SetProperty(ref _showCheckInButton, value); }
            get
            {
                if (PatientSurveyPatientNotificationDetailViewModel != null)
                {
                    if (PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel != null)
                    {
                        _showCheckInButton = PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel.HasQuestions;
                    }
                }
                return _showCheckInButton;
            }
        }

        public bool ShowNotificationButton
        {
            set { SetProperty(ref _showNotificationButton, value); }
            get
            {
                if (PSProcedureNotificationDetails != null && PSProcedureNotificationDetails.Count > 0)
                {
                    _showNotificationButton = true;
                }
                return _showNotificationButton;
            }
        }
                

        public string NotificationTitle
        {
            set { SetProperty(ref _notificationTitle, value); }
            get
            {
                return _notificationTitle;
            }
        }

        public string NotificationHeader
        {
            set { SetProperty(ref _notificationHeader, value); }
            get
            {
                return _notificationHeader;
            }
        }


        public string NotificationHeaderCustom
        {
            get
            {
                if (string.IsNullOrEmpty(NotificationHeader))
                    return "";
                var str = "<html>"
                    + "<header><style type=\"text/css\">"
                    + "@font-face {"
                    + "font-family:georgia;"
                    + "src: url('Fonts/georgia.ttf');"
                    + "} body{font-family:georgia; font-size:17px; padding:0; margin:0} p{font-size:17px} ul li {font-weight:bold; margin-bottom:10px;font-size:17px;} .top-15 {padding-top:15px;} .left-30 {padding-left:15px;} table{width:100%;}</style></header><body>"
                    + NotificationHeader
                    + "</body></html>";
                return str;
            }
        }
        public ObservableCollection<PatientNotification> PatientNotifications { get; set; }

        public PatientSurveyPatientNotificationDetailViewModel PatientSurveyPatientNotificationDetailViewModel { get; set; }

        public List<PSProcedureNotificationDetail> PSProcedureNotificationDetails { get; set; }

        public NotificationPageViewModel()
        {
            PatientNotifications = new ObservableCollection<PatientNotification>();
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iScheduleClient = new ScheduleClient(apiClient);
            CheckinbuttonText = "Check-In Program";
        }

        public async Task ExecuteLoadLatestPatientNotificationCommandAsync(string patientProcedureDetailId)
        {
            if (IsBusy)
            { return; }

            try
            {
                IsBusy = true;

                Guid guidPatientProcedureDetailId = Guid.Empty;
                Guid.TryParse(patientProcedureDetailId, out guidPatientProcedureDetailId);

                PatientNotifications.Clear();

                var response = await _iScheduleClient.GetLatestPatientNotification(patientProcedureDetailId);
                if (response.StatusIsSuccessful)
                {
                    var data = response.Data;

                    if (data != null)
                    {

                        #region PatientScheduleHomePageViewModel

                        PatientScheduleHomePageViewModel patientScheduleHomePageViewModel = data;

                        if (patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels.Any())
                        {
                            var patientSurveyPatientNotificationDetailViewModel = patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels.FirstOrDefault(item => item.PatientProcedureDetailId == guidPatientProcedureDetailId);
                            NotificationTitle = (patientSurveyPatientNotificationDetailViewModel.NotificationMonth + " " + patientSurveyPatientNotificationDetailViewModel.NotificationDay.ToString() + " - " + patientSurveyPatientNotificationDetailViewModel.NotificationTitle);
                            NotificationHeader = patientSurveyPatientNotificationDetailViewModel.NotificationHeader;
                            CheckinbuttonText = "Check-In Program";

                            //Set For Survey
                            PatientSurveyPatientNotificationDetailViewModel = patientSurveyPatientNotificationDetailViewModel;
                            var patientNotifications = new ObservableCollection<PatientNotification>();
                            PSProcedureNotificationDetails = new List<PSProcedureNotificationDetail>();
                            if (patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails.Any())
                            {
                                foreach (var item in patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails)
                                {
                                    PSProcedureNotificationDetails.Add(item);
                                }
                                foreach (var pSProcedureNotificationDetail in patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails)
                                {
                                    PatientNotification patientNotification = new PatientNotification
                                    {
                                        NotificationId = Convert.ToInt64(pSProcedureNotificationDetail.NotificationId),
                                        NotificationTitle = pSProcedureNotificationDetail.NotificationDetailHeader
                                    };
                                    patientNotifications.Add(patientNotification);
                                    ShowNotificationButton = true;
                                }
                            }
                            PatientNotifications = patientNotifications;
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

                Guid guidPatientProcedureDetailId = Guid.Empty;
                Guid.TryParse(patientProcedureDetailId, out guidPatientProcedureDetailId);

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
                            var patientSurveyPatientNotificationDetailViewModel = patientScheduleHomePageViewModel.PatientSurveyPatientNotificationDetailViewModels.FirstOrDefault(item => item.PatientProcedureDetailId == guidPatientProcedureDetailId);
                            NotificationTitle = (patientSurveyPatientNotificationDetailViewModel.NotificationMonth + " " + patientSurveyPatientNotificationDetailViewModel.NotificationDay.ToString() + " - " + patientSurveyPatientNotificationDetailViewModel.NotificationTitle);
                            NotificationHeader = patientSurveyPatientNotificationDetailViewModel.NotificationHeader;
                            CheckinbuttonText = "Check-In Program";

                            //Set For Survey
                            PatientSurveyPatientNotificationDetailViewModel = patientSurveyPatientNotificationDetailViewModel;
                            //if (patientSurveyPatientNotificationDetailViewModel != null)
                            //{
                            //    ShowCheckInButton = patientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel.HasQuestions;
                            //}
                            //else {
                            //    ShowCheckInButton = false;
                            //}
                            var patientNotifications = new ObservableCollection<PatientNotification>();

                            if (patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails.Any())
                            {
                                PSProcedureNotificationDetails = patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails;

                                foreach (var pSProcedureNotificationDetail in patientSurveyPatientNotificationDetailViewModel.PSProcedureNotificationDetails)
                                {
                                    PatientNotification patientNotification = new PatientNotification
                                    {
                                        NotificationId = Convert.ToInt64(pSProcedureNotificationDetail.NotificationId),
                                        NotificationTitle = pSProcedureNotificationDetail.NotificationDetailHeader
                                    };
                                    patientNotifications.Add(patientNotification);
                                    ShowNotificationButton = true;
                                }
                               
                            }
                            PatientNotifications = patientNotifications;
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
