using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Response;
using OntrackHealthApp.ViewModel;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class HospitalInfoClient : ClientBase, IHospitalInfoClient
    {
        private readonly ITokenContainer _iTokenContainer;
        public HospitalInfoClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();            
        }
        
        public async Task<HospitalInfoPageResponse> HospitalInfoPage()
        {
            string hospitalInfoPageUrl = "api/PatientProfile/HospitalInfo/" + _iTokenContainer.CurrentPatientProcedureDetailId;
            return await GetJsonDecodedContent<HospitalInfoPageResponse, SurgicalConciergeDocumentViewModel>(hospitalInfoPageUrl);
        }
    }

    public interface IHospitalInfoClient
    {
        Task<HospitalInfoPageResponse> HospitalInfoPage();
    }
}
