
namespace OntrackHealthApp.ApiService.Client
{
    using OntrackHealthApp.ApiHelper;
    using OntrackHealthApp.ApiHelper.Client;
    using OntrackHealthApp.ApiService.Model;
    using OntrackHealthApp.ApiService.Response;
    using System;
    using System.Threading.Tasks;


    public class ProstateCancerSummaryClient : ClientBase, IProstateCancerSummaryClient
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly string urlProstateCancerSummary = "api/ProstateCancerSummary/GetProstateCancerSummary";
       
        public ProstateCancerSummaryClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
        }

        public async Task<ProstateCancerSummaryResponse> GetProstateCancerSummaryAsync(long practiceProfileId, long patientProfileId, Guid patientProcedureDetailId)
        {
            string url = $"{urlProstateCancerSummary}/{practiceProfileId}/{patientProfileId}/{patientProcedureDetailId}";
            //string url = string.Format(urlProstateCancerSummary, practiceProfileId, patientProfileId);
            var response = await GetJsonDecodedContent<ProstateCancerSummaryResponse, PatientProstateCancerSummary>(url);
            return response;
        }

        public async Task<ProstateCancerSummaryResponse> GetProstateCancerSummaryNewAsync(long practiceProfileId, long patientProfileId, string patientProcedureDetailId)
        {
            string url = string.Empty;
            if (string.IsNullOrEmpty(patientProcedureDetailId))
            {
                url = $"{urlProstateCancerSummary}/{practiceProfileId}/{patientProfileId}/null";
            }
            else
            {
                url = $"{urlProstateCancerSummary}/{practiceProfileId}/{patientProfileId}/{patientProcedureDetailId}";
            }
            //string url = $"{urlProstateCancerSummary}/{practiceProfileId}/{patientProfileId}/{patientProcedureDetailId}";
            //string url = string.Format(urlProstateCancerSummary, practiceProfileId, patientProfileId);
            var response = await GetJsonDecodedContent<ProstateCancerSummaryResponse, PatientProstateCancerSummary>(url);
            return response;
        }

    }
    public interface IProstateCancerSummaryClient
    {
        Task<ProstateCancerSummaryResponse> GetProstateCancerSummaryAsync(long practiceProfileId, long patientProfileId, Guid patientProcedureDetailId);
        Task<ProstateCancerSummaryResponse> GetProstateCancerSummaryNewAsync(long practiceProfileId, long patientProfileId, string patientProcedureDetailId);
    }
}
