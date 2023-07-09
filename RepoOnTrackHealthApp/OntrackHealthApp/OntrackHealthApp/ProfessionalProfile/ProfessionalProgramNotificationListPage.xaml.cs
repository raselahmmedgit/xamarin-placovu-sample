using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Model;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.UserControls;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProgramNotificationListPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private readonly IProcedureClient _iProcedureClient;
        private ProfessionalProfileRestApiService _professionalProfileRestApiService;
        IEnumerable<ProcedureNotification> _procedureNotificationList;

        private long procedureId;
        private string procedureName;
        public ProfessionalProgramNotificationListPage(long procedureId, string procedureName)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            this.procedureId = procedureId;
            this.procedureName = procedureName;
            ProcedureName.Text = procedureName;
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);
            _professionalProfileRestApiService = new ProfessionalProfileRestApiService();
            _procedureNotificationList = new List<ProcedureNotification>();
            ExecuteNotificationListAsync();
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

        public async void ExecuteNotificationListAsync()
        {
            await App.ShowUserDialogDelayAsync();
            ProgramDetailViewModel programDetailViewModel = await _professionalProfileRestApiService.ProfessionalProgramNotification(procedureId);
            if(programDetailViewModel != null)
            {
                _procedureNotificationList = new List<ProcedureNotification>();
                if(programDetailViewModel.ProcedureNotifications != null && programDetailViewModel.ProcedureNotifications.Any())
                {
                    _procedureNotificationList = programDetailViewModel.ProcedureNotifications;
                    List<ProcedureNotification> prenotifications, postnotifications, notificationslist = new List<ProcedureNotification>();
                    prenotifications = _procedureNotificationList.Where(m => m.NotificationTypeId == Enums.NotificationTypeEnum.PostProcedure.ToInt()).ToList();
                    postnotifications = _procedureNotificationList.Where(m => m.NotificationTypeId != Enums.NotificationTypeEnum.PostProcedure.ToInt()).ToList();
                    foreach (var notification in postnotifications)
                    {
                        notificationslist.Add(notification);
                    }
                    foreach (var notification in prenotifications)
                    {
                        notificationslist.Add(notification);
                    }
                    NotificationListView.ItemsSource = notificationslist;
                }
            }
            App.HideUserDialogAsync();
        }

        private void NotificationListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var ProcedureNotification = (ProcedureNotification)e.SelectedItem;

                if (NotificationListView.SelectedItem != null)
                {
                    NotificationListView.SelectedItem = null;

                    if (InternetConnectHelper.CheckConnection())
                    {
                        var notificationId = ProcedureNotification.NotificationId.ToLong();
                        var notification = _procedureNotificationList.FirstOrDefault(x => x.NotificationId == notificationId);

                        Navigation.PushAsync(new ProfessionalProfileNotificationPage(notification.NotificationId, notification.NotificationTitle, procedureName));
                    }
                    else
                    {
                        DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                    }

                }
            }
            catch (Exception)
            {
                DisplayAlert(string.Empty, AppConstant.ApplicationExceptionError, AppConstant.DisplayAlertErrorButtonText);
            }
        }
    }
}