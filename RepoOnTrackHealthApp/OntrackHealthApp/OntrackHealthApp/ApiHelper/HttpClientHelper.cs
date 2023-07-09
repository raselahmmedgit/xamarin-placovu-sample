using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OntrackHealthApp.ApiHelper
{
    public static class HttpClientHelper
    {
        public static CookieContainer cookieContainer = new CookieContainer();
        public static HttpMessageHandler InitializeHttpClientHandler()
        {
            cookieContainer.SetCookies(new Uri(AppConstant.BaseAddress), "ApplicationGatewayAffinity=d960ee4d56eca906ff7458cf139d99fb, ApplicationGatewayAffinityCORS=d960ee4d56eca906ff7458cf139d99fb");
            HttpMessageHandler httpClientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                AllowAutoRedirect = false,
                CookieContainer = cookieContainer
            };
            return httpClientHandler;
        }

        public static HttpClient InitializeHttpClient(ITokenContainer tokenContainer)
        {
            var httpClient = new HttpClient(InitializeHttpClientHandler());
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("Device", "Mobile");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            return httpClient;
        }
    }
}
