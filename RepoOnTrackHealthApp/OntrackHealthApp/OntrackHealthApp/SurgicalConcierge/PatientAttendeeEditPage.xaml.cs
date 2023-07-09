using Acr.UserDialogs;
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

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientAttendeeEditPage : CustomModalContentPage
    {
        PatientAttendeeListPage patientAttendeeListPage;
        public PatientAttendeeEditPage(PatientAttendeeListPage patientAttendeeListPage, PatientAttendeeProfileViewModel patientAttendee, List<CountryViewModel> countryList)
        {
            this.patientAttendeeListPage = patientAttendeeListPage;
            InitializeComponent();
            LoadCountryCode(countryList);
            BindingContext = patientAttendee;

        }
        private void LoadCountryCode(List<CountryViewModel> countryList)
        {
            foreach (var countryCode in countryList)
            {
                countryCodePicker.Items.Add("+" + countryCode.PhoneCode);
            }

        }
        public async void UpdateAttendee(object sender, EventArgs e)
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
                    if (!ValidateEmailAddress() || !ValidatePhoneNumber())
                    {
                        return;
                    }
                    var emailAddress = EmailAddress.Text;
                    var phoneNumber = PhoneNumber.Text;
                    var emailSendAllowed = EmailAllow.IsChecked;
                    var SmsSendAllowed = SmsAllow.IsChecked;
                    var countryCode = countryCodePicker.SelectedItem.ToString();


                    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();

                    //UtilHelper.ShowLoader(loaderView);
                    PatientAttendeeProfileViewModel attendeeModal = (PatientAttendeeProfileViewModel)this.BindingContext;
                    ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = new ApiExecutionResult<PatientAttendeeProfileViewModel>();

                    //ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = await restService.AddOrEditPatientAttendee(attendeeModal.AttendeeProfileId, attendeeModal.PatientProfileId, emailAddress, phoneNumber, countryCode, emailSendAllowed, SmsSendAllowed);
                    //UtilHelper.HideLoader(loaderView);

                    await Navigation.PopModalAsync();
                    if (apiExecutionResult.Success)
                    {
                        patientAttendeeListPage.UpdateListView(apiExecutionResult.DataList);
                        UtilHelper.ShowToastMessage("Attendee profile updated successfully.");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage("Attendee profile update failed. Please try again.");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
                
            }
        }
        private void EmailAddress_TextChanged(object sender, EventArgs e)
        {
            ValidateEmailAddress();
        }
        private bool ValidateEmailAddress()
        {
            ValidationResult validationResult = ValidationHelper.ValidateEmailAddress(EmailAddress.Text?.Trim());
            ErrorEmailAddress.Text = validationResult.message;
            return validationResult.success;
        }

        private void PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            ValidatePhoneNumber();
        }
        private bool ValidatePhoneNumber()
        {
            ValidationResult validationResult = ValidationHelper.ValidatePhoneNumber(PhoneNumber.Text?.Trim());
            ErrorPhoneNumber.Text = validationResult.message;
            return validationResult.success;
        }
        private async void CloseModal(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}