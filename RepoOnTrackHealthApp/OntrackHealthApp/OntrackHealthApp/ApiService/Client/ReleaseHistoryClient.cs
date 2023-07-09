using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.Response;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class ReleaseHistoryClient : ClientBase, IReleaseHistoryClient
    {
        public ReleaseHistoryClient(IApiClient iApiClient) : base(iApiClient)
        {
            //
        }

        private const string TokenUri = "/Token";
        private const string AndroidLastReleaseInfoUri = "api/ReleaseHistory/GetAppAndroidLastReleaseInfo";
        private readonly string IosLastReleaseInfoUri = "api/ReleaseHistory/GetAppIosLastReleaseInfo";

        public async Task<AppAndroidReleaseHistory> GetAppAndroidLastReleaseInfoAsync()
        {
            var response = await GetJsonDecodedContent<AppAndroidReleaseHistoryResponse, AppAndroidReleaseHistory>(AndroidLastReleaseInfoUri);
            if (response.StatusIsSuccessful)
            {
               return response.Data;
            }
            return null;
        }

        public async Task<AppIosReleaseHistory> GetAppIosLastReleaseInfoAsync()
        {
            var response = await GetJsonDecodedContent<AppIosReleaseHistoryResponse, AppIosReleaseHistory>(IosLastReleaseInfoUri);
            if (response.StatusIsSuccessful)
            {
                return response.Data;
            }
            return null;
        }
    }
    public interface IReleaseHistoryClient
    {
        Task<AppAndroidReleaseHistory> GetAppAndroidLastReleaseInfoAsync();
        Task<AppIosReleaseHistory> GetAppIosLastReleaseInfoAsync();
    }
}
