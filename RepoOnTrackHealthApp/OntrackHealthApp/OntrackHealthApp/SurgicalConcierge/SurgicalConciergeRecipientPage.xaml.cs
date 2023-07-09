using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergeRecipientPage : ContentPage
    {
        private SurgicalConciergeViewModel _surgicalConciergeViewModel { get; set; }
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
        private SurgicalConciergeRecipientPageViewModel _surgicalConciergeRecipientPageViewModel;
        private SurgicalConciergePatientView _surgicalConciergePatientView;
        public long PracticeDivisionUnitDest = 0;

        private SurgicalConciergePatientPage _surgicalConciergePatientPage;

        public SurgicalConciergeRecipientPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivisionUnit)
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

            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;

            //PatientFullName.Text = "Patient : " + _surgicalConciergePatientViewModel.PatientFullName;
            //ProcedureName.Text = "Procedure : " + _surgicalConciergePatientViewModel.ProcedureName;
            //ProfessionalName.Text = "Professional : " + _surgicalConciergePatientViewModel.ProfessionalName;

            BindingContext = _surgicalConciergeRecipientPageViewModel = new SurgicalConciergeRecipientPageViewModel();
            _surgicalConciergeRecipientPageViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;

            PracticeDivisionUnitDest = practiceDivisionUnit;
        }

        public SurgicalConciergeRecipientPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivisionUnit, SurgicalConciergePatientPage surgicalConciergePatientPage)
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
            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;

            //PatientFullName.Text = "Patient : " + _surgicalConciergePatientViewModel.PatientFullName;
            //ProcedureName.Text = "Procedure : " + _surgicalConciergePatientViewModel.ProcedureName;
            //ProfessionalName.Text = "Professional : " + _surgicalConciergePatientViewModel.ProfessionalName;

            BindingContext = _surgicalConciergeRecipientPageViewModel = new SurgicalConciergeRecipientPageViewModel();
            _surgicalConciergeRecipientPageViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
            _surgicalConciergePatientPage = surgicalConciergePatientPage;

            PracticeDivisionUnitDest = practiceDivisionUnit;
        }

        public SurgicalConciergeRecipientPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivisionUnit, SurgicalConciergePatientView surgicalConciergePatientView)
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

            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;

            //PatientFullName.Text = "Patient : " + _surgicalConciergePatientViewModel.PatientFullName;
            //ProcedureName.Text = "Procedure : " + _surgicalConciergePatientViewModel.ProcedureName;
            //ProfessionalName.Text = "Professional : " + _surgicalConciergePatientViewModel.ProfessionalName;

            BindingContext = _surgicalConciergeRecipientPageViewModel = new SurgicalConciergeRecipientPageViewModel();
            _surgicalConciergeRecipientPageViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
            _surgicalConciergePatientView = surgicalConciergePatientView;

            PracticeDivisionUnitDest = practiceDivisionUnit;
        }


        private async void LoadPatientContact()
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
                    //PatientContactAddStackLayout.IsVisible = false;
                    //RecipientAddStackLayout.IsVisible = true;
                    if (PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(PracticeDivisionUnitDest) || PracticeDivisionUnitId.GetPathologyDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        var practiceDivisionId = PracticeDivision.GetPracticeDivisionIdByPracticeDivisionUnit(PracticeDivisionUnitDest);
                        var surgicalConciergePatientViewModel = await restApiService.EditSurgicalConciergePatientProfile(_surgicalConciergePatientViewModel.PatientProfileId, _surgicalConciergePatientViewModel.PatientProcedureDetailId, Convert.ToInt16(practiceDivisionId), Convert.ToInt16(PracticeDivisionUnitDest));

                        PatientContactStackLayout.IsVisible = surgicalConciergePatientViewModel.IsPatientContactProvided;

                        _surgicalConciergePatientViewModel.EmailAddress = surgicalConciergePatientViewModel.EmailAddress;
                        _surgicalConciergePatientViewModel.PrimaryPhoneCode = surgicalConciergePatientViewModel.PrimaryPhoneCode;
                        _surgicalConciergePatientViewModel.PrimaryPhone = surgicalConciergePatientViewModel.PrimaryPhone;

                        PatientEmailAddress.Text = "Email: " + _surgicalConciergePatientViewModel.EmailAddress;

                        PatientMobilePhoneWithCountryCode.Text = "Phone: " + _surgicalConciergePatientViewModel.PrimaryPhoneCode + _surgicalConciergePatientViewModel.PrimaryPhone;
                        PatientMobilePhoneWithCountryCode.IsChecked = _surgicalConciergePatientViewModel.EnablePathologySmsNotification;

                        if (!surgicalConciergePatientViewModel.IsPatientContactProvided)
                        {
                            PatientContactStackLayout.IsVisible = true;
                            PatientContactNotExist.IsVisible = true;
                            PatientContactExist.IsVisible = false;
                            btnEditPatientContact.Clicked += (sender, args) =>
                            {
                                AddPatientContactButton_ClickedAsync(sender, args);
                            };

                        }
                        else
                        {
                            PatientContactExist.IsVisible = true;
                            PatientContactNotExist.IsVisible = false;
                            btnEditPatientContact.Clicked += (sender, args) =>
                            {
                                ShowPatientContactEditDialogAsync(sender, args);
                            };
                        }
                    }
                    else
                    {
                        PatientContactStackLayout.IsVisible = false;
                        //PatientContactAddStackLayout.IsVisible = false;
                        //RecipientAddStackLayout.IsVisible = true;
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }

        private async void ShowDeleteDialogAsync(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            Button btn = (Button)sender;
            var id = btn.ClassId.ToLong();
            var answer = await DisplayAlert("Delete Confirm", "Are you sure you want to delete this recipient?", "Yes", "No");
            if (answer)
            {
                // Call delete Function
                using (UserDialogs.Instance.Loading(""))
                {
                    var patientAttendeeProfileViewModel = _surgicalConciergeRecipientPageViewModel.PatientAttendeeProfileViewModels.Where(x => x.AttendeeProfileId == id).FirstOrDefault();
                    var deleteresult = await restApiService.DeleteAttendee(patientAttendeeProfileViewModel);
                    if (deleteresult.Success)
                    {
                        UtilHelper.ShowToastMessage(AppConstant.DeleteSuccessMessage);
                    }
                    UpdateListView();
                }
            }
        }

        public void UpdateListView()
        {
            _surgicalConciergeRecipientPageViewModel.LoadPatientAttendeeProfilesCommand.Execute(null);
        }

        private async void AddRecipientButton_ClickedAsync(object sender, EventArgs e)
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
                    List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList = await restApiService.GetSurgicalConceirgePatientAttendeeProfileType();
                    await Navigation.PushModalAsync(new SurgicalConciergeRecipientAddPage(this, _surgicalConciergePatientViewModel.PatientProfileId, patientAttendeeProfileTypeViewModelList));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async void ContinueToProgramButton_ClickedAsync(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                if (_surgicalConciergeRecipientPageViewModel.PatientAttendeeProfileViewModels.Count == 0 && ((PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(PracticeDivisionUnitDest) || PracticeDivisionUnitId.GetPathologyDivisionUnitId().Contains(PracticeDivisionUnitDest)) && !_surgicalConciergePatientViewModel.IsPatientContactProvided))
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoPatientContactOrRecipientMessage);
                    return;
                }
                if (_surgicalConciergeRecipientPageViewModel.PatientAttendeeProfileViewModels.Count > 0)
                {
                    if (PracticeDivisionUnitId.GetPacuDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergePacuPage(_surgicalConciergePatientViewModel));
                        //await Navigation.PushAsync(new SurgicalConciergePacuNewPage(SurgicalConciergePatientViewModel));
                    }
                    else if (PracticeDivisionUnitId.GetScgNursingRoundDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeNursingRoundPage(_surgicalConciergePatientViewModel, PracticeDivisionUnitDest));
                    }
                    else if (PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeDischargeDetailPage(_surgicalConciergePatientViewModel));
                    }
                    else if (PracticeDivisionUnitId.GetPathologyDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeDischargeDetailPage(_surgicalConciergePatientViewModel));
                    }
                    else if (PracticeDivisionUnitId.GetFloorDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeFloorPage(_surgicalConciergePatientViewModel));
                    }
                    else if (PracticeDivisionUnitId.GetOperatingRoomDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        _surgicalConciergeViewModel = await restApiService.GetSurgicalConciergeStageList(_surgicalConciergePatientViewModel.PatientProcedureDetailId.ToString());
                        var surgicalConciergeDetail = new SurgicalConciergeDetail(_surgicalConciergeViewModel, _surgicalConciergePatientViewModel);
                        surgicalConciergeDetail.LoadData();
                        await Navigation.PushAsync(surgicalConciergeDetail);
                    }
                    else if (PracticeDivisionUnitId.GetPreSurgerySummaryDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel = await restApiService.GetPreSurgerySummaryInitialData(_surgicalConciergePatientViewModel.PatientProcedureDetailId.ToString(), null, PracticeDivisionUnitDest);
                        if (patientPreSurgerySummaryViewModel != null)
                        {
                            if (patientPreSurgerySummaryViewModel.IsAlreadyInsertedSummary)
                            {
                                // Redirect to Summary Page
                                await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryPage(_surgicalConciergePatientViewModel, patientPreSurgerySummaryViewModel));
                            }
                            else
                            {
                                await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryAddPage(_surgicalConciergePatientViewModel, patientPreSurgerySummaryViewModel));
                            }
                        }
                        else
                        {
                            await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullError, AppConstant.DisplayAlertErrorButtonText);
                        }


                    }
                }
                else
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoRecipientMessage);
                }

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (UserDialogs.Instance.Loading(""))
            {
                AddTopThreeTodayLabel.Text = DateTime.UtcNow.Day.ToString();
                LoadPatientContact();
                _surgicalConciergeRecipientPageViewModel.LoadPatientAttendeeProfilesCommand.Execute(null);
            }
            if (_surgicalConciergePatientView == null)
            {
                RecipientPageTopHeader.IsVisible = false;
            }
        }

        #region Top Menu Actions

        private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new AdminChangePassword());
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

        #endregion



        private async void ShowEditDialogAsync(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            Button btn = (Button)sender;
            var id = btn.ClassId.ToLong();
            // Call edit Function
            using (UserDialogs.Instance.Loading(""))
            {
                var patientAttendeeProfileViewModel = _surgicalConciergeRecipientPageViewModel.PatientAttendeeProfileViewModels.Where(x => x.AttendeeProfileId == id).FirstOrDefault();
                List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModels = await restApiService.GetSurgicalConceirgePatientAttendeeProfileType();
                await Navigation.PushModalAsync(new SurgicalConciergeRecipientEditPage(this, patientAttendeeProfileViewModel, patientAttendeeProfileTypeViewModels));
                UpdateListView();
            }
        }

        private async void ShowPatientContactEditDialogAsync(object sender, EventArgs e)
        {
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var practiceDivisionId = PracticeDivision.GetPracticeDivisionIdByPracticeDivisionUnit(PracticeDivisionUnitDest);
                    var surgicalConciergePatientViewModel = await restApiService.GetSurgicalConciergePatientProfile(_surgicalConciergePatientViewModel.PatientProfileId, Convert.ToInt16(practiceDivisionId), Convert.ToInt16(PracticeDivisionUnitDest));
                    SurgicalConciergePatientView surgicalConciergePatientViewPage = null;
                    var surgicalConciergePatientEmailPage = new SurgicalConciergePatientEmailPage(surgicalConciergePatientViewModel, practiceDivisionId, PracticeDivisionUnitDest, surgicalConciergePatientViewPage);
                    await Navigation.PushModalAsync(new SurgicalConciergePatientEmailEditPage(surgicalConciergePatientEmailPage, surgicalConciergePatientViewModel));
                    //LoadPatientContact();
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        private async void AddPatientContactButton_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var practiceDivisionId = PracticeDivision.GetPracticeDivisionIdByPracticeDivisionUnit(PracticeDivisionUnitDest);
                    SurgicalConciergePatientView surgicalConciergePatientViewPage = null;
                    var surgicalConciergePatientEmailPage = new SurgicalConciergePatientEmailPage(_surgicalConciergePatientViewModel, practiceDivisionId, PracticeDivisionUnitDest, surgicalConciergePatientViewPage);
                    if (_surgicalConciergePatientViewModel.IsPatientContactProvided)
                    {
                        await Navigation.PushModalAsync(new SurgicalConciergePatientEmailEditPage(surgicalConciergePatientEmailPage, _surgicalConciergePatientViewModel));
                    }
                    else
                    {
                        await Navigation.PushModalAsync(new SurgicalConciergePatientEmailAddPage(surgicalConciergePatientEmailPage, _surgicalConciergePatientViewModel.PatientProfileId));
                    }
                }

            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
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