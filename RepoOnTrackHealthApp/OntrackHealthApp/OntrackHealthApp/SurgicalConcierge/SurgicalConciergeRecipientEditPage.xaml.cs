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

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergeRecipientEditPage : CustomModalContentPage
    {
        public SurgicalConciergeRecipientPage _surgicalConciergeRecipientPage;
        public long PatientProfileId;
        public List<CountryViewModel> _countryViewModelList;
        private readonly ITokenContainer _iTokenContainer;
        SurgicalConciergeRestApiService restApiService;
        private PatientAttendeeProfileViewModel _patientAttendeeProfileViewModel;
        public List<PatientAttendeeProfileTypeViewModel> _patientAttendeeProfileTypeViewModels;
        public SurgicalConciergeRecipientEditPage(SurgicalConciergeRecipientPage surgicalConciergeRecipientPage, PatientAttendeeProfileViewModel patientAttendeeProfileViewModel, List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList)
        {
            InitializeComponent();
            this._patientAttendeeProfileViewModel = patientAttendeeProfileViewModel;
            this.BindingContext = patientAttendeeProfileViewModel;
            _countryViewModelList = new List<CountryViewModel>();
            _iTokenContainer = new TokenContainer();
            restApiService = new SurgicalConciergeRestApiService();
            this._surgicalConciergeRecipientPage = surgicalConciergeRecipientPage;
            _patientAttendeeProfileTypeViewModels = patientAttendeeProfileTypeViewModelList;
            LoadCountryCode();
            LoadPatientAttendeeProfileType();
            LoadForm();
        }
        public SurgicalConciergeRecipientEditPage(SurgicalConciergeRecipientPage surgicalConciergeRecipientPage, PatientAttendeeProfileViewModel patientAttendeeProfileViewModel /*, List<CountryViewModel> countryList*/)
        {
            InitializeComponent();
            this._patientAttendeeProfileViewModel = patientAttendeeProfileViewModel;
            this.BindingContext = patientAttendeeProfileViewModel;
            _countryViewModelList = new List<CountryViewModel>();
            //LoadCountryCode(countryList);
            LoadCountryCode();
            _iTokenContainer = new TokenContainer();
            this._surgicalConciergeRecipientPage = surgicalConciergeRecipientPage;
            restApiService = new SurgicalConciergeRestApiService();
            //LabelPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
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
        private void LoadPatientAttendeeProfileType()
        {
            List<PatientAttendeeProfileTypeSelectionViewModel> modelList = new List<PatientAttendeeProfileTypeSelectionViewModel>();

            if (_patientAttendeeProfileTypeViewModels.Count > 0)
            {
                _patientAttendeeProfileTypeViewModels.ForEach(x =>
                {
                    modelList.Add(new PatientAttendeeProfileTypeSelectionViewModel() { Id = x.AttendeeProfileTypeId, Name = x.AttendeeProfileTypeName });
                });
            }
            var selectedItem = modelList.FirstOrDefault(x => x.Id == _patientAttendeeProfileViewModel.AttendeeProfileTypeId);
            AttendeeProfileType.ItemsSource = modelList;
            AttendeeProfileType.SelectedItem = selectedItem != null ? selectedItem : null;
        }
        private void LoadForm()
        {
            EmailAddress.Text = _patientAttendeeProfileViewModel.EmailAddress;
            PhoneNumber.Text = _patientAttendeeProfileViewModel.PrimaryPhone;
            AttendeeProfileId.Text = _patientAttendeeProfileViewModel.AttendeeProfileId.ToString();
        }

        private bool ValidateEmailAddress()
        {
            ValidationResult validationResult = ValidationHelper.ValidateEmailAddress(EmailAddress.Text?.Trim());
            ErrorEmailAddressAndPhoneNumber.Text = validationResult.message;
            return validationResult.success;
        }
        private bool ValidatePhoneNumber()
        {
            ValidationResult validationResult = ValidationHelper.ValidatePhoneNumberMti(PhoneNumber.Text?.Trim());
            ErrorEmailAddressAndPhoneNumber.Text = validationResult.message;
            return validationResult.success;
        }
        private void EmailAddres_Validate(object sender, EventArgs e)
        {
            ValidateEmailAddressOrPhoneNumber();
        }
        private void PhoneNumber_Validate(object sender, EventArgs e)
        {
            ValidateEmailAddressOrPhoneNumber();
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

        public async void ModalCloseButton_Clicked(object sender, EventArgs e)
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
                App.ShowUserDialogAsync();

                var countryCode = CountryCodePicker.SelectedItem.ToString();
                var onePatientAttendeeProfileType = (PatientAttendeeProfileTypeSelectionViewModel)AttendeeProfileType.SelectedItem;
                var attendeeProfileId = _patientAttendeeProfileViewModel.AttendeeProfileId;

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

                await Navigation.PopModalAsync();
                if (apiExecutionResult.Success)
                {
                    UtilHelper.ShowToastMessage("Attendee profile updated successfully.");
                    _surgicalConciergeRecipientPage.UpdateListView();
                }
                else
                {
                    UtilHelper.ShowToastMessage("Attendee profile update failed. Please try again.");
                }
                App.HideUserDialogAsync();
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }


    }
}