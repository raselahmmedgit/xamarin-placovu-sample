using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ChartHelper.Models;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.PatientOutComeReport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NerhrectomyPatientReportPage : CustomContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        ProfessionalReportRestApiService _restApi;
        public ChartDataModel _chartDataModel = null;
        private string _chartType = "line";
        private ChartGenerator _chartGenerator;
        private int reportCount = 0;
        public string DefaultGraphUrl = "UroliftReport/IpssBssGraphReport?chartType=bar";
        public long ProcedureId = 0;
        public bool IsPhysicianPatientSelected = true;
        public bool IsPracticePatientSelected = true;

        public NerhrectomyPatientReportPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                _restApi = new ProfessionalReportRestApiService();
                _chartGenerator = new ChartGenerator();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }
        private async void LoadNephrotectomyTabularReport(long procedureId)
        {
            var url = procedureId == (int)Enums.ProcedureIdEnum.RoboticPartialNephrectomy ?
                Enums.ProcedureReportUrl.RoboticPartialNephrectomyTabular.ToDescriptionAttr() : Enums.ProcedureReportUrl.RoboticRadicalNephrectomyTabular.ToDescriptionAttr();
            //page.IsPhysicianPatientSelected = MyPatient.IsChecked;
            //page.IsPracticePatientSelected = PracticePatient.IsChecked;
            RadicalNephrectomyTrendsViewModel radicalNephrectomyTrendsViewModel = await _restApi.GetNephrotectomyTabularReportData(url);
            BuildNephrotectomyReportLayout(radicalNephrectomyTrendsViewModel);

            url = procedureId == (int)Enums.ProcedureIdEnum.RoboticPartialNephrectomy ?
                Enums.ProcedureReportUrl.RoboticPartialNephrectomy.ToDescriptionAttr() : Enums.ProcedureReportUrl.RoboticRadicalNephrectomy.ToDescriptionAttr();
            LoadGraph(url);


        }
        private void BuildNephrotectomyReportLayout(RadicalNephrectomyTrendsViewModel model)
        {

            StackLayout noDataFoundStackLayout = new StackLayout
            {

            };

            //int columnWidth = mobileMenu.IsCompactViewGraph ? 35 : 45;
            //mobileMenu.IsCompactViewGraph = !mobileMenu.IsCompactViewGraph;
            //ButtonExtended btnChart = _chartGenerator.GenerateOutcomeReportButton(OnChartCompactExpandBtn_Clicked, mobileMenu, mobileMenu.IsCompactViewGraph ? "Compact" : "Expand");
            //OutComeReportTypeButtonStackLayout.Children.Add(btnChart);

            //ButtonExtended btnGraph = _chartGenerator.GenerateOutcomeReportButton(OnGraphBtn_Clicked, mobileMenu, "Graph");
            //OutComeReportTypeButtonStackLayout.Children.Add(btnGraph);

            StackLayout publicTopHeaderStackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(0, 5, 0, 10),
            };
            var topHeaderLabel = new Label
            {
                Text = "Robotic Radical Nephrectomy Table",
                TextColor = Color.FromHex("#000"),
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };
            publicTopHeaderStackLayout.Children.Add(topHeaderLabel);
            NephrectomyTabularReport.Children.Add(publicTopHeaderStackLayout);

            int buttonPadding = 5;

            //patient Count block
            {
                StackLayout patientCountStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 14)
                };

                Grid patientReportGrid = new Grid
                {
                    ColumnSpacing = 0,
                    RowSpacing = 0,
                    Margin = new Thickness(0, 0, 0, 0),
                    Padding = new Thickness(0, 0, 0, 0)
                };
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 30},
                    new RowDefinition{ Height = 30}
                };
                patientReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(0.5,GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(0.5,GridUnitType.Star)}
                };
                patientReportGrid.ColumnDefinitions = colDefitions;
                var label = new Button
                {
                    Text = model.ProfessionalProcedureTotalPatient + "",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 0,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#fff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                patientReportGrid.Children.Add(label, 0, 0);
                label = new Button
                {
                    Text = model.SystemProcedureTotalPatient + "",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 0,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#fff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                patientReportGrid.Children.Add(label, 1, 0);
                label = new Button
                {
                    Text = "Your Patients",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 0,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#fff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                patientReportGrid.Children.Add(label, 0, 1);
                label = new Button
                {
                    Text = "All Patients",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 0,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#fff"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                patientReportGrid.Children.Add(label, 1, 1);
                patientCountStackLayout.Children.Add(patientReportGrid);
                NephrectomyTabularReport.Children.Add(patientCountStackLayout);
            }

            //Risk factor Tabular Report
            if (model.ProfessionalRiskFactorReportViewModel != null && model.ProfessionalRiskFactorReportViewModel.Labels != null && model.ProfessionalRiskFactorReportViewModel.Labels.Any())
            {
                var labels = model.ProfessionalRiskFactorReportViewModel.Labels;
                var values = model.ProfessionalRiskFactorReportViewModel.Values;

                StackLayout OutComeReportStackLayout = new StackLayout
                {

                };
                StackLayout reportHeaderStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(10, 0, 0, 10)
                };

                Label reportHeaderLabel = new Label
                {
                    Text = "Risk Factor",
                    TextColor = Color.FromHex("#000"),
                    FontAttributes = FontAttributes.Bold
                };
                reportHeaderStackLayout.Children.Add(reportHeaderLabel);

                StackLayout reportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 0)
                };

                Grid outComeReportGrid = new Grid
                {
                    ColumnSpacing = 0,
                    RowSpacing = 0,
                    Margin = new Thickness(0, 0, 0, 0),
                    Padding = new Thickness(0, 0, 0, 0)
                };
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 50}
                };
                foreach (var x in labels)
                {
                    rowDefinitions.Add(new RowDefinition { Height = 50 });
                }
                rowDefinitions.Add(new RowDefinition { Height = 50 });

                outComeReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                   new ColumnDefinition {Width = new GridLength(0.5,GridUnitType.Star)},
                   new ColumnDefinition {Width = new GridLength(0.5,GridUnitType.Star)}
                };
                outComeReportGrid.ColumnDefinitions = colDefitions;

                for (int i = 0; i < labels.Count(); i++)
                {
                    var label = new Button
                    {
                        Text = labels[i],
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(label, 0, i);
                    var value = new Button
                    {
                        Text = values[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, 1, i);
                }
                reportStackLayout.Children.Add(outComeReportGrid);
                OutComeReportStackLayout.Children.Add(reportHeaderStackLayout);
                OutComeReportStackLayout.Children.Add(reportStackLayout);

                ScrollView scrollView = new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                scrollView.Content = OutComeReportStackLayout;
                NephrectomyTabularReport.Children.Add(scrollView);

            }

            //Pain tabular report
            if (model.ProfessionalPainReportViewModel != null && model.ProfessionalPainReportViewModel.Labels != null && model.ProfessionalPainReportViewModel.Labels.Any())
            {
                var labels = model.ProfessionalPainReportViewModel.Labels;

                StackLayout OutComeReportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 20, 0, 0)
                };
                StackLayout reportHeaderStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                Label reportHeaderLabel = new Label
                {
                    Text = "Pain",
                    TextColor = Color.FromHex("#000")
                };
                reportHeaderStackLayout.Children.Add(reportHeaderLabel);

                StackLayout reportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 0)
                };

                Grid outComeReportGrid = new Grid
                {
                    ColumnSpacing = 0,
                    RowSpacing = 0,
                    Margin = new Thickness(0, 0, 0, 0),
                    Padding = new Thickness(0, 0, 0, 0)
                };
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50}
                };

                outComeReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=200}

                };

                for (int i = 0; i < labels.Count(); i++)
                {
                    colDefitions.Add(new ColumnDefinition { Width = 100 });
                }

                //colDefitions.Add(new ColumnDefinition { Width = 80 });

                outComeReportGrid.ColumnDefinitions = colDefitions;


                var label = new Button
                {
                    Text = "Pain",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#C0C0C0"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 0);
                label = new Button
                {
                    Text = "Level (0-5)",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 1);
                label = new Button
                {
                    Text = "# of pills",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 2);
                label = new Button
                {
                    Text = "Shoulder pain %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 3);


                for (int i = 0; i < labels.Count(); i++)
                {
                    label = new Button
                    {
                        Text = labels[i],
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#C0C0C0"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(label, (i + 1), 0);

                    var value = new Button
                    {
                        Text = model.ProfessionalPainReportViewModel.Level0_5[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 1);

                    value = GetHtmlButton(model.ProfessionalPainReportViewModel.PainPills[i] + "", "#fff");
                    outComeReportGrid.Children.Add(value, (i + 1), 2);

                    value = new Button
                    {
                        Text = model.ProfessionalPainReportViewModel.ShoulderPain[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 3);
                }
                reportStackLayout.Children.Add(outComeReportGrid);
                // OutComeReportStackLayout.Children.Add(reportHeaderStackLayout);
                OutComeReportStackLayout.Children.Add(reportStackLayout);
                ScrollView scrollView = new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                scrollView.Content = OutComeReportStackLayout;
                NephrectomyTabularReport.Children.Add(scrollView);
            }

            //General tabular report
            if (model.ProfessionalGeneralReportViewModel != null && model.ProfessionalGeneralReportViewModel.Labels != null && model.ProfessionalGeneralReportViewModel.Labels.Any())
            {
                var labels = model.ProfessionalGeneralReportViewModel.Labels;

                StackLayout OutComeReportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 20, 0, 0)
                };
                StackLayout reportHeaderStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                Label reportHeaderLabel = new Label
                {
                    Text = "General",
                    TextColor = Color.FromHex("#000")
                };
                reportHeaderStackLayout.Children.Add(reportHeaderLabel);

                StackLayout reportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 0)
                };

                Grid outComeReportGrid = new Grid
                {
                    ColumnSpacing = 0,
                    RowSpacing = 0,
                    Margin = new Thickness(0, 0, 0, 0),
                    Padding = new Thickness(0, 0, 0, 0)
                };
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50}
                };

                outComeReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=200}

                };

                for (int i = 0; i < labels.Count(); i++)
                {
                    colDefitions.Add(new ColumnDefinition { Width = 100 });
                }

                //colDefitions.Add(new ColumnDefinition { Width = 80 });

                outComeReportGrid.ColumnDefinitions = colDefitions;


                var label = new Button
                {
                    Text = "General",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#C0C0C0"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 0);
                label = new Button
                {
                    Text = "Fever %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 1);
                label = new Button
                {
                    Text = "Fever 101.5 %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 2);
                label = new Button
                {
                    Text = "Bruising %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 3);
                label = new Button
                {
                    Text = "IncisionalDrainage %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 4);


                for (int i = 0; i < labels.Count(); i++)
                {
                    label = new Button
                    {
                        Text = labels[i],
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#C0C0C0"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(label, (i + 1), 0);

                    var value = new Button
                    {
                        Text = model.ProfessionalGeneralReportViewModel.Fever[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 1);
                    value = new Button
                    {
                        Text = model.ProfessionalGeneralReportViewModel.Fever101[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 2);
                    value = new Button
                    {
                        Text = model.ProfessionalGeneralReportViewModel.Bruising[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 3);
                    value = new Button
                    {
                        Text = model.ProfessionalGeneralReportViewModel.IncisionalDrainage[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 4);
                }
                reportStackLayout.Children.Add(outComeReportGrid);
                // OutComeReportStackLayout.Children.Add(reportHeaderStackLayout);
                OutComeReportStackLayout.Children.Add(reportStackLayout);
                ScrollView scrollView = new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                scrollView.Content = OutComeReportStackLayout;
                NephrectomyTabularReport.Children.Add(scrollView);
            }

            //GI tabular report
            if (model.ProfessionalGIReportViewModel != null && model.ProfessionalGIReportViewModel.Labels != null && model.ProfessionalGIReportViewModel.Labels.Any())
            {
                var labels = model.ProfessionalGIReportViewModel.Labels;

                StackLayout OutComeReportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 20, 0, 0)
                };
                StackLayout reportHeaderStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                Label reportHeaderLabel = new Label
                {
                    Text = "GI",
                    TextColor = Color.FromHex("#000")
                };
                reportHeaderStackLayout.Children.Add(reportHeaderLabel);

                StackLayout reportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 0)
                };

                Grid outComeReportGrid = new Grid
                {
                    ColumnSpacing = 0,
                    RowSpacing = 0,
                    Margin = new Thickness(0, 0, 0, 0),
                    Padding = new Thickness(0, 0, 0, 0)
                };
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50}
                };

                outComeReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=200}

                };

                for (int i = 0; i < labels.Count(); i++)
                {
                    colDefitions.Add(new ColumnDefinition { Width = 100 });
                }

                //colDefitions.Add(new ColumnDefinition { Width = 80 });

                outComeReportGrid.ColumnDefinitions = colDefitions;


                var label = new Button
                {
                    Text = "GI",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#C0C0C0"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 0);
                label = new Button
                {
                    Text = "Flatus %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 1);
                label = new Button
                {
                    Text = "BM %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 2);
                label = new Button
                {
                    Text = "Nausea %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 3);
                label = new Button
                {
                    Text = "Vomiting %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 4);


                for (int i = 0; i < labels.Count(); i++)
                {
                    label = new Button
                    {
                        Text = labels[i],
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#C0C0C0"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(label, (i + 1), 0);

                    var value = new Button
                    {
                        Text = model.ProfessionalGIReportViewModel.Flatus[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 1);
                    value = new Button
                    {
                        Text = model.ProfessionalGIReportViewModel.BM[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 2);
                    value = new Button
                    {
                        Text = model.ProfessionalGIReportViewModel.Nausea[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 3);
                    value = new Button
                    {
                        Text = model.ProfessionalGIReportViewModel.Vomiting[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 4);
                }
                reportStackLayout.Children.Add(outComeReportGrid);
                // OutComeReportStackLayout.Children.Add(reportHeaderStackLayout);
                OutComeReportStackLayout.Children.Add(reportStackLayout);
                ScrollView scrollView = new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                scrollView.Content = OutComeReportStackLayout;
                NephrectomyTabularReport.Children.Add(scrollView);
            }

            //Readmitted to Hospital tabular report
            if (model.ProfessionalReadmittedHospitalReportViewModel != null && model.ProfessionalReadmittedHospitalReportViewModel.Labels != null && model.ProfessionalReadmittedHospitalReportViewModel.Labels.Any())
            {
                var labels = model.ProfessionalReadmittedHospitalReportViewModel.Labels;

                StackLayout OutComeReportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 20, 0, 0)
                };
                StackLayout reportHeaderStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(10, 0, 0, 10)
                };

                Label reportHeaderLabel = new Label
                {
                    Text = "Readmitted to Hospital",
                    TextColor = Color.FromHex("#000"),
                    FontAttributes = FontAttributes.Bold
                };
                reportHeaderStackLayout.Children.Add(reportHeaderLabel);

                StackLayout reportStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 0)
                };

                Grid outComeReportGrid = new Grid
                {
                    ColumnSpacing = 0,
                    RowSpacing = 0,
                    Margin = new Thickness(0, 0, 0, 0),
                    Padding = new Thickness(0, 0, 0, 0)
                };
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50},
                    new RowDefinition{ Height = 50}
                };

                outComeReportGrid.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=200}

                };

                for (int i = 0; i < labels.Count(); i++)
                {
                    colDefitions.Add(new ColumnDefinition { Width = 100 });
                }

                //colDefitions.Add(new ColumnDefinition { Width = 80 });

                outComeReportGrid.ColumnDefinitions = colDefitions;


                var label = new Button
                {
                    Text = "",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#C0C0C0"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 0);
                label = new Button
                {
                    Text = "Seen elsewhere %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 1);
                label = new Button
                {
                    Text = "Primary Care %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 2);
                label = new Button
                {
                    Text = "Emergency Room %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 3);
                label = new Button
                {
                    Text = "Urgent Room %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 4);
                label = new Button
                {
                    Text = "Hospitalized %",
                    BorderColor = Color.FromHex("#ccc"),
                    BorderWidth = 1,
                    CornerRadius = 0,
                    BackgroundColor = Color.FromHex("#DCDCDC"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = buttonPadding
                };
                outComeReportGrid.Children.Add(label, 0, 5);


                for (int i = 0; i < labels.Count(); i++)
                {
                    label = new Button
                    {
                        Text = labels[i],
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#C0C0C0"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(label, (i + 1), 0);

                    var value = new Button
                    {
                        Text = model.ProfessionalReadmittedHospitalReportViewModel.SeenElseWhere[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 1);
                    value = new Button
                    {
                        Text = model.ProfessionalReadmittedHospitalReportViewModel.PrimaryCare[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 2);
                    value = new Button
                    {
                        Text = model.ProfessionalReadmittedHospitalReportViewModel.EmergencyRoom[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 3);
                    value = new Button
                    {
                        Text = model.ProfessionalReadmittedHospitalReportViewModel.UrgentCare[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 4);
                    value = new Button
                    {
                        Text = model.ProfessionalReadmittedHospitalReportViewModel.Hospitalized[i] + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 1,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    outComeReportGrid.Children.Add(value, (i + 1), 5);
                }
                reportStackLayout.Children.Add(outComeReportGrid);
                OutComeReportStackLayout.Children.Add(reportHeaderStackLayout);
                OutComeReportStackLayout.Children.Add(reportStackLayout);
                ScrollView scrollView = new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                scrollView.Content = OutComeReportStackLayout;
                NephrectomyTabularReport.Children.Add(scrollView);
            }

            StackLayout btnStackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(0, 10, 0, 15),
            };
            ButtonExtended btn = new ButtonExtended
            {
                Text = "View Trends",
                Margin = new Thickness(0, 5, 10, 0),
                WidthRequest = 200,
                BackgroundColor = Color.FromHex("#000"),
                HorizontalOptions = LayoutOptions.Center
            };
            btn.Clicked += BtnChartView_Clicked;

            btnStackLayout.Children.Add(btn);
            NephrectomyTabularReport.Children.Add(btnStackLayout);
        }

        private async void LoadGraph(string url)
        {
            _chartDataModel = await _restApi.ComparativeAnalysisGraphReport(url, IsPhysicianPatientSelected, IsPracticePatientSelected);
            reportCount++;
            if (_chartDataModel != null)
            {
                _chartDataModel.additionalReportNumber = reportCount;
                ButtonExtended btnLine = _chartGenerator.GenerateComparativeAnalysisReportButton(BtnLine_Clicked, _chartDataModel, "Line");
                ButtonExtended btnBar = _chartGenerator.GenerateComparativeAnalysisReportButton(BtnBar_Clicked, _chartDataModel, "Bar");

                if (_chartDataModel.HasPublicTopHeader)
                {
                    StackLayout publicTopHeaderStackLayout = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        Margin = new Thickness(0, 5, 0, 15),
                    };
                    var topHeaderLabel = new Label
                    {
                        Text = _chartDataModel.TopHeaderTitle,
                        TextColor = Color.FromHex("#000"),
                        HorizontalOptions = LayoutOptions.Center,
                        FontAttributes = FontAttributes.Bold
                    };
                    publicTopHeaderStackLayout.Children.Add(topHeaderLabel);
                    NephrectomyChartReport.Children.Add(publicTopHeaderStackLayout);
                }

                //patient Count block
                if (_chartDataModel.HasPublicTotalPatient)
                {
                    StackLayout patientCountStackLayout = new StackLayout
                    {
                        Margin = new Thickness(0, 0, 0, 14)
                    };

                    Grid patientReportGrid = new Grid
                    {
                        ColumnSpacing = 0,
                        RowSpacing = 0,
                        Margin = new Thickness(0, 0, 0, 0),
                        Padding = new Thickness(0, 0, 0, 0)
                    };
                    //define row
                    var rowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition{ Height = 30},
                        new RowDefinition{ Height = 30}
                    };
                    patientReportGrid.RowDefinitions = rowDefinitions;

                    //define colum
                    var colDefitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = new GridLength(0.5,GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(0.5,GridUnitType.Star)}
                    };
                    var buttonPadding = 5;
                    patientReportGrid.ColumnDefinitions = colDefitions;
                    var label = new Button
                    {
                        Text = _chartDataModel.ProfessionalProcedureTotalPatient + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 0,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    patientReportGrid.Children.Add(label, 0, 0);
                    label = new Button
                    {
                        Text = _chartDataModel.SystemProcedureTotalPatient + "",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 0,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    patientReportGrid.Children.Add(label, 1, 0);
                    label = new Button
                    {
                        Text = "Your Patients",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 0,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    patientReportGrid.Children.Add(label, 0, 1);
                    label = new Button
                    {
                        Text = "All Patients",
                        BorderColor = Color.FromHex("#ccc"),
                        BorderWidth = 0,
                        CornerRadius = 0,
                        BackgroundColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = buttonPadding
                    };
                    patientReportGrid.Children.Add(label, 1, 1);
                    patientCountStackLayout.Children.Add(patientReportGrid);
                    NephrectomyChartReport.Children.Add(patientCountStackLayout);
                }

                if (_chartDataModel.IsPublicPortalGraph)
                {
                    StackLayout publicHeaderStackLayout = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        Margin = new Thickness(0, 5, 0, 0),
                        Padding = new Thickness(10, 10, 10, 10),
                        BackgroundColor = Color.FromHex("#929292")
                    };
                    var topHeaderLabel = new Label
                    {
                        Text = _chartDataModel.ReportShortName,
                        TextColor = Color.FromHex("#fff"),
                        HorizontalOptions = LayoutOptions.Center
                    };
                    publicHeaderStackLayout.Children.Add(topHeaderLabel);
                    NephrectomyChartReport.Children.Add(publicHeaderStackLayout);
                }

                StackLayout barLineStackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 0),
                };
                barLineStackLayout.Children.Add(btnLine);
                barLineStackLayout.Children.Add(btnBar);

                StackLayout graphStackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 5, 0, 0),
                    ClassId = reportCount.ToString()
                };

                NephrectomyChartReport.Children.Add(barLineStackLayout);
                NephrectomyChartReport.Children.Add(graphStackLayout);
                _chartGenerator.GenerateGraphCanvas(graphStackLayout, _chartDataModel, _chartType);
                if (_chartDataModel.HasAdditionalReport && !string.IsNullOrEmpty(_chartDataModel.AdditionalReportUrl))
                {
                    LoadGraph(_chartDataModel.AdditionalReportUrl);
                }
                else
                {
                    StackLayout btnStackLayout = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        Margin = new Thickness(0, 10, 0, 15),
                    };
                    ButtonExtended btn = new ButtonExtended
                    {
                        Text = "View Table",
                        Margin = new Thickness(0, 5, 10, 0),
                        WidthRequest = 200,
                        BackgroundColor = Color.FromHex("#000"),
                        HorizontalOptions = LayoutOptions.Center
                    };
                    btn.Clicked += BtnTabularView_Clicked;

                    btnStackLayout.Children.Add(btn);
                    NephrectomyChartReport.Children.Add(btnStackLayout);
                }
            }

            App.HideUserDialogAsync();


        }

        private Button GetHtmlButton(string text, string rowBgColor)
        {
            var formattedText = string.Empty;
            var textColor = Color.FromHex("#000");
            if (!string.IsNullOrEmpty(text))
            {
                formattedText = Regex.Replace(text, "<[^>]*>", string.Empty);
                if (formattedText.Contains("&#10004;")) // convert tick sign
                {
                    formattedText = "\u221A";
                }
                if (text.Contains("check-circle-red"))
                {
                    textColor = Color.FromHex("#FF0000");
                }
            }
            return new HtmlButton
            {
                Text = formattedText,
                BorderColor = Color.FromHex("#ccc"),
                BorderWidth = 1,
                CornerRadius = 0,
                TextColor = textColor,
                BackgroundColor = Color.FromHex(rowBgColor),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 5,
            };
        }
        private void BtnChartView_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                NephrectomyTabularReportStackLayout.IsVisible = false;
                NephrectomyChartReportStackLayout.IsVisible = true;
            }
        }
        private void BtnTabularView_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                NephrectomyTabularReportStackLayout.IsVisible = true;
                NephrectomyChartReportStackLayout.IsVisible = false;
            }
        }
        private void BtnLine_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                var selectedButton = sender as Button;
                var chartDataModel = selectedButton.BindingContext as ChartDataModel;
                var view = NephrectomyChartReport.Children.Where(x => x.GetType() == typeof(StackLayout) && x.ClassId == chartDataModel.additionalReportNumber.ToString()).FirstOrDefault();
                if (view != null)
                {
                    _chartGenerator.GenerateGraphCanvas((StackLayout)view, chartDataModel, "line");
                }

            }
        }

        private void BtnBar_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                var selectedButton = sender as Button;
                var chartDataModel = selectedButton.BindingContext as ChartDataModel;
                var view = NephrectomyChartReport.Children.Where(x => x.GetType() == typeof(StackLayout) && x.ClassId == chartDataModel.additionalReportNumber.ToString()).FirstOrDefault();
                if (view != null)
                {
                    _chartGenerator.GenerateGraphCanvas((StackLayout)view, chartDataModel, "bar");
                }
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadNephrotectomyTabularReport(ProcedureId);
        }
    }

}