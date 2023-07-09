using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientReportedOutcomeSearchPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;

        private string _procedureName { get; set; }

        private PatientReportedOutcomeSearchPageViewModel PatientReportedOutcomeSearchPageViewModel { get; set; }

        private PatientReportedOutcomePageViewModel PatientReportedOutcomePageViewModel;

        private ProfessionalProfileRestApiService professionalProfileRestApiService;

        private List<ProfessionalPatientProfileComplianceViewModel> ProfessionalPatientProfileComplianceViewModelList = new List<ProfessionalPatientProfileComplianceViewModel>();

        public PatientReportedOutcomeSearchPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = procedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public PatientReportedOutcomeSearchPage(PatientReportedOutcomePageViewModel patientReportedOutcomePageViewModel)
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
                }

                LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public async void LoadDataAsync()
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                var professionalPatientProfileComplianceViewModelList = await professionalProfileRestApiService.GetSearchPatientDirectory();

                if (professionalPatientProfileComplianceViewModelList != null)
                {
                    if (professionalPatientProfileComplianceViewModelList.Count() > 0)
                    {
                        PatientReportedOutcomeSearchPageViewModel = new PatientReportedOutcomeSearchPageViewModel();
                        var viewModelList = GetProfessionalPatientReportedOutcomeSearchViewModelList(professionalPatientProfileComplianceViewModelList);
                        PatientReportedOutcomeSearchPageViewModel.ProfessionalPatientReportedOutcomeSearchViewModels = viewModelList.ToList();

                        if (PatientReportedOutcomeSearchPageViewModel != null)
                        {
                            BindingContext = PatientReportedOutcomeSearchPageViewModel;
                            //professionalPatientReportedOutcomeSearchViewModelListView.ItemsSource = PatientReportedOutcomeSearchPageViewModel.ProfessionalPatientReportedOutcomeSearchViewModels;
                        }

                        App.HideUserDialogAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertTitle, AppConstant.NotFound, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
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

        public async void ReloadDataAsync(string searchEntryText)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                var professionalPatientProfileComplianceViewModelList = await professionalProfileRestApiService.GetSearchPatientDirectoryByKeyword(searchEntryText, 1, 100);

                if (professionalPatientProfileComplianceViewModelList != null)
                {
                    if (professionalPatientProfileComplianceViewModelList.Count() > 0)
                    {
                        PatientReportedOutcomeSearchPageViewModel = new PatientReportedOutcomeSearchPageViewModel();
                        var viewModelList = GetProfessionalPatientReportedOutcomeSearchViewModelList(professionalPatientProfileComplianceViewModelList);
                        PatientReportedOutcomeSearchPageViewModel.ProfessionalPatientReportedOutcomeSearchViewModels = viewModelList.ToList();

                        if (PatientReportedOutcomeSearchPageViewModel != null)
                        {
                            BindingContext = PatientReportedOutcomeSearchPageViewModel;
                            //professionalPatientReportedOutcomeSearchViewModelListView.ItemsSource = PatientReportedOutcomeSearchPageViewModel.ProfessionalPatientReportedOutcomeSearchViewModels;
                        }

                        App.HideUserDialogAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertTitle, AppConstant.NotFound, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
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

        private List<ProfessionalPatientReportedOutcomeSearchViewModel> GetProfessionalPatientReportedOutcomeSearchViewModelList(IEnumerable<ProfessionalPatientProfileComplianceViewModel> professionalPatientProfileComplianceViewModelList)
        {
            var professionalPatientReportedOutcomeSearchViewModelList = new List<ProfessionalPatientReportedOutcomeSearchViewModel>();

            try
            {
                if (professionalPatientProfileComplianceViewModelList != null)
                {
                    ProfessionalPatientProfileComplianceViewModelList = professionalPatientProfileComplianceViewModelList.ToList();

                    var loopOne = 1;
                    foreach (var groupProfessionalPatientProfileComplianceViewModel in professionalPatientProfileComplianceViewModelList.GroupBy(x => new { SurgeryMonth = x.SurgeryMonth, SurgeryYear = x.SurgeryYear }))
                    {
                        string surgeryMonthYear = (groupProfessionalPatientProfileComplianceViewModel.Key.SurgeryMonth + " " + groupProfessionalPatientProfileComplianceViewModel.Key.SurgeryYear);

                        var professionalPatientReportedOutcomeSearchViewModel = new ProfessionalPatientReportedOutcomeSearchViewModel();
                        professionalPatientReportedOutcomeSearchViewModel.Id = loopOne;
                        professionalPatientReportedOutcomeSearchViewModel.SurgeryMonthYear = surgeryMonthYear;

                        if (IsOdd(professionalPatientReportedOutcomeSearchViewModel.Id))
                        {
                            professionalPatientReportedOutcomeSearchViewModel.BackgroundColor = "#dff0d8";
                            professionalPatientReportedOutcomeSearchViewModel.BorderColor = "#d6e9c6";
                        }
                        else
                        {
                            professionalPatientReportedOutcomeSearchViewModel.BackgroundColor = "#d9edf7";
                            professionalPatientReportedOutcomeSearchViewModel.BorderColor = "#bce8f1";
                        }

                        professionalPatientReportedOutcomeSearchViewModel.ProfessionalPatientReportedOutcomeSearchDetailViewModels = new List<ProfessionalPatientReportedOutcomeSearchDetailViewModel>();

                        foreach (var professionalPatientProfileComplianceViewModel in groupProfessionalPatientProfileComplianceViewModel)
                        {
                            var professionalPatientReportedOutcomeSearchDetailViewModel = new ProfessionalPatientReportedOutcomeSearchDetailViewModel();
                            professionalPatientReportedOutcomeSearchDetailViewModel.PatientProfileId = professionalPatientProfileComplianceViewModel.PatientProfileId;
                            professionalPatientReportedOutcomeSearchDetailViewModel.ProcedureId = professionalPatientProfileComplianceViewModel.ProcedureId;
                            professionalPatientReportedOutcomeSearchDetailViewModel.PatientProcedureDetailId = professionalPatientProfileComplianceViewModel.PatientProcedureDetailId;
                            professionalPatientReportedOutcomeSearchDetailViewModel.PatientName = professionalPatientProfileComplianceViewModel.PatientName;
                            professionalPatientReportedOutcomeSearchDetailViewModel.SurgeryDate = professionalPatientProfileComplianceViewModel.SurgeryDate;
                            professionalPatientReportedOutcomeSearchDetailViewModel.SurgeryDateTime = professionalPatientProfileComplianceViewModel.SurgeryDateTime;
                            professionalPatientReportedOutcomeSearchDetailViewModel.ProcedureName = professionalPatientProfileComplianceViewModel.ProcedureName;
                            professionalPatientReportedOutcomeSearchDetailViewModel.CompletedStatusClass = professionalPatientProfileComplianceViewModel.CompletedStatusClass;
                            professionalPatientReportedOutcomeSearchDetailViewModel.CompletedStatusName = professionalPatientProfileComplianceViewModel.CompletedStatusName;
                            professionalPatientReportedOutcomeSearchDetailViewModel.SurgeryMonth = professionalPatientProfileComplianceViewModel.SurgeryMonth;
                            professionalPatientReportedOutcomeSearchDetailViewModel.SurgeryYear = professionalPatientProfileComplianceViewModel.SurgeryYear;
                            professionalPatientReportedOutcomeSearchDetailViewModel.IsBphProcedure = professionalPatientProfileComplianceViewModel.IsBphProcedure;

                            professionalPatientReportedOutcomeSearchViewModel.ProfessionalPatientReportedOutcomeSearchDetailViewModels.Add(professionalPatientReportedOutcomeSearchDetailViewModel);
                        }

                        loopOne = loopOne + 1;

                        professionalPatientReportedOutcomeSearchViewModelList.Add(professionalPatientReportedOutcomeSearchViewModel);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

            return professionalPatientReportedOutcomeSearchViewModelList;
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
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

            //BtnResource.IsEnabled = false;
            //BtnResource.BackgroundColor = Color.FromHex("#f7a50f");
            //BtnResource.BorderColor = Color.FromHex("#f7a50f");
        }

        private async void SearchImageButton_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await App.ShowUserDialogDelayAsync();

                var searchImageButton = sender as ImageButton;

                string searchEntryText = SearchEntry.Text?.Trim();

                if (string.IsNullOrWhiteSpace(searchEntryText))
                {
                    LoadDataAsync();
                }
                else
                {
                    ReloadDataAsync(searchEntryText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private async void ComplianceSearchDetailImageButton_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await App.ShowUserDialogDelayAsync();

                using (UserDialogs.Instance.Loading(""))
                {
                    var complianceSearchDetailImageButton = sender as ImageButton;
                    var complianceSearchDetailImageButtonId = complianceSearchDetailImageButton.ClassId.Split('_');
                    long procedureId = Convert.ToInt32(complianceSearchDetailImageButtonId[0]);
                    long patientProfileId = Convert.ToInt32(complianceSearchDetailImageButtonId[1]);
                    string patientProcedureDetailId = Convert.ToString(complianceSearchDetailImageButtonId[2]);
                    bool isBphProcedure = Convert.ToBoolean(complianceSearchDetailImageButtonId[3]);

                    PatientReportedOutcomePageViewModel.PatientProfileId = patientProfileId;
                    PatientReportedOutcomePageViewModel.PatientProcedureDetailId = patientProcedureDetailId.ToString();

                    var professionalPatientProfileComplianceViewModel = ProfessionalPatientProfileComplianceViewModelList.FirstOrDefault(item => item.ProcedureId == procedureId && item.PatientProcedureDetailId == patientProcedureDetailId.ToGuid() && item.PatientProfileId == patientProfileId);

                    var patientReportedOutcomePatientViewModel = new PatientReportedOutcomePatientViewModel();
                    patientReportedOutcomePatientViewModel.PatientProfileId = professionalPatientProfileComplianceViewModel.PatientProfileId;
                    patientReportedOutcomePatientViewModel.ProcedureId = professionalPatientProfileComplianceViewModel.ProcedureId;
                    patientReportedOutcomePatientViewModel.PatientProcedureDetailId = professionalPatientProfileComplianceViewModel.PatientProcedureDetailId.ToGuid();
                    patientReportedOutcomePatientViewModel.PatientName = professionalPatientProfileComplianceViewModel.PatientName;
                    patientReportedOutcomePatientViewModel.SurgeryDate = Convert.ToDateTime(professionalPatientProfileComplianceViewModel.SurgeryDate);
                    patientReportedOutcomePatientViewModel.SurgeryTime = professionalPatientProfileComplianceViewModel.SurgeryTime;
                    patientReportedOutcomePatientViewModel.ProcedureName = professionalPatientProfileComplianceViewModel.ProcedureName;
                    patientReportedOutcomePatientViewModel.SurgeryYear = professionalPatientProfileComplianceViewModel.SurgeryYear.ToInt();
                    patientReportedOutcomePatientViewModel.IsBphProcedure = professionalPatientProfileComplianceViewModel.IsBphProcedure;

                    if (isBphProcedure)
                    {
                        await Navigation.PushAsync(new ProfessionalPatientBphComplianceSearchDetailPage(PatientReportedOutcomePageViewModel, patientReportedOutcomePatientViewModel));
                    }
                    else
                    {
                        await Navigation.PushAsync(new ProfessionalPatientComplianceSearchDetailPage(PatientReportedOutcomePageViewModel, patientReportedOutcomePatientViewModel));
                    }
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
                HomeButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {
            App.ShowUserDialogAsync();
            App.Instance.MainPage = new MenuProfessionalPage();
        }

        #endregion
    }
}