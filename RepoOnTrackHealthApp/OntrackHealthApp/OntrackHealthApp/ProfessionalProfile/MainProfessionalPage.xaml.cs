using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.PatientOutComeReport;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using static OntrackHealthApp.AppCore.Enums;

namespace OntrackHealthApp.ProfessionalProfile
{
    public partial class MainProfessionalPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;
        private ProfessionalProfilePageViewModel ProfessionalProfilePageViewModel { get; set; }

        public MainProfessionalPage()
        {
            App.ShowUserDialogAsync();
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                professionalProfileRestApiService = new ProfessionalProfileRestApiService();
                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public MainProfessionalPage(ProfessionalProfilePageViewModel professionalProfilePageViewModel)
        {
            App.ShowUserDialogAsync();
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                professionalProfileRestApiService = new ProfessionalProfileRestApiService();
                ProfessionalProfilePageViewModel = professionalProfilePageViewModel;
                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadDataAsyc()
        {
            try
            {
                IsBusy = true;
                App.ShowUserDialogAsync();
                ProfessionalProfilePageViewModel professionalProfilePageViewModel = new ProfessionalProfilePageViewModel();
                professionalProfilePageViewModel = await professionalProfileRestApiService.GetProfessionalProfile(_iTokenContainer.ApiProfessionalProfileId);
                //ProfessionalProfilePageViewModel professionalProfilePageViewModel = ProfessionalProfilePageViewModel ?? await professionalProfileRestApiService.GetProfessionalProfile(_iTokenContainer.ApiProfessionalProfileId);
                if (professionalProfilePageViewModel != null)
                {
                    ProfessionalProfilePageViewModel = professionalProfilePageViewModel;
                    //ProfessionalProfilePageViewModel.ProfessionalProfileName = $"Welcome {ProfessionalProfilePageViewModel.ProfessionalProfileName}";
                    BindingContext = ProfessionalProfilePageViewModel;
                    var data = professionalProfilePageViewModel.ProfessionalDashboardDivisionViewModels;
                    BuildPageLayout(data.ToList());
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
            finally
            {
                IsBusy = false;
                GridProfile.IsVisible = true;
                OtherLink.IsVisible = true;
                App.HideUserDialogAsync();
            }
        }
        public void BuildPageLayout(List<ProfessionalDashboardDivisionViewModel> practiceDivisionUnitList)
        {
            App.ShowUserDialogAsync();

            Grid divisionUnitContainerGrid = new Grid
            {
                ColumnSpacing = 10,
                RowSpacing = 10,
                Margin = new Thickness(0, 0, 0, 5)
            };
            divisionUnitContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(90) });
            divisionUnitContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            //divisionUnitContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            var divisionUnitIteration = 0;
            StackLayout frameStackLayout = new StackLayout();
            foreach (var division in practiceDivisionUnitList.OrderBy(x => x.DisplayOrder).ToList())
            {
                if (division.DivisionId == (int)ProfessionPracticeDivision.Hospital)
                {
                    foreach (var divisionUnit in division.ProfessionalDashboardDivisionUnits.OrderBy(x => x.DisplayOrder))
                    {
                        divisionUnitContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.25, GridUnitType.Star) });

                        Frame FrameViewColor = new Frame
                        {
                            //BackgroundColor = Color.FromHex(division.GetDivisionUnitStyleBgColor()),
                            WidthRequest = 80,
                            HeightRequest = 80,
                            BorderColor = Color.FromHex("#0096FF"),
                            HasShadow = false,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Fill,
                            Margin = new Thickness(2, 2, 2, 2),
                            //Padding = new Thickness(25),
                            CornerRadius = 25
                        };

                        //Grid.SetColumnSpan(divisionUnitContainerGrid, 2);

                        Image divisionLogo = new Image
                        {
                            Source = divisionUnit.GetDivisionUnitStyleIcon(),
                            Aspect = Aspect.Fill,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Fill,
                            //Margin = new Thickness(0,10),
                            BackgroundColor = Color.FromHex("#FFFFFF")
                        };

                        var divisionUnitGestureRecognizer = new TapGestureRecognizer();
                        divisionUnitGestureRecognizer.Tapped += async (sender, args) =>
                        {
                            await DivisionUnitButton_ClickedAsync(sender, args, divisionUnit.DivisionUnitId);
                        };
                        divisionUnitGestureRecognizer.CommandParameter = division.DivisionId;
                        divisionLogo.GestureRecognizers.Add(divisionUnitGestureRecognizer);

                        FrameViewColor.Content = divisionLogo;
                        divisionUnitContainerGrid.Children.Add(FrameViewColor, divisionUnitIteration, 0);

                        Label divisionUnitName = new Label
                        {
                            Text = divisionUnit.MobileDivisionUnitName,
                            TextColor = Color.FromHex("#000"),
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontSize = 12,
                            Margin = new Thickness(10, 2, 0, 0)
                        };

                        //labelUnitStackLayout.GestureRecognizers.Add(divisionUnitGestureRecognizer);
                        //labelUnitStackLayout.Children.Add(divisionUnitName);

                        divisionUnitContainerGrid.Children.Add(divisionUnitName, divisionUnitIteration, 1);

                        divisionUnitIteration++;
                    }
                }
                practiceDivisionUnitStackLayout.Children.Clear();
                StackLayout hospitalLinksStackLayout = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = 20,
                    HeightRequest = 40
                };
                Label hospitalLinksLabel = new Label()
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "Hospital Links"
                };
                hospitalLinksStackLayout.Children.Add(hospitalLinksLabel);
                practiceDivisionUnitStackLayout.Children.Add(hospitalLinksStackLayout);
                practiceDivisionUnitStackLayout.Children.Add(divisionUnitContainerGrid);
                practiceDivisionUnitStackLayout.Children.Add(divisionUnitContainerGrid);
            }

            App.HideUserDialogAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async Task DivisionUnitButton_ClickedAsync(object sender, EventArgs e, long id)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();
                var practiceDivisionUnitDest = id;

                if ((long)PracticeDivisionUnit.PatientReportedOutcome == practiceDivisionUnitDest)  //PatientReportedOutcome
                {
                    await Navigation.PushAsync(new PatientReportedOutcomePage());
                }
                else if ((long)PracticeDivisionUnit.ComparativeAnalysis == practiceDivisionUnitDest)  //ComparativeAnalysis
                {
                    await Navigation.PushAsync(new ProfessionalComparativeAnalyticsPage());
                }
                else if ((long)PracticeDivisionUnit.NursePatientInfo == practiceDivisionUnitDest)  //NursePatientInfo
                {
                }
                else if ((long)PracticeDivisionUnit.NursingRounds == practiceDivisionUnitDest)  //NursingRounds
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(practiceDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.OperatingRoom == practiceDivisionUnitDest)  //OperatingRoom
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(practiceDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.PACU == practiceDivisionUnitDest)  //PACU
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(practiceDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Floor == practiceDivisionUnitDest)  //Floor
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(practiceDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Pathology == practiceDivisionUnitDest)  //Pathology
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(practiceDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Discharge == practiceDivisionUnitDest)  //Discharge
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(practiceDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.PreSurgerySummary == practiceDivisionUnitDest)  //PreSurgerySummary
                {
                    await Navigation.PushAsync(new ProfessionalPatientPage(practiceDivisionUnitDest));
                }
                else if ((long)PracticeDivisionUnit.Programs == practiceDivisionUnitDest)  //Programs
                {
                    await Navigation.PushAsync(new ProfessionalProgramPage());
                }
                else if ((long)PracticeDivisionUnit.Billing == practiceDivisionUnitDest)
                {
                    await Navigation.PushAsync(new ProfessionalBillingPage());
                }
                else if ((long)PracticeDivisionUnit.GenericSurvey == practiceDivisionUnitDest)  //GenericSurvey
                {
                    await Navigation.PushAsync(new ProfessionalGenericSurveyPage());
                }
                else
                {
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        //private async Task DivisionUnitButton_ClickedAsync(object sender, EventArgs e, ProfessionalPageOutComePageViewModel model)
        //{
        //    if (InternetConnectHelper.DoIHaveInternet())
        //    {
        //        await Navigation.PushAsync(new ProfessionalDivisionUnitPage(model));
        //    }
        //    else
        //    {
        //        await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
        //    }
        //}

        private async void OnPatientReportedOutcomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await Navigation.PushAsync(new PatientReportedOutcomePage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void BtnOutComeReport_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    App.ShowUserDialogAsync();
                    //await Navigation.PushAsync(new PatientOutComeChartPage());
                    App.HideUserDialogAsync();
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

        private async void BtnOutComeGraph_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    App.ShowUserDialogAsync();
                    //await Navigation.PushAsync(new PatientOutComeGraphPage());
                    App.HideUserDialogAsync();
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

        private async void BtnDivisionUnit_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    long divisionId = 0;
                    if (sender.GetType() == typeof(Label))
                    {
                        divisionId = (sender as Label).ClassId.ToLong();
                    }
                    else if (sender.GetType() == typeof(Image))
                    {
                        divisionId = (sender as Image).ClassId.ToLong();
                    }

                    var item = ProfessionalProfilePageViewModel.ProfessionalDashboardDivisionViewModels.ToList()
                        .Where(m => m.DivisionId == divisionId).FirstOrDefault();
                    var item2 = new List<Model.ProfessionalDashboardDivisionUnit>();
                    foreach (var unit in item.ProfessionalDashboardDivisionUnits)
                    {
                        var unitModel = new Model.ProfessionalDashboardDivisionUnit
                        {
                            DivisionUnitId = unit.DivisionUnitId,
                            DivisionUnitName = unit.DivisionUnitName,
                            DivisionId = unit.DivisionId,
                            IsActive = unit.IsActive,
                            DisplayOrder = unit.DisplayOrder,
                            DivisionUnitStyle = unit.DivisionUnitStyle,
                            DivisionUnitBgColor = unit.DivisionUnitBgColor,
                            DivisionUnitDescription = unit.DivisionUnitDescription,
                            MobileDivisionUnitDescription = unit.MobileDivisionUnitDescription
                        };
                        item2.Add(unitModel);
                    }
                    ProfessionalPageOutComePageViewModel model = new ProfessionalPageOutComePageViewModel()
                    {
                        ProfessionalProfileName = ProfessionalProfilePageViewModel.ProfessionalProfileName,
                        ProfessionalDivisionName = item.DivisionName,
                        ProfessionalDivisionId = item.DivisionId,
                        ProfessionalDashboardDivisionUnits = item2
                    };
                    App.ShowUserDialogAsync();
                    await Navigation.PushAsync(new ProfessionalDivisionUnitPageNew(model));
                    App.HideUserDialogAsync();
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
    }
}