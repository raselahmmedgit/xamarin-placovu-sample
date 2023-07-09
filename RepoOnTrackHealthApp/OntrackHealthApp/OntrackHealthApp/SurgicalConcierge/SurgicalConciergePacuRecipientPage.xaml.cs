using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePacuRecipientPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();

        private SurgicalConciergePacuViewModel SurgicalConciergePacuViewModel { get; set; }
        private PatientProfileWithProfessionalProcedureView PatientProfileWithProfessionalProcedureView { get; set; }
        SurgicalConciergeRecipientPageViewModel viewModel = new SurgicalConciergeRecipientPageViewModel();

        public SurgicalConciergePacuRecipientPage(PatientProfileWithProfessionalProcedureView patientProfileWithProfessionalProcedureView)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            PatientProfileWithProfessionalProcedureView = patientProfileWithProfessionalProcedureView;
            PatientFullName.Text = "Patient : " + PatientProfileWithProfessionalProcedureView.PatientFullName;
            ProcedureName.Text = "Procedure : " + PatientProfileWithProfessionalProcedureView.ProcedureName;
            ProfessionalName.Text = "Professional : " + PatientProfileWithProfessionalProcedureView.ProfessionalName;
            Title = _iTokenContainer.ApiPracticeName;
            BindingContext = viewModel = new SurgicalConciergeRecipientPageViewModel();
            viewModel.PatientProfileId = patientProfileWithProfessionalProcedureView.PatientProfileId;
        }

        public void UpdateListView()
        {
            viewModel.LoadPatientAttendeeProfilesCommand.Execute(PatientProfileWithProfessionalProcedureView.PatientProfileId);
        }

        private async void btnAddRecipient_OnClickedAsync(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    List<CountryViewModel> countryList = await restService.GetCountryList();
                    await Navigation.PushModalAsync(new SurgicalConciergePacuRecipientAddPage(this, PatientProfileWithProfessionalProcedureView.PatientProfileId, countryList));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }

        private async void ShowDeleteDialogAsync(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                Button btn = (Button)sender;
                var id = btn.ClassId.ToLong();

                var answer = await DisplayAlert("Delete Confirm", "Are you sure you want to delete this attendee profile?", "Yes", "No");
                if (answer)
                {
                    // Call delete Functioin
                    var patientAttendeeProfileViewModel = viewModel.PatientAttendeeProfileViewModels.Where(x => x.AttendeeProfileId == id).FirstOrDefault();
                    DeleteAttendee(patientAttendeeProfileViewModel);
                }
            }
        }

        private async void DeleteAttendee(PatientAttendeeProfileViewModel patientAttendeeProfileViewModel)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                    ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = await restService.DeleteAttendee(patientAttendeeProfileViewModel);
                    if (apiExecutionResult.Success)
                    {
                        UpdateListView();
                        UtilHelper.ShowToastMessage("Attendee profile deleted successfully.");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage("Attendee profile delete failed. Please try again.");
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
            viewModel.LoadPatientAttendeeProfilesCommand.Execute(PatientProfileWithProfessionalProcedureView.PatientProfileId);
        }

        private async void btnContinueToProgram_ClickedAsync(object sender, EventArgs e)
        {
        }
    }
}