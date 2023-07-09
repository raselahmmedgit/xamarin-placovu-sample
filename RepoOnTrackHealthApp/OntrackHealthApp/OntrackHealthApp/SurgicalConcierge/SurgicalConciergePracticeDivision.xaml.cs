using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePracticeDivision : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private List<SurgicalConceirgePracticeDivision> PracticeDivisionList { get; set; }
        private int PracticeDivisionListCount { get; set; }
        public SurgicalConciergePracticeDivision()
        {
            //DependencyService.Get<IAppPermissionChecker>().CheakPowerSaverPermission();
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }
        }
        public SurgicalConciergePracticeDivision(List<SurgicalConceirgePracticeDivision> practiceDivisionList)
        {
            //DependencyService.Get<IAppPermissionChecker>().CheakPowerSaverPermission();
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            //Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }

            PracticeDivisionList = practiceDivisionList;
        }

        public async Task LoadPageLayout()
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                App.HideUserDialogAsync();
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            PracticeDivisionList = PracticeDivisionList ?? await TempDataContainer.GetPracticeDivisionFromJsonAsync();
            PracticeDivisionListCount = PracticeDivisionList.Count();
            BuildPageLayout();
        }
        public void BuildPageLayout()
        {

            if (PracticeDivisionList != null)
            {

                List<SurgicalConceirgePracticeDivision> row;
                StackLayout divisionUnitContainer = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.AliceBlue
                };

                for (int i = 0; i < PracticeDivisionListCount - 1;)
                {
                    int divisionIteration = 0;

                    row = GetPracticeDivisionRows(i, 1);
                    i = i + 2;

                    Grid divisionContainerGrid = new Grid
                    {
                        ColumnSpacing = 20,
                        RowSpacing = 30,
                        Margin = new Thickness(12, 15, 12, 10)
                    };

                    divisionContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });

                    foreach (var division in row)
                    {
                        divisionContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        var boxView = GetBoxFrame(division.DivisionName, PracticeDivision.GetPracticeDivisionImage(division.DivisionId), division.DivisionId);
                        divisionContainerGrid.Children.Add(boxView, divisionIteration, 0);
                        divisionIteration++;
                    }

                    divisionUnitContainer.Children.Add(divisionContainerGrid);
                }
                practiceDivisionStackLayout.Children.Clear();
                practiceDivisionStackLayout.Children.Add(divisionUnitContainer);

                row = GetPracticeDivisionRows(0, 2);
                Grid divisionContainerGridLast = new Grid
                {
                    ColumnSpacing = 20,
                    RowSpacing = 30,
                    Margin = new Thickness(12, 15, 12, 10)
                };
                divisionContainerGridLast.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });

                foreach (var division in row)
                {
                    divisionContainerGridLast.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    var boxView = GetBoxFrame(division.DivisionName, PracticeDivision.GetPracticeDivisionImage(division.DivisionId), division.DivisionId, 180);
                    divisionContainerGridLast.Children.Add(boxView, 0, 0);
                }
                practiceDivisionStackLayoutLast.Children.Clear();
                practiceDivisionStackLayoutLast.Children.Add(divisionContainerGridLast);
            }
        }
        //public void BuildPageLayout(List<SurgicalConceirgePracticeDivision> practiceDivisionList)
        //{
        //    if (practiceDivisionList != null)
        //    {
        //        //var surgicalConciergeTrackerPracticeDivisionId = (int)PracticeDivisionId.SurgicalConciergeTracker;
        //        //var patientReportedOutcomePracticeDivisionId = (int)PracticeDivisionId.PatientReportedOutcome;
        //        //var surgicalConciergeTrackerRemove = practiceDivisionList.FirstOrDefault(x => x.DivisionId == surgicalConciergeTrackerPracticeDivisionId);
        //        //var patientReportedOutcomeRemove = practiceDivisionList.FirstOrDefault(x => x.DivisionId == patientReportedOutcomePracticeDivisionId);
        //        //practiceDivisionList.Remove(surgicalConciergeTrackerRemove);
        //        //practiceDivisionList.Remove(patientReportedOutcomeRemove);

        //        StackLayout divisionUnitContainer = new StackLayout
        //        {
        //            Orientation = StackOrientation.Vertical,
        //            Padding = 10
        //        };

        //        Grid divisionContainerGrid = new Grid
        //        {
        //            ColumnSpacing = 0,
        //            RowSpacing = 30,
        //            Margin = new Thickness(20, 0, 20, 0)
        //        };

        //        divisionContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
        //        divisionContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        //        var divisionIteration = 0;

        //        foreach (var division in practiceDivisionList.OrderBy(x => x.DisplayOrder).ToList())
        //        {
        //            divisionContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });

        //            BoxView boxViewColor = new BoxView
        //            {
        //                Color = Color.FromHex(division.DivisionBgColor),
        //            };

        //            divisionContainerGrid.Children.Add(boxViewColor, 0, divisionIteration);
        //            Grid.SetColumnSpan(divisionContainerGrid, 2);

        //            Image divisionLogo = new Image
        //            {
        //                Source = PracticeDivision.GetPracticeDivisionImage(division.DivisionId),
        //                Aspect = Aspect.AspectFit,
        //                WidthRequest = 30,
        //                HeightRequest = 30,
        //                BackgroundColor = Color.FromHex(PracticeDivision.GetPracticeDivisionImageBackgroundColor(division.DivisionId))
        //            };

        //            var divisionGestureRecognizer = new TapGestureRecognizer();

        //            if (division.DivisionId == (int)PracticeDivisionId.Urology)
        //            {
        //                divisionGestureRecognizer.Tapped += async (sender, args) =>
        //                {
        //                    await NavigationToNextpage(sender, division.DivisionId);
        //                };
        //            }
        //            else if (division.DivisionId == (int)PracticeDivisionId.PatientInfo)
        //            {
        //                divisionGestureRecognizer.Tapped += async (sender, args) =>
        //                {
        //                    await NavigationToNextpage(sender, division.DivisionId);
        //                };
        //            }
        //            else if (division.DivisionId == (int)PracticeDivisionId.Gynaecology) //Gynaecology
        //            {
        //                divisionGestureRecognizer.Tapped += async (sender, args) =>
        //                {
        //                    await NavigationToNextpage(sender, division.DivisionId);
        //                };
        //            }
        //            else if (division.DivisionId == (int)PracticeDivisionId.PatientRegistration) //PatientRegistration
        //            {
        //                divisionGestureRecognizer.Tapped += async (sender, args) =>
        //                {
        //                    await NavigationToNextpage(sender, division.DivisionId);
        //                };
        //            }
        //            else if (division.DivisionId == (int)PracticeDivisionId.GeneralSurgery) //GeneralSurgery
        //            {
        //                divisionGestureRecognizer.Tapped += async (sender, args) =>
        //                {
        //                    await NavigationToNextpage(sender, division.DivisionId);
        //                };
        //            }


        //            divisionGestureRecognizer.CommandParameter = division.DivisionId;
        //            divisionLogo.GestureRecognizers.Add(divisionGestureRecognizer);

        //            divisionContainerGrid.Children.Add(divisionLogo, 0, divisionIteration);

        //            StackLayout labelStackLayout = new StackLayout
        //            {
        //                BackgroundColor = Color.FromHex(division.DivisionBgColor),
        //                Padding = new Thickness(10, 0, 10, 0)

        //            };
        //            Label divisionName = new Label
        //            {
        //                Text = division.DivisionName,
        //                TextColor = Color.FromHex("#FFF"),
        //                HorizontalTextAlignment = TextAlignment.Center,
        //                FontSize = 20,
        //                Margin = new Thickness(0, 10, 10, 0),

        //            };

        //            Label divisionDesc = new Label
        //            {
        //                Text = division.DivisionDescription,
        //                TextColor = Color.FromHex("#FFF"),
        //                HorizontalTextAlignment = TextAlignment.Center,
        //                FontSize = 12,
        //                Margin = new Thickness(0, 5, 10, 0),
        //            };
        //            labelStackLayout.GestureRecognizers.Add(divisionGestureRecognizer);
        //            labelStackLayout.Children.Add(divisionName);
        //            labelStackLayout.Children.Add(divisionDesc);

        //            divisionContainerGrid.Children.Add(labelStackLayout, 1, divisionIteration);

        //            divisionIteration++;
        //        }
        //        practiceDivisionStackLayout.Children.Clear();
        //        practiceDivisionStackLayout.Children.Add(divisionContainerGrid);


        //        //    foreach (var division in practiceDivisionList.OrderBy(x => x.DisplayOrder).ToList())
        //        //    {
        //        //        Button divisionUnitButton = new Button
        //        //        {
        //        //            HeightRequest = 70,
        //        //            CornerRadius = 35,
        //        //            TextColor = Color.FromHex("#FFF"),
        //        //            FontSize = 24,
        //        //            Text = division.DivisionName,
        //        //            Margin = new Thickness(0, 10),
        //        //            BackgroundColor = Color.FromHex(division.DivisionBgColor)
        //        //        };

        //        //        if (division.DivisionId == (int)PracticeDivisionId.Urology)
        //        //        {
        //        //            divisionUnitButton.Clicked += async (sender, args) =>
        //        //            {
        //        //                await NavigationToNextpage(divisionUnitButton, division.DivisionId);
        //        //            };
        //        //        }
        //        //        else if (division.DivisionId == (int)PracticeDivisionId.PatientInfo)
        //        //        {
        //        //            divisionUnitButton.Clicked += async (sender, args) =>
        //        //            {
        //        //                await NavigationToNextpage(divisionUnitButton, division.DivisionId);
        //        //            };
        //        //        }
        //        //        else if (division.DivisionId == (int)PracticeDivisionId.Gynaecology) //Gynaecology
        //        //        {
        //        //            divisionUnitButton.Clicked += async (sender, args) =>
        //        //            {
        //        //                await NavigationToNextpage(divisionUnitButton, division.DivisionId);
        //        //            };
        //        //        }
        //        //        else if (division.DivisionId == (int)PracticeDivisionId.PatientRegistration) //PatientRegistration
        //        //        {
        //        //            divisionUnitButton.Clicked += async (sender, args) =>
        //        //            {
        //        //                await NavigationToNextpage(divisionUnitButton, division.DivisionId);
        //        //            };
        //        //        }

        //        //        divisionUnitContainer.Children.Add(divisionUnitButton);
        //        //    }
        //        //    practiceDivisionStackLayout.Children.Clear();
        //        //    practiceDivisionStackLayout.Children.Add(divisionUnitContainer);
        //        //}
        //        App.HideUserDialogAsync();
        //    }
        //}

        public async Task NavigationToNextpage(object sender, long divisionId)
        {
            // button.IsEnabled = false;
            App.ShowUserDialogAsync();
            await Task.Delay(100);
            if (divisionId == (int)PracticeDivisionId.Urology)
            {
                await Navigation.PushAsync(new SurgicalConciergePracticeDivisionUnit(divisionId));
            }
            else if (divisionId == (int)PracticeDivisionId.PatientInfo)
            {
                await Navigation.PushAsync(new NursePatientInfoPatientViewPageNew(divisionId));
            }
            else if (divisionId == (int)PracticeDivisionId.Gynaecology) //Gynaecology
            {
                await Navigation.PushAsync(new SurgicalConciergePracticeDivisionUnit(divisionId));
            }
            else if (divisionId == (int)PracticeDivisionId.PatientRegistration) //PatientRegistration
            {
                await Navigation.PushAsync(new SurgicalConciergePatientView(divisionId, 0));
            }
            else if (divisionId == (int)PracticeDivisionId.GeneralSurgery)
            {
                await Navigation.PushAsync(new SurgicalConciergePracticeDivisionUnit(divisionId));
            }
            else if (divisionId == (int)PracticeDivisionId.ColoRectal)
            {
                await Navigation.PushAsync(new SurgicalConciergePracticeDivisionUnit(divisionId));
            }
        }

        private Frame GetBoxFrame(string label, string imgSrc, long divisionId, double? width = null)
        {
            Frame boxView;
            Frame boxViewInner;
            boxView = new Frame
            {
                BorderColor = Color.FromHex("#0000FF"),
                CornerRadius = 10,
                BackgroundColor = Color.FromHex("#0000FF"),
                HorizontalOptions = width == null ? LayoutOptions.FillAndExpand : LayoutOptions.Center,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 3
            };
            if (width != null)
            {
                boxView.WidthRequest = width.GetValueOrDefault();
            }
            boxViewInner = new Frame
            {
                CornerRadius = 8,
                BackgroundColor = Color.FromHex("#FFFFFF"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Grid innerGrid = new Grid
            {
                RowSpacing = 30,
            };
            innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });

            Image divisionLogo = new Image
            {
                Source = imgSrc,
                Aspect = Aspect.AspectFit,
                WidthRequest = 30,
                HeightRequest = 30
            };
            innerGrid.Children.Add(divisionLogo, 0, 0);
            Label divisionName = new Label
            {
                Text = label.ToUpper(),
                TextColor = Color.FromHex("#0000FF"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 13,
                Margin = new Thickness(0, 0, 0, 0),
            };
            innerGrid.Children.Add(divisionName, 0, 1);
            boxViewInner.Content = innerGrid;
            boxView.Content = boxViewInner;
            var divisionGestureRecognizer = new TapGestureRecognizer();
            divisionGestureRecognizer.Tapped += async (sender, args) =>
            {
                await NavigationToNextpage(sender, divisionId);
            };
            divisionGestureRecognizer.CommandParameter = divisionId;
            boxView.GestureRecognizers.Add(divisionGestureRecognizer);
            return boxView;
        }
        //public async Task NavigationToNextpage(Button button, long divisionId)
        //{
        //    button.IsEnabled = false;
        //    App.ShowUserDialogAsync();
        //    await Task.Delay(100);
        //    if (divisionId == (int)PracticeDivisionId.Urology)
        //    {               
        //        await Navigation.PushAsync(new SurgicalConciergePracticeDivisionUnit(divisionId));
        //    }
        //    else if (divisionId == (int)PracticeDivisionId.PatientInfo)
        //    {
        //        await Navigation.PushAsync(new NursePatientInfoPatientViewPageNew(divisionId));
        //    }
        //    else if (divisionId == (int)PracticeDivisionId.Gynaecology) //Gynaecology
        //    {
        //        await Navigation.PushAsync(new SurgicalConciergePracticeDivisionUnit(divisionId));
        //    }
        //    else if (divisionId == (int)PracticeDivisionId.PatientRegistration) //PatientRegistration
        //    {
        //        await Navigation.PushAsync(new SurgicalConciergePatientView(divisionId, 0));
        //    }
        //}

        private List<SurgicalConceirgePracticeDivision> GetPracticeDivisionRows(int skip, int group)
        {
            if (group == 1)
            {
                return PracticeDivisionList.Where(m => !AppConstants.DivisionExcludedFromMobileViewAdmin.Contains(m.DivisionId)).OrderBy(x => x.DisplayOrder).Skip(skip).Take(2).ToList();
            }
            else
            {
                return PracticeDivisionList.Where(m => AppConstants.DivisionExcludedFromMobileViewAdmin.Contains(m.DivisionId)).OrderBy(x => x.DisplayOrder).Skip(skip).Take(2).ToList();
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPageLayout();
            App.HideUserDialogAsync();
        }
    }
}