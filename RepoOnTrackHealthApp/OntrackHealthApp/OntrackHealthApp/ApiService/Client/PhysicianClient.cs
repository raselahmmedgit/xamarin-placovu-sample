using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Response;
using OntrackHealthApp.ApiService.ViewModel;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class PhysicianClient : ClientBase, IPhysicianClient
    {
        private readonly ITokenContainer _iTokenContainer;
        private string PhysicianPageUri = string.Empty;

        public PhysicianClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
            PhysicianPageUri = "/api/PatientProfile/Physician/" + _iTokenContainer.CurrentPatientProcedureDetailId;
        }
        
        public async Task<PhysicianPageResponse> PhysicianPage()
        {
            return await GetJsonDecodedContent<PhysicianPageResponse, PhysicianIndexPageViewModel>(PhysicianPageUri);
        }
    }

    public interface IPhysicianClient
    {
        Task<PhysicianPageResponse> PhysicianPage();
    }
}
