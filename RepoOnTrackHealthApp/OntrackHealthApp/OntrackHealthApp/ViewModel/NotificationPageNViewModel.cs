using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.ViewModel
{
    public class NotificationPageNViewModel : ViewModelBase
    {
        #region Property
        private string _notificationTitle = "";
        private string _notificationHeader = "";
        private string _notificationHeaderCustom = "";
        private string _checkinbuttonText = "";
        private bool _showCheckInButton = false;
        private bool _showNotificationButton = false;
        private int _notificationDay = 0;
        private string _notificationDayStr = "";
        private string _notificationMonth = "";
        private DateTime? _notificationSchedule;

        public PatientNotificationShortView PatientNotificationShortView { get; set; }
        public PatientSurveyPatientNotificationDetailViewModel PatientSurveyPatientNotificationDetailViewModel { get; set; }
        public ObservableCollection<PSProcedureNotificationDetail> PSProcedureNotificationDetails { get; set; }
        public Command LoadPatientNotificationCommand { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private readonly IScheduleClient _iScheduleClient;
        public string PatientProcedureDetailId { get; set; }
        public long NotificationId { get; set; }
        private WebViewSource _notificationHeaderHtmlSrc = null;
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
                //if (PatientSurveyPatientNotificationDetailViewModel != null)
                //{
                //    _showCheckInButton = PatientSurveyPatientNotificationDetailViewModel.HasSurveyQuestion;
                //}
                return _showCheckInButton;
            }
        }
        public bool ShowNotificationButton
        {
            set { SetProperty(ref _showNotificationButton, value); }
            get
            {
                //if (PSProcedureNotificationDetails != null && PSProcedureNotificationDetails.Count > 0)
                //{
                //    _showNotificationButton = true;
                //}
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
        public DateTime? NotificationSchedule
        {
            set { SetProperty(ref _notificationSchedule, value); }
            get
            {
                return _notificationSchedule;
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
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var str = "<html>"
                        + "<style type=\"text/css\">"
                        + "@font-face {"
                        + "font-family:georgia;"
                        + "src: url('Fonts/georgia.ttf');"
                        + "} body{font-family:georgia; font-size:17px; padding:0; margin:0} p{font-size:17px} ul li {font-weight:bold; margin-bottom:10px;font-size:17px;} .top-15 {padding-top:15px;} .left-30 {padding-left:15px;} table{width:100%;} tr{border-bottom:1px solid #555;} td{border-bottom:1px solid #555; padding:10px 0px;}</style><body>"
                        + value
                        + "</body></html>";
                    SetProperty(ref _notificationHeaderCustom, str);
                }
                else
                {
                    SetProperty(ref _notificationHeaderCustom, value);
                }
            }
            get
            {
                return _notificationHeaderCustom;
            }
        }
        public string NotificationMonth
        {
            set { SetProperty(ref _notificationMonth, value); }
            get
            {
                return _notificationMonth;
            }
        }
        public int NotificationDay
        {
            set { SetProperty(ref _notificationDay, value); }
            get
            {
                return _notificationDay;
            }
        }
        public string NotificationDayStr
        {
            set { SetProperty(ref _notificationDayStr, value); }
            get
            {
                return _notificationDayStr;
            }
        }

        #endregion

        public NotificationPageNViewModel()
        {
            PatientSurveyPatientNotificationDetailViewModel = new PatientSurveyPatientNotificationDetailViewModel();
            PSProcedureNotificationDetails = new ObservableCollection<PSProcedureNotificationDetail>();           

            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iScheduleClient = new ScheduleClient(apiClient);
            CheckinbuttonText = "Questionnaires";
            LoadPatientNotificationCommand = new Command(async () => { await ExecuteLoadPatientNotificationCommandAsync(); });
        }
        public async Task ExecuteLoadPatientNotificationCommandAsync()
        {
            if (IsBusy) { return; }
            App.ShowUserDialogAsync();
            try
            {
                IsBusy = true;

                PatientSurveyPatientNotificationDetailViewModel = new PatientSurveyPatientNotificationDetailViewModel();
                PSProcedureNotificationDetails = new ObservableCollection<PSProcedureNotificationDetail>();

                var response = await _iScheduleClient.ScheduleByNotificationAndProcedureDetail(NotificationId, PatientProcedureDetailId);

                if (response.StatusIsSuccessful)
                {
                    var patientScheduleHomePageViewModel = response.Data;

                    if (patientScheduleHomePageViewModel != null)
                    {
                        #region PatientScheduleHomePageViewModel
                        if (patientScheduleHomePageViewModel.NotificationSchedule != null)
                        {
                            var dt = patientScheduleHomePageViewModel.NotificationDate.GetValueOrDefault();
                            NotificationDay = dt.Day;
                            NotificationDayStr = dt.Day.ToString();
                            NotificationMonth = dt.ToString("MMM");
                        }
                        NotificationHeader = patientScheduleHomePageViewModel.NotificationHeader;
                        NotificationHeaderCustom = patientScheduleHomePageViewModel.NotificationHeader;
                        NotificationTitle = patientScheduleHomePageViewModel.NotificationTitle;
                        ShowCheckInButton = patientScheduleHomePageViewModel.HasSurveyQuestion;
                        NotificationSchedule = patientScheduleHomePageViewModel.NotificationSchedule;

                        foreach (var item in patientScheduleHomePageViewModel.PSProcedureNotificationDetails)
                        {
                            PSProcedureNotificationDetails.Add(item);
                            ShowNotificationButton = true;
                        }

                        PatientNotificationShortView = new PatientNotificationShortView();
                        PatientNotificationShortView.NotificationId = patientScheduleHomePageViewModel.NotificationId;
                        NotificationId = patientScheduleHomePageViewModel.NotificationId;
                        PatientNotificationShortView.NotificationTitle = patientScheduleHomePageViewModel.NotificationTitle;
                        PatientNotificationShortView.NotificationSchedule = patientScheduleHomePageViewModel.NotificationSchedule;
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
