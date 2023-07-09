using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly ILoginClient _iLoginClient;

        public ChangePasswordPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = "Change Password";
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iLoginClient = new LoginClient(apiClient);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void BtnChangePasswordUpdate_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var changePasswordViewModel = new ChangePasswordViewModel
                    {
                        Email = EmailEntry.Text,
                        OldPassword = CurrentPasswordEntry.Text,
                        NewPassword = NewPasswordEntry.Text,
                        ConfirmPassword = ConfirmNewPasswordEntry.Text,
                    };

                    var appMessage = await ChangePasswordAsync(changePasswordViewModel);

                    if (appMessage.MessageType == AppMessageType.Success)
                    {
                        MessageLabel.Text = appMessage.Message;
                        MessageLabelFrame.IsVisible = true;
                        CurrentPasswordEntry.Text = string.Empty;
                        NewPasswordEntry.Text = string.Empty;
                        ConfirmNewPasswordEntry.Text = string.Empty;

                        App.Instance.ClearNavigationAndGoToPage(new ChangePasswordSuccessPage(appMessage));
                    }
                    else
                    {
                        MessageLabel.Text = appMessage.Message;
                        MessageLabelFrame.IsVisible = true;
                        CurrentPasswordEntry.Text = string.Empty;
                        NewPasswordEntry.Text = string.Empty;
                        ConfirmNewPasswordEntry.Text = string.Empty;
                    }
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task<AppMessage> ChangePasswordAsync(ChangePasswordViewModel changePasswordViewModel)
        {
            AppMessage appMessage = IsValid(changePasswordViewModel);

            #region Login

            if (appMessage.MessageType == AppMessageType.Success)
            {
                #region Api Client

                try
                {
                    var response = await _iLoginClient.ChangePassword(changePasswordViewModel);

                    if (response.StatusIsSuccessful)
                    {
                        appMessage = SetAppMessage.SetSuccessMessage("Password has been changed successfully.");
                    }
                    else
                    {
                        appMessage = SetAppMessage.SetErrorMessage(response.ErrorState.Message);
                    }

                }
                catch (Exception)
                {
                    appMessage = SetAppMessage.SetErrorMessage("Password has not been changed.");
                }

                #endregion

            }

            #endregion

            return appMessage;
        }

        private AppMessage IsValid(ChangePasswordViewModel changePasswordViewModel)
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrWhiteSpace(changePasswordViewModel.Email))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is required.");
            }
            if (!changePasswordViewModel.Email.Contains("@"))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Email is not valid.");
            }
            if (string.IsNullOrWhiteSpace(changePasswordViewModel.OldPassword))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Current password is required.");
            }
            if (string.IsNullOrWhiteSpace(changePasswordViewModel.NewPassword))
            {
                return appMessage = SetAppMessage.SetErrorMessage("New password is required.");
            }
            if (string.IsNullOrWhiteSpace(changePasswordViewModel.ConfirmPassword))
            {
                return appMessage = SetAppMessage.SetErrorMessage("Confirm new password is required.");
            }
            if (changePasswordViewModel.NewPassword.ToLower() != changePasswordViewModel.ConfirmPassword.ToLower())
            {
                return appMessage = SetAppMessage.SetErrorMessage("The new password and confirmation password do not match.");
            }

            return appMessage = SetAppMessage.SetSuccessMessage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            EmailEntry.Text = _iTokenContainer.ApiUserEmail;
        }

        private async void OnChangePasswordCancelButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                //App.Instance.ClearNavigationAndGoToPage(new MainPage());
                //await Navigation.PopAsync();
                //App.Current.MainPage = new CustomNavigation.CustomNavigationPage(new MainPatientPage());
                //App.Instance.ClearNavigationAndGoToPage(new MainPatientPage());
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
    }
}