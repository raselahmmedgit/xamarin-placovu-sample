using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OntrackHealthApp
{
    public partial class App : Application
    {
        public static DeviceSize CurrentDeviceSize { get; set; }
        public class DeviceSize
        {
            public double Height { get; set; }
            public double Weidth { get; set; }
        }
        public static bool IsDialogShowing { get; set; } = false;
        public static bool IsApplicationUsesLatestVersion { get; set; } = true;
        public static App Instance;
        public static ITokenContainer _iTokenContainer;
        private readonly ILoginClient _iLoginClient;
        private ILatestVersion _ILatestVersion;
        public static double _notificationIntervalMinute = Convert.ToDouble(2.00); //default 2 minute
        public static double _notificationElapsedMinute = Convert.ToDouble(5.00); //default 5 minute      
        

        public App()
        {
            InitializeComponent();
            Instance = this;
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iLoginClient = new LoginClient(apiClient);
            _ILatestVersion = new iOS.LatestVersion();

            if (InternetConnectHelper.CheckConnection())
            {
                GetApplicationSettingAsync();

                // Reset CurrentPatientProcedureDetailId to null
                _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                _iTokenContainer.CurrentProcedureName = string.Empty;

                if (_iTokenContainer.GetUserIdentityModel() != null)
                {
                    LoadTempData();
                }

                InitializeNavigation();
                GetApplicationVersionAsync();
            }
            else
            {
                _iTokenContainer.ApiIsAdmin = false;
                MainPage = new InternetConnectPage();
            }
            if (Current.MainPage != null)
            {
                CurrentDeviceSize = new DeviceSize { Weidth = Current.MainPage.Width, Height = Current.MainPage.Height };
            }
        }

        private void InitializeNavigation()
        {
            if (!_iTokenContainer.IsApiToken())
            {
                MainPage = new NavigationPage(new LoginPageNew());
            }
            else
            {
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();

                if (userIdentityModel != null && userIdentityModel.RoleId != null &&
                    userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
                {
                    _iTokenContainer.ApiIsAdmin = true;
                    MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null &&
                         userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
                {
                    _iTokenContainer.ApiIsAdmin = true;
                    MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null &&
                         userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
                {
                    _iTokenContainer.ApiIsAdmin = true;
                    MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null &&
                         userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
                {
                    _iTokenContainer.ApiIsAdmin = true;
                    MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null &&
                         userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
                {
                    _iTokenContainer.ApiIsAdmin = false;
                    MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null &&
                         userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
                {
                    _iTokenContainer.ApiIsAdmin = false;
                    MainPage = new MenuPage(userIdentityModel);
                }
                else
                {
                    _iTokenContainer.ApiIsAdmin = false;
                    MainPage = new NavigationPage(new LoginPageNew());
                }
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (_currentDialog == null)
            {
                _currentDialog = UserDialogs.Instance;
            }
        }

        public void ClearNavigationAndGoToPage(ContentPage page)
        {
            if(page is LoginPage){
                MainPage = new NavigationPage(new LoginPageNew());
            }
            else {
                MainPage = page;
            }            
        }
        public void ClearNavigationAndGoToMenuPage()
        {
            InitializeNavigation();
        }
        public void ChangeTitle(string title)
        {
            MainPage.Title = title;
        }

        #region Methods

        private async void GetApplicationSettingAsync()
        {
            if (string.IsNullOrEmpty(_iTokenContainer.ApiMobileNotificationIntervalMinute) && string.IsNullOrEmpty(_iTokenContainer.ApiMobileNotificationElapsedMinute))
            {
                #region Api Client

                try
                {
                    var response = await _iLoginClient.GetApplicationSetting();
                    if (response.StatusIsSuccessful)
                    {
                        var model = response.Data;
                        _iTokenContainer.SetApplicationSettingModel(response.Data);

                        if (!string.IsNullOrEmpty(model.MobileNotificationIntervalMinute))
                        {
                            _notificationIntervalMinute = Convert.ToDouble(model.MobileNotificationIntervalMinute);
                        }

                        if (!string.IsNullOrEmpty(model.MobileNotificationElapsedMinute))
                        {
                            _notificationElapsedMinute = Convert.ToDouble(model.MobileNotificationElapsedMinute);
                        }
                    }
                    else
                    {
                    }
                }
                catch
                {
                }

                #endregion
            }
            else
            {
                _notificationIntervalMinute = Convert.ToDouble(_iTokenContainer.ApiMobileNotificationIntervalMinute);
                _notificationElapsedMinute = Convert.ToDouble(_iTokenContainer.ApiMobileNotificationElapsedMinute);
            }

        }

        private void GetApplicationVersionAsync()
        {
            try
            {
                ApplicationApiService _ApplicationApiService = new ApplicationApiService();
                var result = AsyncHelper.RunSync(() => _ApplicationApiService.GetAppIosReleaseHistory());

                if (result != null)
                {
                    _iTokenContainer.AndroidVersionCode = result.IosVersionCode;
                    _iTokenContainer.AppStoreAppName = result.ApplicationName;

                    if (_ILatestVersion.InstalledVersionCode < result.IosVersion)
                    {
                        _iTokenContainer.IsUsingLatestVersion = false;
                        if (!_iTokenContainer.IsUsingLatestVersion)
                        {
                            MainPage = new NewVersionPage();
                        }
                    }
                    else
                    {
                        _iTokenContainer.IsUsingLatestVersion = true;
                    }
                }
            }
            catch
            {
                _iTokenContainer.IsUsingLatestVersion = true;
                /*Do nothing. Do not show error. Please write log here*/
            }
        }

        #endregion

        #region Dialog
        private static IUserDialogs _currentDialog { get; set; }
        public static void ShowUserDialog()
        {
            if (_currentDialog == null)
            {
                _currentDialog = UserDialogs.Instance;
            }
            if (IsDialogShowing == false)
            {
                IsDialogShowing = true;
                _currentDialog.ShowLoading("");
            }
        }
        public static void HideUserDialog()
        {
            if (_currentDialog != null)
            {
                _currentDialog.HideLoading();
                //_currentDialog = null;
                IsDialogShowing = false;
            }
        }
        public static async Task ShowUserDialogDelayAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ShowUserDialog();
            });
            await Task.Delay(20);
        }
        public static void ShowUserDialogAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ShowUserDialog();
            });
        }
        public static void HideUserDialogAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                HideUserDialog();
            });
        }
        #endregion

        public static async void LoadTempData()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                try
                {
                    var restApiService = new SurgicalConciergeRestApiService();
                    var _countryViewModelList = await restApiService.GetCountryPhoneCodesAsString();
                    TempDataContainer.CountryViewModelJson = _countryViewModelList;
                }
                catch { }
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new InternetConnectPage());
            }
        }
    }
}
