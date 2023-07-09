using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.ApiService.Client
{
    public class PatientSurveyClientNew
    {
        private ITokenContainer tokenContainer;
        public PatientSurveyClientNew()
        {
            tokenContainer = new TokenContainer();
        }
        public async Task<ApiExecutionResult> PostPatientSurvey(PatientSurveyProcedureNotificationCreateModel createModel)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/PatientSchedule/CreatePatientSurveyNew";
                var uri = restUrl.ToAbsoluteUri();
                var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
                //httpClient.MaxResponseContentBufferSize = 556000;
                httpClient.MaxResponseContentBufferSize = 9999999;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());

                var json = JsonConvert.SerializeObject(createModel);
                var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult>(result);
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }
    }
}
