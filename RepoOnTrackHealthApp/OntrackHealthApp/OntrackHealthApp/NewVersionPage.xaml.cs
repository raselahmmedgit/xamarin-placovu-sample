using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.iOS;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewVersionPage : ContentPage
    {
        private ILatestVersion _ILatestVersion;
        public static ITokenContainer _iTokenContainer;

        public NewVersionPage()
        {
            InitializeComponent();
            _ILatestVersion = new LatestVersion();
            //_ILatestVersion = DependencyService.Get<ILatestVersion>();
            _iTokenContainer = new TokenContainer();
            CurrentVersion.Text = "Current Version  : " + _ILatestVersion.InstalledVersionNumber;
            ReleasedVersion.Text = "Released Version: " + _iTokenContainer.AndroidVersionCode;
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                await _ILatestVersion.OpenAppInStore();
            }
            catch { }
        }
    }
}