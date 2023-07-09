using Acr.UserDialogs;
using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;

using OntrackHealthApp.AppCore;
using OntrackHealthApp.ChartHelper.Models;
using OntrackHealthApp.ChartHelper.ViewModels;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.PatientProgressReportGraph.RestApiService
{
    public class PatientProgressReportApiService
    {
        private ITokenContainer tokenContainer;
        public PatientProgressReportApiService()
        {
            tokenContainer = new TokenContainer();
        }
        private HttpClient InitializeHttpClient()
        {
            var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
            //httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.MaxResponseContentBufferSize = 9999999;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            return httpClient;
        }

        #region Actions

        public async Task<PatientProgressReportViewModel> GetGraphReport(string patientProcedureDetailId)
        {
            PatientProgressReportViewModel patientProgressReportViewModelList = new PatientProgressReportViewModel();
            try
            {
                var restUrl = $"api/PatientProgressReportGraph/GetGraphReport/" + patientProcedureDetailId;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientProgressReportViewModelList = JsonConvert.DeserializeObject<PatientProgressReportViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return patientProgressReportViewModelList;
        }

        public async Task<ChartDataModel> GetPatientProgressGraphReport(string patientChartUrl, PatientProgressReportGraphRequestViewModel requestModel)
        {
            ChartDataModel chartData = null;
            try
            {                
                var json = JsonConvert.SerializeObject(requestModel);
                var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var uri = patientChartUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        chartData = JsonConvert.DeserializeObject<ChartDataModel>(result);
                        if (chartData.PatientData == null)
                        {
                            chartData.PatientData = new PatientDataModel();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return chartData;
        }

        public async Task<ChartDataModel> GetTempGraphReportData()
        {
            string patientChartUrl = "https://ontrack-healthdemo.com/webapi/v3/api/Account/GetTempGraphReportData";
            ChartDataModel chartData = null;
            try
            {

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(patientChartUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        chartData = JsonConvert.DeserializeObject<ChartDataModel>(result);
                        if (chartData.PatientData == null)
                        {
                            chartData.PatientData = new PatientDataModel();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return chartData;
        }

        #endregion
    }
}
