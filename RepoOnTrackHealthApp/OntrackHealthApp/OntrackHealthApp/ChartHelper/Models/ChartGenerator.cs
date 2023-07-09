using OntrackHealthApp.AppCore;
using OntrackHealthApp.PatientOutComeReport.ViewModel;
using OntrackHealthApp.UserControls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OntrackHealthApp.ChartHelper.Models
{
    public class ChartGenerator
    {
        private ChartConfiguration chartConfiguration;
        public ChartGenerator()
        {
                
        }
        public ChartGenerator(ChartDataModel chartData,string chartType)
        {
            chartConfiguration = new ChartConfiguration(chartData, chartType);            
        }
        
        public string GenerateChart()
        {
            return chartConfiguration.GenerateChart();
        }

        private string GenerateChart(ChartDataModel chartData, string chartType)
        {
            chartConfiguration = new ChartConfiguration(chartData, chartType);
            return chartConfiguration.GenerateChart();
        }

        public void GenerateGraphCanvas(StackLayout canvasStackLayout, ChartDataModel chartDataModel, string chartType)
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var width = 500.0;
            if (chartDataModel.IsCompactViewGraph)
            {
                width = mainDisplayInfo.Width;
            }
            string graphHtmlContent = GenerateChart(chartDataModel, chartType);
            WebView webView = new WebView
            {
                WidthRequest = width,
                HeightRequest = 500,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, 2, 0, 0),
            };
            HtmlWebViewSource source = new HtmlWebViewSource
            {
                Html = graphHtmlContent
            };
            webView.Source = source;
            canvasStackLayout.Children.Clear();
            canvasStackLayout.Children.Add(webView);
        }

        public ButtonExtended GenerateOutcomeReportButton(EventHandler eventHandler, ProfessionalSurveyReportMobileMenu mobileMenu, string btnText, string btnColorCode = "#467494")
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = DeviceInfo.Name;
                if (deviceName == AppConstant.iOSName5s)
                {
                    IsiOSName5s = true;
                }

                var deviceDisplay = DeviceDisplay.MainDisplayInfo;
                if (deviceDisplay.Width == 640)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 750)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 960)
                {
                    IsiOSName5s = true;
                }
            }
            #endregion

            ButtonExtended btn = new ButtonExtended
            {
                Text = btnText,
                BindingContext = mobileMenu,
                Margin = new Thickness(0, 5, 10, 0),
                WidthRequest = (IsiOSName5s == true ? 70 : 80),
                BackgroundColor = Color.FromHex(btnColorCode),
            };
            btn.Clicked += eventHandler;
            return btn;
        }

        public ButtonExtended GenerateComparativeAnalysisReportButton(EventHandler eventHandler, ChartDataModel chartDataModel, string btnText, string btnColorCode = "#467494")
        {
            #region iOS Apps Base Code
            bool IsiOSName5s = false;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var deviceName = DeviceInfo.Name;
                if (deviceName == AppConstant.iOSName5s)
                {
                    IsiOSName5s = true;
                }

                var deviceDisplay = DeviceDisplay.MainDisplayInfo;
                if (deviceDisplay.Width == 640)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 750)
                {
                    IsiOSName5s = true;
                }
                else if (deviceDisplay.Width == 960)
                {
                    IsiOSName5s = true;
                }
            }
            #endregion

            ButtonExtended btn = new ButtonExtended
            {
                Text = btnText,
                BindingContext = chartDataModel,
                Margin = new Thickness(0, 5, 10, 0),
                WidthRequest = (IsiOSName5s == true ? 70 : 80),
                BackgroundColor = Color.FromHex(btnColorCode),
            };
            btn.Clicked += eventHandler;
            return btn;
        }

        //public ButtonExtended GenerateBarChartButton(EventHandler eventHandler, string graphName = "")
        //{
        //    #region iOS Apps Base Code
        //    bool IsiOSName5s = false;
        //    if (Device.RuntimePlatform == Device.iOS)
        //    {
        //        var deviceName = DeviceInfo.Name;
        //        if (deviceName == AppConstant.iOSName5s)
        //        {
        //            IsiOSName5s = true;
        //        }

        //        var deviceDisplay = DeviceDisplay.MainDisplayInfo;
        //        if (deviceDisplay.Width == 640)
        //        {
        //            IsiOSName5s = true;
        //        }
        //        else if (deviceDisplay.Width == 750)
        //        {
        //            IsiOSName5s = true;
        //        }
        //        else if (deviceDisplay.Width == 960)
        //        {
        //            IsiOSName5s = true;
        //        }
        //    }
        //    #endregion

        //    ButtonExtended btnBar = new ButtonExtended
        //    {
        //        Text = "Bar",
        //        SelectedDataItem = graphName,
        //        Margin = new Thickness(0, 5, 20, 0),
        //        WidthRequest = (IsiOSName5s == true ? 70 : 80),
        //        BackgroundColor = Color.FromHex("#467494"),
        //    };
        //    btnBar.Clicked += eventHandler;
        //    return btnBar;
        //}

        //public ButtonExtended GenerateChartViewButton(EventHandler eventHandler, ProfessionalSurveyReportMobileMenu mobileMenu)
        //{
        //    #region iOS Apps Base Code
        //    bool IsiOSName5s = false;
        //    if (Device.RuntimePlatform == Device.iOS)
        //    {
        //        var deviceName = DeviceInfo.Name;
        //        if (deviceName == AppConstant.iOSName5s)
        //        {
        //            IsiOSName5s = true;
        //        }

        //        var deviceDisplay = DeviceDisplay.MainDisplayInfo;
        //        if (deviceDisplay.Width == 640)
        //        {
        //            IsiOSName5s = true;
        //        }
        //        else if (deviceDisplay.Width == 750)
        //        {
        //            IsiOSName5s = true;
        //        }
        //        else if (deviceDisplay.Width == 960)
        //        {
        //            IsiOSName5s = true;
        //        }
        //    }
        //    #endregion

        //    ButtonExtended btnChart = new ButtonExtended
        //    {
        //        Text = "Chart",
        //        BindingContext = mobileMenu,
        //        Margin = new Thickness(0, 5, 0, 0),
        //        WidthRequest = (IsiOSName5s == true ? 70 : 80),
        //        BackgroundColor = Color.FromHex("#467494"),
        //    };
        //    btnChart.Clicked += eventHandler;
        //    return btnChart;
        //}

        //public ButtonExtended GenerateGraphViewButton(EventHandler eventHandler, ProfessionalSurveyReportMobileMenu mobileMenu)
        //{
        //    #region iOS Apps Base Code
        //    bool IsiOSName5s = false;
        //    if (Device.RuntimePlatform == Device.iOS)
        //    {
        //        var deviceName = DeviceInfo.Name;
        //        if (deviceName == AppConstant.iOSName5s)
        //        {
        //            IsiOSName5s = true;
        //        }

        //        var deviceDisplay = DeviceDisplay.MainDisplayInfo;
        //        if (deviceDisplay.Width == 640)
        //        {
        //            IsiOSName5s = true;
        //        }
        //        else if (deviceDisplay.Width == 750)
        //        {
        //            IsiOSName5s = true;
        //        }
        //        else if (deviceDisplay.Width == 960)
        //        {
        //            IsiOSName5s = true;
        //        }
        //    }
        //    #endregion

        //    ButtonExtended btnChart = new ButtonExtended
        //    {
        //        Text = "Graph",
        //        BindingContext = mobileMenu,
        //        Margin = new Thickness(0, 5, 0, 0),
        //        WidthRequest = (IsiOSName5s == true ? 70 : 80),
        //        BackgroundColor = Color.FromHex("#467494"),
        //    };
        //    btnChart.Clicked += eventHandler;
        //    return btnChart;
        //}
    }
}
