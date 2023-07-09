

namespace OntrackHealthApp.ApiService.Client
{
    using OntrackHealthApp.ApiHelper;
    using OntrackHealthApp.ApiHelper.Client;
    using OntrackHealthApp.ApiService.Response;
    using OntrackHealthApp.ApiService.ViewModel;
    using System.Threading.Tasks;
    public class PatientSurveyClient : ClientBase, IPatientSurveyClient
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly string urlPostPatientSurvey = "api/PatientSchedule/CreatePatientSurvey";
        private readonly string urlNextPatientSurvey = "api/PatientSchedule/NextPatientSurvey";

        public PatientSurveyClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
        }

        public async Task<PatientSurveyPostResponse> PostPatientSurvey(PatientSurveyProcedureNotificationViewModel viewModel)
        {
            var patientSurveyResponse = await PostJsonEncodedContentAsync<PatientSurveyPostResponse, PatientSurveyProcedureNotificationViewModel>(urlPostPatientSurvey, viewModel);
            return patientSurveyResponse;
        }

        public async Task<PatientSurveyGetResponse> GetNextPatientSurvey(string practiceProfileId, string professionalProfileId, string patientProcedureDetailId, string notificationId)
        {            
            return await GetJsonDecodedContent<PatientSurveyGetResponse, PatientSurveyQuestionSetViewModel>(urlNextPatientSurvey, "PracticeProfileId".AsPair(practiceProfileId), "professionalProfileId".AsPair(professionalProfileId), "patientProcedureDetailId".AsPair(patientProcedureDetailId), "notificationId".AsPair(notificationId));
        }
    }

    public interface IPatientSurveyClient
    {
        Task<PatientSurveyPostResponse> PostPatientSurvey(PatientSurveyProcedureNotificationViewModel viewModel);
        Task<PatientSurveyGetResponse> GetNextPatientSurvey(string practiceProfileId, string professionalProfileId, string patientProcedureDetailId, string notificationId);
    }
}
