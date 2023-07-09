using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using OntrackHealthApp.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SurgicalConciergePatientPage : ContentPage
	{
        public long PracticeDivisionDest = 0;
        public long PracticeDivisionUnitDest = 0;
        public string PracticeName { get; set; }
        public string ProfessionalName { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string DateofBirth { get; set; }
        public string PatientPhoneCode { get; set; }
        public string PatientPhone { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public DateTime? SelectedDate { get; set; }
        public DateTime? PastDay { get; set; }
        public string SelectedPracticeProfile { get; set; }
        public string SelectedProfessionalProfile { get; set; }
        public string SelectedProcedure { get; set; }
        public string SelectedPracticeLocation { get; set; }

        private SurgicalPatientDataService _surgicalPatientDataService;
        public readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergePatientViewPageViewModel PageViewModel;
        //private ObservableCollection<SurgicalConciergePatientViewModel> SurgicalConciergePatientViewModelList = new ObservableCollection<SurgicalConciergePatientViewModel>();

        private DateTime _searchDateTime;
        private DateTime _surgeryDateTime;

        private bool IsSurgicalConciergePatientPage = false;
        private bool IsNursePatientInfoPatientViewPageNew = false;
        private bool IsCalendarSelectedDate = false;

        private List<SurgicalConciergePatientCalendarViewModel> _surgicalConciergePatientCalendarViewModelList;
        private SurgicalConciergeRestApiService _restApiService = new SurgicalConciergeRestApiService();

        public SurgicalConciergePatientPage(long practiceDivision, long practiceDivisionUnit)
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
            _surgicalPatientDataService = new SurgicalPatientDataService();

            PastDay = DateTime.UtcNow;
            _searchDateTime = DateTime.UtcNow;

            BindingContext = PageViewModel = new SurgicalConciergePatientViewPageViewModel(this);

            this.IsSurgicalConciergePatientPage = true;
            this.IsNursePatientInfoPatientViewPageNew = false;

            ReLoadData();
        }

        public SurgicalConciergePatientPage(long practiceDivisionUnit, DateTime? pastDay)
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
            PracticeDivisionUnitDest = practiceDivisionUnit;

            //SelectedDate = pastDay;
            PastDay = pastDay;
            _searchDateTime = DateTime.UtcNow;

            BindingContext = PageViewModel = new SurgicalConciergePatientViewPageViewModel(this);

            this.IsSurgicalConciergePatientPage = true;
            this.IsNursePatientInfoPatientViewPageNew = false;

            ReLoadData();
        }

        public void ReLoadData()
        {
            try
            {
                PatientSearchCalendarList();

                PageViewModel.ReDownloadSurgicalConciergePatientPage();
                PatientView.ItemsSource = PageViewModel.SurgicalConciergePatientViewModeslInfiniteScroll;
            }
            catch { App.HideUserDialogAsync(); }
        }

        private void PatientSearchCalendarList()
        {
            if (this.IsCalendarSelectedDate == false)
            {
                if (this.SelectedDate != null)
                {
                    _surgeryDateTime = (DateTime)this.SelectedDate;
                    _searchDateTime = _surgeryDateTime;
                }
                else
                {
                    _searchDateTime = DateTime.UtcNow;
                }
                BindPatientSearchCalendarList(_searchDateTime);
            }
            else
            {
                this.IsCalendarSelectedDate = false;
            }
        }

        public async void NavigationToDestination(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            var selectedPatient = surgicalConciergePatientViewModel;
            SurgicalConciergeViewModel surgicalConciergeViewModel = new SurgicalConciergeViewModel();
            SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();

            //SurgicalConciergeDetail surgicalConciergeDetail = new SurgicalConciergeDetail(surgicalConciergeViewModel, selectedPatient);
            //surgicalConciergeDetail.LoadData();
            //await Navigation.PushAsync(surgicalConciergeDetail);
            if (PracticeDivisionUnitId.GetPathologyDivisionUnitId().Contains(PracticeDivisionUnitDest))
            {
                //await Navigation.PushAsync(new SurgicalConciergePatientEmailPage(selectedPatient, (long)PracticeDivisionUnit.Pathology));
                await Navigation.PushAsync(new SurgicalConciergePatientEmailPage(selectedPatient, Convert.ToInt16(PracticeDivisionDest), (long)PracticeDivisionUnit.Pathology, this));
            }
            else if (PracticeDivisionUnitId.GetOperatingRoomDivisionUnitId().Contains(PracticeDivisionUnitDest))
            {
                await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
            }
            else if (PracticeDivisionUnitId.GetPacuDivisionUnitId().Contains(PracticeDivisionUnitDest))
            {
                await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
            }
            else if (PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(PracticeDivisionUnitDest))
            {
                await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
            }
            else if (PracticeDivisionUnitId.GetPreSurgerySummaryDivisionUnitId().Contains(PracticeDivisionUnitDest))
            {
                PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel = await restApiService.GetPreSurgerySummaryInitialData(selectedPatient.PatientProcedureDetailId.ToString(), null, PracticeDivisionUnitDest);
                if (patientPreSurgerySummaryViewModel != null)
                {
                    if (patientPreSurgerySummaryViewModel.IsAlreadyInsertedSummary)
                    {
                        // Redirect to Summary Page
                        await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryPage(selectedPatient, patientPreSurgerySummaryViewModel));
                    }
                    else
                    {
                        await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryAddPage(selectedPatient, patientPreSurgerySummaryViewModel));
                    }
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullError, AppConstant.DisplayAlertErrorButtonText);
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

                await Navigation.PushAsync(new SurgicalConceirgePatientAddNew(this, PracticeDivisionDest, PracticeDivisionUnitDest, false));
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

                await Navigation.PushModalAsync(new PatientSearchView(this));
            }
        }

        private async void ShowPatientCalendarSearchModal(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }

            await Navigation.PushModalAsync(new PatientCalendarSearchModal(this));
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

                await Navigation.PushModalAsync(new PatientSearchFilterView(this));
            }
        }

        public async void OpenSurgicalDetailPage(SurgicalConciergeViewModel surgicalConciergeViewModel, SurgicalConciergePatientViewModel model)
        {
            try
            {
                SurgicalConciergeDetail surgicalConciergeDetail = new SurgicalConciergeDetail(surgicalConciergeViewModel, model);
                surgicalConciergeDetail.LoadData();
                await Navigation.PushAsync(surgicalConciergeDetail);
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void ShowToolbar()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                // move layout under the status bar
                this.Padding = new Thickness(0, 20, 0, 0);

                var toolbarItem = new ToolbarItem("AddPatient", null, () =>
                {
                    if (!InternetConnectHelper.CheckConnection())
                    {
                        UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                        return;
                    }
                    Navigation.PushModalAsync(new SurgicalConceirgePatientAdd(this, false));

                }, 0, 0);
                toolbarItem.Text = "Add Patient";
                ToolbarItems.Add(toolbarItem);
            }

            else
            {

                var toolbarItem = new ToolbarItem("AddPatient", null, () =>
                {
                    if (!InternetConnectHelper.CheckConnection())
                    {
                        UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                        return;
                    }
                    Navigation.PushModalAsync(new SurgicalConceirgePatientAdd(this, false));

                }, 0, 0);
                toolbarItem.Text = "Add Patient";
                ToolbarItems.Add(toolbarItem);

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

        private async void ButtonContinueExtended_Clicked(object sender, EventArgs e)
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
                    var button = (ButtonExtended)sender;

                    if (button.SelectedDataItem == null)
                        return;

                    var selectedPatient = button.SelectedDataItem as SurgicalConciergePatientViewModel;
                    SurgicalConciergeViewModel surgicalConciergeViewModel = new SurgicalConciergeViewModel();
                    SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();

                    //SurgicalConciergeDetail surgicalConciergeDetail = new SurgicalConciergeDetail(surgicalConciergeViewModel, selectedPatient);
                    //surgicalConciergeDetail.LoadData();
                    //await Navigation.PushAsync(surgicalConciergeDetail);

                    if (PracticeDivisionDest == (int)PracticeDivisionId.PatientRegistration) //PatientRegistration
                    {
                        //await Navigation.PushAsync(new SurgicalConceirgePatientAddNew(this, PracticeDivisionUnitDest, false));
                    }
                    else if (PracticeDivisionUnitId.GetScgNursingRoundDivisionUnitId().Contains(PracticeDivisionUnitDest))  //NursingRound
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetPathologyDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        //await Navigation.PushAsync(new SurgicalConciergePathologyPage(selectedPatient));
                        //await Navigation.PushAsync(new SurgicalConciergePatientEmailPage(selectedPatient, (long)PracticeDivisionUnit.Pathology));
                        await Navigation.PushAsync(new SurgicalConciergePatientEmailPage(selectedPatient, Convert.ToInt16(PracticeDivisionDest), (long)PracticeDivisionUnit.Pathology, this));
                    }
                    else if (PracticeDivisionUnitId.GetOperatingRoomDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetPacuDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetFloorDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetPreSurgerySummaryDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel = await restApiService.GetPreSurgerySummaryInitialData(selectedPatient.PatientProcedureDetailId.ToString(), null, PracticeDivisionUnitDest);
                        if (patientPreSurgerySummaryViewModel != null)
                        {
                            if (patientPreSurgerySummaryViewModel.IsAlreadyInsertedSummary)
                            {
                                // Redirect to Summary Page
                                await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryPage(selectedPatient, patientPreSurgerySummaryViewModel));
                            }
                            else
                            {
                                await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryAddPage(selectedPatient, patientPreSurgerySummaryViewModel));
                            }
                        }
                        else
                        {
                            await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullError, AppConstant.DisplayAlertErrorButtonText);
                        }
                    }


                    //PatientView.SelectedItem = null;
                    //((ListView)sender).SelectedItem = null;
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }

        private async void ButtonEditExtended_Clicked(object sender, EventArgs e)
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
                    var button = (ButtonExtended)sender;
                    button.Image = "update_icon.png";
                    if (button.SelectedDataItem == null)
                        return;

                    var selectedSurgicalConciergePatientViewModel = button.SelectedDataItem as SurgicalConciergePatientViewModel;

                    SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                    SurgicalConciergePatientViewModel surgicalConciergePatientViewModel = await restApiService.EditSurgicalConciergePatientProfile(selectedSurgicalConciergePatientViewModel.PatientProfileId, selectedSurgicalConciergePatientViewModel.PatientProcedureDetailId, Convert.ToInt16(PracticeDivisionDest), Convert.ToInt16(PracticeDivisionUnitDest));

                    if (surgicalConciergePatientViewModel != null)
                    {
                        await Navigation.PushAsync(new SurgicalConceirgePatientAddNew(this, PracticeDivisionDest, PracticeDivisionUnitDest, selectedSurgicalConciergePatientViewModel.PatientProcedureDetailId, surgicalConciergePatientViewModel, false));
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.NotFound, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }

        private async void ButtonDeleteExtended_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Patient Delete Confirmation", "Are you sure? you want to delete this record?", "Ok", "Cancel");
            if (answer == true)
            {
                var button = (ButtonExtended)sender;
                if (button.SelectedDataItem == null)
                    return;
                var selectedSurgicalConciergePatientViewModel = button.SelectedDataItem as SurgicalConciergePatientViewModel;
                using (UserDialogs.Instance.Loading(""))
                {
                    var result = await _surgicalPatientDataService.DeleteSurgicalConciergePatientProfileAndReLoad(selectedSurgicalConciergePatientViewModel.PatientProfileId);
                    if (result)
                    {
                        ReLoadData();
                    }
                }
            }
        }

        private async void PatientViewSelected(object sender, SelectedItemChangedEventArgs e)
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
                    var data = (SurgicalConciergePatientViewModel)e.SelectedItem;
                    if (data == null)
                        return;

                    var selectedPatient = data as SurgicalConciergePatientViewModel;
                    SurgicalConciergeViewModel surgicalConciergeViewModel = new SurgicalConciergeViewModel();
                    SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();

                    //SurgicalConciergeDetail surgicalConciergeDetail = new SurgicalConciergeDetail(surgicalConciergeViewModel, selectedPatient);
                    //surgicalConciergeDetail.LoadData();
                    //await Navigation.PushAsync(surgicalConciergeDetail);
                    if (PracticeDivisionUnitId.GetPathologyDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        //await Navigation.PushAsync(new SurgicalConciergePatientEmailPage(selectedPatient, (long)PracticeDivisionUnit.Pathology));
                        await Navigation.PushAsync(new SurgicalConciergePatientEmailPage(selectedPatient, Convert.ToInt16(PracticeDivisionDest), (long)PracticeDivisionUnit.Pathology, this));
                    }
                    else if (PracticeDivisionUnitId.GetOperatingRoomDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetPacuDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetDischargeDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        await Navigation.PushAsync(new SurgicalConciergeRecipientPage(selectedPatient, PracticeDivisionUnitDest, this));
                    }
                    else if (PracticeDivisionUnitId.GetPreSurgerySummaryDivisionUnitId().Contains(PracticeDivisionUnitDest))
                    {
                        PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel = await restApiService.GetPreSurgerySummaryInitialData(selectedPatient.PatientProcedureDetailId.ToString(), null, PracticeDivisionUnitDest);
                        if (patientPreSurgerySummaryViewModel != null)
                        {
                            if (patientPreSurgerySummaryViewModel.IsAlreadyInsertedSummary)
                            {
                                // Redirect to Summary Page
                                await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryPage(selectedPatient, patientPreSurgerySummaryViewModel));
                            }
                            else
                            {
                                await Navigation.PushAsync(new SurgicalConciergePreSurgerySummaryAddPage(selectedPatient, patientPreSurgerySummaryViewModel));
                            }
                        }
                        else
                        {
                            await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullError, AppConstant.DisplayAlertErrorButtonText);
                        }
                    }

                    PatientView.SelectedItem = null;
                    ((ListView)sender).SelectedItem = null;
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }
        }

        private async void ShowPatientSearchCalendar(object sender, EventArgs e)
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
                    var selectedDate = DateTime.UtcNow;
                    this.PracticeName = string.Empty;
                    this.ProfessionalName = string.Empty;
                    this.PatientName = string.Empty;
                    this.PatientEmail = string.Empty;
                    this.DateofBirth = string.Empty;
                    this.PatientPhoneCode = string.Empty;
                    this.PatientPhone = string.Empty;
                    this.SurgeryDate = null;
                    this.SelectedDate = selectedDate;
                    this.PastDay = selectedDate;

                    this.ReLoadData();

                    ResetPatientSearchCalendar(true);
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }

        private async void BtnHome_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                //App.ShowUserDialogAsync();
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                App.Instance.MainPage = new MenuPage(userIdentityModel);
                //App.Instance.MainPage = new MenuPracticePage();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void BindPatientSearchCalendarList(DateTime searchDate)
        {
            try
            {
                _searchDateTime = searchDate;

                DateTime todayDateTime = DateTime.UtcNow;
                int todayMonth = todayDateTime.Month;
                int todayDay = todayDateTime.Day;


                WeekStackLayout.Children.Clear();
                DateTime[] weekDays = new DateTime[7];
                DateTime[] lastWeekDays = new DateTime[7];

                int i = 0;
                for (i = 0; i < 7; i++)
                {
                    DateTime currentDate = searchDate.AddDays(i);
                    lastWeekDays[i] = currentDate;
                    if (currentDate.DayOfWeek.ToString().Substring(0, 2).ToLower().Equals("sa"))
                    {
                        break;
                    }
                }
                int dayCount = 0;
                for (int j = i + 1; j < 7; j++)
                {
                    weekDays[dayCount++] = searchDate.AddDays(j - 7);
                }
                for (int j = 0; dayCount < 7; j++)
                {
                    weekDays[dayCount++] = lastWeekDays[j];
                }

                DateTime startSurgeryDate = weekDays[0];
                DateTime endSurgeryDate = weekDays[6];

                _surgicalConciergePatientCalendarViewModelList = await _restApiService.GetPatientProfileWithProfessionalNoCountForCalendarAsync(
                    this.PracticeDivisionDest,
                    this.PracticeDivisionUnitDest,
                    startSurgeryDate.ToString("MM/dd/yyyy"),
                    endSurgeryDate.ToString("MM/dd/yyyy"),
                    "",
                    this.ProfessionalName,
                    this.PatientName,
                    "",
                    "",
                    "",
                    "",
                    null,
                    null,
                    null,
                    this.SelectedPracticeProfile,
                    this.SelectedProfessionalProfile,
                    this.SelectedProcedure,
                    this.SelectedPracticeLocation);

                string monthName = searchDate.ToString("MMMM");
                string lastWeekFirstDayMonthName = weekDays[0].ToString("MMMM");
                string lastWeekLastDayMonthName = weekDays[6].ToString("MMMM");

                for (i = 0; i < 7; i++)
                {
                    bool isSelectedDate = false;
                    bool hasPatient = false;

                    DateTime currentWeekDate = weekDays[i];
                    StackLayout firstStackLayout = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        WidthRequest = 44
                    };

                    string txtColor = "#000";
                    var fontAttributes = FontAttributes.None;

                    Label dayNameLabel = new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 16,
                        HeightRequest = 18,
                        Text = currentWeekDate.DayOfWeek.ToString().Substring(0, 3),
                        FontAttributes = fontAttributes
                    };

                    Color backgroundColor = Color.Transparent;
                    Color textColor = Color.FromHex(txtColor);

                    //Current Date, Dot, Select Month
                    string dateButtonClassId = currentWeekDate.ToString("MM/dd/yyyy");

                    Button dateButton = new Button()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        BackgroundColor = backgroundColor,
                        TextColor = textColor,
                        Text = currentWeekDate.Day.ToString("00"),
                        HeightRequest = 40,
                        FontSize = 18,
                        FontAttributes = fontAttributes
                    };

                    //if (IsSurgicalConciergePatientView == true && IsNursePatientInfoPatientViewPageNew == false)
                    //{
                    //    dateButton.Clicked += async (sender, args) => await PatientSearchDateSelected(sender, args, currentWeekDate);
                    //}
                    //else if (IsSurgicalConciergePatientView == false && IsNursePatientInfoPatientViewPageNew == true)
                    //{
                    //    dateButton.Clicked += async (sender, args) => await NursePatientSearchDateSelected(sender, args, currentWeekDate);
                    //}

                    if (IsSurgicalConciergePatientPage == true && IsNursePatientInfoPatientViewPageNew == false)
                    {
                        dateButton.Clicked += async (sender, args) => await PatientSearchDateSelected(sender, args, currentWeekDate);
                    }
                    else if (IsSurgicalConciergePatientPage == false && IsNursePatientInfoPatientViewPageNew == true)
                    {
                        dateButton.Clicked += async (sender, args) => await NursePatientSearchDateSelected(sender, args, currentWeekDate);
                    }

                    Image dotImage = new Image()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Source = "dot_icon.png",
                        HeightRequest = 12,
                        IsVisible = false,
                        ClassId = "0"
                    };

                    StackLayout selectedDayStackLayout = new StackLayout()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(10, 0),
                        Margin = new Thickness(0),
                        IsVisible = false,
                        ClassId = "0"
                    };

                    string selectedMonthNameText = currentWeekDate.ToString("MMMM");
                    Label selectedMonthNameLabel = new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 12,
                        HeightRequest = 16,
                        Text = selectedMonthNameText.Substring(0, 3)
                    };

                    BoxView selectedDayBoxView = new BoxView()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex("#007fff"),
                        HeightRequest = 4
                    };

                    selectedDayStackLayout.Children.Add(selectedMonthNameLabel);
                    selectedDayStackLayout.Children.Add(selectedDayBoxView);

                    if (_surgeryDateTime != default(DateTime))
                    {
                        if (currentWeekDate.Day == _surgeryDateTime.Day)
                        {
                            isSelectedDate = true;
                            txtColor = "#56b6fa";
                            fontAttributes = FontAttributes.Bold;
                        }
                    }
                    else
                    {
                        if (currentWeekDate.Day == todayDateTime.Day)
                        {
                            isSelectedDate = true;
                            txtColor = "#56b6fa";
                            fontAttributes = FontAttributes.Bold;
                        }
                    }


                    if (_surgicalConciergePatientCalendarViewModelList.Any())
                    {
                        var surgicalConciergePatientCalendarViewMode = _surgicalConciergePatientCalendarViewModelList.FirstOrDefault(x => x.SurgeryDate.ToString("MM/dd/yyyy") == currentWeekDate.ToString("MM/dd/yyyy"));
                        if (surgicalConciergePatientCalendarViewMode != null)
                        {
                            dotImage.ClassId = "1";
                            if (currentWeekDate.Day != todayDateTime.Day)
                            {
                                hasPatient = true;
                            }
                        }

                        if (hasPatient)
                        {
                            dotImage.IsVisible = true;
                            dateButtonClassId += "_1";
                        }
                        else
                        {
                            dotImage.ClassId = "0";
                            dateButtonClassId += "_0";
                        }
                    }

                    if (isSelectedDate)
                    {
                        selectedDayStackLayout.IsVisible = true;
                        selectedDayStackLayout.ClassId = "1";
                        dateButtonClassId += "_1";
                    }
                    else
                    {
                        selectedDayStackLayout.ClassId = "0";
                        dateButtonClassId += "_0";
                    }

                    dateButton.ClassId = dateButtonClassId;

                    firstStackLayout.Children.Add(dayNameLabel);

                    firstStackLayout.Children.Add(dateButton);

                    firstStackLayout.Children.Add(dotImage);

                    firstStackLayout.Children.Add(selectedDayStackLayout);

                    WeekStackLayout.Children.Add(firstStackLayout);

                }//end

            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private async Task PatientSearchDateSelected(object sender, EventArgs e, DateTime selectedDate)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    ResetPatientSearchCalendar();

                    Button btn = (Button)sender;
                    if (btn != null)
                    {
                        btn.TextColor = Color.FromHex("#56b6fa");
                        btn.FontAttributes = FontAttributes.Bold;

                        StackLayout stackLayout = btn.Parent as StackLayout;

                        if (stackLayout != null)
                        {
                            foreach (var item in stackLayout.Children)
                            {
                                Label lbl = item as Label;
                                if (lbl != null)
                                {
                                    lbl.FontAttributes = FontAttributes.Bold;
                                }

                                Image img = item as Image;
                                if (img != null)
                                {
                                    img.IsVisible = false;
                                }

                                StackLayout sl = item as StackLayout;
                                if (sl != null)
                                {
                                    sl.IsVisible = true;
                                }
                            }
                        }

                    }

                    this.PracticeName = string.Empty;
                    this.ProfessionalName = string.Empty;
                    this.PatientName = string.Empty;
                    this.PatientEmail = string.Empty;
                    this.DateofBirth = string.Empty;
                    this.PatientPhoneCode = string.Empty;
                    this.PatientPhone = string.Empty;
                    this.SurgeryDate = null;
                    this.SelectedDate = selectedDate;
                    this.PastDay = selectedDate;

                    this.IsCalendarSelectedDate = true;

                    this.ReLoadData();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async Task NursePatientSearchDateSelected(object sender, EventArgs e, DateTime selectedDate)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    ResetPatientSearchCalendar();

                    Button btn = (Button)sender;
                    if (btn != null)
                    {
                        btn.TextColor = Color.FromHex("#56b6fa");
                        btn.FontAttributes = FontAttributes.Bold;

                        StackLayout stackLayout = btn.Parent as StackLayout;

                        if (stackLayout != null)
                        {
                            foreach (var item in stackLayout.Children)
                            {
                                Label lbl = item as Label;
                                if (lbl != null)
                                {
                                    lbl.FontAttributes = FontAttributes.Bold;
                                }

                                Image img = item as Image;
                                if (img != null)
                                {
                                    img.IsVisible = false;
                                }

                                StackLayout sl = item as StackLayout;
                                if (sl != null)
                                {
                                    sl.IsVisible = true;
                                }
                            }
                        }

                    }

                    this.PracticeName = string.Empty;
                    this.ProfessionalName = string.Empty;
                    this.PatientName = string.Empty;
                    this.PatientEmail = string.Empty;
                    this.DateofBirth = string.Empty;
                    this.PatientPhoneCode = string.Empty;
                    this.PatientPhone = string.Empty;
                    this.SurgeryDate = null;
                    this.SelectedDate = selectedDate;
                    this.PastDay = selectedDate;

                    this.ReLoadData();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private void ResetPatientSearchCalendar(bool isTodayDateTime = false)
        {
            DateTime todayDateTime = DateTime.UtcNow;

            foreach (var item in WeekStackLayout.Children)
            {
                StackLayout stackLayout = item as StackLayout;

                if (stackLayout != null)
                {
                    foreach (var itemInner in stackLayout.Children)
                    {
                        Button btn = itemInner as Button;
                        string btnClassId = string.Empty;
                        int btnDay = 0;
                        bool isToday = false;
                        if (btn != null)
                        {
                            btn.TextColor = Color.FromHex("#000");
                            btn.FontAttributes = FontAttributes.None;
                            btnDay = Convert.ToInt32(btn.Text.ToString());
                            btnClassId = btn.ClassId.ToString();
                        }

                        if (isTodayDateTime)
                        {
                            if (btnDay == todayDateTime.Day)
                            {
                                isToday = true;
                            }
                        }

                        if (isToday == false)
                        {
                            Label lbl = itemInner as Label;
                            if (lbl != null)
                            {
                                lbl.FontAttributes = FontAttributes.None;
                            }

                            Image img = itemInner as Image;
                            string imgClassId = string.Empty;
                            bool dotImageVal = false;
                            if (img != null)
                            {
                                imgClassId = img.ClassId.ToString();
                                dotImageVal = Convert.ToBoolean(Convert.ToInt32(imgClassId));

                                if (dotImageVal == true)
                                {
                                    if (img.IsVisible == false)
                                    {
                                        img.IsVisible = true;
                                    }
                                }
                            }

                            StackLayout sl = itemInner as StackLayout;
                            if (sl != null)
                            {
                                sl.IsVisible = false;
                            }
                        }
                    }
                }
            }
        }

        private void PatientSearchCalendarPrevButton_Clicked(object sender, EventArgs e)
        {
            BindPatientSearchCalendarList(_searchDateTime.AddDays(-7));
        }

        private void PatientSearchCalendarNextButton_Clicked(object sender, EventArgs e)
        {
            BindPatientSearchCalendarList(_searchDateTime.AddDays(+7));
        }

        protected override void OnAppearing()
        {
            AddTopThreeTodayLabel.Text = DateTime.UtcNow.Day.ToString();
            base.OnAppearing();

        }
    }
}