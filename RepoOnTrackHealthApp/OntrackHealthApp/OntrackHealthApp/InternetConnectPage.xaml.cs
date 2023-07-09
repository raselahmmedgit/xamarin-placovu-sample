using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InternetConnectPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;

        public InternetConnectPage ()
		{
			InitializeComponent ();
            _iTokenContainer = new TokenContainer();
        }

        private  void OnRefreshButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    //Navigation.InsertPageBefore(new MainPage() { Title = _iTokenContainer.ApiPracticeName }, this);
                    RoleWisePageRoutingOnNetAvailable();
                    //await Navigation.PopAsync();
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void RoleWisePageRoutingOnNetAvailable()
        {
            if (!_iTokenContainer.IsApiToken())
            {
                //Navigation.InsertPageBefore(new LoginPageNew(), this);
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            else
            {
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.PracticeAdmin))
                {
                    //Navigation.InsertPageBefore(new MenuPracticePage(), this);
                    _iTokenContainer.ApiIsAdmin = true;
                    //Navigation.InsertPageBefore(new MenuPracticePage(), this);
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomPersonnel))
                {
                    _iTokenContainer.ApiIsAdmin = true;
                    //Navigation.InsertPageBefore(new SurgicalConciergePracticeDivisionUnit(), this);
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomNurse))
                {
                    _iTokenContainer.ApiIsAdmin = true;
                    //Navigation.InsertPageBefore(new SurgicalConciergePracticeDivision(), this);
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.OperatingRoomMD))
                {
                    _iTokenContainer.ApiIsAdmin = true;
                    //Navigation.InsertPageBefore(new SurgicalConciergePracticeDivision(), this);
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Professional))
                {
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
                else if (userIdentityModel != null && userIdentityModel.RoleId != null && userIdentityModel.RoleId.Equals(RoleIdConstants.Patient))
                {
                    //Navigation.InsertPageBefore(new MenuPatientPage(), this);
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
                else
                {
                    //Navigation.InsertPageBefore(new MenuPatientPage(), this);
                    App.Instance.MainPage = new MenuPage(userIdentityModel);
                }
            }
            
        }
        
    }
}