﻿using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProgramNotificationDetailPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private readonly NotificationDetailPageViewModel viewModel;

        public ProfessionalProgramNotificationDetailPage(PatientNotificationShortView patientNotificationShortView)
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