using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ProfessionalProfile.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.ProfessionalProfile.RestApiService
{
    public class ProfessionalOutcomeRestApiService
    {
        private ITokenContainer tokenContainer;
        public ProfessionalOutcomeRestApiService()
        {
            tokenContainer = new TokenContainer();
        }

        private HttpClient InitializeHttpClient()
        {
            var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
            //httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.MaxResponseContentBufferSize = 9999999;
            httpClient.DefaultRequestHeaders.Add("Device", "Mobile");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            return httpClient;
        }

        // please procedureid = 0 ???
        public async Task<PatientAggregateSurveyReportModel> GetPatientSurveyActivity( long patientProfileId, string patientProcedureDetailId, bool isActiveNotification = false, long procedureId = 0)
        {
            PatientAggregateSurveyReportModel patientAggregateSurveyReportModel = new PatientAggregateSurveyReportModel();
            try
            {
                var restUrl = $"api/PatientReportedOutcome/GetPatientSurveyActivity"
                    + "?procedureId=" + procedureId // procedureid = 0
                    + "&patientProfileId=" + patientProfileId
                    + "&patientProcedureDetailId=" + patientProcedureDetailId
                    + "&isActiveNotification=" + isActiveNotification;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientAggregateSurveyReportModel = JsonConvert.DeserializeObject<PatientAggregateSurveyReportModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return patientAggregateSurveyReportModel;
        }

        // please procedureid = 0 ???
        public async Task<PatientAggregateSurveyReportForBphViewModel> GetPatientBphSurveyActivity(long patientProfileId, bool isActiveNotification = false, long procedureId = 0)
        {
            PatientAggregateSurveyReportForBphViewModel patientAggregateSurveyReportForBphViewModel = new PatientAggregateSurveyReportForBphViewModel();
            try
            {
                var restUrl = $"api/PatientReportedOutcome/GetPatientBphSurveyActivity"
                    + "?procedureId=" + procedureId // procedureid = 0
                    + "&patientProfileId=" + patientProfileId
                    + "&isActiveNotification" + isActiveNotification;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientAggregateSurveyReportForBphViewModel = JsonConvert.DeserializeObject<PatientAggregateSurveyReportForBphViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return patientAggregateSurveyReportForBphViewModel;
        }

        //procedureid = 0 ??
        public async Task<PatientAggregateSurveyReportModel> GetPatientBphSurveyActivityResult( long patientProfileId, string patientProcedureDetailId, bool isActiveNotification = false, long procedureId = 0)
        {
            PatientAggregateSurveyReportModel patientAggregateSurveyReportModel = new PatientAggregateSurveyReportModel();
            try
            {
                var restUrl = $"api/PatientReportedOutcome/GetPatientSurveyActivity"
                    + "?procedureId=" + procedureId // procedureid = 0
                    + "&patientProfileId=" + patientProfileId
                    + "&patientProcedureDetailId" + patientProcedureDetailId
                    + "&isActiveNotification" + isActiveNotification;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientAggregateSurveyReportModel = JsonConvert.DeserializeObject<PatientAggregateSurveyReportModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return patientAggregateSurveyReportModel;
        }

    }
}
