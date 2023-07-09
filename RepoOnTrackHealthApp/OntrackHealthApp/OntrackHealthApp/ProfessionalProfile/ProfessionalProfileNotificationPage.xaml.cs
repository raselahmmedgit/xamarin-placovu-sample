using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Model;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProfileNotificationPage : ContentPage
	{

        private readonly ITokenContainer _iTokenContainer;

        NotificationPageNViewModel NotificationPageViewModel { get; set; }
        PatientNotificationShortView PatientNotificationShortView { get; set; }
        ProfessionalProfileRestApiService _professionalProfileRestApiService;
        ProgramDetailViewModel _programDetailViewModel;
        private long  notificationId;
        private string notifcationTitle;
        private string procedureName;
        public ProfessionalProfileNotificationPage(long notificationId, string title, string procedureName)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            _professionalProfileRestApiService = new ProfessionalProfileRestApiService();
            _programDetailViewModel = new ProgramDetailViewModel();
            Title = _iTokenContainer.ApiPracticeName;
            ProcedureName.Text = procedureName;
            this.notificationId = notificationId;
            this.notifcationTitle = title;
            this.procedureName = procedureName;

            LoadDataAsync();
            NotificationTitle.Text = title;
            ContentHeader.IsVisible = true;
        }

        private async void LoadDataAsync()
        {
            await App.ShowUserDialogDelayAsync();
            _programDetailViewModel = await _professionalProfileRestApiService.ProfessionalProgramNotificationDetail(notificationId);
            
            if (_programDetailViewModel != null)
            {
                NotificationHeader.Text = _programDetailViewModel.ProcedureNotification.NotificationHeader;
                if(_programDetailViewModel.ProcedureNotificationDetails != null)
                {
                    List<ProcedureNotificationDetail> procedureNotificationDetails = new List<ProcedureNotificationDetail>(_programDetailViewModel.ProcedureNotificationDetails);
                    MainRepeater.ItemsSource = procedureNotificationDetails;
                    if (procedureNotificationDetails.Count > 0)
                    {
                        MainRepeater.IsVisible = true;
                        TodaysProgram.IsVisible = true;
                    }
                }
                if(_programDetailViewModel.ProcedureNotificationSurveys.Count() > 0)
                {
                    CheckInProgramButton.IsVisible = true;
                }
                App.HideUserDialogAsync();
            }
        }
            private async void ShowMoreButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                var showMoreButton = sender as Button;
                var notificationDetailId = Convert.ToInt64(showMoreButton.ClassId);
                var notification = _programDetailViewModel.ProcedureNotificationDetails.Where(m => m.NotificationDetailId == notificationDetailId).FirstOrDefault();
                PatientNotificationShortView patientNotificationShortView = new PatientNotificationShortView() {
                    PatientNotificationDetailId = notification.NotificationDetailId,
                    NotificationTitle = notification.NotificationDetailHeader,
                    NotificationId = notificationId
                };
                await Navigation.PushAsync(new ProfessionalProgramNotificationDetailPage(patientNotificationShortView));
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
                PatientNotificationShortView patientNotificationShortView = new PatientNotificationShortView()
                {
                    PatientNotificationDetailId = _programDetailViewModel.ProcedureNotification.NotificationId,
                    NotificationTitle = notifcationTitle,
                    NotificationId = notificationId
                };

                await Navigation.PushAsync(new ProfessionalProgramPatientSurveyPage(patientNotificationShortView,_programDetailViewModel));
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
            ContentHeader.IsVisible = true;
        }
    }
}