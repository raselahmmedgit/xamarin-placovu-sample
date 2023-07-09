using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ScgPathology;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPracticePage : MasterDetailPage
    {
        private List<MenuViewModel> MenuViewModelList { get; set; }
        private readonly ITokenContainer _iTokenContainer;

        public MenuPracticePage ()
		{
            _iTokenContainer = new TokenContainer();

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

            MenuViewModelList = new List<MenuViewModel>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var pageHome = new MenuViewModel() { Title = "Home", Icon = "home_nav_icon_1.png", TargetType = typeof(MainPracticePage) };
            var pageChangePassword = new MenuViewModel() { Title = "Change Password", Icon = "changepassword_nav_icon.png", TargetType = typeof(AdminChangePassword) };
            var pageSignOut = new MenuViewModel() { Title = "Sign Out", Icon = "signout_nav_icon.png", TargetType = typeof(AdminSignOutPage) };

            // Adding menu items to MenuViewModelList
            MenuViewModelList.Add(pageHome);
            MenuViewModelList.Add(pageChangePassword);
            MenuViewModelList.Add(pageSignOut);

            if (this.navigationPracticeDrawerList != null)
            {
                this.navigationPracticeDrawerList.ItemsSource = MenuViewModelList;
            }

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPracticePage)));

        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuPracticeItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MenuViewModel)e.SelectedItem;
            Type page = item.TargetType;

            string pageName = page?.Name;

            if (pageName == "MainPracticePage")
            {
                OnMainButtonClickedAsync();
            }
            else if (pageName == "AdminChangePassword")
            {
                OnChangePasswordButtonClickedAsync();
            }
            else if (pageName == "AdminSignOutPage")
            {
                OnSignOutButtonClickedAsync();
            }

            //Detail = new NavigationPage((Page)Activator.CreateInstance(page));

            IsPresented = false;
        }

        #region Menu

        private async void OnMainButtonClickedAsync()
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                MainButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void MainButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new MainPracticePage());

                //Navigation.InsertPageBefore(new MainPracticePage(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPracticePage)));
            }
        }

        private async void OnChangePasswordButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                //await Navigation.PushAsync(new AdminChangePassword());

                //Navigation.InsertPageBefore(new AdminChangePassword(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(AdminChangePassword)));
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void OnSignOutButtonClickedAsync()
        {
            if (_iTokenContainer != null)
            {
                _iTokenContainer.ClearApiToken();
            }
            DependencyService.Get<IToast>().SetSettingsForUserLogout();
            //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
            App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
        }

        #endregion


    }
}