using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
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
    public partial class SurgicalConciergePatientEmailPage : ContentPage
    {
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();

        private long PatientProfileId;
        private Guid? PatientProcedureDetailId;

        public long PracticeDivisionDest = 0;
        public long PracticeDivisionUnitDest = 0;

        private SurgicalConciergePatientView _surgicalConciergePatientView;
        private SurgicalConciergePatientPage _surgicalConciergePatientPage;
        public SurgicalConciergePatientEmailPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivision, long practiceDivisionUnit, SurgicalConciergePatientView surgicalConciergePatientView)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;

            PatientProfileId = surgicalConciergePatientViewModel.PatientProfileId;
            PatientProcedureDetailId = surgicalConciergePatientViewModel.PatientProcedureDetailId;

            _surgicalConciergePatientView = surgicalConciergePatientView;
            //RebindForm(_surgicalConciergePatientViewModel.EmailAddress, _surgicalConciergePatientViewModel.PrimaryPhoneCode, _surgicalConciergePatientViewModel.PrimaryPhone);
            LoadAndRebindFormAsync(surgicalConciergePatientViewModel);

        }

        public SurgicalConciergePatientEmailPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivision, long practiceDivisionUnit, SurgicalConciergePatientPage surgicalConciergePatientPage)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }

            PracticeDivisionDest = practiceDivision;
            PracticeDivisionUnitDest = practiceDivisionUnit;

            PatientProfileId = surgicalConciergePatientViewModel.PatientProfileId;
            PatientProcedureDetailId = surgicalConciergePatientViewModel.PatientProcedureDetailId;

            _surgicalConciergePatientPage = surgicalConciergePatientPage;
            //RebindForm(_surgicalConciergePatientViewModel.EmailAddress, _surgicalConciergePatientViewModel.PrimaryPhoneCode, _surgicalConciergePatientViewModel.PrimaryPhone);
            LoadAndRebindFormAsync(surgicalConciergePatientViewModel);

        }

        public async void LoadAndRebindFormAsync(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                _surgicalConciergePatientViewModel = await restApiService.EditSurgicalConciergePatientProfile(surgicalConciergePatientViewModel.PatientProfileId, surgicalConciergePatientViewModel.PatientProcedureDetailId, Convert.ToInt16(PracticeDivisionDest), Convert.ToInt16(PracticeDivisionUnitDest));
                _surgicalConciergePatientViewModel.PatientProcedureDetailId = PatientProcedureDetailId;

                //PatientFullName.Text = "Patient : " + _surgicalConciergePatientViewModel.PatientFullName;
                //ProcedureName.Text = "Procedure : " + surgicalConciergePatientViewModel.ProcedureName;
                //ProfessionalName.Text = "Professional : " + surgicalConciergePatientViewModel.ProfessionalName;

                _surgicalConciergePatientViewModel.ProcedureName = surgicalConciergePatientViewModel.ProcedureName;
                _surgicalConciergePatientViewModel.ProfessionalName = surgicalConciergePatientViewModel.ProfessionalName;

                //StackLayoutPatientEmail.IsVisible = false;
                //ShowPatientEmailAddModal.IsVisible = true;
                StackLayoutPatientEmailPhone.IsVisible = true;


                LabelPatientEmail.Text = "Email: " + _surgicalConciergePatientViewModel.EmailAddress;
                LabelPatientPhone.Text = "Phone: " + _surgicalConciergePatientViewModel.PrimaryPhoneCode + _surgicalConciergePatientViewModel.PrimaryPhone;

                LabelPatientEmail.IsChecked = !string.IsNullOrEmpty(_surgicalConciergePatientViewModel.EmailAddress);
                LabelPatientPhone.IsChecked = !string.IsNullOrEmpty(_surgicalConciergePatientViewModel.PrimaryPhone);

                if (!string.IsNullOrEmpty(_surgicalConciergePatientViewModel.EmailAddress) || !string.IsNullOrEmpty(_surgicalConciergePatientViewModel.PrimaryPhone))
                {
                    PatientContactNotExist.IsVisible = false;
                    PatientContactExist.IsVisible = true;
                    btnEditPatientContact.Clicked += (sender, args) =>
                    {
                        btnEditEmail_ClickedAsync(sender, args);
                    };

                }
                else
                {
                    PatientContactNotExist.IsVisible = true;
                    PatientContactExist.IsVisible = false;
                    LabelPatientEmail.Text = string.Empty;
                    LabelPatientPhone.Text = string.Empty;
                    btnEditPatientContact.Clicked += (sender, args) =>
                    {
                        ShowPatientEmailAddModal_ClickedAsync(sender, args);
                    };
                }

                //if (_surgicalConciergePatientViewModel.EmailAddress != null && _surgicalConciergePatientViewModel.EmailAddress != "")
                //{
                //    ShowPatientEmailAddModal.IsVisible = false;
                //    StackLayoutPatientEmail.IsVisible = true;
                //    StackLayoutPatientEmailPhone.IsVisible = true;
                //    LabelPatientEmail.Text = _surgicalConciergePatientViewModel.EmailAddress.Trim();
                //}
                //if (_surgicalConciergePatientViewModel.PrimaryPhone != null && _surgicalConciergePatientViewModel.PrimaryPhone != "")
                //{
                //    ShowPatientEmailAddModal.IsVisible = false;
                //    StackLayoutPatientEmailPhone.IsVisible = true;
                //    LabelPatientPhone.Text = _surgicalConciergePatientViewModel.PrimaryPhoneCode.Trim() + " " + _surgicalConciergePatientViewModel.PrimaryPhone.ToFormatedPhoneNumber();
                //}
            }
        }

        public void RebindForm(string email, string phoneCode, string phone)
        {

            using (UserDialogs.Instance.Loading(""))
            {
                //StackLayoutPatientEmail.IsVisible = false;
                //ShowPatientEmailAddModal.IsVisible = true;
                //StackLayoutPatientEmailPhone.IsVisible = false;

                //if (email != null && email != "")
                //{
                //    ShowPatientEmailAddModal.IsVisible = false;
                //    StackLayoutPatientEmail.IsVisible = true;
                //    StackLayoutPatientEmailPhone.IsVisible = true;
                //    LabelPatientEmail.Text = email.Trim();
                //}
                //if (phone != null && phone != "")
                //{
                //    ShowPatientEmailAddModal.IsVisible = false;
                //    StackLayoutPatientEmailPhone.IsVisible = true;
                //    LabelPatientPhone.Text = phoneCode.Trim() + " " + phone.ToFormatedPhoneNumber();
                //}

                LabelPatientEmail.Text = "Email: " + email;
                LabelPatientPhone.Text = "Phone: " + phoneCode + phone.ToFormatedPhoneNumber();

                LabelPatientEmail.IsChecked = !string.IsNullOrEmpty(email);
                LabelPatientPhone.IsChecked = !string.IsNullOrEmpty(phone);

                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone))
                {
                    PatientContactNotExist.IsVisible = false;
                    PatientContactExist.IsVisible = true;
                    btnEditPatientContact.Clicked += (sender, args) =>
                    {
                        btnEditEmail_ClickedAsync(sender, args);
                    };

                }
                else
                {
                    PatientContactNotExist.IsVisible = true;
                    PatientContactExist.IsVisible = false;
                    btnEditPatientContact.Clicked += (sender, args) =>
                    {
                        ShowPatientEmailAddModal_ClickedAsync(sender, args);
                    };
                }
            }

        }

        private async void btnContinueToProgram_ClickedAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LabelPatientEmail.Text))
            {
                UtilHelper.ShowToastMessage(AppConstant.PatientEmailAddressRequired);
                return;
            }
            await Navigation.PushAsync(new SurgicalConciergePathologyPage(_surgicalConciergePatientViewModel));
        }

        private async void ShowPatientEmailAddModal_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SurgicalConciergePatientEmailAddPage(this, _surgicalConciergePatientViewModel.PatientProfileId));
        }

        private async void btnEditEmail_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var surgicalConciergePatientViewModel = await restApiService.GetSurgicalConciergePatientProfile(_surgicalConciergePatientViewModel.PatientProfileId, Convert.ToInt16(PracticeDivisionDest), Convert.ToInt16(PracticeDivisionUnitDest));

                    await Navigation.PushModalAsync(new SurgicalConciergePatientEmailEditPage(this, surgicalConciergePatientViewModel));
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        //private async void GetAndBindPatientInformation()
        //{
        //    try
        //    {
        //        var patientProfile = await restApiService.GetPatientShortProfile(_surgicalConciergePatientViewModel.PatientProfileId);
        //        RebindForm(patientProfile.EmailAddress, patientProfile.PrimaryPhoneCode, patientProfile.PrimaryPhone);
        //    }
        //    catch (Exception)
        //    {
        //        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
        //    }
        //}

        protected override void OnAppearing()
        {
            AddTopThreeTodayLabel.Text = DateTime.UtcNow.Day.ToString();
            base.OnAppearing();
            //GetAndBindPatientInformation();
            if (_surgicalConciergePatientView == null)
            {
                RecipientPageTopHeader.IsVisible = false;
            }
        }

        private async void ShowPatientSearchModal(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }

                await Navigation.PushModalAsync(new PatientSearchView(_surgicalConciergePatientView));
            }
        }

        private async void PatientListTodayClicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }

                try
                {
                    _surgicalConciergePatientView.SelectedDate = DateTime.UtcNow;
                    _surgicalConciergePatientView.ReLoadData();
                    await Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }

        private async void ShowPatientSearchFilterModal(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }
                if (_surgicalConciergePatientView != null)
                {
                    await Navigation.PushModalAsync(new PatientSearchFilterView(_surgicalConciergePatientView));
                }
            }
        }

        private async void ShowAddPatientModal(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }

                if (_surgicalConciergePatientView != null)
                {
                    await Navigation.PushAsync(new SurgicalConceirgePatientAddNew(_surgicalConciergePatientView, _surgicalConciergePatientView.PracticeDivisionDest, _surgicalConciergePatientView.PracticeDivisionUnitDest, false));
                }
            }
        }
    }
}