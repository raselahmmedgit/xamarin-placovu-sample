using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ChartHelper.Models;
using OntrackHealthApp.ChartHelper.ViewModels;
using OntrackHealthApp.PatientOutComeReport.ViewModel;
using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.ProfessionalProfile.RestApiService
{
    public class ProfessionalProfileRestApiService
    {
        private ITokenContainer tokenContainer;

        private HttpClient InitializeHttpClient()
        {
            var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
            //httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.MaxResponseContentBufferSize = 9999999;
            httpClient.DefaultRequestHeaders.Add("Device", "Mobile");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());

            return httpClient;
        }

        public ProfessionalProfileRestApiService()
        {
            tokenContainer = new TokenContainer();
        }

        public async Task<ProfessionalProfilePageViewModel> GetProfessionalProfile(long? professionalProfileId)
        {
            ProfessionalProfilePageViewModel professionalProfilePageViewModel = new ProfessionalProfilePageViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProfile/GetProfessionalProfile?professionalProfileId={professionalProfileId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        professionalProfilePageViewModel = JsonConvert.DeserializeObject<ProfessionalProfilePageViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return professionalProfilePageViewModel;
        }

        public async Task<List<PatientReportedOutcomePatientViewModel>> GetPatientList(long professionalProfileId, int pageNo, int pageSize, int reportType, DateTime? selectedDate)
        {
            List<PatientReportedOutcomePatientViewModel> patientReportedOutcomePatientViewModelList = new List<PatientReportedOutcomePatientViewModel>();
            try
            {
                var restUrl = "api/ProfessionalProfile/GetPatientList"
                      + "?professionalProfileId=" + professionalProfileId
                      + "&pageNo=" + pageNo
                      + "&pageSize=" + pageSize
                      + "&reportType=" + reportType
                      + "&selectedDate=" + selectedDate;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientReportedOutcomePatientViewModelList = JsonConvert.DeserializeObject<List<PatientReportedOutcomePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return patientReportedOutcomePatientViewModelList;
        }

        public async Task<ProfessionalPatientComplianceDetailViewModel> PatientComplianceSearchDetail(long patientProfileId, string patientProcedureDetailId, bool isSearchPatientDirectory = false, int reportType = 1)
        {
            ProfessionalPatientComplianceDetailViewModel professionalProfileComplianceDetailViewModel = new ProfessionalPatientComplianceDetailViewModel();
            try
            {
                var restUrl = "api/ProfessionalProfile/PatientComplianceSearchDetail"
                    + "?patientProfileId=" + patientProfileId
                    + "&patientProcedureDetailId=" + patientProcedureDetailId
                    + "&isSearchPatientDirectory=" + isSearchPatientDirectory
                    + "&reportType=" + reportType;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        professionalProfileComplianceDetailViewModel = JsonConvert.DeserializeObject<ProfessionalPatientComplianceDetailViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalProfileComplianceDetailViewModel;
        }

        public async Task<ProfessionalPatientBphComplianceDetailViewModel> PatientBphComplianceSearchDetail(long patientProfileId, bool isSearchPatientDirectory = false, int reportType = 1)
        {
            ProfessionalPatientBphComplianceDetailViewModel professionalPatientBphComplianceDetailViewModel = new ProfessionalPatientBphComplianceDetailViewModel();
            try
            {
                var restUrl = "api/ProfessionalProfile/PatientBphComplianceSearchDetail"
                    + "?patientProfileId=" + patientProfileId
                    + "&isSearchPatientDirectory=" + isSearchPatientDirectory
                    + "&reportType=" + reportType;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        professionalPatientBphComplianceDetailViewModel = JsonConvert.DeserializeObject<ProfessionalPatientBphComplianceDetailViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalPatientBphComplianceDetailViewModel;
        }

        public async Task<IEnumerable<ProcedureShortViewModel>> GetProfessionalSurveyReportProcedure()
        {
            IEnumerable<ProcedureShortViewModel> procedureShortViewModels = new List<ProcedureShortViewModel>();
            try
            {
                var restUrl = $"api/ComparativeAnalytics/GetProfessionalSurveyReportProcedure";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        procedureShortViewModels = JsonConvert.DeserializeObject<IEnumerable<ProcedureShortViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return procedureShortViewModels;
        }

        public async Task<IEnumerable<ProfessionalSurveyReportMobileMenu>> GetProfessionalSurveyReportMenus(long procedureId)
        {
            IEnumerable<ProfessionalSurveyReportMobileMenu> procedureShortViewModels = new List<ProfessionalSurveyReportMobileMenu>();
            try
            {
                var restUrl = $"api/ComparativeAnalytics/GetProfessionalSurveyReportMenus/{procedureId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        procedureShortViewModels = JsonConvert.DeserializeObject<IEnumerable<ProfessionalSurveyReportMobileMenu>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return procedureShortViewModels;
        }


        public async Task<IEnumerable<ProfessionalPatientProfileComplianceViewModel>> GetSearchPatientDirectory()
        {
            IEnumerable<ProfessionalPatientProfileComplianceViewModel> professionalPatientProfileComplianceViewModels = new List<ProfessionalPatientProfileComplianceViewModel>();
            try
            {
                var restUrl = $"api/ProfessionalProfile/GetSearchPatientDirectory";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        professionalPatientProfileComplianceViewModels = JsonConvert.DeserializeObject<IEnumerable<ProfessionalPatientProfileComplianceViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalPatientProfileComplianceViewModels;
        }

        public async Task<IEnumerable<ProfessionalPatientProfileComplianceViewModel>> GetSearchPatientDirectoryByKeyword(string patientComplianceSearchKeyword, int pageNo, int pageSize)
        {
            IEnumerable<ProfessionalPatientProfileComplianceViewModel> professionalPatientProfileComplianceViewModels = new List<ProfessionalPatientProfileComplianceViewModel>();
            try
            {
                var restUrl = $"api/ProfessionalProfile/GetSearchPatientDirectoryByKeyword"
                    + "?patientComplianceSearchKeyword=" + patientComplianceSearchKeyword
                    + "&page=" + pageNo
                    + "&pageSize=" + pageSize;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        professionalPatientProfileComplianceViewModels = JsonConvert.DeserializeObject<IEnumerable<ProfessionalPatientProfileComplianceViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalPatientProfileComplianceViewModels;
        }

        #region Program

        
        public async Task<ProgramProcedureViewModel> ProfessionalProgram()
        {
            ProgramProcedureViewModel programProcedureViewModel = new ProgramProcedureViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/GetProgram";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programProcedureViewModel = JsonConvert.DeserializeObject<ProgramProcedureViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programProcedureViewModel;
        }

        public async Task<ProgramDetailViewModel> ProfessionalProgramNotification(long? procedureId)
        {
            ProgramDetailViewModel programDetailViewModel = new ProgramDetailViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/Notification?procedureId={procedureId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programDetailViewModel = JsonConvert.DeserializeObject<ProgramDetailViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programDetailViewModel;
        }

        public async Task<ProgramDetailViewModel> ProfessionalProgramNotificationDetail(long? notificationId)
        {
            ProgramDetailViewModel programDetailViewModel = new ProgramDetailViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/NotificationDetail?notificationId={notificationId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programDetailViewModel = JsonConvert.DeserializeObject<ProgramDetailViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programDetailViewModel;
        }

        public async Task<ProgramResourceViewModel> ProfessionalProgramResource(long? procedureId)
        {
            ProgramResourceViewModel programResourceViewModel = new ProgramResourceViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/Resource?procedureId={procedureId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programResourceViewModel = JsonConvert.DeserializeObject<ProgramResourceViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programResourceViewModel;
        }

        public async Task<ProgramResourceDetailViewModel> ProfessionalProgramResourceDetail(long procedureId, Guid? patientPortalResourceId)
        {
            ProgramResourceDetailViewModel programResourceDetailViewModel = new ProgramResourceDetailViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/ResourceDetail?procedureId={procedureId}&patientPortalResourceId={patientPortalResourceId.ToString()}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programResourceDetailViewModel = JsonConvert.DeserializeObject<ProgramResourceDetailViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programResourceDetailViewModel;
        }

        public async Task<ProgramResourceViewModel> ProfessionalProgramSurgicalConciergeStage(long? procedureId)
        {
            ProgramResourceViewModel programResourceViewModel = new ProgramResourceViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/SurgicalConciergeStage?procedureId={procedureId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programResourceViewModel = JsonConvert.DeserializeObject<ProgramResourceViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programResourceViewModel;
        }

        public async Task<ProgramResourceViewModel> ProfessionalProgramSurgicalConciergeResource(long? procedureId)
        {
            ProgramResourceViewModel programResourceViewModel = new ProgramResourceViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/SurgicalConciergeResource?procedureId="+ procedureId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programResourceViewModel = JsonConvert.DeserializeObject<ProgramResourceViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programResourceViewModel;
        }

        public async Task<ProgramResourceViewModel> ProfessionalProgramSurgicalConciergePacu()
        {
            ProgramResourceViewModel programResourceViewModel = new ProgramResourceViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/SurgicalConciergePacu";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programResourceViewModel = JsonConvert.DeserializeObject<ProgramResourceViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programResourceViewModel;
        }
        #endregion

        public async Task<ViewModel.ScgNursingRoundTemplateCategoryApiViewModel> ProfessionalProgramSurgicalConciergeFloor()
        {
            ViewModel.ScgNursingRoundTemplateCategoryApiViewModel scgNursingRoundTemplateCategoryApiViewModel = new ViewModel.ScgNursingRoundTemplateCategoryApiViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/SurgicalConciergeFloor";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        scgNursingRoundTemplateCategoryApiViewModel = JsonConvert.DeserializeObject<ViewModel.ScgNursingRoundTemplateCategoryApiViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return scgNursingRoundTemplateCategoryApiViewModel;
        }

        public async Task<ProgramResourceViewModel> ProfessionalProgramSurgicalConciergeDischarge()
        {
            ProgramResourceViewModel programResourceViewModel = new ProgramResourceViewModel();
            try
            {
                var restUrl = $"api/ProfessionalProgram/SurgicalConciergeDischarge";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        programResourceViewModel = JsonConvert.DeserializeObject<ProgramResourceViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return programResourceViewModel;
        }
                  
    }
}
