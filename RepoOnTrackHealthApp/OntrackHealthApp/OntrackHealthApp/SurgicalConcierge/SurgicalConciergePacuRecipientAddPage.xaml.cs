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

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePacuRecipientAddPage : CustomModalContentPage
    {
        public SurgicalConciergePacuRecipientPage _surgicalConciergePacuRecipientPage;
        public long PatientProfileId;
        public List<CountryViewModel> CountryList;
        private readonly ITokenContainer _iTokenContainer;

        public SurgicalConciergePacuRecipientAddPage(SurgicalConciergePacuRecipientPage surgicalConciergePacuRecipientPage, long patientProfileId, List<CountryViewModel> countryList)
        {
            InitializeComponent();
            _surgicalConciergePacuRecipientPage = surgicalConciergePacuRecipientPage;
            PatientProfileId = patientProfileId;
            CountryList = new List<CountryViewModel>();
            LoadCountryCode(countryList);
            _iTokenContainer = new TokenContainer();
            //lblPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
            //Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                lblPracticeTitle.Text = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                lblPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
            }
        }
        private void LoadCountryCode(List<CountryViewModel> countryList)
        {
            CountryList = countryList.Where(c => !c.CountryIso.Equals("UM")).ToList();
            foreach (var countryCode in countryList)
            {
                countryCodePicker.Items.Add("+" + countryCode.PhoneCode);
            }
            countryCodePicker.SelectedIndex = countryCodePicker.Items.IndexOf("+1");
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
        public async void ModalClose(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        public async void AddAttendee(object sender, EventArgs e)
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

                    //UtilHelper.ShowLoader(loaderView);
                    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                    ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = new ApiExecutionResult<PatientAttendeeProfileViewModel>();

                    //ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = await restService.AddOrEditPatientAttendee(0, this.PatientProfileId, emailAddress, phoneNumber, countryCode, emailSendAllowed, SmsSendAllowed);
                    //UtilHelper.HideLoader(loaderView);

                    await Navigation.PopModalAsync();
                    if (apiExecutionResult.Success)
                    {
                        UtilHelper.ShowToastMessage("Attendee profile added successfully.");
                        _surgicalConciergePacuRecipientPage.UpdateListView();
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage("Attendee profile add failed. Please try again.");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }
    }
}