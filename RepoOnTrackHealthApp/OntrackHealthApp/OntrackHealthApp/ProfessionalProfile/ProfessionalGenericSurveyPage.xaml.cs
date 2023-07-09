using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalGenericSurveyPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        public ProfessionalGenericSurveyPage()
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            App.HideUserDialogAsync();
        }

        private async void BtnHome_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                App.ShowUserDialogAsync();
                App.Instance.MainPage = new MenuProfessionalPage();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
    }
}