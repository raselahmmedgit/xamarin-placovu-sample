using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.Response;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class DischargeNotificationClient : ClientBase, IDischargeNotificationClient
    {
        private readonly ITokenContainer _iTokenContainer;
        public DischargeNotificationClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
        }

        public async Task<DischargeNotificationResponse> GetDischargeNotificationAsync()
        {
            string url = "api/DischargeNotification/DischargeInformationNotification/";
            var response = await GetJsonDecodedContent<DischargeNotificationResponse, PatientDischargeNotificationModel>(url);
            return response;
        }        
    }

    public interface IDischargeNotificationClient
    {
        Task<DischargeNotificationResponse> GetDischargeNotificationAsync();
    }
}
