using Acr.UserDialogs;
using GraphDemo.Chart;
using GraphDemo.Chart.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GraphDemo
{
    public class ChartReportPageViewModel
    {
        public string ReportHtml { set; get; }
        private HttpClient _client;
        private ChartData chartData;
        private string chartType;

        public ChartReportPageViewModel(string chartType, string url)
        {
            _client = new HttpClient();
            this.chartType = chartType;
            PopulateChart(url);
            //BuildReportHtml();
        }
        private async void PopulateChart(string url)
        {
            chartData = await GetChartDataFromUrl(url);
        }

        private async Task<ChartData> GetChartDataFromUrl(string url)
        {
            try
            {
                ChartData chartData = null;
                var uri = new Uri(string.Format(url, string.Empty));
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    chartData = JsonConvert.DeserializeObject<ChartData>(content);
                    await Task.Delay(10000);
                    if (chartData.PatientData == null)
                    {
                        chartData.PatientData = new PatientData();
                        this.chartData = chartData;
                        BuildReportHtml();
                    }
                }

                return chartData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void BuildReportHtml()
        {
            var chartConfigScript = GetChartScript(chartType);
            var html = GetHtmlWithChartConfig(chartConfigScript);
            ReportHtml = html;            
        }

        private string GetHtmlWithChartConfig(string chartConfig)
        {
            var inlineStyle = "style=\"width:100%;height:100%;overflow:auto;\"";
            var chartJsScript = "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.bundle.min.js\"></script>";
            var chartConfigJsScript = $"<script>{chartConfig}</script>";
            var chartContent = $@"<div id=""chart-container"" {inlineStyle}>
                              <canvas id=""chart""></canvas>
                                </div>";
            var document = $@"<html style=""width:97%;height:100%;"">
                              <head>{chartJsScript}</head>
                              <body {inlineStyle}>
                                {chartContent}
                                {chartConfigJsScript}
                              </body>
                              </html>";
            return document;
        }

        private string GetChartScript(string chartType)
        {
            var chartConfig = GetSpendingChartConfig(chartType);
            var script = $@"var config = {chartConfig};
                        window.onload = function() {{
                          var canvasContext = document.getElementById(""chart"").getContext(""2d"");
                          new Chart(canvasContext, config);
                        }};";
            return script;
        }

        private string GetSpendingChartConfig(string chartType)
        {            
            var config = new
            {
                type = chartType,
                data = GetChartDataTemp(),
                options = new
                {
                    responsive = true,
                    maintainAspectRatio = false,
                    legend = new
                    {
                        position = "top"
                    },
                    animation = new
                    {
                        animateScale = true
                    }
                }
            };
            var jsonConfig = JsonConvert.SerializeObject(config);
            return jsonConfig;
        }

        private object GetChartDataTempo()
        {
            var colors = GetDefaultColors();
            var labels = new[] { "PreOp", "2 Month", "3 Month", "6 Month", "9 Month", "12 Month", "18 Month", "24 Month", "30 Month", "36 Month", "42 Month", "48 Month", "54 Month", "60 Month" };
            var randomGen = new Random();
            var dataPoints1 = new[]{ 18.0, 25.0, 25.0, 25.0, 25.0, 25.0, 24.0, 25.0, 25.0, 25.0, 25.0, 22.0, 25.0, 25.0 };
            var dataPoints2 = new[] { 15.8, 15.73, 15.67, 15.92, 15.23, 15.67, 15.88, 18.1, 16.4, 14.45, 18.0, 15.92, 17.91, 15.86 };
            var data = new
            {
                datasets = new[]
                {
                    new
                    {
                        label = "Spending",
                        data = dataPoints1,
                        backgroundColor = "blue",
                        fill = false,
                        borderColor = "blue",
                        borderWidth = 2,
                        lineTension = 0,
                    },
                    new
                    {
                        label = "My Spending",
                        data = dataPoints2,
                        backgroundColor = "orange",
                        fill = false,
                        borderColor = "orange",
                        borderWidth = 2,
                        lineTension = 0

                    }
                },
                labels
            };
            return data;
        }

        private object GetChartData()
        {
            var colors = GetDefaultColors();
            var labels = new[] { "Groceries", "Car", "Flat", "Electronics", "Entertainment", "Insurance" };
            var randomGen = new Random();
            var dataPoints1 = Enumerable.Range(0, labels.Length)
                .Select(i => randomGen.Next(5, 25))
                .ToList();
            var dataPoints2 = Enumerable.Range(0, labels.Length)
                .Select(i => randomGen.Next(5, 25))
                .ToList();
            var data = new
            {
                datasets = new[]
                {
                    new
                    {
                        label = "Spending",
                        data = dataPoints1,
                        backgroundColor = "blue",
                        fill = false,
                        borderColor = "blue",
                        borderWidth = 2,
                        lineTension = 0,
                    },
                    new
                    {
                        label = "My Spending",
                        data = dataPoints2,
                        backgroundColor = "orange",
                        fill = false,
                        borderColor = "orange",
                        borderWidth = 2,
                        lineTension = 0

                    }
                },
                labels
            };
            return data;
        }
        private object GetData()
        {
            var data = new List<object>();
            chartData.Series.ForEach(c =>
            {
                data.Add(new
                {
                    label = c.LabelName,
                    data = c.DataList,
                    fill = c.GraphProperty.Fill,
                    backgroundColor = c.GraphProperty.BackgroundColor,
                    pointRadius = c.GraphProperty.PointRadius,
                    borderColor = c.GraphProperty.BorderColor,
                    pointBorderColor = c.GraphProperty.PointBorderColor,
                    pointBackgroundColor = c.GraphProperty.PointBackgroundColor,
                    pointBorderWidth = c.GraphProperty.PointBorderWidth,
                    lineTension = c.GraphProperty.LineTension
                });
            });

            return data;
        }
        private object GetChartDataTemp()
        {
            var labels = chartData.Labels;
            var data = new
            {
                datasets = GetData(),
                labels
            };
            return data;
        }

        private List<Tuple<int, int, int>> GetDefaultColors()
        {
            return new List<Tuple<int, int, int>>
            {
                new Tuple<int, int, int>(255, 99, 132),
                new Tuple<int, int, int>(255, 159, 64),
                new Tuple<int, int, int>(255, 205, 86),
                new Tuple<int, int, int>(75, 192, 192),
                new Tuple<int, int, int>(54, 162, 235),
                new Tuple<int, int, int>(153, 102, 255),
                new Tuple<int, int, int>(201, 203, 207)
            };
        }
    }
}
