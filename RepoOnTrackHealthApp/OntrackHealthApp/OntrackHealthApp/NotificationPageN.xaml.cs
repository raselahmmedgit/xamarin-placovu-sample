using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPageN : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private AdministrationPatientProfileRestApiService restApiService;

        NotificationPageNViewModel NotificationPageViewModel { get; set; }
        PatientNotificationShortView PatientNotificationShortView { get; set; }

        public NotificationPageN()
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            restApiService = new AdministrationPatientProfileRestApiService();
            _iTokenContainer.HasStartByClickToastNotification = true;
            NotificationPageViewModel = new NotificationPageNViewModel();
            NotificationPageViewModel.NotificationId = 0;
            NotificationPageViewModel.PatientProcedureDetailId = _iTokenContainer.CurrentPatientProcedureDetailId;
            BindingContext = NotificationPageViewModel;
        }
        public NotificationPageN(long notificationId, int day, string monthName, string title)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            
            Title = _iTokenContainer.ApiPracticeName;
            ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            restApiService = new AdministrationPatientProfileRestApiService();
            NotificationPageViewModel = new NotificationPageNViewModel();
            NotificationPageViewModel.NotificationId = notificationId;
            NotificationPageViewModel.PatientProcedureDetailId = _iTokenContainer.CurrentPatientProcedureDetailId;
            BindingContext = NotificationPageViewModel;

            // set title
            NotificationMonth.Text = monthName;
            NotificationDay.Text = day.ToString();
            NotificationTitle.Text = title;
            ContentHeader.IsVisible = true;
        }
        public NotificationPageN(string notificationId, string patientProcedureDetailId)
        {
            InitializeComponent();
            
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            restApiService = new AdministrationPatientProfileRestApiService();
            _iTokenContainer.HasStartByClickToastNotification = true;
            NotificationPageViewModel = new NotificationPageNViewModel();
            NotificationPageViewModel.NotificationId = notificationId.ToLong();
            NotificationPageViewModel.PatientProcedureDetailId = patientProcedureDetailId;
            BindingContext = NotificationPageViewModel;
        }
        private async void ShowMoreButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                var showMoreButton = sender as Button;
                var notificationDetailId = Convert.ToInt64(showMoreButton.ClassId);
                NotificationPageViewModel.PatientNotificationShortView.PatientNotificationDetailId = notificationDetailId;
                await Navigation.PushAsync(new NotificationDetailPage(NotificationPageViewModel.PatientNotificationShortView));
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
                PatientSurveyPageViewModel model = new PatientSurveyPageViewModel
                {
                    PatientSurveyPatientNotificationDetailViewModel = NotificationPageViewModel.PatientSurveyPatientNotificationDetailViewModel,
                    NotificationTitle = NotificationPageViewModel.NotificationTitle
                };
                NotificationPageViewModel.PatientNotificationShortView.NotificationId = NotificationPageViewModel.NotificationId;
                if (NotificationPageViewModel.PatientNotificationShortView.NotificationId == (int)Enums.NotificationIdEnum.PastMedicalMaleSurvey
                 || NotificationPageViewModel.PatientNotificationShortView.NotificationId == (int)Enums.NotificationIdEnum.PastMedicalFemaleSurvey)
                {
                    await Navigation.PushAsync(new PatientPastMedicalSurveyPage(NotificationPageViewModel.PatientNotificationShortView));
                }
                else
                {
                    await Navigation.PushAsync(new PatientSurveyPage(NotificationPageViewModel.PatientNotificationShortView));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        protected override async void OnAppearing()
        {
            await IsAgreePatientMonitoringConsentAsync();

            NavigationPage.SetBackButtonTitle(this, "");

            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPage());
            }
            if (NotificationPageViewModel.PatientNotificationShortView == null)
            {
                NotificationPageViewModel.LoadPatientNotificationCommand.Execute(null);
                MainRepeater.ItemsSource = NotificationPageViewModel.PSProcedureNotificationDetails;
            }
            ContentHeader.IsVisible = true;
        }

        private async Task IsAgreePatientMonitoringConsentAsync()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                bool result = await restApiService.PatientMonitoringConsentIsAgreed(_iTokenContainer.ApiUserId);
                if (result == true)
                {
                    _iTokenContainer.ApiIsAgreePatientMonitoringConsent = true;
                    _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = false;
                }
                else
                {
                    _iTokenContainer.ApiIsAgreePatientMonitoringConsent = false;
                    _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent = true;
                }

                if (_iTokenContainer.ApiIsAgreePatientMonitoringConsent == false && _iTokenContainer.ApiIsDoNotAgreePatientMonitoringConsent == true)
                {
                    App.Instance.ClearNavigationAndGoToPage(new PatientMonitoringConsentPage());
                }
            }
        }
    }
}