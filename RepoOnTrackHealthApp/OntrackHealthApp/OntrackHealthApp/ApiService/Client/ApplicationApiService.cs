using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
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
    public class ApplicationApiService
    {
        private ITokenContainer tokenContainer;

        public ApplicationApiService()
        {
            tokenContainer = new TokenContainer();
        }
        private HttpClient InitializeHttpClient()
        {
            var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
            //httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.MaxResponseContentBufferSize = 9999999;
            return httpClient;
        }
        private HttpClient InitializeHttpClientWithToken()
        {
            var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
            //httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.MaxResponseContentBufferSize = 9999999;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            return httpClient;
        }
        public async Task<AppAndroidReleaseHistory> GetAppAndroidReleaseHistory()
        {
            AppAndroidReleaseHistory appAndroidReleaseHistory = new AppAndroidReleaseHistory();
            try
            {
                var restUrl = "api/ReleaseHistory/GetAppAndroidLastReleaseInfo";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        appAndroidReleaseHistory = JsonConvert.DeserializeObject<AppAndroidReleaseHistory>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return appAndroidReleaseHistory;
        }

        public async Task<AppIosReleaseHistory> GetAppIosReleaseHistory()
        {
            AppIosReleaseHistory appIosReleaseHistory = new AppIosReleaseHistory();
            try
            {
                var restUrl = "api/ReleaseHistory/GetAppIosLastReleaseInfo/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        appIosReleaseHistory = JsonConvert.DeserializeObject<AppIosReleaseHistory>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return appIosReleaseHistory;
        }

        public async Task<bool> UpdateUserLoginTimeZone()
        {
            try
            {
                var restUrl = "api/Account/LoginUserIdentityWithTimeZone";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClientWithToken())
                {
                    string timeZone = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ? TimeZoneInfo.Local.DaylightName : TimeZoneInfo.Local.StandardName;
                    var json = "{timeZoneId:'" + timeZone + "'}";
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return false;
        }


        public async Task<bool> RegisterPushNotificationDeviceToken(PatientProfileDeviceToken patientProfileDeviceToken)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/PatientProfile/RegisterPushNotificationDeviceToken";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClientWithToken())
                {
                    var json = JsonConvert.SerializeObject(patientProfileDeviceToken);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        apiExecutionResult.Message = AppConstant.PatientDeviceTokenSaveSuccessMessage;
                        apiExecutionResult.Success = true;
                    }
                    else
                    {
                        apiExecutionResult.Message = AppConstant.PatientDeviceTokenSaveErrorMessage;
                        apiExecutionResult.Success = false;
                    }
                    return apiExecutionResult.Success;
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return false;
        }

        public async Task<bool> MarkedPatientProfileDeviceTokenAsExpired(PatientProfileDeviceToken patientProfileDeviceToken)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/PatientProfile/MarkedPatientProfileDeviceTokenAsExpired";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClientWithToken())
                {
                    var json = JsonConvert.SerializeObject(patientProfileDeviceToken);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        apiExecutionResult.Message = AppConstant.UpdateSuccessMessage;
                        apiExecutionResult.Success = true;
                    }
                    else
                    {
                        apiExecutionResult.Message = AppConstant.UpdateInformationMessage;
                        apiExecutionResult.Success = false;
                    }
                    return apiExecutionResult.Success;
                } 
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return false;
        }
    }
}
