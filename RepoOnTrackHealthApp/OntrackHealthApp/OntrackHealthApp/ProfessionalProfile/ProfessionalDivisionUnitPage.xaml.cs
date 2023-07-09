using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.PatientOutComeReport;
using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessionalDivisionUnitPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;

        private long PracticeDivisionDest = 0;
        private long PracticeDivisionUnitDest = 0;

        private ProfessionalPageOutComePageViewModel ProfessionalPageOutComePageViewModel { get; set; }

        public ProfessionalDivisionUnitPage()
        {
            InitializeComponent();
        }

        public ProfessionalDivisionUnitPage(ProfessionalPageOutComePageViewModel model)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            ProfessionalPageOutComePageViewModel = model;
            BindingContext = ProfessionalPageOutComePageViewModel;
        }

        public void BuildPageLayout(List<ProfessionalDashboardDivisionUnit> practiceDivisionUnitList)
        {
            try
            {
                App.ShowUserDialog();

                Grid divisionUnitContainerGrid = new Grid
                {
                    ColumnSpacing = 0,
                    RowSpacing = 30,
                    Margin = new Thickness(10, 10, 10, 10)
                };

                divisionUnitContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                divisionUnitContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                var divisionUnitIteration = 0;

                foreach (var divisionUnit in practiceDivisionUnitList.OrderBy(x => x.DisplayOrder).ToList())
                {
                    divisionUnitContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });

                    BoxView boxViewColor = new BoxView
                    {
                        Color = Color.FromHex(divisionUnit.DivisionUnitStyleIconBgColor),
                    };

                    divisionUnitContainerGrid.Children.Add(boxViewColor, 0, divisionUnitIteration);
                    Grid.SetColumnSpan(divisionUnitContainerGrid, 2);

                    Image divisionLogo = new Image
                    {
                        Source = "professionaldashboard/" + divisionUnit.DivisionUnitStyleIcon,
                        Aspect = Aspect.AspectFit,
                        WidthRequest = 30,
                        HeightRequest = 30
                    };

                    var divisionUnitGestureRecognizer = new TapGestureRecognizer();
                    divisionUnitGestureRecognizer.Tapped += async (sender, args) =>
                    {
                        await DivisionUnitButton_ClickedAsync(sender, args, divisionUnit.DivisionUnitId);
                    };
                    divisionUnitGestureRecognizer.CommandParameter = divisionUnit.DivisionId;
                    divisionLogo.GestureRecognizers.Add(divisionUnitGestureRecognizer);

                    divisionUnitContainerGrid.Children.Add(divisionLogo, 0, divisionUnitIteration);

                    StackLayout labelUnitStackLayout = new StackLayout
                    {
                        BackgroundColor = Color.FromHex(divisionUnit.DivisionUnitBgColor),
                        Padding = new Thickness(10, 0, 10, 0),
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                    Label divisionUnitName = new Label
                    {
                        Text = divisionUnit.DivisionUnitName,
                        TextColor = Color.FromHex("#FFF"),
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,                       
                        FontSize = 18,
                        Margin = new Thickness(10, 15, 0, 5)
                    };

                    Label divisionUnitDesc = new Label
                    {
                        Text = divisionUnit.DivisionUnitDescription,
                        TextColor = Color.FromHex("#FFF"),
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 12,
                        Margin = new Thickness(0, 0, 0, 0)
                    };

                    labelUnitStackLayout.GestureRecognizers.Add(divisionUnitGestureRecognizer);
                    labelUnitStackLayout.Children.Add(divisionUnitName);
                    labelUnitStackLayout.Children.Add(divisionUnitDesc);

                    divisionUnitContainerGrid.Children.Add(labelUnitStackLayout, 1, divisionUnitIteration);

                    divisionUnitIteration++;
                }
                practiceDivisionUnitStackLayout.Children.Clear();
                practiceDivisionUnitStackLayout.Children.Add(divisionUnitContainerGrid);
            }
            catch (Exception)
            {
                App.HideUserDialog();
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }
        }

        private async Task DivisionUnitButton_ClickedAsync(object sender, EventArgs e, long id)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();
                PracticeDivisionUnitDest = id;

                if ((long)PracticeDivisionUnit.PatientReportedOutcome == PracticeDivisionUnitDest)  //PatientReportedOutcome
                {
                    await Navigation.PushAsync(new PatientReportedOutcomePage());
                }
                else if ((long)PracticeDivisionUnit.ComparativeAnalysis == PracticeDivisionUnitDest)  //ComparativeAnalysis
                {
                    await Navigation.PushAsync(new ProfessionalComparativeAnalyticsPage());
                }
                else if ((long)PracticeDivisionUnit.NursePatientInfo == PracticeDivisionUnitDest)  //NursePatientInfo
                {
                }
                else if ((long)PracticeDivisionUnit.NursingRounds == PracticeDivisionUnitDest)  //NursingRounds
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(PracticeDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.OperatingRoom == PracticeDivisionUnitDest)  //OperatingRoom
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(PracticeDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.PACU == PracticeDivisionUnitDest)  //PACU
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(PracticeDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Floor == PracticeDivisionUnitDest)  //Floor
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(PracticeDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Pathology == PracticeDivisionUnitDest)  //Pathology
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(PracticeDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Discharge == PracticeDivisionUnitDest)  //Discharge
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(PracticeDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.PreSurgerySummary == PracticeDivisionUnitDest)  //PreSurgerySummary
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(PracticeDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Programs == PracticeDivisionUnitDest)  //Programs
                {
                    await Navigation.PushAsync(new ProfessionalProgramPage());
                }
                else if ((long)PracticeDivisionUnit.Billing == PracticeDivisionUnitDest)
                {
                    await Navigation.PushAsync(new ProfessionalBillingPage());
                }
                else if ((long)PracticeDivisionUnit.GenericSurvey == PracticeDivisionUnitDest)  //GenericSurvey
                {
                    await Navigation.PushAsync(new ProfessionalGenericSurveyPage());
                }
                else {
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            Title = _iTokenContainer.ApiPracticeName;
            BuildPageLayout(ProfessionalPageOutComePageViewModel.ProfessionalDashboardDivisionUnits);
        }
    }
}