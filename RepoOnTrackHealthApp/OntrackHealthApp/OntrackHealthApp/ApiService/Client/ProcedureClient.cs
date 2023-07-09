using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class ProcedureClient : ClientBase, IProcedureClient
    {
        private readonly ITokenContainer _iTokenContainer;

        public ProcedureClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
        }

        public async Task<PatientProcedureDetailResponse> GetPatientProcedureDetail(string patientProcedureDetailId)
        {
            var url = "api/PatientProfile/GetPatientProcedureDetail/" + patientProcedureDetailId;
            return await GetJsonDecodedContent<PatientProcedureDetailResponse, PatientProcedureDetailModel>(url);
        }

        public async Task<PatientProcedureDetailResponse> GetPatientProcedureDetail()
        {
            var url = "api/PatientProfile/GetPatientProcedureDetail/" + _iTokenContainer.CurrentPatientProcedureDetailId;
            return await GetJsonDecodedContent<PatientProcedureDetailResponse, PatientProcedureDetailModel>(url);
        }

        public async Task<PatientProcedureDetailResponse> CurrentActiveProcedure()
        {
            var url = "api/PatientProfile/CurrentActiveProcedure/" + _iTokenContainer.ApiPatientProfileId;
            return await GetJsonDecodedContent<PatientProcedureDetailResponse, PatientProcedureDetailModel>(url);
        }

        public async Task<PatientProcedureDetailListResponse> ActiveProcedures()
        {
            var url = "api/PatientProfile/ActiveProcedures/" + _iTokenContainer.ApiPatientProfileId;
            return await GetJsonListDecodedContent<PatientProcedureDetailListResponse, List<PatientProcedureDetailModel>>(url);
        }
    }

    public interface IProcedureClient
    {
        Task<PatientProcedureDetailResponse> GetPatientProcedureDetail(string patientProcedureDetailId);
        Task<PatientProcedureDetailResponse> GetPatientProcedureDetail();
        Task<PatientProcedureDetailResponse> CurrentActiveProcedure();
        Task<PatientProcedureDetailListResponse> ActiveProcedures();
    }
}
