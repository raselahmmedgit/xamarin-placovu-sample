using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Xamarin.Forms.Maps;
//using Plugin.Geolocator;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly ILocationClient _iLocationClient;
        private readonly IProcedureClient _iProcedureClient;

        private ObservableCollection<PatientProcedureLocationViewModel> patientProcedureLocationViewModelList { get; set; }

        public LocationPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();

                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iLocationClient = new LocationClient(apiClient);
                _iProcedureClient = new ProcedureClient(apiClient);

                LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public async void LoadDataAsync()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    var response = await _iLocationClient.LocationPage();
                    if (response.StatusIsSuccessful)
                    {
                        var data = response.Data;

                        if (data != null)
                        {
                            #region LocationIndexPageViewModel

                            LocationIndexPageViewModel locationIndexPageViewModel = data;

                            #endregion

                            #region PatientProcedureLocationViewModel

                            var PatientProcedureLocationViewModels = locationIndexPageViewModel.PatientProcedureLocationViewModels.ToList().Select(item =>
                                        new PatientProcedureLocationViewModel
                                        {
                                            PatientProfileId = item.PatientProfileId,
                                            ProcedureId = item.ProcedureId,
                                            ProcedureName = item.ProcedureName,
                                            LocationName = item.LocationName,
                                            StreetAddress = item.StreetAddress,
                                            CityName = item.CityName,
                                            StateName = item.StateName,
                                            ZipCode = item.ZipCode,
                                            Address = (item.LocationName + ", " + item.StreetAddress + ", " + item.CityName + ", " + item.ZipCode)
                                        });

                            patientProcedureLocationViewModelList = new ObservableCollection<PatientProcedureLocationViewModel>(PatientProcedureLocationViewModels);
                            locationListView.ItemsSource = patientProcedureLocationViewModelList;

                            #endregion

                            #region Map

                            //var locator = CrossGeolocator.Current;
                            //locator.DesiredAccuracy = 20;
                            //var position = await locator.GetPositionAsync();

                            //string Latitude = position.Latitude.ToString();
                            //string Longitude = position.Longitude.ToString();

                            //var map = new Map
                            //{
                            //    WidthRequest = 320,
                            //    HeightRequest = 200,
                            //    IsShowingUser = true,
                            //    MapType = MapType.Street
                            //};

                            //mapStackLayout.Children.Add(map);

                            #endregion
                        }

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
            //using (UserDialogs.Instance.Loading(""))
            //{
            //    //NotificationListPageViewModel model = new NotificationListPageViewModel();
            //    //await model.ExecuteLoadCommandAsync();
            //    //var notificationListPage = new NotificationListPage(model);

            //    //await Navigation.PushAsync(notificationListPage);

            //    //Navigation.InsertPageBefore(new MainPatientPage(), this);
            //    //await Navigation.PopToRootAsync();

            //    //MenuPatientPage menuPatientPage = new MenuPatientPage();
            //    //menuPatientPage.Detail = new NavigationPage(notificationListPage);

            //}

            //App.ShowUserDialogAsync();
            await Navigation.PushAsync(new NotificationListPageN());
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