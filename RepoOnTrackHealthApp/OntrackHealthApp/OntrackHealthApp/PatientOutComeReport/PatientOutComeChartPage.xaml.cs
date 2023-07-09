using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ChartHelper.Models;
using OntrackHealthApp.PatientOutComeReport.ViewModel;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.PatientOutComeReport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientOutComeChartPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        ProfessionalReportRestApiService _restApi;
        PatientReportedOutcomePatientViewModel _patientReportedOutcomePatientViewModel;
        public ChartDataModel _chartDataModel = null;
        private string _chartType = "line";
        private ChartGenerator _chartGenerator;
        private int postOpButtonPadding = 5;
        private int outcomeButtonPadding = 5;
        public string DefaultGraphUrl = "UroliftReport/IpssBssGraphReport?chartType=bar";
        public bool IsPhysicianPatientSelected = true;
        public bool IsPracticePatientSelected = true;
        private long _activeReportMenuId = 0;

        public PatientOutComeChartPage(PatientReportedOutcomePatientViewModel patientReportedOutcomePatientViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                _patientReportedOutcomePatientViewModel = patientReportedOutcomePatientViewModel;
                _restApi = new ProfessionalReportRestApiService();
                _chartGenerator = new ChartGenerator();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }
        public PatientOutComeChartPage(long menuId)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                _patientReportedOutcomePatientViewModel = new PatientReportedOutcomePatientViewModel();
                _restApi = new ProfessionalReportRestApiService();
                _chartGenerator = new ChartGenerator();
                _activeReportMenuId = menuId;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void BuildOutComeReportButtons()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                var mobileSurveyReportViewModel = await _restApi.GetMobileSurveyReportViewModel();
                var mobileMenus = mobileSurveyReportViewModel.ProfessionalSurveyReportMobileMenus.ToList();
                if(_patientReportedOutcomePatientViewModel.PatientProfileId == 0)
                {
                    mobileMenus =  mobileSurveyReportViewModel.ProfessionalSurveyReportMobileMenus.Where(c => c.GraphReportAction != null).ToList();
                }
                foreach (var item in mobileMenus)
                {
                    item.PhysicianPatientOutCome = IsPhysicianPatientSelected;
                    item.HospitalReportedOutCome = IsPracticePatientSelected;
                }

                var selectedItem = _activeReportMenuId == 0 ? mobileMenus.FirstOrDefault() : mobileMenus.FirstOrDefault(c=>c.MenuId==_activeReportMenuId);
                selectedItem.IsCompactViewGraph = true;                

                foreach (var menu in mobileMenus)
                {                    
                    Button btn = new Button
                    {
                        Text = menu.MenuTitle,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BorderColor = Color.FromHex("#98185E"),
                        TextColor = menu.MenuId == selectedItem.MenuId ? Color.FromHex("#FFF") : Color.FromHex("#98185E"),
                        BorderWidth = 2,
                        BackgroundColor = menu.MenuId== selectedItem.MenuId ? Color.FromHex("#98185E"): Color.Transparent,
                        BindingContext = menu
                    };
                    btn.Clicked += OnReportBtn_Clicked;
                    outComeReportButtonStackLayout.Children.Add(btn);
                }
                if (_patientReportedOutcomePatientViewModel!=null && _patientReportedOutcomePatientViewModel.PatientProfileId > 0)
                {
                    LoadChart(selectedItem);
                }
                else
                {
                    LoadGraph(selectedItem);
                }
                    
            }
        }
        private async void LoadChart(ProfessionalSurveyReportMobileMenu mobileMenu)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                var report = await _restApi.OutComeChartReport(mobileMenu, _patientReportedOutcomePatientViewModel);
                if (mobileMenu.ActionName.Equals("PostOp"))
                {
                    BuildOutComePostOpReportLayout(report,mobileMenu);
                }
                else
                {
                    BuildOutComeReportLayout(report,mobileMenu);
                }
            }
            
        }

        private async void LoadGraph(ProfessionalSurveyReportMobileMenu mobileMenu)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                var graphReport = await _restApi.OutComeGraphReport(mobileMenu, _patientReportedOutcomePatientViewModel);
                if (graphReport != null)
                {
                    _chartDataModel = new ChartDataModel();
                    _chartDataModel = graphReport;
                    BuildInitialRoboticProstatectomyGraphReportHtml(_chartType,mobileMenu);
                }
                else
                {
                    BuildInitialRoboticProstatectomyGraphReportHtml(_chartType, mobileMenu);
                    await Task.Yield();
                }
            }
        }

        #region graph
        public void BuildInitialRoboticProstatectomyGraphReportHtml(string chartType, ProfessionalSurveyReportMobileMenu mobileMenu)
        {
            outComeReportGrid.Children.Clear();
            noDataFoundStackLayout.Children.Clear();
            OutComeReportTypeButtonStackLayout.Children.Clear();
            reportHeaderLabel.Text = "";

            ButtonExtended btnLine = _chartGenerator.GenerateOutcomeReportButton(BtnLine_Clicked,mobileMenu,"Line");
            OutComeReportTypeButtonStackLayout.Children.Add(btnLine);

            ButtonExtended btnBar = _chartGenerator.GenerateOutcomeReportButton(BtnBar_Clicked, mobileMenu, "Bar");
            OutComeReportTypeButtonStackLayout.Children.Add(btnBar);

            mobileMenu.IsCompactViewGraph = !mobileMenu.IsCompactViewGraph;
            ButtonExtended btnCompactExpand = _chartGenerator.GenerateOutcomeReportButton(OnGraphCompactExpandBtn_Clicked, mobileMenu, mobileMenu.IsCompactViewGraph ? "Compact" : "Expand");
            OutComeReportTypeButtonStackLayout.Children.Add(btnCompactExpand);

            ButtonExtended btnChart = _chartGenerator.GenerateOutcomeReportButton(OnChartBtn_Clicked, mobileMenu, "Chart");
            if (_patientReportedOutcomePatientViewModel != null && _patientReportedOutcomePatientViewModel.PatientProfileId > 0)
            {
                OutComeReportTypeButtonStackLayout.Children.Add(btnChart);
            }
            _chartGenerator.GenerateGraphCanvas(OutComeGraphStackLayout, _chartDataModel, chartType);
        }

        private void BtnLine_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                _chartGenerator.GenerateGraphCanvas(OutComeGraphStackLayout, _chartDataModel, "line");
            }
        }

        private void BtnBar_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                _chartGenerator.GenerateGraphCanvas(OutComeGraphStackLayout, _chartDataModel, "bar");
            }

        }
        #endregion

        private async void OnReportBtn_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    Button selectedButton = sender as Button;
                    if (selectedButton.BackgroundColor == Color.FromHex("#FFF"))
                    {
                        var reportButtonList = outComeReportButtonStackLayout.Children.Where(c => c.GetType() == typeof(Button));
                        foreach (var item in reportButtonList)
                        {
                            var button = item as Button;
                            button.BackgroundColor = Color.FromHex("#FFF");
                            button.TextColor = Color.FromHex("#98185E");
                        }
                        selectedButton.BackgroundColor = Color.FromHex("#98185E");
                        selectedButton.TextColor = Color.FromHex("#FFF");
                        var selectedReport = selectedButton.BindingContext as ProfessionalSurveyReportMobileMenu;
                        selectedReport.IsCompactViewGraph = true;
                        if (_patientReportedOutcomePatientViewModel != null && _patientReportedOutcomePatientViewModel.PatientProfileId > 0)
                        {
                            LoadChart(selectedReport);
                        }
                        else
                        {
                            LoadGraph(selectedReport);
                        }
                    }
                    else
                    {
                        selectedButton.BackgroundColor = Color.FromHex("#FFF");
                        selectedButton.TextColor = Color.FromHex("#98185E");
                    }

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

        private async void OnChartBtn_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    Button selectedButton = sender as Button;
                    selectedButton.BackgroundColor = Color.FromHex("#98185E");
                    selectedButton.TextColor = Color.FromHex("#FFF");
                    var selectedReport = selectedButton.BindingContext as ProfessionalSurveyReportMobileMenu;
                    selectedReport.IsCompactViewGraph = true;
                    LoadChart(selectedReport);

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

        private async void OnChartCompactExpandBtn_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    Button selectedButton = sender as Button;
                    selectedButton.BackgroundColor = Color.FromHex("#98185E");
                    selectedButton.TextColor = Color.FromHex("#FFF");
                    var selectedReport = selectedButton.BindingContext as ProfessionalSurveyReportMobileMenu;
                    LoadChart(selectedReport);

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

        private async void OnGraphBtn_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    Button selectedButton = sender as Button;
                    selectedButton.BackgroundColor = Color.FromHex("#98185E");
                    selectedButton.TextColor = Color.FromHex("#FFF");
                    var selectedReport = selectedButton.BindingContext as ProfessionalSurveyReportMobileMenu;
                    selectedReport.IsCompactViewGraph = true;
                    LoadGraph(selectedReport);

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

        private async void OnGraphCompactExpandBtn_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    Button selectedButton = sender as Button;
                    selectedButton.BackgroundColor = Color.FromHex("#98185E");
                    selectedButton.TextColor = Color.FromHex("#FFF");
                    var selectedReport = selectedButton.BindingContext as ProfessionalSurveyReportMobileMenu;
                    LoadGraph(selectedReport);

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

        

        private void BuildOutComeReportLayout(MobileSurveyReportViewModel model, ProfessionalSurveyReportMobileMenu mobileMenu)
        {
            outComeReportGrid.Children.Clear();
            noDataFoundStackLayout.Children.Clear();
            OutComeReportTypeButtonStackLayout.Children.Clear();
            OutComeGraphStackLayout.Children.Clear();
            int columnWidth = mobileMenu.IsCompactViewGraph ? 35 : 45;
            mobileMenu.IsCompactViewGraph = !mobileMenu.IsCompactViewGraph;
            ButtonExtended btnChart = _chartGenerator.GenerateOutcomeReportButton(OnChartCompactExpandBtn_Clicked, mobileMenu, mobileMenu.IsCompactViewGraph ? "Compact" : "Expand");
            OutComeReportTypeButtonStackLayout.Children.Add(btnChart);

            ButtonExtended btnGraph = _chartGenerator.GenerateOutcomeReportButton(OnGraphBtn_Clicked, mobileMenu, "Graph");
            OutComeReportTypeButtonStackLayout.Children.Add(btnGraph);

            reportHeaderLabel.Text = model.ReportHeader;
            int buttonPadding = 5;
            
            if (model.graphDataProperty != null && model.graphDataProperty.PatientSurveyScore != null && model.graphDataProperty.PatientSurveyScore.Any())
            {
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 50}
                };
                foreach (var x in model.QuestionLabels)
                {
                    rowDefinitions.Add(new RowDefinition { Height = 50 });
                }
                rowDefinitions.Add(new RowDefinition { Height = 50 });

                outComeReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=100},
                };
                foreach (var x in model.NotificationLabels)
                {
                    colDefitions.Add(new ColumnDefinition { Width = columnWidth });
                }
                //colDefitions.Add(new ColumnDefinition { Width = 80 });

                outComeReportGrid.ColumnDefinitions = colDefitions;

                //define header
                var button = new Button
                {
                    Text = string.Empty,
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#fff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(button, 0, 0);
                int i = 0;
                for (i = 0; i < model.NotificationLabels.Count; i++)
                {
                    button = new Button
                    {
                        Text = model.NotificationLabels[i],
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding,
                        TextColor = Color.FromHex("#98185E")
                    };
                    outComeReportGrid.Children.Add(button, i + 1, 0);
                }
                string rowBgColor = "#ddd";
                i = 0;
                if (model.graphDataProperty.ToolTipPatientServeyScore.Any())
                {
                    for (i = 0; i < model.QuestionLabels.Count; i++)
                    {
                        if (i % 2 == 0)
                            rowBgColor = "#ddd";
                        else
                            rowBgColor = "#fff";
                        var data = model.QuestionLabels[i];
                        button = new Button
                        {
                            Text = model.QuestionLabels[i],
                            BorderColor = Color.FromHex("#ccc"),
                            BorderWidth = 1,
                            CornerRadius = 0,
                            BackgroundColor = Color.FromHex(rowBgColor),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Padding = buttonPadding,
                            TextColor = Color.FromHex("#000")
                        };
                        outComeReportGrid.Children.Add(button, 0, i + 1);

                        for (int j = 0; j < model.graphDataProperty.ToolTipPatientServeyScore.Count; j++)
                        {
                            List<decimal> dataList = model.graphDataProperty.ToolTipPatientServeyScore[j];
                            button = new Button
                            {
                                Text = dataList[i].ToRoundedDecimalString(),
                                BorderColor = Color.FromHex("#ccc"),
                                BorderWidth = 1,
                                CornerRadius = 0,
                                BackgroundColor = Color.FromHex(rowBgColor),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Padding = buttonPadding,
                                TextColor = Color.FromHex("#000")
                            };
                            outComeReportGrid.Children.Add(button, j + 1, i + 1);
                        }


                    }
                }

                string totalText = "Total";
                if (model.QuestionLabels.Count == 1)
                {
                    totalText = model.QuestionLabels.FirstOrDefault();
                }
                button = new Button
                {
                    Text = totalText,
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#fff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding,
                    TextColor = Color.FromHex("#000"),
                    FontAttributes = FontAttributes.Bold
                };
                outComeReportGrid.Children.Add(button, 0, i+1);

                for (int j = 0; j < model.graphDataProperty.PatientSurveyScore.Count; j++)
                {                    
                    button = new Button
                    {
                        Text = model.graphDataProperty.PatientSurveyScore[j].ToRoundedDecimalString(),
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex(rowBgColor),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding,
                        TextColor = Color.FromHex("#000"),
                        FontAttributes = FontAttributes.Bold
                    };
                    outComeReportGrid.Children.Add(button, j + 1, i + 1);
                }

            }
            else
            {
                var noDataFoundLayout = new StackLayout
                {
                    Padding = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#007ACC"),
                    Children =
                    {
                        new Label {
                            Text="No Data Found",
                            TextColor=Color.FromHex("#fff")
                        }
                    }
                };

                outComeReportStackLayout.Children.Clear();
                outComeReportStackLayout.Children.Add(noDataFoundLayout);
            }

        }

        private void BuildOutComePostOpReportLayout(MobileSurveyReportViewModel model, ProfessionalSurveyReportMobileMenu mobileMenu)
        {
            outComeReportGrid.Children.Clear();
            noDataFoundStackLayout.Children.Clear();
            OutComeReportTypeButtonStackLayout.Children.Clear();
            OutComeGraphStackLayout.Children.Clear();
            reportHeaderLabel.Text = model.ReportHeader;
            int columnWidth = 40;
            if (model.OutcomeReportViewModels != null && model.OutcomeReportViewModels.Any())
            {
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 50}
                };
                foreach (var x in model.OutcomeReportViewModels)
                {
                    rowDefinitions.Add(new RowDefinition { Height = 50 });
                }
                rowDefinitions.Add(new RowDefinition { Height = 50 });

                outComeReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=100},
                };
                foreach (var x in model.NotificationLabels)
                {
                    colDefitions.Add(new ColumnDefinition { Width = columnWidth });
                }
                outComeReportGrid.ColumnDefinitions = colDefitions;

                //define header
                var button = new Button
                {
                    Text = string.Empty,
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#fff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = postOpButtonPadding
                };
                outComeReportGrid.Children.Add(button, 0, 0);
                int i = 0;
                for (i = 0; i < model.NotificationLabels.Count; i++)
                {
                    button = new Button
                    {
                        Text = model.NotificationLabels[i],
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = postOpButtonPadding,
                        TextColor = Color.FromHex("#98185E"),
                        
                    };
                    outComeReportGrid.Children.Add(button, i + 1, 0);                    
                }
                string rowBgColor = "#ddd";
                i = 0;
                foreach (var surveyQuestionGroup in model.OutcomeReportViewModels.GroupBy(c=>c.SurveyQuestionGroupName))
                {
                    i++;                   
                    if (i % 2 == 0)
                        rowBgColor = "#ddd";
                    else
                        rowBgColor = "#fff";

                    var postOpQuestionGroupButton = new Button
                    {
                        Text = surveyQuestionGroup.Key,
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex(rowBgColor),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = postOpButtonPadding,
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromHex("#98185E")
                    };

                    outComeReportGrid.Children.Add(postOpQuestionGroupButton, 0, i);

                    var htmlButton = GetScoreButton(string.Empty, rowBgColor);
                    outComeReportGrid.Children.Add(htmlButton, 1, i);

                    htmlButton = GetScoreButton(string.Empty, rowBgColor);
                    outComeReportGrid.Children.Add(htmlButton, 2, i);

                    htmlButton = GetScoreButton(string.Empty, rowBgColor);
                    outComeReportGrid.Children.Add(htmlButton, 3, i);

                    htmlButton = GetScoreButton(string.Empty, rowBgColor);
                    outComeReportGrid.Children.Add(htmlButton, 4, i);

                    htmlButton = GetScoreButton(string.Empty, rowBgColor);
                    outComeReportGrid.Children.Add(htmlButton, 5, i);

                    htmlButton = GetScoreButton(string.Empty, rowBgColor);
                    outComeReportGrid.Children.Add(htmlButton, 6, i);

                    htmlButton = GetScoreButton(string.Empty, rowBgColor);
                    outComeReportGrid.Children.Add(htmlButton, 7, i);
                    
                    foreach (var item in surveyQuestionGroup)
                    {
                        i++;
                        if (i % 2 == 0)
                            rowBgColor = "#ddd";
                        else
                            rowBgColor = "#fff";

                        var postOpQuestionButton = new HtmlButton
                        {
                            Text = item.SurveyQuestionShortText,
                            BorderColor = Color.FromHex("#ccc"),
                            BorderWidth = 1,
                            CornerRadius = 0,
                            BackgroundColor = Color.FromHex(rowBgColor),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Padding = postOpButtonPadding,
                            FontSize = 16,
                            TextColor = Color.FromHex("#000"),                            
                            
                        };

                        outComeReportGrid.Children.Add(postOpQuestionButton, 0, i);

                        htmlButton = GetScoreButton(item.Day2, rowBgColor);
                        outComeReportGrid.Children.Add(htmlButton, 1, i);

                        htmlButton = GetScoreButton(item.Day4, rowBgColor);
                        outComeReportGrid.Children.Add(htmlButton, 2, i);

                        htmlButton = GetScoreButton(item.Day6, rowBgColor);
                        outComeReportGrid.Children.Add(htmlButton, 3, i);

                        htmlButton = GetScoreButton(item.Day0, rowBgColor);
                        outComeReportGrid.Children.Add(htmlButton, 4, i);

                        htmlButton = GetScoreButton(item.Day1, rowBgColor);
                        outComeReportGrid.Children.Add(htmlButton, 5, i);

                        htmlButton = GetScoreButton(item.Day4Cath, rowBgColor);
                        outComeReportGrid.Children.Add(htmlButton, 6, i);

                        htmlButton = GetScoreButton(item.Day7, rowBgColor);
                        outComeReportGrid.Children.Add(htmlButton, 7, i);
                    }
                    
                }
                               
            }
            else
            {
                var noDataFoundLayout = new StackLayout
                {
                    Padding = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#007ACC"),
                    Children =
                    {
                        new Label {
                            Text="No Data Found",
                            TextColor=Color.FromHex("#fff")
                        }
                    }
                };

                noDataFoundStackLayout.Children.Clear();
                noDataFoundStackLayout.Children.Add(noDataFoundLayout);
            }

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BuildOutComeReportButtons();
        }
        private Button GetScoreButton(string text, string rowBgColor)
        {
            return new HtmlButton
            {
                Text = text,
                BorderColor = Color.FromHex("#ccc"),
                BorderWidth = 1,
                CornerRadius = 0,
                BackgroundColor = Color.FromHex(rowBgColor),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = postOpButtonPadding,
                //TextColor = Color.FromHex("#000")
            };
        }
    }
}