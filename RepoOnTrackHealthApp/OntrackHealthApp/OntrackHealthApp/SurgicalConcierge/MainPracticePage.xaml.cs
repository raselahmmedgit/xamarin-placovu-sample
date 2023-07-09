
using Plugin.Toasts;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Interfaces;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using Xamarin.Forms;
using OntrackHealthApp.ScgPathology;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using Acr.UserDialogs;
using OntrackHealthApp.PatientOutComeReport;

namespace OntrackHealthApp.SurgicalConcierge
{
    public partial class MainPracticePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;

        public MainPracticePage()
        {
            //DependencyService.Get<IAppPermissionChecker>().CheakPowerSaverPermission();
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }
        }

        private async void BtnSurgicalConcierge_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    App.ShowUserDialogAsync();
                    await Navigation.PushAsync(new SurgicalConciergePracticeDivision());
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }
        //private async void BtnOutComeReport_Clicked(object sender, EventArgs e)
        //{
        //    if (InternetConnectHelper.DoIHaveInternet())
        //    {
        //        try
        //        {
        //            App.ShowUserDialogAsync();
        //            //await Navigation.PushAsync(new PatientOutComeChartPage());
        //            App.HideUserDialogAsync();
        //        }
        //        catch (Exception)
        //        {
        //            await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
        //        }
        //    }
        //    else
        //    {
        //        await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
        //    }

        //}
        

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
