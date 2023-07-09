using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
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
    public partial class SurgicalConciergePastWeekDates : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private long PracticeDivisionUnit = 0;

        public SurgicalConciergePastWeekDates(long practiceDivisionUnit)
        {
            DependencyService.Get<IAppPermissionChecker>().CheakPowerSaverPermission();
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            LoadPageLayout();
            PracticeDivisionUnit = practiceDivisionUnit;
        }

        public void LoadPageLayout()
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            BuildPageLayout();

        }

        public void BuildPageLayout()
        {
            DateTime[] last7Days = Enumerable.Range(0, 7).Select(i => DateTime.UtcNow.Date.AddDays(-i)).ToArray();

            StackLayout pastWeekDateButtonContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = 12
            };

            ScrollView pastWeekDateScrollContainer = new ScrollView();
            pastWeekDateScrollContainer.Content = pastWeekDateButtonContainer;

            foreach (var day in last7Days)
            {
                Button pastWeekDateButton = new Button
                {
                    HeightRequest = 60,
                    CornerRadius = 20,
                    BackgroundColor = Color.FromHex(PracticeDivisionBoxColor.Urology),
                    TextColor = Color.FromHex("#FFF"),
                    FontSize = 20,
                    Text = day.ToString("D"),
                    Margin = new Thickness(0, 4)
                };
                pastWeekDateButton.Clicked += async (sender, args) => await PastWeekDateButton_ClickedAsync(sender, args, day);

                pastWeekDateButtonContainer.Children.Add(pastWeekDateButton);
            }

            Button searchDateButton = new Button
            {
                HeightRequest = 60,
                CornerRadius = 20,
                BackgroundColor = Color.FromHex(PracticeDivisionUnitBoxColor.Search),
                TextColor = Color.FromHex("#FFF"),
                FontSize = 20,
                Text = "Search"
            };
            searchDateButton.Clicked += async (sender, args) => await btnSearchDateButton_ClickedAsync(sender, args);

            pastWeekDateButtonContainer.Children.Add(searchDateButton);
            pastWeekDateListStackLayout.Children.Add(pastWeekDateScrollContainer);
            this.Content = pastWeekDateListStackLayout;

        }

        protected async Task PastWeekDateButton_ClickedAsync(object sender, EventArgs e, DateTime day)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    await Navigation.PushAsync(new SurgicalConciergePatientView(PracticeDivisionUnit, day));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        protected async Task btnSearchDateButton_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    await Navigation.PushAsync(new SurgicalConciergePatientView(0, PracticeDivisionUnit));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

    }
}