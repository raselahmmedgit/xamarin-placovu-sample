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
	public partial class PatientReportedOutcomePatientListPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;

        private PatientReportedOutcomePageViewModel PatientReportedOutcomePageViewModel;
        private PatientReportedOutcomeMonthPageViewModel PatientReportedOutcomeMonthPageViewModel;
        private PatientReportedOutcomePatientViewModel PatientReportedOutcomePatientViewModel;
        public List<PatientReportedOutcomePatientViewModel> PatientReportedOutcomePatientViewModels = new List<PatientReportedOutcomePatientViewModel>();

        public List<string> SelectedProcedure { get; set; }

        public PatientReportedOutcomePatientListPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public PatientReportedOutcomePatientListPage(PatientReportedOutcomePageViewModel patientReportedOutcomePageViewModel, PatientReportedOutcomeMonthPageViewModel patientReportedOutcomeMonthPageViewModel)
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
            }

            if (patientReportedOutcomeMonthPageViewModel != null)
            {
                PatientReportedOutcomeMonthPageViewModel = patientReportedOutcomeMonthPageViewModel;
                PatientReportedOutcomeMonthNameLabel.Text = PatientReportedOutcomeMonthPageViewModel.Text;
                SelectedProcedure = patientReportedOutcomeMonthPageViewModel.SelectedProcedure;
            }

            LoadDataAsyc();
        }

        public async void LoadDataAsyc()
        {
            try
            {
                App.ShowUserDialogAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    long professionalProfileId = _iTokenContainer.ApiProfessionalProfileId.ToLong();
                    int pageNo = 1;
                    int pageSize = 100000;
                    int reportType = PatientReportedOutcomePageViewModel.TypeId;
                    DateTime selectedDate = Convert.ToDateTime(Convert.ToDateTime(PatientReportedOutcomeMonthPageViewModel.Value).ToString("MM/dd/yyyy"));

                    List<PatientReportedOutcomePatientViewModel> patientReportedOutcomePatientViewModelList = await professionalProfileRestApiService.GetPatientList(professionalProfileId, pageNo, pageSize, reportType, selectedDate);
                    if (SelectedProcedure != null && SelectedProcedure.Any())
                    {
                        patientReportedOutcomePatientViewModelList = patientReportedOutcomePatientViewModelList.Where(x => SelectedProcedure.Contains(Convert.ToString(x.ProcedureId))).ToList();
                    }

                    if (patientReportedOutcomePatientViewModelList != null)
                    {
                        PatientReportedOutcomePatientViewModels = patientReportedOutcomePatientViewModelList;
                        if (PatientReportedOutcomePatientViewModels.Count() > 0)
                        {
                            PatientReportedOutcomeStackLayout.IsVisible = true;
                            PatientReportedOutcomeNoDataFoundStackLayout.IsVisible = false;
                            PatientReportedOutcomePatientListView.ItemsSource = PatientReportedOutcomePatientViewModels;
                        }
                        else
                        {
                            PatientReportedOutcomeStackLayout.IsVisible = false;
                            PatientReportedOutcomeNoDataFoundStackLayout.IsVisible = true;
                        }
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
                App.HideUserDialogAsync();
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
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

        private async void PatientReportedOutcomePatientListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                await App.ShowUserDialogDelayAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    var data = (PatientReportedOutcomePatientViewModel)e.SelectedItem;
                    if (data != null)
                    {
                        PatientReportedOutcomePatientListView.SelectedItem = null;
                        PatientReportedOutcomePatientViewModel = data;

                        if (PatientReportedOutcomePatientViewModel.IsBphProcedure)
                        {
                            await Navigation.PushAsync(new ProfessionalPatientBphComplianceSearchDetailPage(PatientReportedOutcomePageViewModel, PatientReportedOutcomePatientViewModel));
                        }
                        else
                        {
                            await Navigation.PushAsync(new ProfessionalPatientComplianceSearchDetailPage(PatientReportedOutcomePageViewModel, PatientReportedOutcomePatientViewModel));
                        }
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