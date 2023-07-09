using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ScgPathology;
using OntrackHealthApp.SurgicalConcierge.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomeMenuPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        public HomeMenuPage ()
		{
			InitializeComponent ();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
        }

        private async void BtnSurgicalConcierge_Clicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            try
            {
                BtnSurgicalConcierge.IsEnabled = false;
                App.ShowUserDialogAsync();
                await Navigation.PushAsync(new SurgicalConciergePracticeDivision());
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BtnSurgicalConcierge.IsEnabled = true;
        }
    }
}