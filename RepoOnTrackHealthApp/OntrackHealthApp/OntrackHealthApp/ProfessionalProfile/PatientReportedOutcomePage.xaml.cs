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
    public partial class PatientReportedOutcomePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;

        public List<string> SelectedProcedure { get; set; }
        public List<PatientReportedOutcomePageViewModel> PatientReportedOutcomePageViewModels = new List<PatientReportedOutcomePageViewModel>();

        public PatientReportedOutcomePage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                SelectedProcedure = new List<string>();

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

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

                    PatientReportedOutcomePageViewModels = GetTypeList();

                    if (PatientReportedOutcomePageViewModels != null)
                    {
                        //BindableLayout.SetItemsSource(PatientReportedOutcomeListView, PatientReportedOutcomePageViewModels);
                        //BindableLayout.SetItemTemplateSelector(PatientReportedOutcomeListView, FlexDataTemplate);
                        #region Image source
                        AllRegisteredPatientImage.Source = PatientReportedOutComeReportTypeName.AllRegisteredPatientIcon.ToString();
                        SurveyCriticalImage.Source = PatientReportedOutComeReportTypeName.SurveyCriticalIcon.ToString();
                        SurveyCompletedImage.Source = PatientReportedOutComeReportTypeName.SurveyCompletedIcon.ToString();
                        SurveyPendingImage.Source = PatientReportedOutComeReportTypeName.SurveyPendingIcon.ToString();
                        #endregion

                        #region Label Text
                        AllRegisteredPatientLabel.Text = PatientReportedOutComeReportTypeName.AllRegisteredPatient.ToString();
                        SurveyCriticalLabel.Text = PatientReportedOutComeReportTypeName.SurveyCritical.ToString();
                        SurveyCompletedLabel.Text = PatientReportedOutComeReportTypeName.SurveyCompleted.ToString();
                        SurveyPendingLabel.Text = PatientReportedOutComeReportTypeName.SurveyPending.ToString();
                        #endregion

                        #region Binding Context 
                        AllRegisteredPatientFrame.BindingContext = PatientReportedOutcomePageViewModels.FirstOrDefault(m => m.TypeId == Enums.PatientReportedOutComeReportType.AllRegisteredPatient.ToInt());
                        SurveyCriticalFrame.BindingContext = PatientReportedOutcomePageViewModels.FirstOrDefault(m => m.TypeId == Enums.PatientReportedOutComeReportType.FlaggedResult.ToInt());
                        SurveyCompletedFrame.BindingContext = PatientReportedOutcomePageViewModels.FirstOrDefault(m => m.TypeId == Enums.PatientReportedOutComeReportType.PastMonthCompletedSurvey.ToInt());
                        SurveyPendingFrame.BindingContext = PatientReportedOutcomePageViewModels.FirstOrDefault(m => m.TypeId == Enums.PatientReportedOutComeReportType.PastMonthPendingSurvey.ToInt());
                        #endregion

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

        private List<PatientReportedOutcomePageViewModel> GetTypeList()
        {
            var patientReportedOutcomePageViewModels = new List<PatientReportedOutcomePageViewModel>() {
                        new PatientReportedOutcomePageViewModel { TypeId = Enums.PatientReportedOutComeReportType.AllRegisteredPatient.ToInt(), TypeName = PatientReportedOutComeReportTypeName.AllRegisteredPatient.ToString(), TypeBgColor = PatientReportedOutComeReportTypeName.AllRegisteredPatientBgColor.ToString(), TypeIcon = PatientReportedOutComeReportTypeName.AllRegisteredPatientIcon.ToString(), PracticeId = _iTokenContainer.ApiPracticeProfileId.ToLong(), DivisionId = 0, DivisionUnitId = 0, IsActive = true, DisplayOrder = 1 },
                        //new PatientReportedOutcomePageViewModel { TypeId = Enums.PatientReportedOutComeReportType.ProgramStarted.ToInt(), TypeName = PatientReportedOutComeReportTypeName.ProgramStarted.ToString(), TypeBgColor = PatientReportedOutComeReportTypeName.ProgramStartedBgColor.ToString(), TypeIcon = PatientReportedOutComeReportTypeName.ProgramStartedIcon.ToString(), PracticeId = _iTokenContainer.ApiPracticeProfileId.ToLong(), DivisionId = 0, DivisionUnitId = 0, IsActive = true, DisplayOrder = 2 },
                        new PatientReportedOutcomePageViewModel { TypeId = Enums.PatientReportedOutComeReportType.FlaggedResult.ToInt(), TypeName = PatientReportedOutComeReportTypeName.SurveyCritical.ToString(), TypeBgColor = PatientReportedOutComeReportTypeName.SurveyCriticalBgColor.ToString(), TypeIcon = PatientReportedOutComeReportTypeName.SurveyCriticalIcon.ToString(), PracticeId = _iTokenContainer.ApiPracticeProfileId.ToLong(), DivisionId = 0, DivisionUnitId = 0, IsActive = true, DisplayOrder = 3 },
                        new PatientReportedOutcomePageViewModel { TypeId = Enums.PatientReportedOutComeReportType.PastMonthCompletedSurvey.ToInt(), TypeName = PatientReportedOutComeReportTypeName.SurveyCompleted.ToString(), TypeBgColor = PatientReportedOutComeReportTypeName.SurveyCompletedBgColor.ToString(), TypeIcon = PatientReportedOutComeReportTypeName.SurveyCompletedIcon.ToString(), PracticeId = _iTokenContainer.ApiPracticeProfileId.ToLong(), DivisionId = 0, DivisionUnitId = 0, IsActive = true, DisplayOrder = 4 },
                        new PatientReportedOutcomePageViewModel { TypeId = Enums.PatientReportedOutComeReportType.PastMonthPendingSurvey.ToInt(), TypeName = PatientReportedOutComeReportTypeName.SurveyPending.ToString(), TypeBgColor = PatientReportedOutComeReportTypeName.SurveyPendingBgColor.ToString(), TypeIcon = PatientReportedOutComeReportTypeName.SurveyPendingIcon.ToString(), PracticeId = _iTokenContainer.ApiPracticeProfileId.ToLong(), DivisionId = 0, DivisionUnitId = 0, IsActive = true, DisplayOrder = 5 },
                        //new PatientReportedOutcomePageViewModel { TypeId = Enums.PatientReportedOutComeReportType.SearchPatientDirectory.ToInt(), TypeName = PatientReportedOutComeReportTypeName.SearchPatient.ToString(), TypeBgColor = PatientReportedOutComeReportTypeName.SearchPatientBgColor.ToString(), TypeIcon = PatientReportedOutComeReportTypeName.SearchPatientIcon.ToString(), PracticeId = _iTokenContainer.ApiPracticeProfileId.ToLong(), DivisionId = 0, DivisionUnitId = 0, IsActive = true, DisplayOrder = 6 }
                    };

            return patientReportedOutcomePageViewModels;
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

        private async void BackButton_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void BtnHome_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                //App.ShowUserDialogAsync();
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                App.Instance.MainPage = new MenuPage(userIdentityModel);
                //App.Instance.MainPage = new MenuProfessionalPage();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void SearchButton_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await Navigation.PushAsync(new PatientReportedOutcomeSearchPage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #endregion

        async void PatientReportedOutcomeListView_ItemSelected(object sender, EventArgs e)
        {
            try
            {
                await App.ShowUserDialogDelayAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    var obj = (Frame)sender;
                    var data = (PatientReportedOutcomePageViewModel)obj.BindingContext;

                    if (data != null)
                    {
                        //PatientReportedOutcomeListView.SelectedItem = null;

                        if (data.TypeId == Enums.PatientReportedOutComeReportType.SearchPatientDirectory.ToInt())
                        {
                            await Navigation.PushAsync(new PatientReportedOutcomeSearchPage(data));
                        }
                        else
                        {
                            data.SelectedProcedure = SelectedProcedure;
                            await Navigation.PushAsync(new PatientReportedOutcomeMonthPage(data));
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
                App.HideUserDialogAsync();
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void PatientOutcomeSearch_Clicked(object sender, EventArgs e)
        {
            try
            {
                await App.ShowUserDialogDelayAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    PatientReportedOutcomePageViewModel patientReportedOutcomePageViewModel = new PatientReportedOutcomePageViewModel();
                    patientReportedOutcomePageViewModel = PatientReportedOutcomePageViewModels.Find(x => x.TypeId == Enums.PatientReportedOutComeReportType.SearchPatientDirectory.ToInt());
                    await Navigation.PushAsync(new PatientReportedOutcomeSearchPage(patientReportedOutcomePageViewModel));
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