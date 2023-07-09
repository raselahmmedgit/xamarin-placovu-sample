using OntrackHealthApp.AppCore;
using System;
using System.Net.Http;

namespace OntrackHealthApp.ApiHelper
{
    public static class HttpClientInstance
    {
        private const string BaseUri = AppConstant.BaseAddress;
        private static readonly HttpClient instance = new HttpClient(HttpClientHelper.InitializeHttpClientHandler()) { BaseAddress = new Uri(BaseUri) };

        public static HttpClient Instance
        {
            get { return instance; }
        }
    }
}
