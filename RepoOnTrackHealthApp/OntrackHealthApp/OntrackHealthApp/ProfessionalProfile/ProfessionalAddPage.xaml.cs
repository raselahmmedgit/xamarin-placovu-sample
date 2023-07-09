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
    public partial class ProfessionalAddPage : CustomModalContentPage
    {
        public ProfessionalAddPage()
        {
            InitializeComponent();
        }

        public long PracticeDivisionDest = 0;
        public long PracticeDivisionUnitDest = 0;
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService;
        ProfessionalPatientAdd _professionalPatientAddPage;
        ProfessionalPatientAddNew _professionalPatientAddNewPage;

        public ProfessionalAddPage(ProfessionalPatientAdd professionalPatientAddPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;

            restApiService = new SurgicalConciergeRestApiService();
            this._professionalPatientAddPage = professionalPatientAddPage;
        }

        public ProfessionalAddPage(ProfessionalPatientAddNew professionalPatientAddNewPage, long practiceDivision, long practiceDivisionUnit)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            LabelPracticeTitle.Text = Title = _iTokenContainer.ApiPracticeName;
            restApiService = new SurgicalConciergeRestApiService();
            this._professionalPatientAddNewPage = professionalPatientAddNewPage;
            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;
        }

        #region Method

        private async void SaveButton_Clicked(object sender, EventArgs e)
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

                    ProfessionalProfileViewModel model = new ProfessionalProfileViewModel();
                    model.PracticeProfileId = (long)_iTokenContainer.ApiPracticeProfileId;
                    model.DoctorFirstName = FirstNameTextBox.Text?.Trim();
                    model.DoctorLastName = LastNameTextBox.Text?.Trim();
                    model.DoctorEmail = EmailTextBox.Text?.Trim();

                    ApiExecutionResult<ProfessionalProfileDropDownViewModel> result = await restApiService.CreateSurgicalProfessional(model);
                    if (result.Success)
                    {
                        var professionalList = result.DataList.Select(c => c.SelectOptions).FirstOrDefault();

                        if (PracticeDivisionDest == (int)PracticeDivisionId.PatientRegistration) //PatientRegistration
                        {
                            _professionalPatientAddNewPage.UpdateProfessionalProfileDropdown(professionalList);
                        }
                        else
                        {
                            _professionalPatientAddPage.UpdateProfessionalProfileDropdown(professionalList);
                        }

                        CloseModal();
                    }
                    else
                    {
                        CloseModal();
                        await DisplayAlert("Message", "Operation Failed!", "OK");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }

        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                CloseModal();
            }
        }

        private void CloseModal()
        {
            Navigation.PopModalAsync();
        }

        #endregion

        #region Form Validation Check

        private bool FormValidationSuccess()
        {
            bool isValid = true;
            ErrorFirstNameTextBox.IsVisible = false;
            ErrorLastNameTextBox.IsVisible = false;

            if (!ValidateFirstNameTextBox() || !ValidateLastNameTextBox() || !ValidateEmailAddress())
            {
                isValid = false;
            }
            return isValid;

        }

        private void FirstNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateFirstNameTextBox();
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
        private void LastNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateLastNameTextBox();
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
        private void EmailAddress_TextChanged(object sender, EventArgs e)
        {
            ValidateEmailAddress();
        }

        private bool ValidateEmailAddress()
        {
            ValidationResult validationResult = ValidationHelper.ValidateEmailAddress(EmailTextBox.Text?.Trim());
            ErrorEmailTextBox.Text = validationResult.message;
            ErrorEmailTextBox.IsVisible = !validationResult.success;
            return validationResult.success;
        }

        #endregion
    }
}