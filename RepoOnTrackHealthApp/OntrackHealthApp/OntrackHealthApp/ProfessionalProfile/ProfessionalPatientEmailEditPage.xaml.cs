using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
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

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalPatientEmailEditPage : CustomModalContentPage
    {
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService;
        private ProfessionalPatientEmailPage _professionalPatientEmailPage;
        private ProfessionalRecipientPage _professionalRecipientPage;
        private long _practiceDivisionUnitId = 0;

        public ProfessionalPatientEmailEditPage(ProfessionalPatientEmailPage professionalPatientEmailPage, SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            LabelPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;
            restApiService = new SurgicalConciergeRestApiService();
            _professionalPatientEmailPage = professionalPatientEmailPage;
        }

        public ProfessionalPatientEmailEditPage(ProfessionalRecipientPage professionalRecipientPage, SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivisionUnitId)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            LabelPracticeTitle.Text = _iTokenContainer.ApiPracticeName;
            restApiService = new SurgicalConciergeRestApiService();
            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;
            _professionalRecipientPage = professionalRecipientPage;
            _practiceDivisionUnitId = practiceDivisionUnitId;
            SMSSendAllowed.IsChecked = _surgicalConciergePatientViewModel.EnablePathologySmsNotification;
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
                    scgPatientEmailAddModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                    scgPatientEmailAddModel.EmailAddress = EmailAddress.Text?.Trim();
                    if (PhoneNumber.Text?.Trim() != "")
                    {
                        scgPatientEmailAddModel.PrimaryPhoneCode = CountryCodePicker.SelectedItem.ToString();
                        scgPatientEmailAddModel.PrimaryPhone = PhoneNumber.Text?.Trim();
                    }
                    scgPatientEmailAddModel.EnablePathologySmsNotification = SMSSendAllowed.IsChecked;

                    ApiExecutionResult<SurgicalConceirgePatientEmailAddModel> apiExecutionResult = await restApiService.AddOrEditPatientEmail(scgPatientEmailAddModel);

                    if (apiExecutionResult.Success)
                    {
                        if (PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(_practiceDivisionUnitId))
                        {
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            _professionalPatientEmailPage.RebindForm(EmailAddress.Text?.Trim(), CountryCodePicker.SelectedItem.ToString(), PhoneNumber.Text?.Trim());
                            await Navigation.PopModalAsync();
                        }

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
                App.HideUserDialogAsync();
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
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

        private async void LoadCountryCode()
        {
            var country = await TempDataContainer.GetCountryViewModelFromJsonAsync();
            foreach (var countryViewModel in country)
            {
                CountryCodePicker.Items.Add(countryViewModel);
            }
            CountryCodePicker.SelectedIndex = CountryCodePicker.Items.IndexOf("+1");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (UserDialogs.Instance.Loading(""))
            {
                LoadCountryCode();

                if (!string.IsNullOrEmpty(_surgicalConciergePatientViewModel.EmailAddress))
                {
                    EmailAddress.Text = _surgicalConciergePatientViewModel.EmailAddress;
                }
                if (!string.IsNullOrEmpty(_surgicalConciergePatientViewModel.PrimaryPhone))
                {
                    PhoneNumber.Text = _surgicalConciergePatientViewModel.PrimaryPhone.ToFormatedPhoneNumber();
                }
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