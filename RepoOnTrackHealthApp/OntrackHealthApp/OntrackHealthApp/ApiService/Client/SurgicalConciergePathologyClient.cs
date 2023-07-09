
namespace OntrackHealthApp.ApiService.Client
{
    using OntrackHealthApp.ApiHelper;
    using OntrackHealthApp.ApiHelper.Client;
    using OntrackHealthApp.ApiService.Model;
    using OntrackHealthApp.ApiService.Response;
    using OntrackHealthApp.ApiService.ViewModel;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class SurgicalConciergePathologyClient : ClientBase, ISurgicalConciergePathologyClient
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly string urlPostPatientPathology = "api/SurgicalConciergePathology/CreatePatientPathology/";
        private readonly string urlGetPatientProfile = "api/SurgicalConciergePathology/GetPatientProfiles/";

        public SurgicalConciergePathologyClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
        }

        public async Task<SurgicalConciergePatientProfileResponse> GetPatientProfilesWithProfessionalProcedureView(int? pageNo, int? pageSize, string searchString)
        {
            var response = await GetJsonListDecodedContent<SurgicalConciergePatientProfileResponse, List<PatientProfileWithProfessionalProcedureView>>(urlGetPatientProfile, "pageNo".AsPair(pageNo?.ToString()), "pageSize".AsPair(pageSize?.ToString()), "searchString".AsPair(searchString));
            return response;
        }

        public async Task<SurgicalConciergePathologyResponse> PostSurgicalConciergePatientPathologyAsync(ScgPatientPathologyViewModel scgPatientPathologyViewModel)
        {
            var response = await PostJsonEncodedContentAsync<SurgicalConciergePathologyResponse, ScgPatientPathologyViewModel>(urlPostPatientPathology, scgPatientPathologyViewModel);
            return response;
        }

    }
    public interface ISurgicalConciergePathologyClient
    {
        Task<SurgicalConciergePathologyResponse> PostSurgicalConciergePatientPathologyAsync(ScgPatientPathologyViewModel viewModel);
        Task<SurgicalConciergePatientProfileResponse> GetPatientProfilesWithProfessionalProcedureView(int? pageNo, int? pageSize, string searchString);
    }
}
