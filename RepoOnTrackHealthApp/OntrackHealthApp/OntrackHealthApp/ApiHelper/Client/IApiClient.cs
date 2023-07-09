using OntrackHealthApp.ApiHelper.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiHelper.Client
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel;
        Task<HttpResponseMessage> PostJsonEncodedContentAsync<T>(string requestUri, T model) where T : ApiModel;
    }
}
