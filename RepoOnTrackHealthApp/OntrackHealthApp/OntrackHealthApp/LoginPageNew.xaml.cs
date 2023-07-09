using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.Response;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.Extensions;
using OntrackHealthApp.Droid;
using OntrackHealthApp.iOS;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageNew : ContentPage
    {
        private ApplicationApiService _applicationApiService;
        private readonly ILoginClient _iLoginClient;
        private readonly ITokenContainer _iTokenContainer;
        private UserPracticeViewModel UserPracticeViewModel;
        private List<UserPracticeViewModel> UserPracticeViewModels = new List<UserPracticeViewModel>();
        private ResetPasswordViewModel ResetPasswordViewModel;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;

        public LoginPageNew()
        {
            InitializeComponent();

            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iLoginClient = new LoginClient(apiClient);
                _applicationApiService = new ApplicationApiService();
                professionalProfileRestApiService = new ProfessionalProfileRestApiService();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        //Last Call
        private async void OnLoginStepOneButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                App.ShowUserDialogAsync();
                var userModel = new UserModel
                {
                    Email = EmailEntry.Text?.Trim(),
                    Password = PasswordEntry.Text?.Trim()
                };

                #region Login

                var loginUserAppMessage = await LoginUserAsync(userModel);
                if (loginUserAppMessage.MessageType == AppMessageType.Success)
                {
                    await LoginUserResetPasswordFirstStepOneSuccess();
                }
                else
                {
                    MessageStackLayout.IsVisible = true;
                    MessageLabel.Text = loginUserAppMessage.Message;
                    PasswordEntry.Text = string.Empty;
                }

                #endregion
                App.HideUserDialogAsync();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        //First Call
        private async void OnNextStepOneButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                App.ShowUserDialogAsync();
                PasswordEntry.Text = string.Empty;
                var userModel = new UserModel
                {
                    Email = EmailEntry.Text?.Trim(),
                    Password = PasswordEntry.Text?.Trim()
                };

                var appMessage = await LoginUserPracticesAsync(userModel);
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    if (UserPracticeViewModels.Count == 0)
                    {
                        MessageStackLayout.IsVisible = true;
                        MessageLabel.Text = "Email is not valid.";
                        PasswordEntry.Text = string.Empty;
                    }
                    else if (UserPracticeViewModels.Count == 1)
                    {

                        UserPracticeViewModel = UserPracticeViewModels.FirstOrDefault();
                        //Varify allowed role
                        if (UserPracticeViewModel.RoleName == RoleIdConstants.Patient
                            || UserPracticeViewModel.RoleName == RoleIdConstants.PracticeAdmin
                            || UserPracticeViewModel.RoleName == RoleIdConstants.OperatingRoomPersonnel
                            || UserPracticeViewModel.RoleName == RoleIdConstants.OperatingRoomNurse
                            || UserPracticeViewModel.RoleName == RoleIdConstants.OperatingRoomMD
                            || UserPracticeViewModel.RoleName == RoleIdConstants.Professional
                            || UserPracticeViewModel.RoleName == RoleIdConstants.PhysicianAssistant)
                        {
                            #region LoginStepOne Visible For Password
                            LoginStepOneVisible();
                            #endregion
                        }
                        else
                        {
                            MessageStackLayout.IsVisible = true;
                            MessageLabel.Text = "Email is not valid.";
                            PasswordEntry.Text = string.Empty;
                        }
                    }
                    else
                    {
                        //Remove unauthorize role
                        var tmpList = new List<UserPracticeViewModel>();
                        UserPracticeViewModels.ForEach(x => {

                            if (x.RoleName == RoleIdConstants.Patient
                            || x.RoleName == RoleIdConstants.PracticeAdmin
                            || x.RoleName == RoleIdConstants.OperatingRoomPersonnel
                            || x.RoleName == RoleIdConstants.OperatingRoomNurse
                            || x.RoleName == RoleIdConstants.OperatingRoomMD
                            || x.RoleName == RoleIdConstants.Professional
                            || x.RoleName == RoleIdConstants.PhysicianAssistant)
                            {
                                tmpList.Add(x);
                            }
                        });
                        UserPracticeViewModels = tmpList;
                        //end Remove unauthorize role

                        var userPracticeList = UserPracticeViewModels.Where(a => a.PracticeLocationId == null)
                                                            .GroupBy(c => c.PracticeProfileId)
                                                            .Select(d => d.First()).ToList();

                        var userLocationList = UserPracticeViewModels.Where(a => a.PracticeLocationId != null)
                                                            .GroupBy(c => c.LocationName)
                                                            .Select(d => d.First()).ToList();

                        var userPracticeAndLocationList = new List<UserPracticeViewModel>();
                        userPracticeAndLocationList.AddRange(userPracticeList);
                        userPracticeAndLocationList.AddRange(userLocationList);

                        if (userPracticeAndLocationList.Count == 0)
                        {
                            MessageStackLayout.IsVisible = true;
                            MessageLabel.Text = "Email is not valid.";
                            PasswordEntry.Text = string.Empty;
                        }
                        else if (userPracticeAndLocationList.Count == 1)
                        {
                            UserRoleListView.ItemsSource = UserPracticeViewModels;

                            UserPracticeStackLayout.IsVisible = false;
                            LoginMainBlock.IsVisible = false;
                            LoginStepTwoStackLayout.IsVisible = true;
                            MessageLabel.Text = string.Empty;
                            MessageStackLayout.IsVisible = false;
                        }
                        else
                        {
                            userPracticeAndLocationList.ForEach(c =>
                            {
                                if (c.PracticeLocationId != null)
                                {
                                    c.PracticeName = c.LocationName;
                                }
                            });
                            UserPracticeListView.ItemsSource = userPracticeAndLocationList;

                            LoginStepTwoStackLayout.IsVisible = false;
                            LoginMainBlock.IsVisible = false;
                            UserPracticeStackLayout.IsVisible = true;
                            MessageLabel.Text = string.Empty;
                            MessageStackLayout.IsVisible = false;
                        }
                    }
                }
                else
                {
                    MessageStackLayout.IsVisible = true;
                    MessageLabel.Text = appMessage.Message;
                    PasswordEntry.Text = string.Empty;
                }
                App.HideUserDialogAsync();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnBtnUserPracticeClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                App.ShowUserDialogAsync();
                var selectedButtonItem = sender as Button;
                var data = selectedButtonItem.BindingContext as UserPracticeViewModel;

                if (data != null)
                {
                    if (data.IsLocationUser)
                    {
                        var locationUserList = UserPracticeViewModels.Where(c => c.PracticeLocationId == data.PracticeLocationId).ToList();
                        if (locationUserList.Count() == 1)
                        {
                            UserPracticeViewModel = locationUserList.FirstOrDefault();
                            #region LoginStepOne Visible For Password
                            LoginStepOneVisible();
                            UserPracticeStackLayout.IsVisible = false;
                            LoginStepTwoStackLayout.IsVisible = false;
                            LoginMainBlock.IsVisible = true;

                            #endregion
                        }
                        else
                        {
                            UserRoleListView.ItemsSource = locationUserList;

                            LoginStepTwoStackLayout.IsVisible = true;
                            UserPracticeStackLayout.IsVisible = false;
                        }
                    }
                    else
                    {
                        var practiceUserList = UserPracticeViewModels.Where(c => c.PracticeProfileId == data.PracticeProfileId && c.PracticeLocationId == null).ToList();
                        if (practiceUserList.Count() == 1)
                        {
                            UserPracticeViewModel = practiceUserList.FirstOrDefault();

                            #region LoginStepOne Visible For Password

                            LoginStepOneVisible();

                            UserPracticeStackLayout.IsVisible = false;
                            LoginStepTwoStackLayout.IsVisible = false;

                            LoginMainBlock.IsVisible = true;

                            #endregion
                        }
                        else
                        {
                            UserRoleListView.ItemsSource = practiceUserList;
                            LoginStepTwoStackLayout.IsVisible = true;
                            UserPracticeStackLayout.IsVisible = false;
                        }
                    }

                } //data
                App.HideUserDialogAsync();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
        //Practice Call
        private async void OnLoginStepTwoButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                App.ShowUserDialogAsync();

                var showMoreButton = sender as Button;
                UserPracticeViewModel = showMoreButton.BindingContext as UserPracticeViewModel;

                #region LoginStepOne Visible For Password

                LoginStepOneVisible();
                UserPracticeStackLayout.IsVisible = false;
                LoginStepTwoStackLayout.IsVisible = false;
                LoginMainBlock.IsVisible = true;

                #endregion

                App.HideUserDialogAsync();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnLoginStepResetPasswordFirstButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                App.ShowUserDialogAsync();

                var resetPasswordViewModel = new ResetPasswordViewModel
                {
                    UserId = _iTokenContainer.ApiUserId,
                    Email = ResetPasswordFirstEmailEntry.Text?.Trim(),
                    OldPassword = ResetPasswordFirstOldPasswordEntry.Text?.Trim(),
                    Password = ResetPasswordFirstPasswordEntry.Text?.Trim(),
                    ConfirmPassword = ResetPasswordFirstPasswordConfirmEntry.Text?.Trim(),
                    Code = ResetPasswordFirstCodeEntry.Text?.Trim(),
                };

                var appMessage = await LoginUserResetPasswordFirstAsync(resetPasswordViewModel);

                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await LoginUserSuccess();
                }
                else
                {
                    ResetPasswordFirstMessageStackLayout.IsVisible = true;
                    ResetPasswordFirstMessageLabel.Text = appMessage.Message;
                    ResetPasswordFirstPasswordEntry.Text = string.Empty;
                    ResetPasswordFirstPasswordConfirmEntry.Text = string.Empty;
                }
                App.HideUserDialogAsync();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #region Methods

        private async Task LoginUserResetPasswordFirstStepOneSuccess()
        {
            var appMessageResetPasswordFirst = await LoginUserResetPasswordFirstAsync();

            if (appMessageResetPasswordFirst.MessageType == AppMessageType.Success)
            {
                if (ResetPasswordViewModel != null)
                {
                    if (ResetPasswordViewModel.IsResetPasswordFirst)
                    {
                        GridResetPasswordFirstForm.IsVisible = true;
                        BtnLoginStepResetPasswordFirst.IsVisible = true;

                        UserPracticeStackLayout.IsVisible = false;
                        LoginStepTwoStackLayout.IsVisible = false;
                        LoginMainBlock.IsVisible = false;

                        ResetPasswordFirstMessageLabel.Text = string.Empty;
                        ResetPasswordFirstMessageStackLayout.IsVisible = false;
                    }
                    else
                    {
                        await LoginUserStepOneSuccess();
                    }
                }
                else
                {
                    await LoginUserStepOneSuccess();
                }
            }
            else
            {
                ResetPasswordFirstMessageStackLayout.IsVisible = true;
                ResetPasswordFirstMessageLabel.Text = appMessageResetPasswordFirst.Message;
            }
        }

        private async Task LoginUserStepOneSuccess()
        {
            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
            if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
            {
                _iTokenContainer.ApiIsAdmin = true;
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
            {
                _iTokenContainer.ApiIsAdmin = true;
                var practiceDivisionList = await TempDataContainer.GetPracticeDivisionFromJsonAsync();
                App.Instance.MainPage = new MenuPage(userIdentityModel, practiceDivisionList);

            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
            {
                _iTokenContainer.ApiIsAdmin = true;
                var practiceDivisionList = await TempDataContainer.GetPracticeDivisionFromJsonAsync();
                App.Instance.MainPage = new MenuPage(userIdentityModel, practiceDivisionList);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
            {
                _iTokenContainer.ApiIsAdmin = true;
                var practiceDivisionList = await TempDataContainer.GetPracticeDivisionFromJsonAsync();
                App.Instance.MainPage = new MenuPage(userIdentityModel, practiceDivisionList);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
            {
                _iTokenContainer.ApiIsAdmin = false;
                await GetCurrentPatientProcedureDetail();
                //bool isSuccess = await NotificationService.RegisterPushNotificationDeviceToken(userIdentityModel.PatientProfileId.Value);
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
            {
                _iTokenContainer.ApiIsAdmin = true;
                ProfessionalProfilePageViewModel professionalProfilePageViewModel = await professionalProfileRestApiService.GetProfessionalProfile(_iTokenContainer.ApiProfessionalProfileId);
                App.Instance.MainPage = new MenuPage(userIdentityModel, professionalProfilePageViewModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant))
            {
                _iTokenContainer.ApiIsAdmin = true;
                int practiceDivisionId = (int)PracticeDivisionId.Urology;
                var practiceDivisionUnitList = await TempDataContainer.GetPracticeDivisionUnitFromJsonAsync(practiceDivisionId);
                App.Instance.MainPage = new MenuPage(userIdentityModel, practiceDivisionUnitList);
            }
            else
            {
                _iTokenContainer.ApiIsAdmin = false;
                //App.Instance.MainPage = new LoginPage();
                App.Instance.MainPage = new LoginPageNew();
            }
            //Reset User Data
            TempDataContainer.PracticeDivisionUnit = string.Empty;
            //======================================================
            await Navigation.PopAsync();
        }

        private async Task LoginUserResetPasswordFirstSuccess()
        {
            var appMessageResetPasswordFirst = await LoginUserResetPasswordFirstAsync();

            if (appMessageResetPasswordFirst.MessageType == AppMessageType.Success)
            {
                if (ResetPasswordViewModel != null)
                {
                    if (ResetPasswordViewModel.IsResetPasswordFirst)
                    {
                        GridResetPasswordFirstForm.IsVisible = true;
                        UserPracticeStackLayout.IsVisible = false;
                        LoginStepTwoStackLayout.IsVisible = false;
                        LoginMainBlock.IsVisible = false;
                        ResetPasswordFirstMessageLabel.Text = string.Empty;
                        ResetPasswordFirstMessageStackLayout.IsVisible = false;
                    }
                    else
                    {
                        await LoginUserSuccess();
                    }
                }
                else
                {
                    await LoginUserSuccess();
                }
            }
            else
            {
                ResetPasswordFirstMessageStackLayout.IsVisible = true;
                ResetPasswordFirstMessageLabel.Text = appMessageResetPasswordFirst.Message;
            }
        }

        private async Task LoginUserSuccess()
        {
            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
            if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
            {
                _iTokenContainer.ApiIsAdmin = true;
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
            {
                _iTokenContainer.ApiIsAdmin = true;
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
            {
                _iTokenContainer.ApiIsAdmin = true;
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
            {
                _iTokenContainer.ApiIsAdmin = true;
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
            {
                _iTokenContainer.ApiIsAdmin = false;
                await GetCurrentPatientProcedureDetail();
                //bool isSuccess = await NotificationService.RegisterPushNotificationDeviceToken(userIdentityModel.PatientProfileId.Value);
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
            {
                _iTokenContainer.ApiIsAdmin = false;
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant))
            {
                _iTokenContainer.ApiIsAdmin = true;
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else
            {
                _iTokenContainer.ApiIsAdmin = false;
                //App.Instance.MainPage = new LoginPage();
                App.Instance.MainPage = new LoginPageNew();
            }

            await Navigation.PopAsync();
        }

        private void LoginStepOneVisible()
        {
            PasswordStackLayout.IsVisible = true;
            LoginStepOneNextStackLayout.IsVisible = false;
            LoginStepOneLoginStackLayout.IsVisible = true;
            MessageLabel.Text = string.Empty;
            MessageStackLayout.IsVisible = false;
        }

        private async void Login()
        {
            #region Login

            var userModel = new UserModel
            {
                Email = EmailEntry.Text?.Trim(),
                Password = PasswordEntry.Text?.Trim()
            };

            var appMessage = await LoginUserAsync(userModel);
            if (appMessage.MessageType == AppMessageType.Success)
            {
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                if (userIdentityModel != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
                {
                    Navigation.InsertPageBefore(new HomeMenuPage(), this);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
                {
                    //Navigation.InsertPageBefore(new SurgicalConciergePracticeDivisionUnit(0), this);
                    Navigation.InsertPageBefore(new SurgicalConciergePracticeDivision(), this);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
                {
                    Navigation.InsertPageBefore(new SurgicalConciergePracticeDivision(), this);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
                {
                    Navigation.InsertPageBefore(new SurgicalConciergePracticeDivision(), this);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
                {
                    await GetCurrentPatientProcedureDetail();
                    //bool isSuccess = await NotificationService.RegisterPushNotificationDeviceToken(userIdentityModel.PatientProfileId.Value);
                    Navigation.InsertPageBefore(new MainPage(), this);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
                {
                    _iTokenContainer.ApiIsAdmin = false;
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PhysicianAssistant))
                {
                    //Navigation.InsertPageBefore(new SurgicalConciergePracticeDivision(), this);
                    int divisionId = (int)PracticeDivisionId.Urology;
                    Navigation.InsertPageBefore(new SurgicalConciergePracticeDivisionUnit(divisionId), this);
                }
                else
                {
                    await GetCurrentPatientProcedureDetail();
                    Navigation.InsertPageBefore(new MainPage(), this);
                }

                await Navigation.PopAsync();
            }
            else
            {
                //UserPracticeStackLayout.IsVisible = false;

                LoginStepOneLoginStackLayout.IsVisible = true;
                BtnLoginStepOne.IsVisible = true;

                EmailStackLayout.IsVisible = true;
                EmailEntry.IsVisible = true;

                PasswordStackLayout.IsVisible = true;
                PasswordEntry.IsVisible = true;

                MessageStackLayout.IsVisible = true;
                MessageLabel.Text = appMessage.Message;
                PasswordEntry.Text = string.Empty;
            }

            #endregion
        }

        private async Task<AppMessage> LoginUserPracticesAsync(UserModel userModel)
        {
            AppMessage appMessage = IsValidForNext(userModel);

            #region Login

            if (appMessage.MessageType == AppMessageType.Success)
            {
                #region Api Client

                try
                {
                    if (UserPracticeViewModels.Count == 0)
                    {

                        var response = await _iLoginClient.LoginUserPractices(userModel.Email);
                        if (response.StatusIsSuccessful)
                        {
                            UserPracticeViewModels = response.DataList;
                            appMessage = SetAppMessage.SetSuccessMessage("Data successfully.");
                        }
                        else
                        {
                            appMessage = SetAppMessage.SetErrorMessage(response.ErrorState.Message);
                        }

                    }
                }
                catch (Exception)
                {
                    appMessage = SetAppMessage.SetErrorMessage("Login failed.");
                }

                #endregion

            }

            #endregion

            return appMessage;
        }

        private async Task<AppMessage> LoginUserResetPasswordFirstAsync(ResetPasswordViewModel resetPasswordViewModel)
        {
            AppMessage appMessage = IsValidResetPasswordFirst(resetPasswordViewModel);

            #region Login

            if (appMessage.MessageType == AppMessageType.Success)
            {
                #region Api Client

                try
                {

                    var response = await _iLoginClient.ResetPasswordFirst(resetPasswordViewModel);

                    if (response.StatusIsSuccessful)
                    {
                        appMessage = SetAppMessage.SetSuccessMessage("Reset successfully.");
                    }
                    else
                    {
                        appMessage = SetAppMessage.SetErrorMessage(response.ErrorState.Message);
                    }
                }
                catch (Exception)
                {
                    appMessage = SetAppMessage.SetErrorMessage("Reset failed.");
                }

                #endregion
            }

            #endregion

            return appMessage;
        }

        private async Task<AppMessage> LoginUserAsync(UserModel userModel)
        {
            AppMessage appMessage = IsValid(userModel);

            #region Login

            if (appMessage.MessageType == AppMessageType.Success)
            {
                #region Api Client

                try
                {
                    var response = await LoginUserAndTokenAsync(UserPracticeViewModel.UserName, userModel.Password);

                    if (response.StatusIsSuccessful)
                    {
                        appMessage = SetAppMessage.SetSuccessMessage("Login successfully.");
                    }
                    else
                    {
                        appMessage = SetAppMessage.SetErrorMessage(response.ErrorState.Message);
                    }

                }
                catch (Exception)
                {
                    appMessage = SetAppMessage.SetErrorMessage("Login failed.");
                }

                #endregion
            }

            #endregion

            return appMessage;
        }

        private async Task<AppMessage> LoginUserResetPasswordFirstAsync()
        {
            AppMessage appMessage = new AppMessage();

            #region Login User ResetPasswordFirst

            try
            {
                if (ResetPasswordViewModel == null)
                {
                    var response = await _iLoginClient.LoginUserResetPasswordFirst();
                    if (response.StatusIsSuccessful)
                    {
                        ResetPasswordViewModel = response.Data;

                        ResetPasswordFirstEmailEntry.Text = ResetPasswordViewModel.Email;
                        string OldPassword = PasswordEntry.Text?.Trim();
                        ResetPasswordFirstOldPasswordEntry.Text = OldPassword;
                        ResetPasswordFirstPasswordEntry.Text = ResetPasswordViewModel.Password;
                        ResetPasswordFirstPasswordConfirmEntry.Text = ResetPasswordViewModel.ConfirmPassword;
                        ResetPasswordFirstCodeEntry.Text = ResetPasswordViewModel.Code;

                        appMessage = SetAppMessage.SetSuccessMessage("Data successfully.");
                    }
                    else
                    {
                        appMessage = SetAppMessage.SetErrorMessage(response.ErrorState.Message);
                    }

                }
            }
            catch (Exception)
            {
                appMessage = SetAppMessage.SetErrorMessage("Reset Password First failed.");
            }

            #endregion

            return appMessage;
        }

        private async Task<TokenResponse> LoginUserAndTokenAsync(string email, string password)
        {
            var response = await _iLoginClient.Login(email, password);
            if (response.StatusIsSuccessful)
            {
                _iTokenContainer.ApiToken = response.Data;

                var responseLoginUserIdentity = await LoginUserIdentityAsync();
            }

            return response;
        }

        private async Task<bool> LoginUserIdentityAsync()
        {
            var response = await _iLoginClient.LoginUserIdentity();
            if (response.StatusIsSuccessful)
            {
                _iTokenContainer.SetUserIdentityModel(response.Data);
            }

            return response.StatusIsSuccessful;
        }

        private void Logout()
        {
            if (_iTokenContainer != null)
            {
                _iTokenContainer.ClearApiToken();

            }
            DependencyService.Get<IToast>().SetSettingsForUserLogout();
        }

        private void LogoutGoLoginPage()
        {
            if (_iTokenContainer != null)
            {
                _iTokenContainer.ClearApiToken();
            }
            DependencyService.Get<IToast>().SetSettingsForUserLogout();
            //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
            App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
        }

        private AppMessage IsValidResetPasswordFirst(ResetPasswordViewModel resetPasswordViewModel)
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrWhiteSpace(resetPasswordViewModel.Email))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is required.");
            }
            if (!resetPasswordViewModel.Email.Contains("@"))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is not valid.");
            }
            if (string.IsNullOrWhiteSpace(resetPasswordViewModel.OldPassword))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Current password is required.");
            }
            if (string.IsNullOrWhiteSpace(resetPasswordViewModel.Code))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Code is required.");
            }
            if (string.IsNullOrWhiteSpace(resetPasswordViewModel.Password))
            {
                return appMessage = SetAppMessage.SetErrorMessage("New password is required.");
            }
            if (string.IsNullOrWhiteSpace(resetPasswordViewModel.ConfirmPassword))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Confirm new password is required.");
            }
            if (resetPasswordViewModel.Password.ToLower() != resetPasswordViewModel.ConfirmPassword.ToLower())
            {
                return appMessage = SetAppMessage.SetErrorMessage("The new password and confirmation password do not match.");
            }

            return appMessage = SetAppMessage.SetSuccessMessage();
        }

        private AppMessage IsValidEmail(string email)
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrWhiteSpace(email))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is required.");
            }
            if (!email.Contains("@"))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is not valid.");
            }

            return appMessage = SetAppMessage.SetSuccessMessage();
        }

        private AppMessage IsValidForNext(UserModel userModel)
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrWhiteSpace(userModel.Email))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is required.");
            }
            if (!userModel.Email.Contains("@"))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is not valid.");
            }

            return appMessage = SetAppMessage.SetSuccessMessage();
        }

        private AppMessage IsValid(UserModel userModel)
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrWhiteSpace(userModel.Email))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is required.");
            }
            if (!userModel.Email.Contains("@"))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is not valid.");
            }
            if (string.IsNullOrWhiteSpace(userModel.Password))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Password is required.");
            }

            return appMessage = SetAppMessage.SetSuccessMessage();
        }

        private async Task GetCurrentPatientProcedureDetail()
        {
            if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    MainPageViewModel mainPageViewModel = new MainPageViewModel();
                    await mainPageViewModel.ExecuteLoadCommandAsync();

                    if (mainPageViewModel != null)
                    {
                        if (mainPageViewModel.PatientProcedureDetailModel != null)
                        {
                            _iTokenContainer.CurrentPatientProcedureDetailId = mainPageViewModel.PatientProcedureDetailModel.PatientProcedureDetailId.ToString();
                            _iTokenContainer.CurrentProcedureName = mainPageViewModel.PatientProcedureDetailModel.ProcedureName;
                        }
                    }

                }
            }
            else
            {

            }
        }

        #endregion
    }
}