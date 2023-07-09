using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationSurgeryDatePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IProcedureClient _iProcedureClient;

        private long NotificationId;

        public NotificationSurgeryDatePage(long notificationId, int day, string monthName, string title)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                NotificationId = notificationId;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;                     
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iProcedureClient = new ProcedureClient(apiClient);

                // set title
                NotificationMonth.Text = monthName;
                NotificationDay.Text = day.ToString();
                NotificationTitle.Text = title;
                ContentHeader.IsVisible = true;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private void OnSurgeryDatePageButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    if (NotificationId == Enums.SurgeryDatePatientNotificationEnum.NotificationId.ToInt())
                    {
                        Navigation.PushAsync(new NotificationListPageN());
                    }
                    else
                    {
                        Navigation.InsertPageBefore(new MainPatientPage(), this);
                        Navigation.PopAsync();
                    }
                }
            }
            else
            {
                DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        #region Bottom Menu Actions

        private async void OnHomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnResourceButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await ResourceButton();
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
                await Navigation.PushAsync(resourcePage);
            }
        }

        private async void OnScheduleButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await Navigation.PushAsync(new NotificationListPageN());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #endregion
    }
}