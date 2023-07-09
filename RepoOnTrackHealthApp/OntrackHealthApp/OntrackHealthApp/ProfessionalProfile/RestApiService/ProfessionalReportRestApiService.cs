using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ChartHelper.Models;
using OntrackHealthApp.ChartHelper.ViewModels;
using OntrackHealthApp.PatientOutComeReport.ViewModel;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.ProfessionalProfile.RestApiService
{
    public class ProfessionalReportRestApiService
    {
        private ITokenContainer tokenContainer;
        public ProfessionalReportRestApiService()
        {
            tokenContainer = new TokenContainer();
        }

        public async Task<MobileSurveyReportViewModel> GetMobileSurveyReportViewModel()
        {
            MobileSurveyReportViewModel reportViewModel = new MobileSurveyReportViewModel();
            try
            {
                var restUrl = "api/MobileSurveyReport/Index";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = HttpClientHelper.InitializeHttpClient(tokenContainer))
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        reportViewModel = JsonConvert.DeserializeObject<MobileSurveyReportViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return reportViewModel;
        }

        public async Task<MobileSurveyReportViewModel> OutComeChartReport(ProfessionalSurveyReportMobileMenu model, PatientReportedOutcomePatientViewModel patientReportedOutcomePatientViewModel)
        {
            MobileSurveyReportViewModel mobileSurveyReportViewModel = new MobileSurveyReportViewModel();
            try
            {
                var restUrl = $"api/{model.ControllerName}/{model.ActionName}";
                var uri = restUrl.ToAbsoluteUri();
                var requestModel = new GraphFilterCriteriaViewModel
                {
                    PatientProcedureDetailId = patientReportedOutcomePatientViewModel.PatientProcedureDetailId,
                    PatientProfileId = patientReportedOutcomePatientViewModel.PatientProfileId,
                    IsPatientSelected = patientReportedOutcomePatientViewModel.PatientProfileId > 0,
                    IsPracticePatientSelected = true,
                    IsPhysicianPatientSelected = true,
                    IsCompactViewGraph = model.IsCompactViewGraph
                };
                using (var httpClient = HttpClientHelper.InitializeHttpClient(tokenContainer))
                {
                    var json = JsonConvert.SerializeObject(requestModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        mobileSurveyReportViewModel = JsonConvert.DeserializeObject<MobileSurveyReportViewModel>(result);

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return mobileSurveyReportViewModel;
        }

        public async Task<RadicalNephrectomyTrendsViewModel> GetNephrotectomyTabularReportData(object url)
        {
            RadicalNephrectomyTrendsViewModel radicalNephrectomyTrendsViewModel = new RadicalNephrectomyTrendsViewModel();
            try
            {
                var restUrl = $"api/"+url;
                var uri = restUrl.ToAbsoluteUri();
                var requestModel = new GraphFilterCriteriaViewModel
                {
                    IsPhysicianPatientSelected = true,
                    IsPracticePatientSelected = true,
                    ChartType = "line"
                };
                using (var httpClient = HttpClientHelper.InitializeHttpClient(tokenContainer))
                {
                    var json = JsonConvert.SerializeObject(requestModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        radicalNephrectomyTrendsViewModel = JsonConvert.DeserializeObject<RadicalNephrectomyTrendsViewModel>(result);

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return radicalNephrectomyTrendsViewModel;
        }

        public async Task<ChartDataModel> OutComeGraphReport(ProfessionalSurveyReportMobileMenu model, PatientReportedOutcomePatientViewModel patientReportedOutcomePatientViewModel)
        {
            ChartDataModel chartDataModel = new ChartDataModel();
            try
            {
                var restUrl = $"api/{model.GraphReportController}/{model.GraphReportAction}";
                var uri = restUrl.ToAbsoluteUri();
                var requestModel = new GraphFilterCriteriaViewModel
                {
                    PatientProcedureDetailId = patientReportedOutcomePatientViewModel.PatientProcedureDetailId,
                    PatientProfileId = patientReportedOutcomePatientViewModel.PatientProfileId,
                    IsPatientSelected = patientReportedOutcomePatientViewModel.PatientProfileId > 0,
                    IsPhysicianPatientSelected = model.PhysicianPatientOutCome,
                    IsPracticePatientSelected = model.HospitalReportedOutCome,
                    ChartType = "line",
                    IsCompactViewGraph = model.IsCompactViewGraph
                };
                using (var httpClient = HttpClientHelper.InitializeHttpClient(tokenContainer))
                {
                    var json = JsonConvert.SerializeObject(requestModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        chartDataModel = JsonConvert.DeserializeObject<ChartDataModel>(result);

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return chartDataModel;
        }

        public async Task<ChartDataModel> ComparativeAnalysisGraphReport(string url, bool isPhysicianPatientSelected = true, bool isPracticePatientSelected = true)
        {
            ChartDataModel chartDataModel = null;
            try
            {
                var restUrl = $"api/{url}";
                var uri = restUrl.ToAbsoluteUri();
                var requestModel = new GraphFilterCriteriaViewModel
                {
                    IsPhysicianPatientSelected = isPhysicianPatientSelected,
                    IsPracticePatientSelected = isPracticePatientSelected,
                    ChartType = "line",
                };
                using (var httpClient = HttpClientHelper.InitializeHttpClient(tokenContainer))
                {
                    var json = JsonConvert.SerializeObject(requestModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        chartDataModel = JsonConvert.DeserializeObject<ChartDataModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return chartDataModel;
        }
    }
}
