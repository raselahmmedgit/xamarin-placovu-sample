﻿using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalPatientAttendeeAddPage : CustomModalContentPage
    {

        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalPatientAddNew _professionalPatientAddNew { get; set; }
        public long PatientProfileId;
        public List<CountryViewModel> _countryViewModelList;
        public List<PatientAttendeeProfileViewModel> _patientAttendeeProfileViewModelList;

        public ProfessionalPatientAttendeeAddPage(ProfessionalPatientAddNew professionalPatientAddNew, long patientProfileId, List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList)
        {
            //DependencyService.Get<IAppPermissionChecker>().CheakPowerSaverPermission();
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;

            this._professionalPatientAddNew = professionalPatientAddNew;
            PatientProfileId = patientProfileId;
            _countryViewModelList = new List<CountryViewModel>();
            LoadCountryCode();
            LoadPatientAttendeeProfileType(patientAttendeeProfileTypeViewModelList);
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
            int selectedAttendeeProfileType = (int)Enums.AttendeeProfileTypeEnum.Others;
            var selectedItem = modelList.FirstOrDefault(x => x.Id == selectedAttendeeProfileType);
            PatientAttendeeProfileType.ItemsSource = modelList;
            PatientAttendeeProfileType.SelectedItem = selectedItem != null ? selectedItem : null;
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

        public async void AddAttendeeButton_Clicked(object sender, EventArgs e)
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

                    var onePatientAttendeeProfileType = (PatientAttendeeProfileTypeSelectionViewModel)PatientAttendeeProfileType.SelectedItem;

                    long attendeeProfileId = 100;
                    if (this._professionalPatientAddNew.PatientAttendeeProfileViewModelList != null && this._professionalPatientAddNew.PatientAttendeeProfileViewModelList.Count > 0)
                    {
                        attendeeProfileId = attendeeProfileId + 1;
                    }
                    else
                    {
                        this._professionalPatientAddNew.PatientAttendeeProfileViewModelList = new List<PatientAttendeeProfileViewModel>();
                    }

                    PatientAttendeeProfileViewModel patientAttendeeProfileViewModel = new PatientAttendeeProfileViewModel();
                    patientAttendeeProfileViewModel.AttendeeProfileId = attendeeProfileId;
                    patientAttendeeProfileViewModel.EmailAddress = emailAddress;
                    patientAttendeeProfileViewModel.PrimaryPhone = phoneNumber;
                    patientAttendeeProfileViewModel.PrimaryPhoneCode = countryCode;
                    patientAttendeeProfileViewModel.EmailAllowed = emailSendAllowed;
                    patientAttendeeProfileViewModel.SmsAllowed = smsSendAllowed;
                    patientAttendeeProfileViewModel.AttendeeProfileTypeId = onePatientAttendeeProfileType != null ? onePatientAttendeeProfileType.Id : (int)Enums.AttendeeProfileTypeEnum.Others;
                    patientAttendeeProfileViewModel.AttendeeProfileTypeName = onePatientAttendeeProfileType != null ? onePatientAttendeeProfileType.Name : string.Empty;
                    patientAttendeeProfileViewModel.PatientProfileId = this.PatientProfileId;

                    if (patientAttendeeProfileViewModel.AttendeeProfileTypeId == (int)Enums.AttendeeProfileTypeEnum.Patient)
                    {
                        if (this._professionalPatientAddNew.SurgicalConciergePatientViewModel != null)
                        {
                            this._professionalPatientAddNew.SurgicalConciergePatientViewModel.EmailAddress = patientAttendeeProfileViewModel.EmailAddress;
                            this._professionalPatientAddNew.SurgicalConciergePatientViewModel.PrimaryPhone = patientAttendeeProfileViewModel.PrimaryPhone;
                            this._professionalPatientAddNew.SurgicalConciergePatientViewModel.PrimaryPhoneCode = patientAttendeeProfileViewModel.PrimaryPhoneCode;

                            UtilHelper.ShowToastMessage("Attendee profile added successfully.");
                            this._professionalPatientAddNew.UpdateSurgicalConciergePatientAttendeeListView(this._professionalPatientAddNew.SurgicalConciergePatientViewModel);
                        }
                    }
                    else
                    {
                        this._professionalPatientAddNew.PatientAttendeeProfileViewModelList.Add(patientAttendeeProfileViewModel);

                        UtilHelper.ShowToastMessage("Attendee profile added successfully.");
                        this._professionalPatientAddNew.UpdateSurgicalConciergePatientAttendeeListView();
                    }

                    await Navigation.PopModalAsync();

                }
                catch (Exception ex)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }
        private void PatientAttendeeProfileType_OnFocused(object sender, FocusEventArgs e)
        {
            var onePatientAttendeeProfileType = e.IsFocused;
            //LabelSelectedTypeName.Text = "";
        }

    }
}