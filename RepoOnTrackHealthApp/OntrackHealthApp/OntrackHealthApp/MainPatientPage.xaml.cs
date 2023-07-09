using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.iOS;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OntrackHealthApp
{
    public partial class MainPatientPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IProcedureClient _iProcedureClient;
        private AdministrationPatientProfileRestApiService restApiService;

        public MainPatientPage()
        {
            try
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iProcedureClient = new ProcedureClient(apiClient);

                restApiService = new AdministrationPatientProfileRestApiService();

                DependencyService.Get<IToast>().SetNotificationSettings();
            }
            catch (Exception)
            {
                DisplayAlert(string.Empty, AppConstant.ApplicationExceptionError, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        public MainPatientPage(string notificationId, string patientProcedureDetailId)
        {
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    InitializeComponent();
                    _iTokenContainer = new TokenContainer();
                    Title = _iTokenContainer.ApiPracticeName;
                    ProcedureName.Text = _iTokenContainer.CurrentProcedureName != null ? _iTokenContainer.CurrentProcedureName : "";
                    var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                    _iProcedureClient = new ProcedureClient(apiClient);
                    restApiService = new AdministrationPatientProfileRestApiService();
                    GetPushNotificationDetail(notificationId, patientProcedureDetailId);
                }

            }
            catch (Exception)
            {
                DisplayAlert(string.Empty, AppConstant.ApplicationExceptionError, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private async void GetPushNotificationDetail(string notificationId, string patientProcedureDetailId)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                var notificationList = await restApiService.GetNotificationListDetail(patientProcedureDetailId);
                await ShowPushNotificationDetail(notificationId, patientProcedureDetailId,notificationList);
            }
        }

        private async Task ShowPushNotificationDetail(string notificationId, string patientProcedureDetailId, PatientScheduleHomePageViewModel notificationList)
        {
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    NotificationListPageViewModel notificationListPageViewModel = new NotificationListPageViewModel();
                    // await notificationListPageViewModel.ExecuteLoadCommandAsync(patientProcedureDetailId);
                    
                    long _notificaitonid = notificationId != null ? Convert.ToInt64(notificationId) : 0;

                    if (notificationList != null && notificationList.PatientSurveyPatientNotificationDetailViewModels != null && notificationList.PatientSurveyPatientNotificationDetailViewModels.Any())
                    {
                        NotificationPageViewModel notificationPageViewModel = new NotificationPageViewModel();

                        var patientSurveyPatientNotificationDetailViewModel = notificationList.PatientSurveyPatientNotificationDetailViewModels.Where(item => item.NotificationId == _notificaitonid).FirstOrDefault();

                        if (patientSurveyPatientNotificationDetailViewModel != null)
                        {

                            notificationPageViewModel.NotificationTitle = patientSurveyPatientNotificationDetailViewModel.NotificationTitle;
                            notificationPageViewModel.NotificationHeader = patientSurveyPatientNotificationDetailViewModel.NotificationHeader;

                            notificationPageViewModel.PSProcedureNotificationDetails = notificationList.PatientSurveyPatientNotificationDetailViewModels.Where(item => item.NotificationId == _notificaitonid).FirstOrDefault().PSProcedureNotificationDetails;
                            //notificationPageViewModel.PatientNotifications = notificationListPageViewModel.PatientNotifications;
                            notificationPageViewModel.PatientSurveyPatientNotificationDetailViewModel = patientSurveyPatientNotificationDetailViewModel;

                            await Navigation.PushAsync(new NotificationPage(notificationPageViewModel));

                        }
                        else
                        {
                            await DisplayAlert(AppConstant.DisplayAlertErrorTitle, "Not active procedure notification", AppConstant.DisplayAlertErrorButtonText);
                        }
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, "Not active procedure notification", AppConstant.DisplayAlertErrorButtonText);
                    }
                }

            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnCheckinButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                btnCheckin.IsEnabled = false;
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await CheckinButton();
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

        private async Task CheckinButton()
        {
            //using (UserDialogs.Instance.Loading(""))
            //{
            //    NotificationPageViewModel model = new NotificationPageViewModel();
            //    await model.ExecuteLoadLatestPatientNotificationCommandAsync(_iTokenContainer.CurrentPatientProcedureDetailId);
            //    await Navigation.PushAsync(new NotificationPage(model));
            //}
            await Navigation.PushAsync(new NotificationPageN());
        }

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

        private async void OnScheduleButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                if (!string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
                {
                    await ScheduleButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
                //var appMessage = await IsCurrentPatientProcedureDetail();
                //if (appMessage.MessageType == AppMessageType.Success)
                //{

                //}
                //else
                //{
                //    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                //}
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ScheduleButton()
        {
            await Navigation.PushAsync(new NotificationListPageN());
        }

        private async void OnPhysicianButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await PhysicianButton();
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

        private async Task PhysicianButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                await Navigation.PushAsync(new PhysicianProfilePage());
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
            ResourcePage resourcePage = new ResourcePage();
            await resourcePage.LoadDataAsync();
            await Navigation.PushAsync(resourcePage);
        }

        private async void OnLocationButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await LocationButton();
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

        private async Task LocationButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                LocationPageNew locationPageNew = new LocationPageNew();
                await Navigation.PushAsync(locationPageNew);
            }
        }

        private async void OnOtherProcedureButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await OtherProcedureButton();
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

        private async Task OtherProcedureButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                await Navigation.PushAsync(new OtherProcedurePage());
            }
        }

        protected override async void OnAppearing()
        {
            await IsAgreePatientMonitoringConsentAsync();

            NavigationPage.SetHasNavigationBar(this, true);
            var deviceDisplay = DeviceDisplay.MainDisplayInfo;
            var deviceInfo = DeviceInfo.Model;
            #region iOS Apps Base Code
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (deviceDisplay.Width == 640)
                {
                    //this.BackgroundImage = "mobile_background_ios320.png";
                    btnCheckin.HeightRequest = 250;
                    //btnCheckin.WidthRequest = 156;
                    //CheckinButtonStackLayout.Margin = new Thickness(0, 60, 0, 0);
                }
                else if (deviceDisplay.Width == 750)
                {
                    //this.BackgroundImage = "mobile_background_ios375.png";
                    btnCheckin.HeightRequest = 300;
                    //btnCheckin.WidthRequest = 166;
                    //CheckinButtonStackLayout.Margin = new Thickness(0, 60, 0, 0);
                }
                else if (deviceDisplay.Width == 1242)
                {
                    //for iOS 8 plus
                    //this.BackgroundImage = "mobile_background_ios414.png";
                    btnCheckin.HeightRequest = 400;
                    //btnCheckin.WidthRequest = 176;
                    //CheckinButtonStackLayout.Margin = new Thickness(0, 60, 0, 0);
                }
                else
                {
                    //this.BackgroundImage = "mobile_background_ios320.png";
                    btnCheckin.HeightRequest = 300;
                    //btnCheckin.WidthRequest = 156;
                    //CheckinButtonStackLayout.Margin = new Thickness(0, 60, 0, 0);
                }
            }
            #endregion
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }

            var appMessage = await CurrentPatientProcedureDetail();
            bool isSuccess = await NotificationService.RegisterPushNotificationDeviceToken(_iTokenContainer.ApiPatientProfileId.Value);

            btnCheckin.IsEnabled = true;

            ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            base.OnAppearing();
        }

        private async Task IsAgreePatientMonitoringConsentAsync()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                bool result = await restApiService.PatientMonitoringConsentIsAgreed(_iTokenContainer.ApiUserId);
                if (result == true)
                {
                    _iTokenContainer.ApiIsAgreePatientMonitoringConsent = true;
                    _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = false;
                }
                else
                {
                    _iTokenContainer.ApiIsAgreePatientMonitoringConsent = false;
                    _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = true;
                }

                if (_iTokenContainer.ApiIsAgreePatientMonitoringConsent == false && _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent == true)
                {
                    //await Navigation.PushModalAsync(new PatientMonitoringConsentPage(), true);
                    App.Instance.ClearNavigationAndGoToPage(new PatientMonitoringConsentPage(){ CallBackPage = this });
                }
            }
        }
        
        private void GetPullNotificationAsync()
        {
            if (_iTokenContainer.IsApiToken())
            {
                if (_iTokenContainer.HasSendToastNotification == false)
                {
                    Task.Run(async () =>
                    {
                        // If you need to do anything with your UI, you need to wrap it in this.
                        NotificationHelper notificationHelper = new NotificationHelper();
                        await notificationHelper.PullNotificationAsync();
                    });
                }
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

        #region Top Menu Actions

        private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new ChangePasswordPage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnSignOutButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {

                if (_iTokenContainer != null)
                {
                    bool isSuccess = await NotificationService.MarkedPatientProfileDeviceTokenAsExpired(_iTokenContainer.ApiPatientProfileId.Value);
                    _iTokenContainer.ClearApiToken();
                }
                DependencyService.Get<IToast>().SetSettingsForUserLogout();
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #endregion
    }
}
