using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
//using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtherProcedurePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IProcedureClient _iProcedureClient;

        private PatientProstateCancerSummary PatientProstateCancerSummary { get; set; }
        private readonly IProstateCancerSummaryClient _iProstateCancerSummaryClient;
        private PatientDischargeNotificationModel PatientDischargeNotificationModel { get; set; }
        private readonly IDischargeNotificationClient _iDischargeNotificationClient;

        public ObservableCollection<PatientProcedureDetailModel> PatientProcedureDetailModels { get; set; }
        public List<OtherProcedurePageViewModel> OtherProcedurePageViewModels = new List<OtherProcedurePageViewModel>();

        public OtherProcedurePage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iProcedureClient = new ProcedureClient(apiClient);

                _iProstateCancerSummaryClient = new ProstateCancerSummaryClient(apiClient);
                _iDischargeNotificationClient = new DischargeNotificationClient(apiClient);
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
                BindingContext = this;

                LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public async void LoadDataAsync()
        {
            try
            {
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    await App.ShowUserDialogDelayAsync();
                    var response = await _iProcedureClient.ActiveProcedures();
                    if (response.StatusIsSuccessful)
                    {
                        var dataList = response.DataList;

                        if (dataList != null && dataList.Any())
                        {
                            PatientProcedureDetailModels = new ObservableCollection<PatientProcedureDetailModel>(dataList);

                            foreach (var patientProcedureDetailModel in PatientProcedureDetailModels)
                            {
                                OtherProcedurePageViewModel otherProcedurePageViewModel = new OtherProcedurePageViewModel
                                {
                                    ProcedureId = patientProcedureDetailModel.ProcedureId,
                                    ProcedureName = (patientProcedureDetailModel.ProcedureName),
                                    PatientProcedureDetailId = patientProcedureDetailModel.PatientProcedureDetailId.ToString(),
                                    PatientProcedureDetailIdAndProcedureName = patientProcedureDetailModel.PatientProcedureDetailId.ToString() + "_" + patientProcedureDetailModel.ProcedureName
                                };

                                OtherProcedurePageViewModels.Add(otherProcedurePageViewModel);
                            }
                        }

                        //var responseTwo = await _iProstateCancerSummaryClient.GetProstateCancerSummaryAsync(_iTokenContainer.ApiPracticeProfileId.ToLong(), _iTokenContainer.ApiPatientProfileId.ToLong(), _iTokenContainer.CurrentPatientProcedureDetailId.ToGuid());
                        //PatientProstateCancerSummary = responseTwo.Data;
                        //if (PatientProstateCancerSummary != null)
                        //{
                        //    if (PatientProstateCancerSummary.PatientProfileId > 0)
                        //    {
                        //        OtherProcedurePageViewModel otherProcedurePageViewModelTwo = new OtherProcedurePageViewModel
                        //        {
                        //            ProcedureId = 0,
                        //            ProcedureName = "Cancer Summary",
                        //            PatientProcedureDetailId = "",
                        //            PatientProcedureDetailIdAndProcedureName = "Cancer Summary"
                        //        };
                        //        OtherProcedurePageViewModels.Add(otherProcedurePageViewModelTwo);
                        //    }
                        //}

                        var dischargeNotificationSurveyResponse = await _iDischargeNotificationClient.GetDischargeNotificationAsync();
                        PatientDischargeNotificationModel = dischargeNotificationSurveyResponse.Data;
                        if (PatientDischargeNotificationModel != null)
                        {
                            OtherProcedurePageViewModel dischargeNotificationSurveyViewModel = new OtherProcedurePageViewModel
                            {
                                ProcedureId = 0,
                                ProcedureName = AppConstant.DischargeNotificationInfo,
                                PatientProcedureDetailId = "",
                                PatientProcedureDetailIdAndProcedureName = AppConstant.DischargeNotificationInfo
                            };
                            OtherProcedurePageViewModels.Add(dischargeNotificationSurveyViewModel);
                        }

                        OtherProcedureListView.ItemsSource = OtherProcedurePageViewModels;

                    }
                    else
                    {
                        await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            Title = _iTokenContainer.ApiPracticeName;
        }

        private async void ShowMoreButton_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var showMoreButton = sender as Button;
                    var patientProcedureDetailIdAndProcedureNameClassId = Convert.ToString(showMoreButton.ClassId);

                    //if (patientProcedureDetailIdAndProcedureNameClassId == "Cancer Summary")
                    //{
                    //    await Navigation.PushAsync(new CancerSummaryPage(PatientProstateCancerSummary));
                    //}
                    //else if (patientProcedureDetailIdAndProcedureNameClassId == AppConstant.DischargeNotificationInfo)
                    if (patientProcedureDetailIdAndProcedureNameClassId == AppConstant.DischargeNotificationInfo)
                    {
                        PatientSurveyPageViewModel model = new PatientSurveyPageViewModel
                        {
                            PatientSurveyPatientNotificationDetailViewModel = PatientDischargeNotificationModel.PatientSurveyPatientNotificationDetailViewModels.OrderByDescending(c => c.NotificationDate).FirstOrDefault(),
                            NotificationTitle = AppConstant.DischargeNotificationInfo
                        };
                        await Navigation.PushAsync(new PatientDischargeSurveyPage(model, PatientDischargeNotificationModel.SurgicalResourcePatientProstatectomyLibraryPageViewModelList));
                    }
                    else
                    {
                        var patientProcedureDetailIdAndProcedureName = patientProcedureDetailIdAndProcedureNameClassId.Split('_');
                        var patientProcedureDetailId = Convert.ToString(patientProcedureDetailIdAndProcedureName[0]);
                        var procedureName = Convert.ToString(patientProcedureDetailIdAndProcedureName[1]);

                        _iTokenContainer.CurrentPatientProcedureDetailId = patientProcedureDetailId;
                        _iTokenContainer.CurrentProcedureName = procedureName;

                        var appMessage = await IsCurrentPatientProcedureDetail();
                        if (appMessage.MessageType == AppMessageType.Success)
                        {
                            await Navigation.PushAsync(new NotificationListPageN());
                        }
                        else
                        {
                            await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                        }
                        
                    }
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        #region Top Menu Actions

        private async void OnHomeButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    //App.Instance.ClearNavigationAndGoToPage(new MainPage());
                    await Navigation.PushAsync(new MainPage());
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnScheduleButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    NotificationListPageViewModel model = new NotificationListPageViewModel();
                    await model.ExecuteLoadCommandAsync();
                    await Navigation.PushAsync(new NotificationListPage(model));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnPhysicianButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await Navigation.PushAsync(new PhysicianProfilePage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnLocationButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                LocationPageNew page = new LocationPageNew();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnResourceButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    ResourcePage page = new ResourcePage();
                    await page.LoadDataAsync();
                    await Navigation.PushAsync(page);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnOtherInfoButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new HospitalInfoPage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

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

        private async void OnUpdatePatientProfileClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new UpdateProfilePage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #endregion


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

        private async void OtherProcedureListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                await App.ShowUserDialogDelayAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    var data = (OtherProcedurePageViewModel)e.SelectedItem;
                    if (data != null)
                    {
                        var patientProcedureDetailIdAndProcedureName = data.ProcedureName;
                        if (patientProcedureDetailIdAndProcedureName == "Cancer Summary")
                        {
                            await Navigation.PushAsync(new CancerSummaryPage(PatientProstateCancerSummary));
                        }
                        else if (patientProcedureDetailIdAndProcedureName == AppConstant.DischargeNotificationInfo)
                        {
                            PatientSurveyPageViewModel model = new PatientSurveyPageViewModel
                            {
                                PatientSurveyPatientNotificationDetailViewModel = PatientDischargeNotificationModel.PatientSurveyPatientNotificationDetailViewModels.OrderByDescending(c => c.NotificationDate).FirstOrDefault(),
                                NotificationTitle = AppConstant.DischargeNotificationInfo
                            };
                            await Navigation.PushAsync(new PatientDischargeSurveyPage(model, PatientDischargeNotificationModel.SurgicalResourcePatientProstatectomyLibraryPageViewModelList));
                        }
                        else
                        {
                            _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId;
                            _iTokenContainer.CurrentProcedureName = patientProcedureDetailIdAndProcedureName;

                            var appMessage = await IsCurrentPatientProcedureDetail();
                            if (appMessage.MessageType == AppMessageType.Success)
                            {
                                await Navigation.PushAsync(new NotificationListPageN());
                            }
                            else
                            {
                                await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                            }
                            
                        }
                        OtherProcedureListView.SelectedItem = null;
                    }
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }

        }
    }
}