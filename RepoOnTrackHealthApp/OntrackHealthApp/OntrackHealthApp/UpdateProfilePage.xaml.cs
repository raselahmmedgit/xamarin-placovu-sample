using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdateProfilePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private AdministrationPatientProfileRestApiService adminRestApiService;
        private PatientProfile patientProfile;
        private SurgicalConciergePatientView _surgicalConciergePatientView { get; set; }
        public long PracticeDivisionDest = 0;
        public long PracticeDivisionUnitDest = 0;
        private long PatientProfileId;
        private long PracticeProfileId;
        public List<CountryViewModel> _countryViewModelList;

        public UpdateProfilePage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = "Update Profile";
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                adminRestApiService = new AdministrationPatientProfileRestApiService();
                PatientProfileId = _iTokenContainer.ApiPatientProfileId.Value;

                LoadAndSetPatientProfileData();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadAndSetPatientProfileData()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    PatientProfile patient = await adminRestApiService.GetPatientProfile(PatientProfileId);
                    patientProfile = patient;
                    PracticeProfileId = (long)_iTokenContainer.ApiPracticeProfileId;
                    string practiceName = _iTokenContainer.ApiPracticeName;
                    PatientProfileIdTextBox.Text = Convert.ToString(PatientProfileId);

                    int practiceProfilePickerSelectedIndex = AppConstants.PickerDefaultIndex;
                    if (patientProfile.PracticeProfileDropDownViewModel != null && patientProfile.PracticeProfileDropDownViewModel.SelectOptions != null)
                    {
                        PracticeProfilePicker.ItemsSource = patientProfile.PracticeProfileDropDownViewModel.SelectOptions.ToList();
                        practiceProfilePickerSelectedIndex = patientProfile.PracticeProfileDropDownViewModel.SelectOptions.ToList().FindIndex(x => x.Value == patientProfile.PracticeProfileDropDownViewModel.PracticeProfileId.ToString()).ToInt();
                        PracticeProfilePicker.SelectedIndex = practiceProfilePickerSelectedIndex;
                    }
                    else
                    {
                        PracticeProfilePicker.ItemsSource = new List<SelectListItem>() { new SelectListItem { Selected = true, Text = practiceName, Value = PracticeProfileId.ToString() } };
                        PracticeProfilePicker.SelectedIndex = 0;
                    }
                    PracticeProfilePicker.IsEnabled = false;

                    var country = await TempDataContainer.GetCountryViewModelFromJsonAsync();
                    foreach (var countryViewModel in country)
                    {
                        CountryCodePicker.Items.Add(countryViewModel);
                    }
                    CountryCodePicker.SelectedIndex = CountryCodePicker.Items.IndexOf("+1");

                    PatientProfileIdTextBox.Text = patientProfile.PatientProfileId.ToString();
                    FirstNameTextBox.Text = patientProfile.FirstName;
                    LastNameTextBox.Text = patientProfile.LastName;

                    if ((!string.IsNullOrEmpty(patientProfile.EmailAddress) || patientProfile.EmailAddress != ""))
                    {
                        EmailTextBox.Text = patientProfile.EmailAddress;
                    }
                    EmailTextBox.IsEnabled = false;

                    if ((!string.IsNullOrEmpty(patientProfile.PrimaryPhone) || patientProfile.PrimaryPhone != ""))
                    {
                        PhoneNumberTextBox.Text = patientProfile.PrimaryPhone;
                    }

                    DateOfBirthDatePicker.Date = Convert.ToDateTime(patientProfile.DateOfBirth);

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
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
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

        private void PhoneNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidatePhoneNumberTextBox();
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

        private bool ValidatePhoneNumberTextBox()
        {
            ValidationResult validationResult = ValidationHelper.ValidatePhoneNumberMti(PhoneNumberTextBox.Text?.Trim());
            if (validationResult.success == false)
            {
                ErrorPhoneNumberTextBox.Text = validationResult.message;

                ErrorPhoneNumberTextBox.IsVisible = true;
            }
            else
            {
                ErrorPhoneNumberTextBox.IsVisible = false;
            }

            return validationResult.success;
        }

        private bool ValidatePracticeProfilePicker()
        {
            bool isValid = true;
            if (PracticeProfilePicker.SelectedIndex == AppConstants.PickerDefaultIndex)
            {
                isValid = false;
                ErrorPracticeProfilePicker.Text = "Select a Practice";
                ErrorPracticeProfilePicker.IsVisible = true;
            }
            else
            {
                ErrorPracticeProfilePicker.IsVisible = false;
            }
            return isValid;
        }

        private bool ValidateEmailAddress()
        {
            ValidationResult validationResult = ValidationHelper.ValidateEmailAddress(EmailTextBox.Text?.Trim());

            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                ErrorEmailTextBox.Text = validationResult.message;

                ErrorEmailTextBox.IsVisible = true;
            }
            else
            {
                ErrorEmailTextBox.IsVisible = false;
            }

            return validationResult.success;
        }

        private bool FormValidationSuccess()
        {
            bool isValid = true;
            ErrorFirstNameTextBox.IsVisible = false;
            ErrorLastNameTextBox.IsVisible = false;
            ErrorPracticeProfilePicker.IsVisible = false;
            ErrorEmailTextBox.IsVisible = false;

            if (!ValidateFirstNameTextBox() || !ValidateLastNameTextBox() || !ValidatePracticeProfilePicker() || !ValidateEmailAddress() || !ValidatePhoneNumberTextBox())
            {
                isValid = false;
            }
            return isValid;

        }

        private async void PatientUpdateButton_Clicked(object sender, EventArgs e)
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

                    PatientProfile model = new PatientProfile();
                    //SurgicalConciergePatientViewModel model = new SurgicalConciergePatientViewModel();
                    model.PatientProfileId = Convert.ToInt32(PatientProfileIdTextBox.Text?.Trim());
                    model.FirstName = FirstNameTextBox.Text?.Trim();
                    model.LastName = LastNameTextBox.Text?.Trim();
                    //model.PracticeProfileId = PracticeProfileId;
                    model.EmailAddress = EmailTextBox.Text?.Trim();
                    model.PrimaryPhone = PhoneNumberTextBox.Text?.Trim();
                    var countryCode = CountryCodePicker.SelectedItem.ToString();
                    model.PrimaryPhoneCode = countryCode;


                    var selectedPractice = PracticeProfilePicker.SelectedItem as SelectListItem;
                    if (selectedPractice != null)
                    {
                        model.PracticeProfileId = long.Parse(selectedPractice.Value);
                        model.PracticeProfileName = selectedPractice.Text;
                    }
                    var dob = Convert.ToDateTime(Convert.ToDateTime(DateOfBirthDatePicker.Date).ToString("dd/MMM/yyyy hh:mm tt"));

                    model.DateOfBirth = dob;

                    ApiExecutionResult<PatientProfile> result = await adminRestApiService.PostPatientProfile(model);
                    if (result.Success)
                    {
                        UtilHelper.ShowToastMessage(result.Message);
                        await Navigation.PopAsync();
                        //await Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, result.Message, AppConstant.DisplayAlertErrorButtonText);
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }

        }

        private async void PatientCancelButton_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    //App.Instance.ClearNavigationAndGoToPage(new MainPage());
                    //await Navigation.PopAsync();
                    //App.Current.MainPage = new CustomNavigation.CustomNavigationPage(new MainPatientPage());
                    UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #region Top Menu Actions

        private async void OnHomeButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    //App.Instance.ClearNavigationAndGoToPage(new MainPage());
                    await Navigation.PushAsync(new MainPage());
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnScheduleButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    NotificationListPageViewModel model = new NotificationListPageViewModel();
                    await model.ExecuteLoadCommandAsync();
                    await Navigation.PushAsync(new NotificationListPage(model));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnPhysicianButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await Navigation.PushAsync(new PhysicianProfilePage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnLocationButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                LocationPageNew page = new LocationPageNew();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnResourceButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    ResourcePage page = new ResourcePage();
                    await page.LoadDataAsync();
                    await Navigation.PushAsync(page);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnOtherInfoButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new HospitalInfoPage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new ChangePasswordPage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnSignOutButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {

                if (_iTokenContainer != null)
                {
                    _iTokenContainer.ClearApiToken();
                }
                DependencyService.Get<IToast>().SetSettingsForUserLogout();
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnUpdatePatientProfileClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new UpdateProfilePage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #endregion

    }
}