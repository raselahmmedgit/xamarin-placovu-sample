using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientDischargeResourceViewPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;

        public PatientDischargeResourceViewPage(SurgicalResourcePatientProstatectomyLibraryPageViewModel model)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = AppConstant.DischargeNotificationInfo;

                NotificationTitle.Text = model.SurgicalResourcePatientProstatectomyLibraryShortName;
                BindForm(model);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }

        }

        private void BindForm(SurgicalResourcePatientProstatectomyLibraryPageViewModel dischargeResource)
        {
            var data = GetNotificationDetailPageViewModel(dischargeResource);
            data.NotificationDetailHeader = "" + data.NotificationDetailHeader;
            BindingContext = data;
        }

        private NotificationDetailPageViewModel GetNotificationDetailPageViewModel(SurgicalResourcePatientProstatectomyLibraryPageViewModel dischargeResource)
        {
            NotificationDetailPageViewModel notificationDetailPageViewModel = new NotificationDetailPageViewModel();
            notificationDetailPageViewModel.NotificationDetailHeader = dischargeResource.SurgicalResourcePatientProstatectomyLibraryShortName;
            notificationDetailPageViewModel.NotificationDetail = dischargeResource.SurgicalResourcePatientProstatectomyLibraryResourceContent;
            return notificationDetailPageViewModel;
        }
        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //
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