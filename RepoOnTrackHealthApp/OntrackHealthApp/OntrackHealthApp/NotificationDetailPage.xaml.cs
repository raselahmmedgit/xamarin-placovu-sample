using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationDetailPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly NotificationDetailPageViewModel viewModel;

        public NotificationDetailPage(PatientNotificationShortView patientNotificationShortView)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                NotificationTitle.Text = patientNotificationShortView.NotificationTitle;
                viewModel = new NotificationDetailPageViewModel();
                viewModel.NotificationTitle = patientNotificationShortView.NotificationTitle;
                viewModel.NotificationDetailId = patientNotificationShortView.PatientNotificationDetailId;
                BindingContext = viewModel;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }

        }

        //private void BindForm(PSProcedureNotificationDetail pSProcedureNotificationDetail)
        //{
        //    var data = GetNotificationDetailPageViewModel(pSProcedureNotificationDetail);
        //    data.NotificationDetailHeader = "" + data.NotificationDetailHeader;
        //    BindingContext = data;
        //}

        //private NotificationDetailPageViewModel GetNotificationDetailPageViewModel(PSProcedureNotificationDetail pSProcedureNotificationDetail)
        //{
        //    NotificationDetailPageViewModel notificationDetailPageViewModel = new NotificationDetailPageViewModel();
        //    notificationDetailPageViewModel.NotificationDetailHeader = pSProcedureNotificationDetail.NotificationDetailHeader;
        //    notificationDetailPageViewModel.NotificationDetail = pSProcedureNotificationDetail.NotificationDetail;
        //    return notificationDetailPageViewModel;
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            viewModel.LoadPatientNotificationDetailCommand.Execute(null);
        }

        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //
            }
        }

    }
}