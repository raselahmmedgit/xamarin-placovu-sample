using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiHelper.Client
{
    public class ApiClient : IApiClient
    {
        public readonly HttpClient httpClient;
        private readonly ITokenContainer _iTokenContainer;

        public ApiClient() { }

        public ApiClient(HttpClient httpClient, ITokenContainer iTokenContainer)
        {
            this.httpClient = httpClient;
            this._iTokenContainer = iTokenContainer;
        }

        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            AddToken();
            requestUri = requestUri.ToAbsoluteUrl();
            using (var content = new FormUrlEncodedContent(values))
            {
                var query = await content.ReadAsStringAsync();
                string requestUriWithQuery = string.Empty;

                if (string.IsNullOrEmpty(query))
                {
                    requestUriWithQuery = string.Concat(requestUri);
                }
                else
                {
                    requestUriWithQuery = string.Concat(requestUri, "?", query);
                }
                var response = await httpClient.GetAsync(requestUriWithQuery);
                return response;
            }
        }

        public async Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            requestUri = requestUri.ToAbsoluteUrl();
            using (var content = new FormUrlEncodedContent(values))
            {
                var response = await httpClient.PostAsync(requestUri, content);
                return response;
            }
        }

        public async Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel
        {
            requestUri = requestUri.ToAbsoluteUrl();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AddToken();
            var response = await httpClient.PostAsJsonAsync(requestUri, content);
            return response;
        }

        public async Task<HttpResponseMessage> PostJsonEncodedContentAsync<T>(string requestUri, T model) where T : ApiModel
        {
            string connectionUrl = requestUri.ToAbsoluteUrl();
            var jsonObj = JsonConvert.SerializeObject(model);
            string dataResult = string.Empty;

            using (var client = new HttpClient(HttpClientHelper.InitializeHttpClientHandler()) { BaseAddress = new System.Uri(AppCore.AppConstant.BaseAddress) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");
                AddToken(client);
                var response = await client.PostAsync(connectionUrl, content);
                return response;
            }
        }

        public void AddToken()
        {
            if (_iTokenContainer.IsApiToken())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _iTokenContainer.ApiToken.ToString());
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);
            }
        }
        public void AddToken(HttpClient client)
        {
            if (_iTokenContainer.IsApiToken())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _iTokenContainer.ApiToken.ToString());
            }
            else
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", null);
            }
        }
    }
}
