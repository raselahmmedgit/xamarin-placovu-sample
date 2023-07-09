using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ChartHelper.Models;
using OntrackHealthApp.ChartHelper.ViewModels;
using OntrackHealthApp.PatientProgressReportGraph.RestApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OntrackHealthApp.ViewModel
{
    public class PatientProgressReportGraphPageViewModel : ViewModelBase
    {
        public enum ChartType { Bar = 1, Line = 2 };

        private readonly ITokenContainer _iTokenContainer;
        private PatientProgressReportApiService restApiService;
        public PatientProgressReportViewModel PatientProgressReportViewModel { get; set; }
        public ChartDataModel _chartDataModel = null;
        public List<string> GraphHtmlContents { get; private set; } = new List<string>();
        public bool HasPatientProgressReport { get; set; }

        public PatientProgressReportGraphPageViewModel()
        {
            _iTokenContainer = new TokenContainer();
            PatientProgressReportViewModel = new PatientProgressReportViewModel();


            restApiService = new PatientProgressReportApiService();
        }

        public async Task<PatientProgressReportViewModel> GeneratePatientProgressGraph(PatientProgressReportViewModel patientProgressReportViewModel, ChartType chartType)
        {
            try
            {
                string _chartType = chartType.ToString().ToLower();
                foreach (var progressReport in patientProgressReportViewModel.PatientProgressReportGraphHistoryViews)
                {
                    PatientProgressReportGraphRequestViewModel requestModel = new PatientProgressReportGraphRequestViewModel
                    {
                        GraphId = progressReport.PatientEmailTemplateGraphId,
                        IsPatientProgressReport = true,
                        IsPatientSelected = true,
                        IsPhysicianPatientSelected = true,
                        chartType = "line",
                        PatientProcedureDetailId = patientProgressReportViewModel.PatientProcedureDetailId,
                        PatientProfileId = patientProgressReportViewModel.PatientProfileId
                    };
                    string patientChartUrl = $"api/PatientProgressReportGraph/" + progressReport.GraphAction;

                    var response = await restApiService.GetPatientProgressGraphReport(patientChartUrl, requestModel);
                    if (response != null)
                    {
                        _chartDataModel = new ChartDataModel();
                        _chartDataModel = response;
                        BuildReportHtml(_chartType);
                        progressReport.PatientEmailTemplateGraphHtml = this.ReportHtml;
                        progressReport.ChartDataModel = response;
                    }
                    else
                    {
                        BuildReportHtml(_chartType);
                        await Task.Yield();
                    }

                }
                return patientProgressReportViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PatientProgressReportGraphHistoryViewModel GeneratePatientProgressGraphSingle(string GraphName, ChartType chartType)
        {
            var progressReport = new PatientProgressReportGraphHistoryViewModel();
            try
            {
                string _chartType = chartType.ToString().ToLower();
                if (PatientProgressReportViewModel != null && PatientProgressReportViewModel.PatientProgressReportGraphHistoryViews != null)
                {
                    progressReport = PatientProgressReportViewModel.PatientProgressReportGraphHistoryViews.FirstOrDefault(x => x.GraphName == GraphName);
                    _chartDataModel = new ChartDataModel();
                    _chartDataModel = progressReport.ChartDataModel;
                    BuildReportHtml(_chartType);
                    progressReport.PatientEmailTemplateGraphHtml = this.ReportHtml;
                }
                return progressReport;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var patientProgressReportViewModel = await restApiService.GetGraphReport(_iTokenContainer.CurrentPatientProcedureDetailId);
                if (patientProgressReportViewModel != null)
                {
                    if (string.IsNullOrEmpty(patientProgressReportViewModel.PatientEmailTemplateSalutation)
                         && string.IsNullOrEmpty(patientProgressReportViewModel.PatientEmailTemplateIntroduction)
                         && string.IsNullOrEmpty(patientProgressReportViewModel.PatientEmailTemplateConclusion)
                         )
                    {
                        HasPatientProgressReport = false;
                    }
                    else
                    {
                        PatientProgressReportViewModel = patientProgressReportViewModel;
                        HasPatientProgressReport = true;
                        await GeneratePatientProgressGraph(PatientProgressReportViewModel, ChartType.Line);
                    }
                }
                else
                {
                    ErrorMessage = AppConstant.DisplayAlertErrorMessage;
                }

            }
            catch (Exception)
            {
                ErrorMessage = AppConstant.DisplayAlertErrorMessage;
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
            ChartGenerator chartGenerator = new ChartGenerator(_chartDataModel, chartType);
            ReportHtml = chartGenerator.GenerateChart();
        }
    }
}
