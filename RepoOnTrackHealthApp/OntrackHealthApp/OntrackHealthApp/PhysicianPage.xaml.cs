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
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhysicianPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IPhysicianClient _iPhysicianClient;
        private readonly IProcedureClient _iProcedureClient;

        private ObservableCollection<ProfessionalBioSection> ProfessionalBioSectionList { get; set; }
        private ObservableCollection<ProfessionalBioEducation> ProfessionalBioEducationList { get; set; }
        private ObservableCollection<ProfessionalBioAssociation> ProfessionalBioAssociationList { get; set; }
        private ObservableCollection<ProfessionalBioInterest> ProfessionalBioInterestList { get; set; }
        private ObservableCollection<ProfessionalBioLicensureView> ProfessionalBioLicensureViewList { get; set; }
        private ObservableCollection<ProfessionalBioCustomSection> ProfessionalBioCustomSectionList { get; set; }

        public PhysicianPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iPhysicianClient = new PhysicianClient(apiClient);
                _iProcedureClient = new ProcedureClient(apiClient);

                Task.Run(async () => {
                    await LoadDataAsync();
                });
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        async Task LoadDataAsync()
        {
            using (UserDialogs.Instance.Loading(""))
            {

                try
                {
                    var response = await _iPhysicianClient.PhysicianPage();
                    if (response.StatusIsSuccessful)
                    {
                        var data = response.Data;

                        if (data != null)
                        {

                            #region PhysicianIndexPageViewModel

                            PhysicianIndexPageViewModel physicianIndexPageViewModel = data;

                            if (physicianIndexPageViewModel.ProfessionalProfileImage == null)
                            {
                                professionalBioGenarelInfoProfessionalProfileImage.Source = "no_user_image.gif";
                            }
                            else
                            {
                                professionalBioGenarelInfoProfessionalProfileImage.Source = physicianIndexPageViewModel.ProfessionalProfileImage;
                            }

                            #endregion

                            #region ProfessionalBioGenarelInfo

                            ProfessionalBioGenarelInfo professionalBioGenarelInfo = physicianIndexPageViewModel.ProfessionalBioGenarelInfo;

                            professionalBioGenarelInfoYearBoardCertifiedLabel.Text = "Board Certified since " + professionalBioGenarelInfo.YearBoardCertified.ToString();
                            professionalBioGenarelInfoCurrentPracticeNameYearJoinedCurrentPracticeLabel.Text = "Joined " + professionalBioGenarelInfo.CurrentPracticeName.ToString() + " in " + professionalBioGenarelInfo.YearBoardCertified.ToString();
                            professionalBioGenarelInfoCurrentPracticeLocationLabel.Text = "Clinic location: " + professionalBioGenarelInfo.CurrentPracticeLocation.ToString();

                            var professionalBioGenarelInfoCareerSummaryWebViewHtmlWebViewSource = new HtmlWebViewSource
                            {
                                Html = professionalBioGenarelInfo.CareerSummary
                            };
                            professionalBioGenarelInfoCareerSummaryWebView.Source = professionalBioGenarelInfoCareerSummaryWebViewHtmlWebViewSource;

                            #endregion

                            #region ProfessionalBioEducation

                            ProfessionalBioEducationList = new ObservableCollection<ProfessionalBioEducation>(physicianIndexPageViewModel.ProfessionalBioEducations);
                            professionalBioEducationsListView.ItemsSource = ProfessionalBioEducationList;

                            #endregion

                            #region ProfessionalBioAssociation

                            ProfessionalBioAssociationList = new ObservableCollection<ProfessionalBioAssociation>(physicianIndexPageViewModel.ProfessionalBioAssociations);
                            professionalBioAssociationsListView.ItemsSource = ProfessionalBioAssociationList;

                            #endregion

                            #region ProfessionalBioInterest

                            ProfessionalBioInterestList = new ObservableCollection<ProfessionalBioInterest>(physicianIndexPageViewModel.ProfessionalBioInterests);
                            professionalBioInterestsListView.ItemsSource = ProfessionalBioInterestList;

                            #endregion

                            #region ProfessionalBioLicensureView

                            ProfessionalBioLicensureViewList = new ObservableCollection<ProfessionalBioLicensureView>(physicianIndexPageViewModel.ProfessionalBioLicensureViews);
                            professionalBioLicensureViewsListView.ItemsSource = ProfessionalBioLicensureViewList;

                            #endregion

                            #region ProfessionalBioCustomSection

                            ProfessionalBioCustomSectionList = new ObservableCollection<ProfessionalBioCustomSection>(physicianIndexPageViewModel.ProfessionalBioCustomSections);
                            professionalBioCustomSectionsListView.ItemsSource = ProfessionalBioCustomSectionList;

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