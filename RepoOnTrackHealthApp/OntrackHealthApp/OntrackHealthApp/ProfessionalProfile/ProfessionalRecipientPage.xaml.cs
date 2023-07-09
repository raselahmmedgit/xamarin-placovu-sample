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

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalRecipientPage : ContentPage
	{
        private SurgicalConciergeViewModel _surgicalConciergeViewModel { get; set; }
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
        private SurgicalConciergeRecipientPageViewModel _surgicalConciergeRecipientPageViewModel;
        public long PracticeDivisionUnitDest = 0;
        ProfessionalPatientPage _professionalPatientPage;

        public ProfessionalRecipientPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivisionUnit)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;

            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;

            //PatientFullName.Text = _surgicalConciergePatientViewModel.PatientFullName;
            //ProcedureName.Text = _surgicalConciergePatientViewModel.ProcedureName;
            //ProfessionalName.Text = _surgicalConciergePatientViewModel.ProfessionalName;

            BindingContext = _surgicalConciergeRecipientPageViewModel = new SurgicalConciergeRecipientPageViewModel();
            _surgicalConciergeRecipientPageViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;

            PracticeDivisionUnitDest = practiceDivisionUnit;

        }

        public ProfessionalRecipientPage(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel, long practiceDivisionUnit, ProfessionalPatientPage professionalPatientPage)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;

            _surgicalConciergePatientViewModel = surgicalConciergePatientViewModel;

            //PatientFullName.Text = _surgicalConciergePatientViewModel.PatientFullName;
            //ProcedureName.Text = _surgicalConciergePatientViewModel.ProcedureName;
            //ProfessionalName.Text = _surgicalConciergePatientViewModel.ProfessionalName;

            BindingContext = _surgicalConciergeRecipientPageViewModel = new SurgicalConciergeRecipientPageViewModel();
            _surgicalConciergeRecipientPageViewModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;

            PracticeDivisionUnitDest = practiceDivisionUnit;

            _professionalPatientPage = professionalPatientPage;
        }

        public async void LoadPatientContact()
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
                    //List<CountryViewModel> countryList = await restApiService.GetCountryList();
                    //await Navigation.PushModalAsync(new ProfessionalRecipientAddPage(this, _surgicalConciergePatientViewModel.PatientProfileId, countryList));

                    List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList = await restApiService.GetSurgicalConceirgePatientAttendeeProfileType();
                    await Navigation.PushModalAsync(new ProfessionalRecipientAddPage(this, _surgicalConciergePatientViewModel.PatientProfileId, patientAttendeeProfileTypeViewModelList));
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
                    UtilHelper.ShowToastMessage(AppConstant.NoPatientContactMessage);
                    return;
                }
                if (_surgicalConciergeRecipientPageViewModel.PatientAttendeeProfileViewModels.Count > 0)
                {
                    if (PracticeDivisionUnitId.GetPacuDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new ProfessionalPacuPage(_surgicalConciergePatientViewModel));
                        //await Navigation.PushAsync(new ProfessionalPacuNewPage(SurgicalConciergePatientViewModel));
                    }
                    else if (PracticeDivisionUnitId.GetScgNursingRoundDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new ProfessionalNursingRoundPage(_surgicalConciergePatientViewModel, PracticeDivisionUnitDest));
                    }
                    else if (PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new ProfessionalDischargeDetailPage(_surgicalConciergePatientViewModel));
                    }
                    else if (PracticeDivisionUnitId.GetPathologyDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new ProfessionalDischargeDetailPage(_surgicalConciergePatientViewModel));
                    }
                    else if (PracticeDivisionUnitId.GetFloorDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        //await Navigation.PushAsync(new ProfessionalFloorPage(_surgicalConciergePatientViewModel, PracticeDivisionUnitDest));
                        await Navigation.PushAsync(new ProfessionalNursingRoundPage(_surgicalConciergePatientViewModel, PracticeDivisionUnitDest));
                    }
                    else if (PracticeDivisionUnitId.GetOperatingRoomDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        _surgicalConciergeViewModel = await restApiService.GetSurgicalConciergeStageList(_surgicalConciergePatientViewModel.PatientProcedureDetailId.ToString());
                        var surgicalConciergeDetail = new ProfessionalOperatingPage(_surgicalConciergeViewModel, _surgicalConciergePatientViewModel);
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
                                await Navigation.PushAsync(new ProfessionalPreSurgerySummaryPage(_surgicalConciergePatientViewModel, patientPreSurgerySummaryViewModel));
                            }
                            else
                            {
                                await Navigation.PushAsync(new ProfessionalPreSurgerySummaryAddPage(_surgicalConciergePatientViewModel, patientPreSurgerySummaryViewModel));
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

                if (_professionalPatientPage == null)
                {
                    RecipientPageTopHeader.IsVisible = false;
                }
            }
        }

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
                //List<CountryViewModel> countryList = await restApiService.GetCountryList();
                //await Navigation.PushModalAsync(new ProfessionalRecipientEditPage(this, patientAttendeeProfileViewModel, countryList));
                List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList = await restApiService.GetSurgicalConceirgePatientAttendeeProfileType();
                await Navigation.PushModalAsync(new ProfessionalRecipientEditPage(this, patientAttendeeProfileViewModel, patientAttendeeProfileTypeViewModelList));
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
                    var surgicalConciergePatientEmailPage = new ProfessionalPatientEmailPage(surgicalConciergePatientViewModel, practiceDivisionId, PracticeDivisionUnitDest, null);
                    await Navigation.PushModalAsync(new ProfessionalPatientEmailEditPage(surgicalConciergePatientEmailPage, surgicalConciergePatientViewModel));
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
                    var surgicalConciergePatientEmailPage = new ProfessionalPatientEmailPage(_surgicalConciergePatientViewModel, practiceDivisionId, PracticeDivisionUnitDest, null);
                    if (_surgicalConciergePatientViewModel.IsPatientContactProvided)
                    {
                        await Navigation.PushModalAsync(new ProfessionalPatientEmailEditPage(surgicalConciergePatientEmailPage, _surgicalConciergePatientViewModel));
                    }
                    else
                    {
                        await Navigation.PushModalAsync(new ProfessionalPatientEmailAddPage(surgicalConciergePatientEmailPage, _surgicalConciergePatientViewModel.PatientProfileId));
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
                
                await Navigation.PushModalAsync(new PatientSearchView(_professionalPatientPage));
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
                    _professionalPatientPage.SelectedDate = DateTime.UtcNow;
                    _professionalPatientPage.ReLoadData();
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
                if (_professionalPatientPage != null)
                {
                    await Navigation.PushModalAsync(new PatientSearchFilterView(_professionalPatientPage));
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

                if (_professionalPatientPage != null)
                {
                    await Navigation.PushAsync(new ProfessionalPatientAddNew(_professionalPatientPage, _professionalPatientPage.PracticeDivisionDest, _professionalPatientPage.PracticeDivisionUnitDest, false));
                    //await Navigation.PushModalAsync(new ProfessionalPatientAddNew(_professionalPatientPage, _professionalPatientPage.PracticeDivisionDest, _professionalPatientPage.PracticeDivisionUnitDest, false));
                }
            }
        }
    }
}