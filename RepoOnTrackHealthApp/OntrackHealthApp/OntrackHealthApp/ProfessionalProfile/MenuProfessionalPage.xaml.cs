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

namespace OntrackHealthApp.ProfessionalProfile
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuProfessionalPage : MasterDetailPage
    {
        private List<MenuViewModel> MenuViewModelList { get; set; }
        private readonly ITokenContainer _iTokenContainer;

        public MenuProfessionalPage()
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

            MenuViewModelList = new List<MenuViewModel>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var pageHome = new MenuViewModel() { Title = "Home", Icon = "home_nav_icon_1.png", TargetType = typeof(MainProfessionalPage) };
            var pageChangePassword = new MenuViewModel() { Title = "Change Password", Icon = "changepassword_nav_icon.png", TargetType = typeof(ProfessionalChangePassword) };
            var pageSignOut = new MenuViewModel() { Title = "Sign Out", Icon = "signout_nav_icon.png", TargetType = typeof(ProfessionalSignOutPage) };

            // Adding menu items to MenuViewModelList
            MenuViewModelList.Add(pageHome);
            MenuViewModelList.Add(pageChangePassword);
            MenuViewModelList.Add(pageSignOut);

            if (this.navigationProfessionalDrawerList != null)
            {
                this.navigationProfessionalDrawerList.ItemsSource = MenuViewModelList;
            }

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainProfessionalPage)));
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuProfessionalItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MenuViewModel)e.SelectedItem;
            Type page = item.TargetType;

            string pageName = page?.Name;

            if (pageName == "MainProfessionalPage")
            {
                OnMainButtonClickedAsync();
            }
            else if (pageName == "ProfessionalChangePassword")
            {
                OnChangePasswordButtonClickedAsync();
            }
            else if (pageName == "ProfessionalSignOutPage")
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
                //await Navigation.PushAsync(new MainProfessionalPage());

                //Navigation.InsertPageBefore(new MainProfessionalPage(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainProfessionalPage)));
            }
        }

        private async void OnChangePasswordButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                //await Navigation.PushAsync(new ProfessionalChangePassword());

                //Navigation.InsertPageBefore(new ProfessionalChangePassword(), this);
                //await Navigation.PopToRootAsync();

                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProfessionalChangePassword)));
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        #endregion
    }
}