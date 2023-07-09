using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OntrackHealthApp.ApiHelper.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OntrackHealthApp.SurgicalConcierge;

namespace OntrackHealthApp.ScgPathology
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScgPathologyPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        ScgPatientPathologyViewModel scgPatientPathologyViewModel { get; set; }
        private SurgicalConciergePathologyClient _SurgicalConciergePathologyClient;
        private PatientProfileWithProfessionalProcedureView PatientProfileWithProfessionalProcedureView { get; set; }

        public ScgPathologyPage(PatientProfileWithProfessionalProcedureView patientProfileWithProfessionalProcedureView)
        {
            InitializeComponent();
            PatientProfileWithProfessionalProcedureView = patientProfileWithProfessionalProcedureView;
            scgPatientPathologyViewModel = new ScgPatientPathologyViewModel();
            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                if (!_iTokenContainer.IsApiToken())
                {
                    //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                    App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
                }
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _SurgicalConciergePathologyClient = new SurgicalConciergePathologyClient(apiClient);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private void InitializeForm() {
            List<ScgPathologyModel> modelPsa = new List<ScgPathologyModel>() {
                new ScgPathologyModel{ Id = 1, Name = "0 - 4"},
                new ScgPathologyModel{ Id = 1, Name = "5 - 10"},
                new ScgPathologyModel{ Id = 1, Name = "11 - 20"},
                new ScgPathologyModel{ Id = 1, Name = "> 20"}
            };

            List<ScgPathologyModel> modelGleason = new List<ScgPathologyModel>() {
                new ScgPathologyModel{ Id = 1, Name = "3 + 3"},
                new ScgPathologyModel{ Id = 1, Name = "3 + 4"},
                new ScgPathologyModel{ Id = 1, Name = "3 + 5"},
                new ScgPathologyModel{ Id = 1, Name = "4 + 3"},
                new ScgPathologyModel{ Id = 1, Name = "4 + 4"},
                new ScgPathologyModel{ Id = 1, Name = "4 + 5"},
                new ScgPathologyModel{ Id = 1, Name = "5 + 3"},
                new ScgPathologyModel{ Id = 1, Name = "5 + 4"},
                new ScgPathologyModel{ Id = 1, Name = "5 + 5"}
            };

            List<ScgPathologyModel> modelStage = new List<ScgPathologyModel>() {
                new ScgPathologyModel{ Id = 1, Name = "T2"},
                new ScgPathologyModel{ Id = 1, Name = "T3a"},
                new ScgPathologyModel{ Id = 1, Name = "T3b"}
            };
            List<ScgPathologyModel> modelMargin = new List<ScgPathologyModel>() {
                new ScgPathologyModel{ Id = 1, Name = "-"},
                new ScgPathologyModel{ Id = 1, Name = "+"}
            };
            List<ScgPathologyModel> modelNodeStatus = new List<ScgPathologyModel>() {
                new ScgPathologyModel{ Id = 1, Name = "-"},
                new ScgPathologyModel{ Id = 1, Name = "+"}
            };
            selectionViewPsa.ItemsSource = modelPsa;
            selectionViewPsa.SelectedItem = null;

            selectionViewGleason.ItemsSource = modelGleason;
            selectionViewGleason.SelectedItem = null;

            selectionViewStage.ItemsSource = modelStage;
            selectionViewStage.SelectedItem = null;

            selectionViewMargin.ItemsSource = modelMargin;
            selectionViewMargin.SelectedItem = null;

            selectionViewNodeStatus.ItemsSource = modelNodeStatus;
            selectionViewNodeStatus.SelectedItem = null;

            PatientFullName.Text = "Patient : " + PatientProfileWithProfessionalProcedureView.PatientFullName;
            ProcedureName.Text = "Procedure : " + PatientProfileWithProfessionalProcedureView.ProcedureName;
            ProfessionalName.Text = "Professional : " + PatientProfileWithProfessionalProcedureView.ProfessionalName;

        }

        private async void btnSave_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    int selectedSection = 0;
                    

                    scgPatientPathologyViewModel.PatientProcedureDetailId = PatientProfileWithProfessionalProcedureView.PatientProcedureDetailId;
                    scgPatientPathologyViewModel.PatientProfileId = PatientProfileWithProfessionalProcedureView.PatientProfileId;
                    scgPatientPathologyViewModel.PracticeProfileId = PatientProfileWithProfessionalProcedureView.PracticeProfileId;
                    scgPatientPathologyViewModel.ProfessionalProfileId = PatientProfileWithProfessionalProcedureView.ProfessionalProfileId.ToLong();
                    scgPatientPathologyViewModel.ProcedureId = PatientProfileWithProfessionalProcedureView.ProcedureId.ToLong();

                    var onePsa = (ScgPathologyModel)selectionViewPsa.SelectedItem;
                    var oneGleason = (ScgPathologyModel)selectionViewGleason.SelectedItem;
                    var oneStage = (ScgPathologyModel)selectionViewStage.SelectedItem;
                    var oneMargin = (ScgPathologyModel)selectionViewMargin.SelectedItem;
                    var oneNodeStatus = (ScgPathologyModel)selectionViewNodeStatus.SelectedItem;

                    if (onePsa != null)
                    {
                        scgPatientPathologyViewModel.PreopPsa = onePsa.Name;
                        selectedSection++;
                    }
                    if (oneGleason != null)
                    {
                        scgPatientPathologyViewModel.GleasonScore = oneGleason.Name;
                        selectedSection++;
                    }
                    if (oneStage != null)
                    {
                        scgPatientPathologyViewModel.StageScore = oneStage.Name;
                        selectedSection++;
                    }
                    if (oneMargin != null)
                    {
                        scgPatientPathologyViewModel.MarginScore = oneMargin.Name;
                        selectedSection++;
                    }
                    if (oneNodeStatus != null)
                    {
                        scgPatientPathologyViewModel.NodeStatus = oneNodeStatus.Name;
                        selectedSection++;
                    }
                    if(selectedSection == 0)
                    {
                        await DisplayAlert("Warning!", AppConstant.NullReferenceExceptionError, AppConstant.MessageOkButtonText);
                    }
                    else
                    {
                        if (InternetConnectHelper.CheckConnection())
                        {
                            var response = await _SurgicalConciergePathologyClient.PostSurgicalConciergePatientPathologyAsync(scgPatientPathologyViewModel);
                            if (response.StatusIsSuccessful)
                            {
                                await DisplayAlert("Success!", AppConstant.SaveSuccessMessage, AppConstant.MessageOkButtonText);
                            }

                        }
                        else
                        {
                            await DisplayAlert("Warning!", AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                        }
                    }
                }
                
            }
            catch
            {
                await DisplayAlert("Warning!", AppConstant.DisplayAlertErrorMessage, AppConstant.MessageOkButtonText);
            }            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeForm();
        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            selectionViewPsa.SelectedItem = null;
            selectionViewGleason.SelectedItem = null;
            selectionViewStage.SelectedItem = null;
            selectionViewMargin.SelectedItem = null;
            selectionViewNodeStatus.SelectedItem = null;
        }

        #region Top Menu Actions

        private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new AdminChangePassword());
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

        #endregion
    }
}