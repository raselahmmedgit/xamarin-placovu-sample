using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Response;
using OntrackHealthApp.ApiService.ViewModel;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class LocationClient : ClientBase, ILocationClient
    {
        private readonly ITokenContainer _iTokenContainer;
        private string LocationPageUri = string.Empty;

        public LocationClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
            LocationPageUri = "api/PatientProfile/Location/" + _iTokenContainer.CurrentPatientProcedureDetailId;
        }
        
        public async Task<LocationPageResponse> LocationPage()
        {
            return await GetJsonDecodedContent<LocationPageResponse, LocationIndexPageViewModel>(LocationPageUri);
        }
    }

    public interface ILocationClient
    {
        Task<LocationPageResponse> LocationPage();
    }
}
