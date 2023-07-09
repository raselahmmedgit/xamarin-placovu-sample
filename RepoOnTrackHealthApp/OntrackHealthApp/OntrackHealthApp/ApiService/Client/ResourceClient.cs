using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.ProfessionalProfile.ViewModel;

namespace OntrackHealthApp.ApiService.Client
{
    public class ResourceClient : ClientBase, IResourceClient
    {
        private readonly ITokenContainer _iTokenContainer;
        private string ResourcePageUri = string.Empty;
        private string ResourceDetailPageUri = string.Empty;

        public ResourceClient(IApiClient iApiClient) : base(iApiClient)
        {
            _iTokenContainer = new TokenContainer();
            ResourcePageUri = "api/PatientProfile/Resource/" + _iTokenContainer.CurrentPatientProcedureDetailId;
            ResourceDetailPageUri = "api/PatientProfile/ResourceDetail/" + _iTokenContainer.CurrentPatientProcedureDetailId;
        }
        
        public async Task<ResourcePageResponse> ResourcePage()
        {
            return await GetJsonDecodedContent<ResourcePageResponse, ResourceIndexPageViewModel>(ResourcePageUri);
        }

        public async Task<ProfessionalResourcePageResponse> ProfessionalResourcePage(long? procedureId)
        {
            string ProfessionalResourcePageUri = "api/ProfessionalProgram/Resource?procedureId=" + procedureId;
            return await GetJsonDecodedContent<ProfessionalResourcePageResponse, ProgramResourceViewModel>(ProfessionalResourcePageUri);
        }

        public async Task<ResourceDetailContentPageResponse> ResourceDetailPage(Guid? patientPortalResourceId)
        {
            return await GetJsonDecodedContent<ResourceDetailContentPageResponse, ResourceDetailContentPageViewModel>(ResourceDetailPageUri + patientPortalResourceId);
        }
    }

    public interface IResourceClient
    {
        Task<ResourcePageResponse> ResourcePage();
        Task<ProfessionalResourcePageResponse> ProfessionalResourcePage(long? procedureId);
        Task<ResourceDetailContentPageResponse> ResourceDetailPage(Guid? patientPortalResourceId);
    }
}
