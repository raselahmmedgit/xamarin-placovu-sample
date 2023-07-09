using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientReportedOutcomeMonthPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;

        private PatientReportedOutcomePageViewModel PatientReportedOutcomePageViewModel;
        private PatientReportedOutcomeMonthPageViewModel PatientReportedOutcomeMonthPageViewModel;
        public List<PatientReportedOutcomeMonthPageViewModel> PatientReportedOutcomeMonthPageViewModels = new List<PatientReportedOutcomeMonthPageViewModel>();

        public List<string> SelectedProcedure { get; set; }

        public PatientReportedOutcomeMonthPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

                LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public PatientReportedOutcomeMonthPage(PatientReportedOutcomePageViewModel patientReportedOutcomePageViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

                if (patientReportedOutcomePageViewModel != null)
                {
                    PatientReportedOutcomePageViewModel = patientReportedOutcomePageViewModel;
                    PatientReportedOutcomePageTitleIconImage.Source = PatientReportedOutcomePageViewModel.TypeIcon;
                    PatientReportedOutcomePageTitleLabel.Text = PatientReportedOutcomePageViewModel.TypeName;
                    SelectedProcedure = patientReportedOutcomePageViewModel.SelectedProcedure;
                }

                LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadDataAsync()
        {
            try
            {
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    await App.ShowUserDialogDelayAsync();

                    PatientReportedOutcomeMonthPageViewModels = GetMonthList();

                    if (PatientReportedOutcomeMonthPageViewModels != null)
                    {
                        PatientReportedOutcomeMonthListView.ItemsSource = PatientReportedOutcomeMonthPageViewModels;

                        App.HideUserDialogAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
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
        }

        private List<PatientReportedOutcomeMonthPageViewModel> GetMonthList()
        {
            var selectedDate = DateTime.UtcNow.AddMonths(-6);
            int totalMonths = 6;

            var monthList = new List<PatientReportedOutcomeMonthPageViewModel>();

            var dateList = Enumerable.Range(1, totalMonths).Select(i => selectedDate.AddMonths(i)).OrderByDescending(e => e).AsEnumerable();

            foreach (var item in dateList)
            {
                monthList.Add(new PatientReportedOutcomeMonthPageViewModel()
                {
                    Text = item.ToString("MMMM"),
                    Value = item.ToString("MM/dd/yyyy"),
                    PracticeId = _iTokenContainer.ApiPracticeProfileId.ToLong(),
                    DivisionId = 0,
                    DivisionUnitId = 0,
                    IsActive = true,
                    DisplayOrder = item.Month.ToInt()
                });
            }

            return monthList;
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

        #region Bottom Menu Actions

        private async void OnHomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                HomeButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {
            //App.ShowUserDialogAsync();
            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
            App.Instance.MainPage = new MenuPage(userIdentityModel);
            //App.Instance.MainPage = new MenuProfessionalPage();
        }

        #endregion

        private async void PatientReportedOutcomeMonthListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await App.ShowUserDialogDelayAsync();
            var data = (PatientReportedOutcomeMonthPageViewModel)e.SelectedItem;
            if (data != null)
            {
                PatientReportedOutcomeMonthListView.SelectedItem = null;
                PatientReportedOutcomeMonthPageViewModel = data;
                PatientReportedOutcomeMonthPageViewModel.SelectedProcedure = SelectedProcedure;
                await Navigation.PushAsync(new PatientReportedOutcomePatientListPage(PatientReportedOutcomePageViewModel, PatientReportedOutcomeMonthPageViewModel));
            }
        }

        private async void PatientOutcomeSearch_Clicked(object sender, EventArgs e)
        {
            try
            {
                await App.ShowUserDialogDelayAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    await Navigation.PushAsync(new PatientReportedOutcomeSearchPage(PatientReportedOutcomePageViewModel));
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void ShowOutcomeSearchFilterModal(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            await Navigation.PushModalAsync(new OutcomeSearchFilterView(this));

        }
    }
}