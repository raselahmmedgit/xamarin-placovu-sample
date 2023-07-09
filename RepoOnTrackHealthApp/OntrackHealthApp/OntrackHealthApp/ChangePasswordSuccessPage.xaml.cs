using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePasswordSuccessPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        public AppMessage AppMessage { get; set; }

        public ChangePasswordSuccessPage (AppMessage appMessage)
		{
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = "Change Password";
                ProcedureName.Text = "Change Password";

                AppMessage = appMessage;
                messageLoginLabel.Text = appMessage.Message;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private void OnLoginPageButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    if (AppMessage.MessageType == AppMessageType.Success)
                    {
                        //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                        App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
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
        
    }
}