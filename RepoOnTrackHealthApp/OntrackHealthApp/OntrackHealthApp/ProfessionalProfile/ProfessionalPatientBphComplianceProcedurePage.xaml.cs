using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Model;
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
	public partial class ProfessionalPatientBphComplianceProcedurePage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;

        private string _procedureName { get; set; }

        private ProfessionalPatientBphComplianceProcedurePageViewModel ProfessionalPatientBphComplianceProcedurePageViewModel { get; set; }
        private PatientReportedOutcomePatientViewModel PatientReportedOutcomePatientViewModel;

        private ProfessionalOutcomeRestApiService professionalOutcomeRestApiService;

        private List<PatientProcedureDetailViewModelForBphAggregateSurvey> PatientProcedureDetailViewModelForBphAggregateSurveyList = new List<PatientProcedureDetailViewModelForBphAggregateSurvey>();

        private List<ProfessionalPatientBphComplianceProcedureViewModel> ProfessionalPatientBphComplianceProcedureViewModelList = new List<ProfessionalPatientBphComplianceProcedureViewModel>();

        public ProfessionalPatientBphComplianceProcedurePage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = procedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalOutcomeRestApiService = new ProfessionalOutcomeRestApiService();

            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public ProfessionalPatientBphComplianceProcedurePage(PatientReportedOutcomePatientViewModel patientReportedOutcomePatientViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = procedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalOutcomeRestApiService = new ProfessionalOutcomeRestApiService();

                if (patientReportedOutcomePatientViewModel != null)
                {
                    PatientReportedOutcomePatientViewModel = patientReportedOutcomePatientViewModel;
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
                var patientAggregateSurveyReportForBphViewModel = await professionalOutcomeRestApiService.GetPatientBphSurveyActivity(PatientReportedOutcomePatientViewModel.PatientProfileId, false, PatientReportedOutcomePatientViewModel.ProcedureId);

                if (patientAggregateSurveyReportForBphViewModel != null)
                {
                    if (patientAggregateSurveyReportForBphViewModel.PatientProcedureDetailViewModels.Count() > 0)
                    {
                        if (patientAggregateSurveyReportForBphViewModel.PatientProcedureDetailViewModels.Count() >= 1)
                        {
                            #region This Page

                            ProfessionalPatientBphComplianceProcedurePageViewModel = new ProfessionalPatientBphComplianceProcedurePageViewModel();
                            var viewModelList = GetProfessionalPatientBphComplianceProcedureTypeViewModelList(patientAggregateSurveyReportForBphViewModel.PatientProcedureDetailViewModels);
                            ProfessionalPatientBphComplianceProcedurePageViewModel.ProfessionalPatientBphComplianceProcedureTypeViewModels = viewModelList.ToList();

                            if (ProfessionalPatientBphComplianceProcedurePageViewModel != null)
                            {
                                BindingContext = ProfessionalPatientBphComplianceProcedurePageViewModel;
                                //ProfessionalPatientBphComplianceProcedureTypeViewModelListView.ItemsSource = ProfessionalPatientBphComplianceProcedurePageViewModel.ProfessionalPatientBphComplianceProcedureTypeViewModels;
                            }

                            App.HideUserDialogAsync();

                            #endregion
                        }
                        else
                        {
                            #region Other Page

                            #endregion
                        }
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

        private List<ProfessionalPatientBphComplianceProcedureTypeViewModel> GetProfessionalPatientBphComplianceProcedureTypeViewModelList(IEnumerable<PatientProcedureDetailViewModelForBphAggregateSurvey> patientProcedureDetailViewModelForBphAggregateSurveyList)
        {
            PatientProcedureDetailViewModelForBphAggregateSurveyList = patientProcedureDetailViewModelForBphAggregateSurveyList.ToList();

            var professionalPatientBphComplianceProcedureTypeViewModelList = new List<ProfessionalPatientBphComplianceProcedureTypeViewModel>();

            try
            {
                if (patientProcedureDetailViewModelForBphAggregateSurveyList != null)
                {
                    var loopOne = 1;
                    foreach (var groupPatientProcedureDetailViewModelForBphAggregateSurvey in patientProcedureDetailViewModelForBphAggregateSurveyList.GroupBy(x => new { ProcedureTypeId = x.ProcedureTypeId, ProcedureTypeName = x.ProcedureTypeName }))
                    {
                        int procedureTypeId = (groupPatientProcedureDetailViewModelForBphAggregateSurvey.Key.ProcedureTypeId);
                        string procedureTypeName = (groupPatientProcedureDetailViewModelForBphAggregateSurvey.Key.ProcedureTypeName);

                        var professionalPatientBphComplianceProcedureTypeViewModel = new ProfessionalPatientBphComplianceProcedureTypeViewModel();
                        professionalPatientBphComplianceProcedureTypeViewModel.ProcedureTypeId = procedureTypeId;
                        professionalPatientBphComplianceProcedureTypeViewModel.ProcedureTypeName = procedureTypeName;

                        professionalPatientBphComplianceProcedureTypeViewModel.ProfessionalPatientBphComplianceProcedureViewModels = new List<ProfessionalPatientBphComplianceProcedureViewModel>();

                        foreach (var patientProcedureDetailViewModelForBphAggregateSurvey in groupPatientProcedureDetailViewModelForBphAggregateSurvey)
                        {
                            var professionalPatientBphComplianceProcedureViewModel = new ProfessionalPatientBphComplianceProcedureViewModel();
                            professionalPatientBphComplianceProcedureViewModel.PatientProfileId = patientProcedureDetailViewModelForBphAggregateSurvey.PatientProfileId;
                            professionalPatientBphComplianceProcedureViewModel.BphProcedureTypeId = patientProcedureDetailViewModelForBphAggregateSurvey.BphProcedureTypeId;
                            professionalPatientBphComplianceProcedureViewModel.ProcedureTypeId = patientProcedureDetailViewModelForBphAggregateSurvey.ProcedureTypeId;
                            professionalPatientBphComplianceProcedureViewModel.ProcedureTypeName = patientProcedureDetailViewModelForBphAggregateSurvey.ProcedureTypeName;
                            professionalPatientBphComplianceProcedureViewModel.ProcedureName = patientProcedureDetailViewModelForBphAggregateSurvey.ProcedureName;

                            professionalPatientBphComplianceProcedureViewModel.ProcedureId = PatientReportedOutcomePatientViewModel.ProcedureId;
                            professionalPatientBphComplianceProcedureViewModel.PatientProcedureDetailId = PatientReportedOutcomePatientViewModel.PatientProcedureDetailId;

                            professionalPatientBphComplianceProcedureTypeViewModel.ProfessionalPatientBphComplianceProcedureViewModels.Add(professionalPatientBphComplianceProcedureViewModel);
                        }

                        loopOne = loopOne + 1;

                        ProfessionalPatientBphComplianceProcedureViewModelList = professionalPatientBphComplianceProcedureTypeViewModel.ProfessionalPatientBphComplianceProcedureViewModels;

                        professionalPatientBphComplianceProcedureTypeViewModelList.Add(professionalPatientBphComplianceProcedureTypeViewModel);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

            return professionalPatientBphComplianceProcedureTypeViewModelList;
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

        private async void ProcedureNameButton_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await App.ShowUserDialogDelayAsync();

                using (UserDialogs.Instance.Loading(""))
                {
                    var procedureNameButton = sender as Button;

                    var procedureNameButtonId = procedureNameButton.ClassId.Split('_');
                    long procedureId = Convert.ToInt32(procedureNameButtonId[0]);
                    long patientProfileId = Convert.ToInt32(procedureNameButtonId[1]);
                    string patientProcedureDetailId = Convert.ToString(procedureNameButtonId[2]);

                    var professionalPatientBphComplianceProcedureViewModel = ProfessionalPatientBphComplianceProcedureViewModelList.FirstOrDefault(item => item.ProcedureId == procedureId && item.PatientProcedureDetailId == patientProcedureDetailId.ToGuid() && item.PatientProfileId == patientProfileId);
                    await Navigation.PushAsync(new PatientReportedOutcomeSearchCompliancePage(professionalPatientBphComplianceProcedureViewModel));
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
            //App.ShowUserDialogAsync();
            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
            App.Instance.MainPage = new MenuPage(userIdentityModel);
            //App.Instance.MainPage = new MenuProfessionalPage();
        }

        #endregion
    }
}