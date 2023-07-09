using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.Response;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessionalPathologyPage : ContentPage
    {
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private ScgPatientPathologyViewModel _scgPatientPathologyViewModel { get; set; }
        private SurgicalConciergePathologyClient _surgicalConciergePathologyClient;

        public ProfessionalPathologyPage(SurgicalConciergePatientViewModel surgicalConciergeViewModel)
        {
            InitializeComponent();
            _surgicalConciergePatientViewModel = surgicalConciergeViewModel;
            _scgPatientPathologyViewModel = new ScgPatientPathologyViewModel();
            _iTokenContainer = new TokenContainer();
            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                if (!_iTokenContainer.IsApiToken())
                {
                    //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                    App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
                }
                Title = _iTokenContainer.ApiPracticeName;

                //ShowToolbar();

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _surgicalConciergePathologyClient = new SurgicalConciergePathologyClient(apiClient);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private void InitializeForm()
        {
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
                new ScgPathologyModel{ Id = 1, Name = "T3b"},
                new ScgPathologyModel{ Id = 1, Name = "T4"}
            };

            List<ScgPathologyModel> modelMargin = new List<ScgPathologyModel>() {
                new ScgPathologyModel{ Id = 1, Name = "-"},
                new ScgPathologyModel{ Id = 1, Name = "+"}
            };
            List<ScgPathologyModel> modelNodeStatus = new List<ScgPathologyModel>() {
                new ScgPathologyModel{ Id = 1, Name = "-"},
                new ScgPathologyModel{ Id = 1, Name = "+"}
            };
            List<ScgPathologyModel> modelMetastasis = new List<ScgPathologyModel>() {
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

            selectionViewMetastasis.ItemsSource = modelMetastasis;
            selectionViewMetastasis.SelectedItem = null;

            PatientFullName.Text = _surgicalConciergePatientViewModel.PatientFullName;
            ProcedureName.Text = _surgicalConciergePatientViewModel.ProcedureName;
            ProfessionalName.Text = _surgicalConciergePatientViewModel.ProfessionalName;

        }

        private async void SaveButton_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                int selectedSection = 0;

                _scgPatientPathologyViewModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid();
                _scgPatientPathologyViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                _scgPatientPathologyViewModel.PracticeProfileId = _surgicalConciergePatientViewModel.PracticeProfileId;
                _scgPatientPathologyViewModel.ProfessionalProfileId = _surgicalConciergePatientViewModel.ProfessionalProfileId.ToLong();
                _scgPatientPathologyViewModel.ProcedureId = _surgicalConciergePatientViewModel.ProcedureId.ToLong();

                var onePsa = (ScgPathologyModel)selectionViewPsa.SelectedItem;
                var oneGleason = (ScgPathologyModel)selectionViewGleason.SelectedItem;
                var oneStage = (ScgPathologyModel)selectionViewStage.SelectedItem;
                var oneMargin = (ScgPathologyModel)selectionViewMargin.SelectedItem;
                var oneNodeStatus = (ScgPathologyModel)selectionViewNodeStatus.SelectedItem;
                var oneMetastasis = (ScgPathologyModel)selectionViewMetastasis.SelectedItem;

                if (!string.IsNullOrEmpty(CancerInvolvement.Text))
                {
                    _scgPatientPathologyViewModel.CancerInvolvement = CancerInvolvement.Text.ToDecimal();
                    selectedSection++;
                }
                if (onePsa != null)
                {
                    _scgPatientPathologyViewModel.PreopPsa = onePsa.Name;
                    selectedSection++;
                }
                if (oneGleason != null)
                {
                    _scgPatientPathologyViewModel.GleasonScore = oneGleason.Name;
                    selectedSection++;
                }
                if (oneStage != null)
                {
                    _scgPatientPathologyViewModel.StageScore = oneStage.Name;
                    selectedSection++;
                }
                if (oneMargin != null)
                {
                    _scgPatientPathologyViewModel.MarginScore = oneMargin.Name;
                    selectedSection++;
                }
                if (oneNodeStatus != null)
                {
                    _scgPatientPathologyViewModel.NodeStatus = oneNodeStatus.Name;
                    selectedSection++;
                }
                if (oneMetastasis != null)
                {
                    _scgPatientPathologyViewModel.Metastasis = oneMetastasis.Name;
                    selectedSection++;
                }
                if (selectedSection == 0)
                {
                    await DisplayAlert("Warning!", AppConstant.NullReferenceExceptionError, AppConstant.MessageOkButtonText);
                }
                else
                {
                    if (InternetConnectHelper.CheckConnection())
                    {
                        SurgicalConciergePathologyResponse response = new SurgicalConciergePathologyResponse();
                        App.ShowUserDialogAsync();
                        response = await _surgicalConciergePathologyClient.PostSurgicalConciergePatientPathologyAsync(_scgPatientPathologyViewModel);
                        App.HideUserDialog();
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

        private void ShowToolbar()
        {
            var toolbarItemToBeRemoved = ToolbarItems.FirstOrDefault(c => c.Text.Equals("Recipient"));
            if (toolbarItemToBeRemoved != null)
                ToolbarItems.Remove(toolbarItemToBeRemoved);

            if (Device.RuntimePlatform == Device.iOS)
            {
                // move layout under the status bar
                this.Padding = new Thickness(0, 20, 0, 0);

                var toolbarItem = new ToolbarItem("Recipient", null, () =>
                {
                    Navigation.PushAsync(new ProfessionalRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.Pathology));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }
            else
            {
                var toolbarItem = new ToolbarItem("Recipient", null, () =>
                {
                    Navigation.PushAsync(new ProfessionalRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.Pathology));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }

        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            selectionViewPsa.SelectedItem = null;
            selectionViewGleason.SelectedItem = null;
            selectionViewStage.SelectedItem = null;
            selectionViewMargin.SelectedItem = null;
            selectionViewNodeStatus.SelectedItem = null;
            selectionViewMetastasis.SelectedItem = null;
            CancerInvolvement.Text = "";
        }
    }
}