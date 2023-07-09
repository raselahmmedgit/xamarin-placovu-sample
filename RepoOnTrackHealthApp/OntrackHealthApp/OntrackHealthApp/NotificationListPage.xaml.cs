using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationListPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        NotificationListPageViewModel _notificationListPageViewModel;

        private readonly IProcedureClient _iProcedureClient;

        public NotificationListPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iProcedureClient = new ProcedureClient(apiClient);

                BindingContext = _notificationListPageViewModel = new NotificationListPageViewModel();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public NotificationListPage(NotificationListPageViewModel notificationListPageViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iProcedureClient = new ProcedureClient(apiClient);

                if (!_iTokenContainer.IsApiToken())
                {
                    //Navigation.InsertPageBefore(new LoginPage(), this);
                    Navigation.InsertPageBefore(new LoginPageNew(), this);
                    Navigation.PopAsync();
                }

                BindingContext = _notificationListPageViewModel = notificationListPageViewModel;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public NotificationListPage(string notificationId, string patientProcedureDetailId)
        {
            if (InternetConnectHelper.CheckConnection())
            {                
                using (UserDialogs.Instance.Loading(""))
                {
                    InitializeComponent();
                    _iTokenContainer = new TokenContainer();
                    Title = _iTokenContainer.ApiPracticeName;
                    //Subtitle = _iTokenContainer.CurrentProcedureName;
                    ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
                    var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                    _iProcedureClient = new ProcedureClient(apiClient);

                    _iTokenContainer.HasStartByClickToastNotification = true;
                    LoadNotificationListPageViewModel(notificationId, patientProcedureDetailId);
                }
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public async void LoadNotificationListPageViewModel(string notificationId, string patientProcedureDetailId)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    NotificationListPageViewModel notificationListPageViewModel = new NotificationListPageViewModel();
                    await notificationListPageViewModel.ExecuteLoadCommandAsync(patientProcedureDetailId);
                    BindingContext = _notificationListPageViewModel = notificationListPageViewModel;
                    long _notificaitonid = notificationId != null ? Convert.ToInt64(notificationId) : 0;

                    NotificationPageViewModel notificationPageViewModel = new NotificationPageViewModel();

                    var patientSurveyPatientNotificationDetailViewModel = notificationListPageViewModel.PatientSurveyPatientNotificationDetailViewModels.Where(item => item.NotificationId == _notificaitonid).FirstOrDefault();

                    if (patientSurveyPatientNotificationDetailViewModel != null)
                    {

                        notificationPageViewModel.NotificationTitle = patientSurveyPatientNotificationDetailViewModel.NotificationTitle;
                        notificationPageViewModel.NotificationHeader = patientSurveyPatientNotificationDetailViewModel.NotificationHeader;

                        notificationPageViewModel.PSProcedureNotificationDetails = notificationListPageViewModel.PatientSurveyPatientNotificationDetailViewModels.Where(item => item.NotificationId == _notificaitonid).FirstOrDefault().PSProcedureNotificationDetails;
                        notificationPageViewModel.PatientNotifications = notificationListPageViewModel.PatientNotifications;
                        notificationPageViewModel.PatientSurveyPatientNotificationDetailViewModel = patientSurveyPatientNotificationDetailViewModel;

                        //if (notificationPageViewModel.PSProcedureNotificationDetails != null && notificationPageViewModel.PSProcedureNotificationDetails.Count > 0)
                        //{
                        //    notificationPageViewModel.ShowNotificationButton = true;
                        //}
                        //if (notificationPageViewModel.PatientSurveyPatientNotificationDetailViewModel != null)
                        //{
                        //    notificationPageViewModel.ShowCheckInButton = notificationPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel.HasQuestions;
                        //}
                        //else
                        //{
                        //    notificationPageViewModel.ShowCheckInButton = false;
                        //}

                        await Navigation.PushAsync(new NotificationPage(notificationPageViewModel));

                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

            //BtnSchedule.IsEnabled = false;
            //BtnSchedule.BackgroundColor = Color.FromHex("#f7a50f");
            //BtnSchedule.BorderColor = Color.FromHex("#f7a50f");
        }

        private async void ShowMore_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var btnShowMore = sender as Button;
                    var notificationId = Convert.ToInt32(btnShowMore.ClassId);

                    NotificationPageViewModel model = new NotificationPageViewModel();

                    var patientSurveyPatientNotificationDetailViewModel = _notificationListPageViewModel.PatientSurveyPatientNotificationDetailViewModels.Where(item => item.NotificationId == notificationId).FirstOrDefault();
                    //model.NotificationTitle = patientSurveyPatientNotificationDetailViewModel.NotificationTitle;
                    model.NotificationTitle = (patientSurveyPatientNotificationDetailViewModel.NotificationMonth + " " + patientSurveyPatientNotificationDetailViewModel.NotificationDay.ToString() + " - " + patientSurveyPatientNotificationDetailViewModel.NotificationTitle);
                    model.NotificationHeader = patientSurveyPatientNotificationDetailViewModel.NotificationHeader;

                    model.PSProcedureNotificationDetails = _notificationListPageViewModel.PatientSurveyPatientNotificationDetailViewModels.Where(item => item.NotificationId == notificationId).FirstOrDefault().PSProcedureNotificationDetails;
                    model.PatientNotifications = _notificationListPageViewModel.PatientNotifications;
                    model.PatientSurveyPatientNotificationDetailViewModel = patientSurveyPatientNotificationDetailViewModel;
                    model.PatientNotificationDetailId = patientSurveyPatientNotificationDetailViewModel.PatientNotificationDetailId;
                    await Navigation.PushAsync(new NotificationPage(model));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        #region Bottom Menu Actions

        private async void OnHomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    HomeButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new MainPatientPage());
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
        }

        private async void OnResourceButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await ResourceButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ResourceButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                ResourcePage resourcePage = new ResourcePage();
                await resourcePage.LoadDataAsync();
                await Navigation.PushAsync(resourcePage);
            }
        }

        private async void OnScheduleButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await ScheduleButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ScheduleButton()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                //using (UserDialogs.Instance.Loading(""))
                //{
                //    NotificationListPageViewModel model = new NotificationListPageViewModel();
                //    await model.ExecuteLoadCommandAsync();
                //    await Navigation.PushAsync(new NotificationListPage(model));
                //}

                App.ShowUserDialogAsync();
                await Navigation.PushAsync(new NotificationListPageN());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task<AppMessage> IsCurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }
            else
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            using (UserDialogs.Instance.Loading(""))
            {
                if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
                {
                    return appMessage = await CurrentActiveProcedureWiseButtonShowHideAsync();
                }
                else
                {
                    return appMessage = await CurrentPatientProcedureDetailWiseButtonShowHideAsync();
                }
            }
        }

        private async Task<AppMessage> CurrentActiveProcedureWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentActiveProcedure = await _iProcedureClient.CurrentActiveProcedure();
            if (responseCurrentActiveProcedure.StatusIsSuccessful)
            {
                var data = responseCurrentActiveProcedure.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;

                    #region Physician, Location show and hide

                    if (data.IsSurgeryCompleted)
                    {
                    }
                    else
                    {
                    }

                    #endregion

                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;

                    #region Physician, Location show and hide

                    #endregion

                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetailWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentPatientProcedureDetail = await _iProcedureClient.GetPatientProcedureDetail();
            if (responseCurrentPatientProcedureDetail.StatusIsSuccessful)
            {
                var data = responseCurrentPatientProcedureDetail.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;

                    #region Physician, Location show and hide

                    if (data.IsSurgeryCompleted)
                    {
                    }
                    else
                    {
                    }

                    #endregion

                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;

                    #region Physician, Location show and hide

                    #endregion

                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        #endregion
    }
}