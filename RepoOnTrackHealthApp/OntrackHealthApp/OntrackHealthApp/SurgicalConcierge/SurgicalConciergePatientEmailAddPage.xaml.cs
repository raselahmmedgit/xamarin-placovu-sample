using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using Plugin.InputKit.Shared.Controls;
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
    public partial class SurgicalConciergePatientEmailAddPage : CustomModalContentPage
    {
        public long PatientProfileId;
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService;
        private List<CountryViewModel> CountryList;
        private SurgicalConciergePatientEmailPage _surgicalConciergePatientEmailPage;

        public SurgicalConciergePatientEmailAddPage(SurgicalConciergePatientEmailPage surgicalConciergePatientEmailPage, long patientProfileId)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            LabelPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
            PatientProfileId = patientProfileId;
            restApiService = new SurgicalConciergeRestApiService();
            _surgicalConciergePatientEmailPage = surgicalConciergePatientEmailPage;
        }

        private async void ModalCloseButton_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddEmailButton_ClickedAsync(object sender, EventArgs e)
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
                    var result = ValidateEmailAddress();
                    if (SMSSendAllowed.IsChecked)
                    {
                        result = ValidatePhoneNumber();
                    }

                    ErrorMessage.Text = string.Empty;
                    ErrorMessageStackLayout.IsVisible = false;

                    if (result)
                    {
                        SurgicalConceirgePatientEmailAddModel scgPatientEmailAddModel = new SurgicalConceirgePatientEmailAddModel();
                        scgPatientEmailAddModel.PatientProfileId = PatientProfileId;
                        scgPatientEmailAddModel.EmailAddress = EmailAddress.Text;
                        if (PhoneNumber.Text != "")
                        {
                            scgPatientEmailAddModel.PrimaryPhoneCode = CountryCodePicker.SelectedItem.ToString();
                            scgPatientEmailAddModel.PrimaryPhone = PhoneNumber.Text;
                        }
                        scgPatientEmailAddModel.EnablePathologySmsNotification = SMSSendAllowed.IsChecked;

                        ApiExecutionResult<SurgicalConceirgePatientEmailAddModel> apiExecutionResult = await restApiService.AddOrEditPatientEmail(scgPatientEmailAddModel);

                        if (apiExecutionResult.Success)
                        {
                            this._surgicalConciergePatientEmailPage.RebindForm(EmailAddress.Text?.Trim(), CountryCodePicker.SelectedItem.ToString(), PhoneNumber.Text?.Trim());
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            //await DisplayAlert(AppConstant.DisplayAlertErrorTitle, apiExecutionResult.Message, AppConstant.DisplayAlertErrorButtonText);

                            ErrorMessage.Text = apiExecutionResult.Message;
                            ErrorMessageStackLayout.IsVisible = true;
                        }

                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        private void EmailAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateEmailAddress();
        }
        private bool ValidateEmailAddress()
        {
            ErrorMessage.Text = string.Empty;
            ErrorMessageStackLayout.IsVisible = false;

            ValidationResult validationResult = ValidationHelper.ValidateEmailAddress(EmailAddress.Text?.Trim());
            ErrorEmailAddress.Text = validationResult.message;
            if (validationResult.success == false)
                ErrorEmailAddress.IsVisible = true;
            return validationResult.success;
        }
        private void PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidatePhoneNumber();
        }
        private bool ValidatePhoneNumber()
        {
            ErrorMessage.Text = string.Empty;
            ErrorMessageStackLayout.IsVisible = false;

            ValidationResult validationResult = ValidationHelper.ValidatePhoneNumberMti(PhoneNumber.Text?.Trim());
            ErrorPhoneNumber.Text = validationResult.message;
            if (validationResult.success == false)
                ErrorPhoneNumber.IsVisible = true;
            return validationResult.success;
        }

        private void LoadCountryCode()
        {
            CountryList = CountryList.Where(c => !c.CountryIso.Equals("UM")).ToList();
            foreach (var countryCode in CountryList)
            {
                CountryCodePicker.Items.Add("+" + countryCode.PhoneCode);
            }
            CountryCodePicker.SelectedIndex = CountryCodePicker.Items.IndexOf("+1");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            using (UserDialogs.Instance.Loading(""))
            {
                CountryList = await restApiService.GetCountryList();
                LoadCountryCode();
            }
        }

        private void SMSSendAllowed_CheckChanged(object sender, EventArgs e)
        {
            CheckBox o = sender as CheckBox;
            if (o.IsChecked)
                ValidatePhoneNumber();
            else
            {
                ErrorPhoneNumber.Text = "";
            }
        }
    }
}