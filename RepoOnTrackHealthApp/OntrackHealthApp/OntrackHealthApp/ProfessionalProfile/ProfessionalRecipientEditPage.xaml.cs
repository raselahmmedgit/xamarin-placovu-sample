using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
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
	public partial class ProfessionalRecipientEditPage : CustomModalContentPage
    {
		public ProfessionalRecipientEditPage ()
		{
			InitializeComponent ();
		}

        public ProfessionalRecipientPage _professionalRecipientPage;
        public long PatientProfileId;
        public List<CountryViewModel> _countryViewModelList;
        private readonly ITokenContainer _iTokenContainer;
        SurgicalConciergeRestApiService restApiService;
        private PatientAttendeeProfileViewModel _patientAttendeeProfileViewModel;

        public ProfessionalRecipientEditPage(ProfessionalRecipientPage professionalRecipientPage, PatientAttendeeProfileViewModel patientAttendeeProfileViewModel, List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList/*, List<CountryViewModel> countryList*/)
        {
            InitializeComponent();
            this._patientAttendeeProfileViewModel = patientAttendeeProfileViewModel;
            this.BindingContext = patientAttendeeProfileViewModel;
            _countryViewModelList = new List<CountryViewModel>();
            //LoadCountryCode(countryList);
            LoadCountryCode();
            _iTokenContainer = new TokenContainer();
            this._professionalRecipientPage = professionalRecipientPage;
            restApiService = new SurgicalConciergeRestApiService();

            LoadPatientAttendeeProfileType(patientAttendeeProfileTypeViewModelList);
            LoadForm();
        }

        public ProfessionalRecipientEditPage(ProfessionalRecipientPage professionalRecipientPage, PatientAttendeeProfileViewModel patientAttendeeProfileViewModel /*, List<CountryViewModel> countryList*/)
        {
            InitializeComponent();
            this._patientAttendeeProfileViewModel = patientAttendeeProfileViewModel;
            this.BindingContext = patientAttendeeProfileViewModel;
            _countryViewModelList = new List<CountryViewModel>();
            //LoadCountryCode(countryList);
            LoadCountryCode();
            _iTokenContainer = new TokenContainer();
            this._professionalRecipientPage = professionalRecipientPage;
            restApiService = new SurgicalConciergeRestApiService();
            //LabelPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
        }
        private void LoadForm()
        {
            EmailAddress.Text = _patientAttendeeProfileViewModel.EmailAddress;
            PhoneNumber.Text = _patientAttendeeProfileViewModel.PrimaryPhone;
            AttendeeProfileId.Text = _patientAttendeeProfileViewModel.AttendeeProfileId.ToString();
        }
        private void LoadCountryCode(List<CountryViewModel> countryList)
        {
            _countryViewModelList = countryList.Where(c => !c.CountryIso.Equals("UM")).ToList();
            foreach (var countryCode in countryList)
            {
                CountryCodePicker.Items.Add("+" + countryCode.PhoneCode);
            }
            CountryCodePicker.SelectedIndex = CountryCodePicker.Items.IndexOf("+1");
        }
        private async void LoadCountryCode()
        {
            var country = await TempDataContainer.GetCountryViewModelFromJsonAsync();
            foreach (var countryViewModel in country)
            {
                CountryCodePicker.Items.Add(countryViewModel);
            }
            CountryCodePicker.SelectedIndex = CountryCodePicker.Items.IndexOf("+1");
        }
        private void EmailAddress_TextChanged(object sender, EventArgs e)
        {
            ValidateEmailAddress();
        }
        private bool ValidateEmailAddress()
        {
            HideEmailAddressAndPhoneNumberBothRequiredMessage();
            ValidationResult validationResult = ValidationHelper.ValidateEmailAddressMti(EmailAddress.Text?.Trim());
            ErrorEmailAddressAndPhoneNumber.Text = validationResult.message;
            if (validationResult.success == false)
                ErrorEmailAddressAndPhoneNumberStackLayout.IsVisible = true;
            return validationResult.success;
        }
        private void PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            ValidatePhoneNumber();
        }
        private bool ValidatePhoneNumber()
        {
            HideEmailAddressAndPhoneNumberBothRequiredMessage();
            ValidationResult validationResult = ValidationHelper.ValidatePhoneNumberMti(PhoneNumber.Text?.Trim());
            ErrorPhoneNumber.Text = validationResult.message;
            if (validationResult.success == false)
                ErrorPhoneNumber.IsVisible = true;
            return validationResult.success;
        }
        void HideEmailAddressAndPhoneNumberBothRequiredMessage()
        {
            ErrorEmailAddressAndPhoneNumberStackLayout.IsVisible = true;
            ErrorEmailAddressAndPhoneNumber.Text = "";
            //ErrorEmailAddress.IsVisible = false;
            //ErrorEmailAddress.Text = "";
            ErrorPhoneNumber.IsVisible = false;
            ErrorPhoneNumber.Text = "";
        }

        public async void ModalClose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public async void UpdateAttendeeButton_Clicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            try
            {
                ValidationResult validationResult = ValidationHelper.ValidateEmailAddressOrPhoneNumber(EmailAddress.Text, PhoneNumber.Text);
                HideEmailAddressAndPhoneNumberBothRequiredMessage();
                if (!ValidateEmailAddressOrPhoneNumber())
                {
                    return;
                }

                var emailAddress = EmailAddress.Text?.Trim();
                var phoneNumber = PhoneNumber.Text?.Trim();
                var emailSendAllowed = true;
                var smsSendAllowed = true;
                if (EmailAddress.Text?.Trim() == "")
                {
                    emailSendAllowed = false;
                }
                if (PhoneNumber.Text?.Trim() == "")
                {
                    smsSendAllowed = false;
                }

                var countryCode = CountryCodePicker.SelectedItem.ToString();
                var onePatientAttendeeProfileType = (PatientAttendeeProfileTypeSelectionViewModel)AttendeeProfileType.SelectedItem;
                var attendeeProfileTypeId = onePatientAttendeeProfileType != null ? onePatientAttendeeProfileType.Id : (int)Enums.AttendeeProfileTypeEnum.Others;
                App.ShowUserDialogAsync();

                PatientAttendeeProfileViewModel attendeeModal = new PatientAttendeeProfileViewModel();
                attendeeModal.AttendeeProfileId = _patientAttendeeProfileViewModel.AttendeeProfileId;
                attendeeModal.PatientProfileId = _patientAttendeeProfileViewModel.PatientProfileId;
                attendeeModal.EmailAddress = emailAddress;
                attendeeModal.PrimaryPhone = phoneNumber;
                attendeeModal.PrimaryPhoneCode = countryCode;
                attendeeModal.EmailAllowed = emailSendAllowed;
                attendeeModal.SmsAllowed = smsSendAllowed;
                attendeeModal.AttendeeProfileTypeId = onePatientAttendeeProfileType.Id;
                attendeeModal.AttendeeProfileTypeName = onePatientAttendeeProfileType.Name;
                ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = await restApiService.AddOrEditPatientAttendee(attendeeModal);

                //ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = await restApiService.AddOrEditPatientAttendee(attendeeModal.AttendeeProfileId, attendeeModal.PatientProfileId, emailAddress, phoneNumber, countryCode, emailSendAllowed, smsSendAllowed);
                //UtilHelper.HideLoader(loaderView);
                App.HideUserDialogAsync();

                await Navigation.PopModalAsync();
                if (apiExecutionResult.Success)
                {
                    UtilHelper.ShowToastMessage("Attendee profile updated successfully.");
                    _professionalRecipientPage.UpdateListView();
                }
                else
                {
                    UtilHelper.ShowToastMessage("Attendee profile update failed. Please try again.");
                }
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        private bool ValidateEmailAddressOrPhoneNumber()
        {
            ValidationResult validationResult = new ValidationResult();
            if (EmailAddress.Text != null || PhoneNumber.Text != null)
            {
                validationResult = ValidationHelper.ValidateEmailAddressOrPhoneNumber(EmailAddress.Text?.Trim(), PhoneNumber.Text?.Trim());

                if (validationResult.success)
                {
                    ErrorEmailAddressAndPhoneNumberStackLayout.IsVisible = false;
                    ErrorEmailAddressAndPhoneNumber.Text = string.Empty;
                }
                else
                {
                    ErrorEmailAddressAndPhoneNumberStackLayout.IsVisible = true;
                    ErrorEmailAddressAndPhoneNumber.Text = validationResult.message;
                }

                return validationResult.success;
            }
            else
            {
                validationResult.success = false;
                validationResult.message = AppConstants.AtLeastOneErrorMessage;
                ErrorEmailAddressAndPhoneNumberStackLayout.IsVisible = true;
                ErrorEmailAddressAndPhoneNumber.Text = validationResult.message;
                return validationResult.success;
            }

        }
        private void LoadPatientAttendeeProfileType(List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList)
        {
            List<PatientAttendeeProfileTypeSelectionViewModel> modelList = new List<PatientAttendeeProfileTypeSelectionViewModel>();

            if (patientAttendeeProfileTypeViewModelList.Count > 0)
            {
                patientAttendeeProfileTypeViewModelList.ForEach(x =>
                {
                    modelList.Add(new PatientAttendeeProfileTypeSelectionViewModel() { Id = x.AttendeeProfileTypeId, Name = x.AttendeeProfileTypeName });
                });
            }
            var selectedItem = modelList.FirstOrDefault(x => x.Id == _patientAttendeeProfileViewModel.AttendeeProfileTypeId);

            AttendeeProfileType.ItemsSource = modelList;
            AttendeeProfileType.SelectedItem = selectedItem != null ? selectedItem : null;
        }
        private void EmailAddres_Validate(object sender, EventArgs e)
        {
            ValidateEmailAddressOrPhoneNumber();
        }
        private void PhoneNumber_Validate(object sender, EventArgs e)
        {
            ValidateEmailAddressOrPhoneNumber();
        }

    }
}