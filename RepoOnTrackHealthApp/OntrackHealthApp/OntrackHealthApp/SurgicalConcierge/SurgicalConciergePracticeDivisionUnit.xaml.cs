using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePracticeDivisionUnit : ContentPage{
        public long PracticeDivisionDest = 0;
        private readonly ITokenContainer _iTokenContainer;
        private List<SurgicalConceirgePracticeDivisionUnit> PracticeDivisionUnitList { get; set; }
        private int PracticeDivisionUnitListCount { get; set; }

        public SurgicalConciergePracticeDivisionUnit()
        {
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
            int practiceDivisionId = (int)PracticeDivisionId.Urology;
            PracticeDivisionDest = practiceDivisionId;
            LoadPageLayout();
        }
        public SurgicalConciergePracticeDivisionUnit(List<SurgicalConceirgePracticeDivisionUnit> practiceDivisionUnitList)
        {
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
            PracticeDivisionUnitList = practiceDivisionUnitList;
            LoadPageLayout();
        }

        public SurgicalConciergePracticeDivisionUnit(long practiceDivisionId)
        {
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
            PracticeDivisionDest = practiceDivisionId;
            LoadPageLayout();
        }

        public async void LoadPageLayout()
        {
            try
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    App.HideUserDialogAsync();
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }
                PracticeDivisionUnitList = PracticeDivisionUnitList ?? await TempDataContainer.GetPracticeDivisionUnitFromJsonAsync(PracticeDivisionDest);
                PracticeDivisionUnitListCount = PracticeDivisionUnitList.Count();
                BuildPageLayout();
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public void BuildPageLayout()
        {
            if (PracticeDivisionUnitList != null)
            {
                List<SurgicalConceirgePracticeDivisionUnit> row;
                StackLayout divisionUnitContainer = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.AliceBlue
                };

                for (int i = 0; i < PracticeDivisionUnitListCount;)
                {
                    int divisionIteration = 0;

                    row = PracticeDivisionUnitList.OrderBy(x => x.DisplayOrder).Skip(i).Take(2).ToList();
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
                        if (row.Count == 1)
                        {
                            divisionContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            var boxView = GetBoxFrame(division.DivisionUnitName2, PracticeDivision.getPracticeDivisionUnitImage(division.DivisionUnitId), division.DivisionUnitId);
                            divisionContainerGrid.Children.Add(boxView, 0, 0);
                            var emptyBoxView = GetEmptyBoxFrame();
                            divisionContainerGrid.Children.Add(emptyBoxView, 1, 0);
                        }
                        else
                        {
                            divisionContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            var boxView = GetBoxFrame(division.DivisionUnitName2, PracticeDivision.getPracticeDivisionUnitImage(division.DivisionUnitId), division.DivisionUnitId);
                            divisionContainerGrid.Children.Add(boxView, divisionIteration, 0);
                        }
                        divisionIteration++;
                    }

                    divisionUnitContainer.Children.Add(divisionContainerGrid);
                }
                PracticeDivisionUnitStackLayout.Children.Clear();
                PracticeDivisionUnitStackLayout.Children.Add(divisionUnitContainer);


            }
            App.HideUserDialog();
        }
        private Frame GetBoxFrame(string label, string imgSrc, long divisionUnitId, double? width = null)
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
                await NavigationToNextPage(sender, divisionUnitId);
            };
            divisionGestureRecognizer.CommandParameter = divisionUnitId;
            boxView.GestureRecognizers.Add(divisionGestureRecognizer);
            return boxView;
        }


        private Frame GetEmptyBoxFrame(double? width = null)
        {
            Frame boxView;
            Frame boxViewInner;
            boxView = new Frame
            {
                Opacity = 0,
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
                Opacity = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Grid innerGrid = new Grid
            {
                RowSpacing = 30,
            };
            innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            Label divisionName = new Label
            {
                Text = string.Empty,
                Opacity = 0,
                Margin = new Thickness(0, 0, 0, 0),
            };
            innerGrid.Children.Add(divisionName, 0, 1);
            boxViewInner.Content = innerGrid;
            boxView.Content = boxViewInner;
            return boxView;
        }

        public async Task NavigationToNextPage(object sender, long divisionUnitId)
        {

            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync(); await Task.Delay(100);
                await Navigation.PushAsync(new SurgicalConciergePatientView(PracticeDivisionDest, divisionUnitId));
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        protected async Task PastWeekDateButton_ClickedAsync(object sender, EventArgs e, long divisionUnitId)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    await Navigation.PushAsync(new SurgicalConciergePastWeekDates(divisionUnitId));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        protected async Task DivisionUnitButton_ClickedAsync(object sender, EventArgs e, long divisionUnitId)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await App.ShowUserDialogDelayAsync();
                await Navigation.PushAsync(new SurgicalConciergePatientView(PracticeDivisionDest, divisionUnitId));
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }
    }
}