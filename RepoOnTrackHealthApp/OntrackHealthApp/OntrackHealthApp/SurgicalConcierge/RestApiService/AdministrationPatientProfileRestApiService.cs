using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.SurgicalConcierge.RestApiService
{
    public class AdministrationPatientProfileRestApiService
    {
        private ITokenContainer tokenContainer;
        private HttpClient InitializeHttpClient()
        {
            var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
            //httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.MaxResponseContentBufferSize = 9999999;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            return httpClient;
        }
        public AdministrationPatientProfileRestApiService()
        {
            tokenContainer = new TokenContainer();
        }
        public async Task<PracticeProcedureLocationDropDownViewModel> GetPracticeProcedureLocationDropdown(long practiceProfileId, long? procedureId)
        {
            PracticeProcedureLocationDropDownViewModel practiceProcedureLocationDropDownViewModel = new PracticeProcedureLocationDropDownViewModel();
            try
            {
                var restUrl = $"api/AdministrationPatientProfile/GetPracticeProcedureLocationDropdown?practiceProfileId={practiceProfileId}/";
                if (procedureId.HasValue)
                    restUrl += "&procedureId=" + procedureId.Value;
                else
                    restUrl += "&procedureId=null";

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        practiceProcedureLocationDropDownViewModel = JsonConvert.DeserializeObject<PracticeProcedureLocationDropDownViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return practiceProcedureLocationDropDownViewModel;
        }
        public async Task<ProfessionalProcedureDropDownViewModel> GetPracticeProcedureDropdown(long practiceProfileId)
        {
            ProfessionalProcedureDropDownViewModel professionalProcedureDropDownViewModel = new ProfessionalProcedureDropDownViewModel();
            try
            {
                var restUrl = $"api/AdministrationPatientProfile/GetPracticeProcedureDropdown?practiceProfileId={practiceProfileId}/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        professionalProcedureDropDownViewModel = JsonConvert.DeserializeObject<ProfessionalProcedureDropDownViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalProcedureDropDownViewModel;
        }

        public async Task<ProfessionalProfileDropDownViewModel> GetPracticeProfessionalProfileDropdown(long practiceProfileId)
        {
            ProfessionalProfileDropDownViewModel professionalProfileDropDownViewModel = new ProfessionalProfileDropDownViewModel();
            try
            {
                var restUrl = $"api/AdministrationPatientProfile/GetPracticeProfessionalProfileDropdown?practiceProfileId={practiceProfileId}/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        professionalProfileDropDownViewModel = JsonConvert.DeserializeObject<ProfessionalProfileDropDownViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalProfileDropDownViewModel;
        }
        
        #region Paitent Update
        public async Task<PatientProfile> GetPatientProfile(long PatientProfileId)
        {
            PatientProfile patient = new PatientProfile();
            try
            {
                var restUrl = $"api/PatientProfile/UpdatePatientProfile/{PatientProfileId}/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patient = JsonConvert.DeserializeObject<PatientProfile>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return patient;
        }

        public async Task<ApiExecutionResult<PatientProfile>> PostPatientProfile(PatientProfile patient)
        {
            ApiExecutionResult<PatientProfile> apiExecutionResult = new ApiExecutionResult<PatientProfile>();
            try
            {
                var restUrl = "api/PatientProfile/UpdatePatientProfile/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(patient);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<PatientProfile>>(result);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }
        #endregion

        #region Patient Monitoring Consent
        public async Task<bool> PatientMonitoringConsentIsAgreed(string userId)
        {
            bool isAgreePatientMonitoringConsent = false;
            try
            {
                var restUrl = $"api/MonitoringConsent/MonitoringConsentIsAgreed/?userId={userId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        isAgreePatientMonitoringConsent = Convert.ToBoolean(result);
                    }
                }

                return isAgreePatientMonitoringConsent;
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return isAgreePatientMonitoringConsent;
        }

        public async Task<bool> UpdateMonitoringConsent(string userId, bool agree)
        {
            bool isUpdateResult = false;
            try
            {
                var values = new Dictionary<string, string>
                {
                   { "userId", userId+"" },
                   {"agree",agree.ToString() }
                };
                var content = new FormUrlEncodedContent(values);

                var restUrl = "api/MonitoringConsent/UpdateMonitoringConsent/"
                    + "?userId=" + userId
                     + "&agree=" + agree;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        isUpdateResult = Convert.ToBoolean(result);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return isUpdateResult;
        }
        #endregion
    }
}
