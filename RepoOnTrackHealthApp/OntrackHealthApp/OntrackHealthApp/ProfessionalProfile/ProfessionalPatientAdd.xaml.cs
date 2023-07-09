using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
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
	public partial class ProfessionalPatientAdd : CustomModalContentPage
    {
        private bool IsValid { get; set; } = false;

        public long PracticeDivisionUnitDest = 0;
        private readonly ITokenContainer _iTokenContainer;
        private AdministrationPatientProfileRestApiService adminRestApiService;
        private SurgicalConciergeRestApiService restApiService;
        private long practiceProfileId;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel;
        private ProfessionalOperatingPage professionalOperatingPage;
        private ProfessionalPatientPage professionalPatientPage;
        private bool isProfessionalOperatingPagePage = false;
        private ProfessionalPatientPage ProfessionalPatientPage { get; set; }

        public ProfessionalPatientAdd(ProfessionalPatientPage professionalPatientPage, long practiceDivisionUnit)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            ProfessionalPatientPage = professionalPatientPage;
            SetSurgicalConciergePatientViewModelData();
        }

        public ProfessionalPatientAdd(ProfessionalOperatingPage professionalOperatingPage, bool isProfessionalOperatingPagePage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this.professionalOperatingPage = professionalOperatingPage;
            this.isProfessionalOperatingPagePage = isProfessionalOperatingPagePage;
            SetSurgicalConciergePatientViewModelData();
        }

        public ProfessionalPatientAdd(ProfessionalPatientPage professionalPatientPage, bool isProfessionalOperatingPagePage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            adminRestApiService = new AdministrationPatientProfileRestApiService();
            restApiService = new SurgicalConciergeRestApiService();
            this.professionalPatientPage = professionalPatientPage;
            this.isProfessionalOperatingPagePage = isProfessionalOperatingPagePage;
            SetSurgicalConciergePatientViewModelData();
        }

        private async void SetSurgicalConciergePatientViewModelData()
        {
            SurgicalConciergePatientViewModel data = await restApiService.GetSurgicalConciergePatient();
            _surgicalConciergePatientViewModel = data;
            practiceProfileId = (long)_iTokenContainer.ApiPracticeProfileId;
            PracticeProfileNameTextBox.Text = _iTokenContainer.ApiPracticeName;
            if (_surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions != null)
            {
                PracticeProcedurePicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProcedureDropDownViewModel.SelectOptions.ToList();
            }
            else
            {
                PracticeProcedurePicker.ItemsSource = new List<SelectListItem>();
            }
            PracticeProcedurePicker.SelectedIndex = 0;

            if (_surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel != null && _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions != null)
            {
                PracticeProfessionalPicker.ItemsSource = _surgicalConciergePatientViewModel.ProfessionalProfileDropDownViewModel.SelectOptions.ToList();
            }
            else
            {
                PracticeProfessionalPicker.ItemsSource = new List<SelectListItem>();
            }
            PracticeProfessionalPicker.SelectedIndex = 0;
        }

        public void UpdateProfessionalProfileDropdown(IEnumerable<SelectListItem> professionalList)
        {
            PracticeProfessionalPicker.ItemsSource = professionalList.ToList();
            PracticeProfessionalPicker.SelectedIndex = 0;
        }

        private async void SurgicalConciergePatientAddButton_Clicked(object sender, EventArgs e)
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
                    if (!FormValidationSuccess())
                        return;

                    SurgicalConciergePatientViewModel model = new SurgicalConciergePatientViewModel();
                    model.FirstName = FirstNameTextBox.Text?.Trim();
                    model.LastName = LastNameTextBox.Text?.Trim();
                    model.PracticeProfileId = practiceProfileId;
                    model.EmailAddress = EmailTextBox.Text?.Trim();

                    var selectedProcedure = PracticeProcedurePicker.SelectedItem as SelectListItem;
                    if (selectedProcedure != null)
                    {
                        model.ProcedureId = long.Parse(selectedProcedure.Value);
                        model.ProcedureName = selectedProcedure.Text?.Trim();
                    }

                    var selectedProfessional = PracticeProfessionalPicker.SelectedItem as SelectListItem;
                    if (selectedProfessional != null)
                    {
                        model.ProfessionalProfileId = long.Parse(selectedProfessional.Value);
                        model.ProfessionalName = selectedProfessional.Text?.Trim();
                    }

                    model.SurgeryDate = SurgeryDateDatePicker.Date;
                    model.SurgeryDateInString = model.SurgeryDate.GetValueOrDefault().ToString("dd/MMM/yyyy");
                    model.SurgeryTime = SurgeryTimeTimePicker.Time.ToString();
                    model.SurgeryDateTime = SurgeryDateDatePicker.Date + SurgeryTimeTimePicker.Time;

                    ApiExecutionResult<SurgicalConciergePatientViewModel> result = await restApiService.PostSurgicalConciergePatient(model);
                    if (result.Success)
                    {

                        var surgicalConciergeViewModel = await restApiService.GetSurgicalConciergeStageList(result.Message);
                        if (isProfessionalOperatingPagePage)
                        {
                            professionalOperatingPage.RebindData(surgicalConciergeViewModel, model);
                            //await Navigation.PopModalAsync();
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            //await Navigation.PopModalAsync();
                            await Navigation.PopAsync();
                            model.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;
                            model.PatientProcedureDetailId = new Guid(result.Message);

                            professionalPatientPage.NavigationToDestination(model);
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

        private async void SurgicalConciergePatientCancelButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PopModalAsync();
            await Navigation.PopAsync();
        }

        private async void ProfessionalAddButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    await Navigation.PushModalAsync(new ProfessionalAddPage(this));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private bool FormValidationSuccess()
        {
            bool isValid = true;
            ErrorFirstNameTextBox.IsVisible = false;
            ErrorLastNameTextBox.IsVisible = false;

            if (!ValidateFirstNameTextBox() || !ValidateLastNameTextBox() || !ValidateProcedurePicker() || !ValidateProfessionalPicker())
            {
                isValid = false;
            }
            return isValid;

        }

        private void FirstNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateFirstNameTextBox();
        }
        private void LastNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateLastNameTextBox();
        }
        private void EmailTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateEmailAddress();
        }
        private void PracticeProcedurePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateProcedurePicker();
        }
        private void PracticeProfessionalPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateProfessionalPicker();
        }

        public bool ValidateFirstNameTextBox()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(FirstNameTextBox.Text))
            {
                isValid = false;
                ErrorFirstNameTextBox.Text = "First Name Required";
                ErrorFirstNameTextBox.IsVisible = true;
            }
            else
            {
                ErrorFirstNameTextBox.IsVisible = false;
            }
            return isValid;
        }

        public bool ValidateLastNameTextBox()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(LastNameTextBox.Text))
            {
                isValid = false;
                ErrorLastNameTextBox.Text = "Last Name Required";
                ErrorLastNameTextBox.IsVisible = true;
            }
            else
            {
                ErrorLastNameTextBox.IsVisible = false;
            }
            return isValid;
        }

        public bool ValidateProcedurePicker()
        {
            bool isValid = true;
            if (PracticeProcedurePicker.SelectedIndex == AppConstants.PickerDefaultIndex)
            {
                isValid = false;
                ErrorPracticeProcedurePicker.Text = "Select a Procedure";
                ErrorPracticeProcedurePicker.IsVisible = true;
            }
            else
            {
                ErrorPracticeProcedurePicker.IsVisible = false;
            }
            return isValid;
        }

        public bool ValidateProfessionalPicker()
        {
            bool isValid = true;
            if (PracticeProfessionalPicker.SelectedIndex == AppConstants.PickerDefaultIndex)
            {
                isValid = false;
                ErrorPracticeProfessionalPicker.Text = "Select a Professional";
                ErrorPracticeProfessionalPicker.IsVisible = true;
            }
            else
            {
                ErrorPracticeProfessionalPicker.IsVisible = false;
            }
            return isValid;
        }

        private bool ValidateEmailAddress()
        {
            ValidationResult validationResult = ValidationHelper.ValidateEmailAddress(EmailTextBox.Text?.Trim());
            ErrorEmailTextBox.Text = validationResult.message;
            ErrorEmailTextBox.IsVisible = !validationResult.success;
            if (EmailTextBox.Text == "")
            {
                ErrorEmailTextBox.IsVisible = false;
            }
            return validationResult.success;
        }
    }
}