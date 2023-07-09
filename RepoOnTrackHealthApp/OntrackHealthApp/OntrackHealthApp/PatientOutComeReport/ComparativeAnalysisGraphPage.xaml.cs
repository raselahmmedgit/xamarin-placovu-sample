using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ChartHelper.Models;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.PatientOutComeReport
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ComparativeAnalysisGraphPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        ProfessionalReportRestApiService _restApi;
        public ChartDataModel _chartDataModel = null;
        private string _chartType = "line";
        private ChartGenerator _chartGenerator;
        private int reportCount = 0;
        public string DefaultGraphUrl = "UroliftReport/IpssBssGraphReport?chartType=bar";
        public bool IsPhysicianPatientSelected = true;
        public bool IsPracticePatientSelected = true;

        public ComparativeAnalysisGraphPage ()
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
        private async void LoadGraph(string url)
        {
            _chartDataModel =  await _restApi.ComparativeAnalysisGraphReport(url, IsPhysicianPatientSelected, IsPracticePatientSelected);
            reportCount++;
            if(_chartDataModel!=null)
            {
                _chartDataModel.additionalReportNumber = reportCount;
                ButtonExtended btnLine = _chartGenerator.GenerateComparativeAnalysisReportButton(BtnLine_Clicked, _chartDataModel, "Line");
                ButtonExtended btnBar = _chartGenerator.GenerateComparativeAnalysisReportButton(BtnBar_Clicked, _chartDataModel, "Bar");
                
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

                ComparativeAnalysisGraph.Children.Add(barLineStackLayout);
                ComparativeAnalysisGraph.Children.Add(graphStackLayout);
                _chartGenerator.GenerateGraphCanvas(graphStackLayout, _chartDataModel, _chartType);
                if (_chartDataModel.HasAdditionalReport && !string.IsNullOrEmpty(_chartDataModel.AdditionalReportUrl))
                {
                    LoadGraph(_chartDataModel.AdditionalReportUrl);
                }
            }

            App.HideUserDialogAsync();


        }

        private void BtnLine_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                var selectedButton =  sender as Button;
                var chartDataModel =  selectedButton.BindingContext as ChartDataModel;
                var view = ComparativeAnalysisGraph.Children.Where(x => x.GetType() == typeof(StackLayout) && x.ClassId == chartDataModel.additionalReportNumber.ToString()).FirstOrDefault();
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
                var view = ComparativeAnalysisGraph.Children.Where(x => x.GetType() == typeof(StackLayout) && x.ClassId == chartDataModel.additionalReportNumber.ToString()).FirstOrDefault();
                if (view != null)
                {
                    _chartGenerator.GenerateGraphCanvas((StackLayout)view, chartDataModel, "bar");
                }
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();            
            LoadGraph(DefaultGraphUrl);
        }
    }
}