using OntrackHealthApp.ChartHelper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OntrackHealthApp.PatientProgressReportGraph.RestApiService;

namespace OntrackHealthApp.ChartHelper.ViewModels
{
    public class ChartReportPageViewModel : BaseViewModel
    {
        public bool IsSuccess = false;
        private string chartType = "line";
        public ChartDataModel ChartDataModel = null;

        private PatientProgressReportApiService patientProgressReportApiService;

        public Command LoadChartDataCommand { get; set; }
        public ChartReportPageViewModel()
        {
            patientProgressReportApiService = new PatientProgressReportApiService();
            LoadChartDataCommand = new Command(async () => await ExecuteLoadChartDataCommandAsync());
        }
        public ChartReportPageViewModel(string chartType)
        {
            this.chartType = chartType;
            patientProgressReportApiService = new PatientProgressReportApiService();
            LoadChartDataCommand = new Command(async () => await ExecuteLoadChartDataCommandAsync());
        }
        public async Task ExecuteLoadChartDataCommandAsync()
        {
            if (IsBusy) { return; }
            if (ChartDataModel != null) { return; }
            try
            {
                IsBusy = true;
                var response = await patientProgressReportApiService.GetTempGraphReportData();
                if (response != null)
                {
                    ChartDataModel = new ChartDataModel();
                    ChartDataModel = response;
                    BuildReportHtml(chartType);
                    //await Task.Yield();
                }
                else
                {
                    BuildReportHtml(chartType);
                    await Task.Yield();
                }
            }
            catch (Exception)
            {
                IsSuccess = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        string reporthtml = string.Empty;

        public string ReportHtml
        {
            get { return reporthtml; }
            set { SetProperty(ref reporthtml, value); }
        }

        public void BuildReportHtml(string chartType)
        {
            ChartGenerator chartGenerator = new ChartGenerator(ChartDataModel, chartType);
            ReportHtml = chartGenerator.GenerateChart();
        }
    }

    public class GraphFilterCriteriaViewModel
    {
        public string ChartType { get; set; }
        public int? MinimumAge { get; set; }
        public int? MaximumAge { get; set; }
        public string SurveyQuestionIds { get; set; }
        public decimal? MinScore { get; set; }
        public decimal? MaxScore { get; set; }
        public long? PatientProfileId { get; set; }
        public bool IsPatientSelected { set; get; }
        public bool IsPhysicianPatientSelected { set; get; }
        public bool IsPracticePatientSelected { set; get; }
        public bool IsSystemPatientSelected { set; get; }
        public long? GraphId { get; set; }
        public bool? IsPatientProgressReport { get; set; }
        public Guid? PatientProcedureDetailId { set; get; }
        public bool IsCompactViewGraph { get; set; }

    }
}
