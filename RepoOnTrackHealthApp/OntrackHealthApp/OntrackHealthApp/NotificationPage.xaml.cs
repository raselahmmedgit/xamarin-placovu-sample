using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;

        //NotificationListPageViewModel notificationListPageViewModel;
        NotificationPageViewModel NotificationPageViewModel { get; set; }

        public NotificationPage(NotificationPageViewModel notificationPageViewModel)
        {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                NotificationPageViewModel = notificationPageViewModel;
                BindingContext = NotificationPageViewModel;
                NotificationPageViewModel.NotificationTitle = NotificationPageViewModel.NotificationTitle;
        }
        
        private async void ShowMoreButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var btnShowMore = sender as Button;
                    var notificationId = Convert.ToInt32(btnShowMore.ClassId);
                    var pSProcedureNotificationDetail = NotificationPageViewModel.PSProcedureNotificationDetails.FirstOrDefault(item => item.NotificationDetailId == notificationId);
                    pSProcedureNotificationDetail.NotificationTitle = NotificationPageViewModel.NotificationTitle;
                    //await Navigation.PushAsync(new NotificationDetailPage(pSProcedureNotificationDetail));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private async void CheckInProgramButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    PatientSurveyPageViewModel model = new PatientSurveyPageViewModel
                    {
                        PatientSurveyPatientNotificationDetailViewModel = NotificationPageViewModel.PatientSurveyPatientNotificationDetailViewModel,
                        NotificationTitle = NotificationPageViewModel.NotificationTitle
                    };
                    //await Navigation.PushAsync(new PatientSurveyPage(model));
                    await Navigation.PushAsync(new PatientSurveyPage(new ApiService.ViewModel.PatientNotificationShortView()));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }            
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            var d = ((Grid)sender).Height;
        }
    }
}