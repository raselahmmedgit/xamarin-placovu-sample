using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HospitalInfoPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private HospitalInfoPageViewModel HospitalInfoPageViewModel { get; set; }
        private int defaultResourceId = 101;
        private readonly IProcedureClient _iProcedureClient;

        List<SurgicalResourceViewModel> _surgicalResourceViewModelList = new List<SurgicalResourceViewModel>();

        public HospitalInfoPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();

                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = "Hospital Info";
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
                HospitalInfoPageViewModel = new HospitalInfoPageViewModel();

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iProcedureClient = new ProcedureClient(apiClient);

                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
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

        private async void LoadDataAsyc()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    await HospitalInfoPageViewModel.LoadDataAsync();
                    CreateFormCheckbox();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private void CreateFormCheckbox()
        {
            SurgicalConciergeDocumentViewModel surgicalConciergeDocumentViewModel = HospitalInfoPageViewModel.SurgicalConciergeDocumentViewModel;

            #region SurgicalResourceDocumentStackLayout

            Grid grdLayout = new Grid();
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grdLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

            int loop_one = 0;
            int resourceId = defaultResourceId;

            if (surgicalConciergeDocumentViewModel.SurgicalResourceDocumentList != null && surgicalConciergeDocumentViewModel.SurgicalResourceDocumentList.Any())
            {
                foreach (var resource in surgicalConciergeDocumentViewModel.SurgicalResourceDocumentList)
                {
                    //string image = resource.ScgResourceDocumentIcon.ToLower().Replace("/content/images/", string.Empty).Replace("surgical_floor_library", "scg").Replace(".png", "_icon.png").Replace("foley", "catheter");
                    string image = "scg_dischrarge/" + resource.ScgResourceDocumentIconName;
                    Button btnImageLink = new Button
                    {
                        //Text = resource.SurgicalResourceShortName,
                        HeightRequest = 110,
                        BackgroundColor = (resourceId == defaultResourceId ? Color.FromHex("#61CFD3") : Color.FromHex("#C4D8F9")),
                        Image = image,
                        ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Top, 0),
                        FontSize = 12,
                        //TextColor = Color.Black,
                        CornerRadius = 4,
                        ClassId = resourceId.ToString(),
                        BorderWidth = 2,
                        BorderColor = Color.FromHex("#000000"),
                        Margin = new Thickness(6)
                    };

                    btnImageLink.Clicked += BtnImageLink_Clicked;

                    grdLayout.Children.Add(btnImageLink, loop_one, 0);

                    #region Details

                    foreach (var item in resource.SurgicalResourceList)
                    {

                        SurgicalResourceViewModel surgicalResourceViewModel = new SurgicalResourceViewModel { ResourceId = resourceId, ResourceTitle = resource.SurgicalResourceShortName, ResourceText = item.ResourceText };
                        _surgicalResourceViewModelList.Add(surgicalResourceViewModel);
                    }

                    #endregion

                    loop_one++;
                    resourceId++;
                }
            }

            if (surgicalConciergeDocumentViewModel.PracticeInformationViewModel != null)
            {
                var practiceInformationViewModel = surgicalConciergeDocumentViewModel.PracticeInformationViewModel;

                Button btnImageLink = new Button
                {
                    //Text = "Hospital Info",
                    HeightRequest = 110,
                    BackgroundColor = (resourceId == defaultResourceId ? Color.FromHex("#61CFD3") : Color.FromHex("#C4D8F9")),
                    Image = "scg_dischrarge/scg_hospital_icon.png",
                    ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Top, 10),
                    FontSize = 12,
                    //TextColor = Color.Black,
                    CornerRadius = 4,
                    ClassId = resourceId.ToString(),
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("#000000"),
                    Margin = new Thickness(6)
                };

                btnImageLink.Clicked += BtnImageLink_Clicked;

                grdLayout.Children.Add(btnImageLink, loop_one, 0);

                #region Details

                string resourceTitle = "Hospital Info";
                string resourceText = string.Empty;

                if (!string.IsNullOrEmpty(practiceInformationViewModel.VisitorGuide))
                {
                    resourceText += "<h3>Visitor Guide</h3>";
                    resourceText += practiceInformationViewModel.VisitorGuide;
                }

                if (!string.IsNullOrEmpty(practiceInformationViewModel.DiningInfo))
                {
                    resourceText += "<h3>Dining</h3>";
                    resourceText += practiceInformationViewModel.DiningInfo;
                }

                if (!string.IsNullOrEmpty(practiceInformationViewModel.PharmacyInfo))
                {
                    resourceText += "<h3>Pharmacy</h3>";
                    resourceText += practiceInformationViewModel.PharmacyInfo;
                }

                if (!string.IsNullOrEmpty(practiceInformationViewModel.ParkingInfo))
                {
                    resourceText += "<h3>Parking lot</h3>";
                    resourceText += practiceInformationViewModel.ParkingInfo;
                }

                if (!string.IsNullOrEmpty(practiceInformationViewModel.InternetInfo))
                {
                    resourceText += "<h3>Internet</h3>";
                    resourceText += practiceInformationViewModel.InternetInfo;
                }

                if (!string.IsNullOrEmpty(practiceInformationViewModel.AtmInfo))
                {
                    resourceText += "<h3>ATM</h3>";
                    resourceText += practiceInformationViewModel.AtmInfo;
                }

                SurgicalResourceViewModel surgicalResourceViewModel = new SurgicalResourceViewModel { ResourceId = resourceId, ResourceTitle = resourceTitle, ResourceText = resourceText };
                _surgicalResourceViewModelList.Add(surgicalResourceViewModel);

                #endregion

            }

            SurgicalResourceDocumentStackLayout.Children.Add(grdLayout);

            #endregion

            DefaultSelected();
        }

        private void DefaultSelected()
        {
            int resourceId = Convert.ToInt32(defaultResourceId);

            SurgicalResourceDocumentDetailStackLayout.IsVisible = false;

            var surgicalResourceViewModel = _surgicalResourceViewModelList.FirstOrDefault(x => x.ResourceId == resourceId);
            SurgicalResourceDocumentDetailLabel.Text = surgicalResourceViewModel.ResourceTitle;
            HospitalInfoPageViewModel.SurgicalConciergeDocumentViewModel.SurgicalResourceDocumentDetailHtmlWebViewSource = surgicalResourceViewModel.ResourceText;
            SurgicalResourceDocumentDetailWebView.Source = new HtmlWebViewSource { Html = surgicalResourceViewModel.ResourceTextFormated };

            SurgicalResourceDocumentDetailStackLayout.IsVisible = true;
        }

        private void BtnImageLink_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            foreach (var item in SurgicalResourceDocumentStackLayout.Children)
            {
                Grid grid = item as Grid;

                if (grid != null)
                {
                    foreach (var item2 in grid.Children)
                    {
                        Button btnAll = item2 as Button;
                        btnAll.BackgroundColor = Color.FromHex("#C4D8F9");
                    }
                }
            }

            int resourceIdClassId = Convert.ToInt32(btn.ClassId);

            SurgicalResourceDocumentDetailStackLayout.IsVisible = false;

            var surgicalResourceViewModel = _surgicalResourceViewModelList.FirstOrDefault(x => x.ResourceId == resourceIdClassId);
            SurgicalResourceDocumentDetailLabel.Text = surgicalResourceViewModel.ResourceTitle;
            HospitalInfoPageViewModel.SurgicalConciergeDocumentViewModel.SurgicalResourceDocumentDetailHtmlWebViewSource = surgicalResourceViewModel.ResourceText;
            SurgicalResourceDocumentDetailWebView.Source = new HtmlWebViewSource { Html = surgicalResourceViewModel.ResourceTextFormated };

            if (btn.BackgroundColor == Color.FromHex("#61CFD3"))
            {
                btn.BackgroundColor = Color.FromHex("#C4D8F9");
                //btn.TextColor = Color.FromHex("#000000");
            }
            else
            {
                btn.BackgroundColor = Color.FromHex("#61CFD3");
                //btn.TextColor = Color.FromHex("#000000");
            }

            SurgicalResourceDocumentDetailStackLayout.IsVisible = true;
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