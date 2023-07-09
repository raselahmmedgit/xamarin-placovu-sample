using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.PatientProgressReportGraph;
using OntrackHealthApp.PatientViews;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace OntrackHealthApp
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : MasterDetailPage
    {
        private List<MenuViewModel> MenuViewModelList { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private readonly IProcedureClient _iProcedureClient;

        public MenuPage(UserIdentityModel userIdentityModel)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);

            //PracticeName.Text = _iTokenContainer.ApiPracticeName;
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                PracticeName.Text = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                PracticeName.Text = _iTokenContainer.ApiPracticeName;
            }

            MenuViewModelList = new List<MenuViewModel>();

            if (userIdentityModel != null && (userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin)
                || userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel)
                || userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse)
                || userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD)
                || userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant)))
            {
                AdminMenuList();
                //MenuPageStackLayoutStyle.Padding = new Thickness(50, 5, 5, 5);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
            {
                ProfessionalMenuList();
                //MenuPageStackLayoutStyle.Padding = new Thickness(50, 5, 5, 5);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
            {
                PatientMenuList();
                MenuPageStackLayoutStyle.HeightRequest = 62;
            }
            else
            {
                PatientMenuList();
                MenuPageStackLayoutStyle.HeightRequest = 62;
            }

            // Setting our list to be ItemSource for ListView in MainPatientPage.xaml
            this.navigationDrawerList.ItemsSource = MenuViewModelList;

            if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.MainPracticePage)));
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPatientPage)));
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProfessionalProfile.MainProfessionalPage)));
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant))
            {
                //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivisionUnit)));
            }
            else
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPatientPage)));
            }

        }

        public MenuPage(UserIdentityModel userIdentityModel, ProfessionalProfilePageViewModel professionalProfilePageViewModel)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);

            PracticeName.Text = _iTokenContainer.ApiPracticeName;

            MenuViewModelList = new List<MenuViewModel>();

            if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
            {
                ProfessionalMenuList();
            }

            this.navigationDrawerList.ItemsSource = MenuViewModelList;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProfessionalProfile.MainProfessionalPage), professionalProfilePageViewModel));
        }

        public MenuPage(UserIdentityModel userIdentityModel, List<SurgicalConceirgePracticeDivision> practiceDivisionList)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);

            PracticeName.Text = _iTokenContainer.ApiPracticeLocationName;

            MenuViewModelList = new List<MenuViewModel>();

            AdminMenuList();

            this.navigationDrawerList.ItemsSource = MenuViewModelList;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision), practiceDivisionList));
        }

        public MenuPage(UserIdentityModel userIdentityModel, List<SurgicalConceirgePracticeDivisionUnit> practiceDivisionUnitList)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                PracticeName.Text = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                PracticeName.Text = _iTokenContainer.ApiPracticeName;
            }

            MenuViewModelList = new List<MenuViewModel>();

            AdminMenuList();

            this.navigationDrawerList.ItemsSource = MenuViewModelList;

            //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            if (practiceDivisionUnitList == null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivisionUnit)));
            }
            else
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivisionUnit), practiceDivisionUnitList));
            }

        }

        public MenuPage(string notificationId, string patientProcedureDetailId)
        {
            InitializeComponent();

            _iTokenContainer = new TokenContainer();

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);

            PracticeName.Text = _iTokenContainer.ApiPracticeName;
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            MenuViewModelList = new List<MenuViewModel>();

            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();

            if (userIdentityModel != null && (userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin)
            || userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel)
            || userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse)
            || userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD)
            || userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant)))
            {
                AdminMenuList();
                //MenuPageStackLayoutStyle.Padding = new Thickness(50, 5, 5, 5);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
            {
                ProfessionalMenuList();
                //MenuPageStackLayoutStyle.Padding = new Thickness(50, 5, 5, 5);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
            {
                PatientMenuList();
                MenuPageStackLayoutStyle.HeightRequest = 49;
            }
            else
            {
                PatientMenuList();
                MenuPageStackLayoutStyle.HeightRequest = 49;
            }

            // Setting our list to be ItemSource for ListView in MainPatientPage.xaml
            this.navigationDrawerList.ItemsSource = MenuViewModelList;

            //if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.MainPracticePage)));
            //}
            //else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            //}
            //else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            //}
            //else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            //}
            //else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPatientPage)));
            //}
            //else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProfessionalProfile.MainProfessionalPage)));
            //}
            //else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant))
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
            //}
            //else
            //{
            //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPatientPage)));
            //}

            Task.Run(async () => { await CurrentPatientProcedureDetailAsync(patientProcedureDetailId); });

            Detail.Navigation.PushAsync(new NotificationPageN(notificationId, patientProcedureDetailId));

        }

        private void AdminMenuList()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var pageHome = new MenuViewModel() { Title = "Home", Icon = "home_nav_icon_1.png", TargetType = typeof(SurgicalConcierge.MainPracticePage) };
            var pageChangePassword = new MenuViewModel() { Title = "Change Password", Icon = "changepassword_nav_icon.png", TargetType = typeof(SurgicalConcierge.AdminChangePassword) };
            var pageSignOut = new MenuViewModel() { Title = "Sign Out", Icon = "signout_nav_icon.png", TargetType = typeof(SurgicalConcierge.AdminSignOutPage) };

            // Adding menu items to MenuViewModelList
            MenuViewModelList.Add(pageHome);
            MenuViewModelList.Add(pageChangePassword);
            MenuViewModelList.Add(pageSignOut);
        }

        private void PatientMenuList()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var pageHome = new MenuViewModel() { Title = "Home", Icon = "home_nav_icon_1.png", TargetType = typeof(MainPatientPage) };
            var pageSchedule = new MenuViewModel() { Title = "Schedule", Icon = "schedule_nav_icon.png", TargetType = typeof(NotificationListPage) };
            var pagePhysician = new MenuViewModel() { Title = "Physician", Icon = "physician_nav_icon.png", TargetType = typeof(PhysicianProfilePage) };
            var pageLocation = new MenuViewModel() { Title = "Location", Icon = "location_nav_icon.png", TargetType = typeof(LocationPageNew) };
            var pageResources = new MenuViewModel() { Title = "Resources", Icon = "resources_nav_icon.png", TargetType = typeof(ResourcePage) };
            var pageCancerSummary = new MenuViewModel() { Title = "Cancer Summary", Icon = "resources_nav_icon.png", TargetType = typeof(CancerSummaryPage) };
            //var pageOtherInfo = new MenuViewModel() { Title = "Other Info", Icon = "resources_nav_icon.png", TargetType = typeof(HospitalInfoPage) };
            var pagePatientProgressReportGraph = new MenuViewModel() { Title = "My Progress", Icon = "resources_nav_icon.png", TargetType = typeof(PatientProgressReportGraphPage) };
            var pageOthers = new MenuViewModel() { Title = "Other Procedure", Icon = "otherprocedure_nav_icon.png", TargetType = typeof(OtherProcedurePage) };
            var pageChangePassword = new MenuViewModel() { Title = "Change Password", Icon = "changepassword_nav_icon.png", TargetType = typeof(ChangePasswordPage) };
            var pageUpdateProfile = new MenuViewModel() { Title = "Update Profile", Icon = "userprofile_nav_icon.png", TargetType = typeof(UpdateProfilePage) };
            var pageSignOut = new MenuViewModel() { Title = "Sign Out", Icon = "signout_nav_icon.png", TargetType = typeof(SignOutPage) };

            // Adding menu items to MenuViewModelList
            MenuViewModelList.Add(pageHome);
            MenuViewModelList.Add(pageSchedule);
            MenuViewModelList.Add(pagePhysician);
            MenuViewModelList.Add(pageLocation);
            MenuViewModelList.Add(pageResources);
            MenuViewModelList.Add(pageCancerSummary);
            //MenuViewModelList.Add(pageOtherInfo);
            MenuViewModelList.Add(pagePatientProgressReportGraph);
            MenuViewModelList.Add(pageOthers);
            MenuViewModelList.Add(pageChangePassword);
            MenuViewModelList.Add(pageUpdateProfile);
            MenuViewModelList.Add(pageSignOut);
        }

        private void ProfessionalMenuList()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var pageHome = new MenuViewModel() { Title = "Home", Icon = "home_nav_icon_1.png", TargetType = typeof(ProfessionalProfile.MainProfessionalPage) };
            var pageChangePassword = new MenuViewModel() { Title = "Change Password", Icon = "changepassword_nav_icon.png", TargetType = typeof(ProfessionalProfile.ProfessionalChangePassword) };
            var pageSignOut = new MenuViewModel() { Title = "Sign Out", Icon = "signout_nav_icon.png", TargetType = typeof(ProfessionalProfile.ProfessionalSignOutPage) };

            // Adding menu items to MenuViewModelList
            MenuViewModelList.Add(pageHome);
            MenuViewModelList.Add(pageChangePassword);
            MenuViewModelList.Add(pageSignOut);
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MenuViewModel)e.SelectedItem;
            Type page = item.TargetType;

            string pageName = page?.Name;

            if (pageName == "MainPatientPage")
            {
                OnMainButtonClickedAsync();
            }
            else if (pageName == "NotificationListPage")
            {
                OnScheduleButtonClickedAsync();
            }
            else if (pageName == "PhysicianProfilePage")
            {
                OnPhysicianButtonClickedAsync();
            }
            else if (pageName == "LocationPageNew")
            {
                OnLocationButtonClickedAsync();
            }
            else if (pageName == "ResourcePage")
            {
                OnResourceButtonClickedAsync();
            }
            else if (pageName == "CancerSummaryPage")
            {
                OnCancerSummaryButtonClickedAsync();
            }
            else if (pageName == "HospitalInfoPage")
            {
                OnHospitalInfoButtonClickedAsync();
            }
            else if (pageName == "PatientProgressReportGraphPage")
            {
                OnPatientProgressReportGraphPageButtonClickedAsync();
            }
            else if (pageName == "OtherProcedurePage")
            {
                OnOtherProcedureButtonClickedAsync();
            }
            else if (pageName == "ChangePasswordPage")
            {
                OnChangePasswordButtonClickedAsync();
            }
            else if (pageName == "UpdateProfilePage")
            {
                OnUpdateProfileButtonClickedAsync();
            }
            else if (pageName == "SignOutPage")
            {
                OnSignOutButtonClickedAsync();
            }
            else if (pageName == "MainPracticePage")
            {
                OnAdminMainButtonClickedAsync();
            }
            else if (pageName == "AdminChangePassword")
            {
                OnAdminChangePasswordButtonClickedAsync();
            }
            else if (pageName == "AdminSignOutPage")
            {
                OnAdminSignOutButtonClickedAsync();
            }
            else if (pageName == "MainProfessionalPage")
            {
                OnProfessionalMainButtonClickedAsync();
            }
            else if (pageName == "ProfessionalChangePassword")
            {
                OnProfessionalChangePasswordButtonClickedAsync();
            }
            else if (pageName == "ProfessionalSignOutPage")
            {
                OnProfessionalSignOutButtonClickedAsync();
            }

            //Detail = new NavigationPage((Page)Activator.CreateInstance(page));

            IsPresented = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        #region Menu

        private async Task<AppMessage> CurrentActiveProcedureWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentActiveProcedure = await _iProcedureClient.CurrentActiveProcedure();
            if (responseCurrentActiveProcedure.StatusIsSuccessful)
            {
                var data = responseCurrentActiveProcedure.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;
                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;
                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetailWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentPatientProcedureDetail = await _iProcedureClient.GetPatientProcedureDetail();
            if (responseCurrentPatientProcedureDetail.StatusIsSuccessful)
            {
                var data = responseCurrentPatientProcedureDetail.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;
                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;
                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            using (UserDialogs.Instance.Loading(""))
            {
                if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
                {
                    return appMessage = await CurrentActiveProcedureWiseButtonShowHideAsync();
                }
                else
                {
                    return appMessage = await CurrentPatientProcedureDetailWiseButtonShowHideAsync();
                }
            }

        }

        private async Task CurrentPatientProcedureDetailAsync(string patientProcedureDetailId)
        {
            var responseCurrentPatientProcedureDetail = await _iProcedureClient.GetPatientProcedureDetail(patientProcedureDetailId);
            if (responseCurrentPatientProcedureDetail.StatusIsSuccessful)
            {
                var data = responseCurrentPatientProcedureDetail.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;
                }

            }

        }

        private async Task<AppMessage> IsCurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }
            else
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }

        }

        private async void OnMainButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                MainButton();

                //var appMessage = await IsCurrentPatientProcedureDetail();
                //if (appMessage.MessageType == AppMessageType.Success)
                //{
                //    MainButton();
                //}
                //else
                //{
                //    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                //}
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void MainButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new MainPatientPage());

                //Navigation.InsertPageBefore(new MainPatientPage(), this);
                //await Navigation.PopToRootAsync();

                //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPatientPage)));
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                //Detail = new MenuPage(userIdentityModel);
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
        }

        private async void OnScheduleButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    ScheduleButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void ScheduleButton()
        {
            //using (UserDialogs.Instance.Loading(""))
            //{
            //    //NotificationListPageViewModel model = new NotificationListPageViewModel();
            //    //await model.ExecuteLoadCommandAsync();
            //    //var notificationListPage = new NotificationListPage(model);

            //    //await Navigation.PushAsync(notificationListPage);

            //    //Navigation.InsertPageBefore(notificationListPage, this);
            //    //await Navigation.PopToRootAsync();

            //    //Detail = new NavigationPage(notificationListPage);

            //}

            //App.ShowUserDialogAsync();
            Detail = new NavigationPage(new NotificationListPageN());
        }

        private async void OnPhysicianButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    PhysicianButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void PhysicianButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new PhysicianProfilePage());

                //Navigation.InsertPageBefore(new PhysicianProfilePage(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(PhysicianProfilePage)));
            }
        }

        private async void OnResourceButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await ResourceButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ResourceButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                ResourcePage resourcePage = new ResourcePage();
                await resourcePage.LoadDataAsync();

                //await Navigation.PushAsync(resourcePage);

                //Navigation.InsertPageBefore(resourcePage, this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage(resourcePage);
            }
        }

        private async void OnLocationButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    LocationButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void LocationButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //LocationPageNew locationPageNew = new LocationPageNew();
                //await Navigation.PushAsync(locationPageNew);

                //Navigation.InsertPageBefore(new LocationPageNew(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(LocationPageNew)));
            }
        }

        private async void OnCancerSummaryButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                //var appMessage = await IsCurrentPatientProcedureDetail();
                //if (appMessage.MessageType == AppMessageType.Success)
                //{
                //    CancerSummaryButton();
                //}
                //else
                //{
                //    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                //}
                CancerSummaryButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void CancerSummaryButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(CancerSummaryPage)));
            }
        }

        private async void OnHospitalInfoButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    HospitalInfoButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void HospitalInfoButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HospitalInfoPage)));
            }
        }

        private async void OnPatientProgressReportGraphPageButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    PatientProgressReportGraphPageButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void PatientProgressReportGraphPageButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(PatientProgressReportGraphPage)));
            }
        }

        private async void OnOtherProcedureButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                //var appMessage = await IsCurrentPatientProcedureDetail();
                //if (appMessage.MessageType == AppMessageType.Success)
                //{
                //    OtherProcedureButton();
                //}
                //else
                //{
                //    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                //}

                OtherProcedureButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void OtherProcedureButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new OtherProcedurePage());

                //Navigation.InsertPageBefore(new OtherProcedurePage(), this);
                //await Navigation.PopToRootAsync();

                //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(OtherProcedurePage)));
            }
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(OtherProcedurePage)));
        }

        private async void OnChangePasswordButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ChangePasswordPage)));
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnUpdateProfileButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(UpdateProfilePageNew)));
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void OnSignOutButtonClickedAsync()
        {
            if (_iTokenContainer != null)
            {
                _iTokenContainer.ClearApiToken();
            }
            DependencyService.Get<IToast>().SetSettingsForUserLogout();

            //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
            //Detail = new LoginPage();
            //Detail = new LoginPageNew();
            App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
        }

        #region Admin

        private async void OnAdminMainButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                AdminMainButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void AdminMainButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new SurgicalConcierge.MainPracticePage());

                //Navigation.InsertPageBefore(new SurgicalConcierge.MainPracticePage(), this);
                //await Navigation.PopToRootAsync();

                //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.MainPracticePage)));

                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();

                if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.MainPracticePage)));
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant))
                {
                    //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivision)));
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.SurgicalConciergePracticeDivisionUnit)));
                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.MainPracticePage)));
                }
            }
        }

        private async void OnAdminChangePasswordButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                //await Navigation.PushAsync(new SurgicalConcierge.AdminChangePassword());

                //Navigation.InsertPageBefore(new SurgicalConcierge.AdminChangePassword(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SurgicalConcierge.AdminChangePassword)));
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void OnAdminSignOutButtonClickedAsync()
        {
            if (_iTokenContainer != null)
            {
                _iTokenContainer.ClearApiToken();
            }
            DependencyService.Get<IToast>().SetSettingsForUserLogout();
            //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
            App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
        }


        #endregion

        #region Professional

        private async void OnProfessionalMainButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                ProfessionalMainButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void ProfessionalMainButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new MainProfessionalPage());

                //Navigation.InsertPageBefore(new MainProfessionalPage(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProfessionalProfile.MainProfessionalPage)));
            }
        }

        private async void OnProfessionalChangePasswordButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                //await Navigation.PushAsync(new ProfessionalChangePassword());

                //Navigation.InsertPageBefore(new ProfessionalChangePassword(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProfessionalProfile.ProfessionalChangePassword)));
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void OnProfessionalSignOutButtonClickedAsync()
        {
            if (_iTokenContainer != null)
            {
                _iTokenContainer.ClearApiToken();
            }
            DependencyService.Get<IToast>().SetSettingsForUserLogout();
            //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
            App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
        }

        #endregion

        #endregion
    }
}