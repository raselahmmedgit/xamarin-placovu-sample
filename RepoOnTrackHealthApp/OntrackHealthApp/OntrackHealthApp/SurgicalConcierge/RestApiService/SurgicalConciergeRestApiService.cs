using Acr.UserDialogs;
using Newtonsoft.Json;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Model;
using OntrackHealthApp.PatientOutComeReport.ViewModel;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace OntrackHealthApp.SurgicalConcierge.RestApiService
{
    public class SurgicalConciergeRestApiService
    {
        private ITokenContainer tokenContainer;
        public SurgicalConciergeRestApiService()
        {
            tokenContainer = new TokenContainer();
        }
        private HttpClient InitializeHttpClient() {
            var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler());
            httpClient.MaxResponseContentBufferSize = 556000;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            return httpClient;

        }

        #region Patient

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetPatientList(long practiceDivisionUnitId, int pageNo, int itemPerPage, DateTime? pastDay)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                if (pastDay != null && pastDay.HasValue)
                {
                    surgicalConciergeList = await GetPastWeekSurgicalPatientList(practiceDivisionUnitId, pageNo, itemPerPage, pastDay);
                }
                else
                {
                    surgicalConciergeList = await GetSurgicalPatientList(practiceDivisionUnitId, pageNo, itemPerPage);
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetPatientSearchList(long practiceDivisionUnitId, int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                surgicalConciergeList = await GetSurgicalPatientSearchList(practiceDivisionUnitId, pageNo, itemPerPage, practiceName, professionalName, patientName, patientEmail, dateofBirth, patientPhoneCode, patientPhone, surgeryDate, pastDay);
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetSurgicalPatientList(long practiceDivisionUnitId, int pageNo, int itemPerPage)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = AppConstant.BaseAddress + "api/SurgicalConciergePatient/GetPatientProfiles"
                    + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                    + "&pageNo=" + pageNo
                    + "&pageSize=" + itemPerPage;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<ObservableCollection<SurgicalConciergePatientViewModel>>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetSurgicalPatientSearchList(long practiceDivisionUnitId, int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = AppConstant.BaseAddress + "api/SurgicalConciergePatient/GetPatientProfilesSearch"
                    + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                    + "&pageNo=" + pageNo
                    + "&pageSize=" + itemPerPage
                    + "&practiceName=" + practiceName
                    + "&professionalName=" + professionalName
                    + "&patientName=" + patientName
                    + "&patientEmail=" + patientEmail
                    + "&dateofBirth=" + dateofBirth
                    + "&patientPhoneCode=" + patientPhoneCode
                    + "&patientPhone=" + patientPhone
                    + "&surgeryDate=" + surgeryDate?.Ticks
                    + "&selectedDate=" + pastDay?.Ticks;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<ObservableCollection<SurgicalConciergePatientViewModel>>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetPastWeekSurgicalPatientList(long practiceDivisionUnitId, int pageNo, int itemPerPage, DateTime? pastDay)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = AppConstant.BaseAddress + "api/SurgicalConcierge/GetPastWeekPatients"
                    + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                    + "&pageNo=" + pageNo
                    + "&pageSize=" + itemPerPage
                    + "&pastDay=" + pastDay?.Ticks;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<ObservableCollection<SurgicalConciergePatientViewModel>>(result);
                    }
                }              
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetPastWeekSurgicalPatientSearchList(long practiceDivisionUnitId, int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = "api/SurgicalConcierge/GetPatientProfilesSearch"
                    + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                    + "&pageNo=" + pageNo
                    + "&pageSize=" + itemPerPage
                    + "&practiceName=" + practiceName
                    + "&professionalName=" + professionalName
                    + "&patientName=" + patientName
                    + "&patientEmail=" + patientEmail
                    + "&dateofBirth=" + dateofBirth
                    + "&patientPhoneCode=" + patientPhoneCode
                    + "&patientPhone=" + patientPhone
                    + "&surgeryDate=" + surgeryDate?.Ticks
                    + "&pastDay=" + pastDay?.Ticks;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<ObservableCollection<SurgicalConciergePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<SurgicalConciergePatientViewModel> EditSurgicalConciergePatientProfile(long patientProfileId, Guid? patientProcedureDetailId, int? practiceDivisionId, int? practiceDivisionUnitId)
        {
            SurgicalConciergePatientViewModel surgicalConciergePatientViewModel = new SurgicalConciergePatientViewModel();
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var restUrl = "api/SurgicalConciergePatient/EditSurgicalConciergePatientProfile"
                        + "?patientProfileId=" + patientProfileId
                        + "&patientProcedureDetailId=" + patientProcedureDetailId.ToString()
                        + "&practiceDivisionId=" + practiceDivisionId
                        + "&practiceDivisionUnitId=" + practiceDivisionUnitId;

                    var uri = restUrl.ToAbsoluteUri();
                    using (var httpClient = InitializeHttpClient())
                    {
                        var response = await httpClient.GetAsync(uri);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            if (result != null)
                                surgicalConciergePatientViewModel = JsonConvert.DeserializeObject<SurgicalConciergePatientViewModel>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return surgicalConciergePatientViewModel;
        }

        public async Task<SurgicalConciergePatientViewModel> GetSurgicalConciergePatientProfile(long patientProfileId, int? practiceDivisionId, int? practiceDivisionUnitId)
        {
            SurgicalConciergePatientViewModel surgicalConciergePatientViewModel = new SurgicalConciergePatientViewModel();
            try
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var restUrl =  "api/SurgicalConciergePatient/GetSurgicalConciergePatientProfile"
                        + "?patientProfileId=" + patientProfileId
                        + "&practiceDivisionId=" + practiceDivisionId
                        + "&practiceDivisionUnitId=" + practiceDivisionUnitId;

                    var uri = restUrl.ToAbsoluteUri();
                    using (var httpClient = InitializeHttpClient())
                    {
                        var response = await httpClient.GetAsync(uri);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            if (result != null)
                                surgicalConciergePatientViewModel = JsonConvert.DeserializeObject<SurgicalConciergePatientViewModel>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return surgicalConciergePatientViewModel;
        }

        public async Task<ApiExecutionResult<SurgicalConciergePatientDeleteViewModel>> DeleteSurgicalConciergePatientProfile(long patientProfileId)
        {
            ApiExecutionResult<SurgicalConciergePatientDeleteViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergePatientDeleteViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/DeleteSurgicalConciergePatientProfile/" + patientProfileId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergePatientDeleteViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult<SurgicalConciergePatientDeleteViewModel>> DeleteSurgicalConciergePatientProcedureDetail(long patientProfileId, Guid? patientProcedureDetailId)
        {
            ApiExecutionResult<SurgicalConciergePatientDeleteViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergePatientDeleteViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/DeleteSurgicalConciergePatientProcedureDetail/" + patientProfileId + "/" + patientProcedureDetailId.ToGuid();
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergePatientDeleteViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        //New
        public async Task<int> GetSurgicalConciergePatientProfileWithProfessionalCountAsync(long practiceDivisionUnitId, int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay, long? procedureId = null, string selectedPracticeProfile = null, string selectedProfessionalProfile = null, string selectedProcedure = null, string selectedPracticeLocation = null)
        {
            int totalRecord = 0;
            try
            {
                var restUrl = "api/SurgicalConciergePatient/GetSurgicalConciergePatientProfileWithProfessionalCount"
                     + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                     + "&pageNo=" + pageNo
                     + "&pageSize=" + itemPerPage
                     + "&practiceName=" + practiceName
                     + "&professionalName=" + professionalName
                     + "&patientName=" + patientName
                     + "&patientEmail=" + patientEmail
                     + "&dateofBirth=" + dateofBirth
                     + "&patientPhoneCode=" + patientPhoneCode
                     + "&patientPhone=" + patientPhone
                     + "&surgeryDate=" + surgeryDate?.Ticks
                     + "&selectedDate=" + pastDay?.Ticks
                     + "&procedureId=" + procedureId

                     + "&selectedPracticeProfile=" + selectedPracticeProfile
                     + "&selectedProfessionalProfile=" + selectedProfessionalProfile
                     + "&selectedProcedure=" + selectedProcedure
                     + "&selectedPracticeLocation=" + selectedPracticeLocation;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        totalRecord = result.ToInt();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return totalRecord;
        }
        public async Task<IEnumerable<SurgicalConciergePatientViewModel>> GetSurgicalConciergePatientProfileWithProfessionalNoCountAsync(long practiceDivisionUnitId, int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay, long? procedureId = null, string selectedPracticeProfile = null, string selectedProfessionalProfile = null, string selectedProcedure = null, string selectedPracticeLocation = null)
        {
            IEnumerable<SurgicalConciergePatientViewModel> surgicalConciergeList = new List<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/GetSurgicalConciergePatientProfileWithProfessionalNoCount"
                      + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                      + "&pageNo=" + pageNo
                      + "&pageSize=" + itemPerPage
                      + "&practiceName=" + practiceName
                      + "&professionalName=" + professionalName
                      + "&patientName=" + patientName
                      + "&patientEmail=" + patientEmail
                      + "&dateofBirth=" + dateofBirth
                      + "&patientPhoneCode=" + patientPhoneCode
                      + "&patientPhone=" + patientPhone
                      + "&surgeryDate=" + surgeryDate?.Ticks
                      + "&selectedDate=" + pastDay?.Ticks
                      + "&procedureId=" + procedureId

                      + "&selectedPracticeProfile=" + selectedPracticeProfile
                      + "&selectedProfessionalProfile=" + selectedProfessionalProfile
                      + "&selectedProcedure=" + selectedProcedure
                      + "&selectedPracticeLocation=" + selectedPracticeLocation;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<IEnumerable<SurgicalConciergePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        //Professional
        public async Task<int> GetPatientProfileWithProfessionalCountAsync(long practiceDivisionUnitId, int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay, long? procedureId = null, string selectedPracticeProfile = null, string selectedProfessionalProfile = null, string selectedProcedure = null, string selectedPracticeLocation = null)
        {
            int totalRecord = 0;
            try
            {
                var restUrl = "api/ProfessionalProfile/GetPatientProfileWithProfessionalCount"
                     + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                     + "&pageNo=" + pageNo
                     + "&pageSize=" + itemPerPage
                     + "&practiceName=" + practiceName
                     + "&professionalName=" + professionalName
                     + "&patientName=" + patientName
                     + "&patientEmail=" + patientEmail
                     + "&dateofBirth=" + dateofBirth
                     + "&patientPhoneCode=" + patientPhoneCode
                     + "&patientPhone=" + patientPhone
                     + "&surgeryDate=" + surgeryDate?.Ticks
                     + "&selectedDate=" + pastDay?.Ticks
                     + "&procedureId=" + procedureId

                     + "&selectedPracticeProfile=" + selectedPracticeProfile
                     + "&selectedProfessionalProfile=" + selectedProfessionalProfile
                     + "&selectedProcedure=" + selectedProcedure
                     + "&selectedPracticeLocation=" + selectedPracticeLocation;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        totalRecord = result.ToInt();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return totalRecord;
        }
        public async Task<IEnumerable<SurgicalConciergePatientViewModel>> GetPatientProfileWithProfessionalNoCountAsync(long practiceDivisionUnitId, int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay, long? procedureId = null, string selectedPracticeProfile = null, string selectedProfessionalProfile = null, string selectedProcedure = null, string selectedPracticeLocation = null)
        {
            IEnumerable<SurgicalConciergePatientViewModel> surgicalConciergeList = new List<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = "api/ProfessionalProfile/GetPatientProfileWithProfessionalNoCount"
                      + "?practiceDivisionUnitId=" + practiceDivisionUnitId
                      + "&pageNo=" + pageNo
                      + "&pageSize=" + itemPerPage
                      + "&practiceName=" + practiceName
                      + "&professionalName=" + professionalName
                      + "&patientName=" + patientName
                      + "&patientEmail=" + patientEmail
                      + "&dateofBirth=" + dateofBirth
                      + "&patientPhoneCode=" + patientPhoneCode
                      + "&patientPhone=" + patientPhone
                      + "&surgeryDate=" + surgeryDate?.Ticks
                      + "&selectedDate=" + pastDay?.Ticks
                      + "&procedureId=" + procedureId

                      + "&selectedPracticeProfile=" + selectedPracticeProfile
                      + "&selectedProfessionalProfile=" + selectedProfessionalProfile
                      + "&selectedProcedure=" + selectedProcedure
                      + "&selectedPracticeLocation=" + selectedPracticeLocation;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<IEnumerable<SurgicalConciergePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<List<SurgicalConciergePatientCalendarViewModel>> GetPatientProfileWithProfessionalNoCountForCalendarAsync(long practiceDivisionId, long practiceDivisionUnitId, string startSurgeryDate, string endSurgeryDate, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate, DateTime? pastDay, long? procedureId = null, string selectedPracticeProfile = null, string selectedProfessionalProfile = null, string selectedProcedure = null, string selectedPracticeLocation = null)
        {
            List<SurgicalConciergePatientCalendarViewModel> surgicalConciergeList = new List<SurgicalConciergePatientCalendarViewModel>();
            try
            {
                var restUrl = "api/ProfessionalProfile/GetPatientProfileWithProfessionalNoCountForCalendar"
                      + "?practiceDivisionId=" + practiceDivisionId
                      + "&practiceDivisionUnitId=" + practiceDivisionUnitId
                      + "&startSurgeryDate=" + startSurgeryDate
                      + "&endSurgeryDate=" + endSurgeryDate
                      + "&practiceName=" + practiceName
                      + "&professionalName=" + professionalName
                      + "&patientName=" + patientName
                      + "&patientEmail=" + patientEmail
                      + "&dateofBirth=" + dateofBirth
                      + "&patientPhoneCode=" + patientPhoneCode
                      + "&patientPhone=" + patientPhone
                      + "&surgeryDate=" + surgeryDate?.Ticks
                      + "&selectedDate=" + pastDay?.Ticks
                      + "&procedureId=" + procedureId

                      + "&selectedPracticeProfile=" + selectedPracticeProfile
                      + "&selectedProfessionalProfile=" + selectedProfessionalProfile
                      + "&selectedProcedure=" + selectedProcedure
                      + "&selectedPracticeLocation=" + selectedPracticeLocation;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<List<SurgicalConciergePatientCalendarViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        #endregion

        #region Nurse Patient Info

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetNursePatientInfoPatientList(int pageNo, int itemPerPage)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = $"api/NursePatientInfo/PatientList?pageNo={pageNo}&pageSize={itemPerPage}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<ObservableCollection<SurgicalConciergePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<ObservableCollection<SurgicalConciergePatientViewModel>> GetNursePatientInfoPatientListSearchList(int pageNo, int itemPerPage, string practiceName, string professionalName, string patientName, string patientEmail, string dateofBirth, string patientPhoneCode, string patientPhone, DateTime? surgeryDate)
        {
            ObservableCollection<SurgicalConciergePatientViewModel> surgicalConciergeList = new ObservableCollection<SurgicalConciergePatientViewModel>();
            try
            {
                DateTime? pastDay = null;
                var restUrl = "api/NursePatientInfo/PatientListSearch?pageNo=" + pageNo
                    + "&pageSize=" + itemPerPage
                    + "&practiceName=" + practiceName
                    + "&professionalName=" + professionalName
                    + "&patientName=" + patientName
                    + "&patientEmail=" + patientEmail
                    + "&dateofBirth=" + dateofBirth
                    + "&patientPhoneCode=" + patientPhoneCode
                    + "&patientPhone=" + patientPhone
                    + "&surgeryDate=" + surgeryDate?.Ticks
                    + "&selectedDate=" + pastDay?.Ticks;

                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeList = JsonConvert.DeserializeObject<ObservableCollection<SurgicalConciergePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeList;
        }

        public async Task<SurgicalConciergeNursePatientSurveyViewModel> GetNursePatientInfoPatientSurvey(Guid patientProcedureDetailId)
        {
            SurgicalConciergeNursePatientSurveyViewModel patientSurveyViewModel = new SurgicalConciergeNursePatientSurveyViewModel();
            try
            {
                var restUrl = $"api/NursePatientInfo/PatientSurvey?id={patientProcedureDetailId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientSurveyViewModel = JsonConvert.DeserializeObject<SurgicalConciergeNursePatientSurveyViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return patientSurveyViewModel;
        }

        #endregion

        #region Surgical Concierge

        public async Task<SurgicalConciergeViewModel> GetSurgicalConciergeStageList(string PatientProcedureDetailId)
        {
            SurgicalConciergeViewModel surgicalConciergeViewModel = new SurgicalConciergeViewModel();
            try
            {
                var restUrl = $"api/SurgicalConciergeOperatingRoom/GetDetail?id={PatientProcedureDetailId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeViewModel = JsonConvert.DeserializeObject<SurgicalConciergeViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeViewModel;
        }

        public async Task<ScgPacuTimeModel> GetScgPacuTime(long ProfessionalProfileId, long ProcedureId, long PatientProfileId, System.Guid patientProcedureDetailId)
        {
            ScgPacuTimeModel scgPacuTimeModel = new ScgPacuTimeModel();
            try
            {
                var restUrl = $"api/SurgicalConciergePacu/ScgPacuORStartandEstEndTime?ProfessionalProfileId={ProfessionalProfileId}&ProcedureId={ProcedureId}&PatientProfileId={PatientProfileId}&patientProcedureDetailId={patientProcedureDetailId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        scgPacuTimeModel = JsonConvert.DeserializeObject<ScgPacuTimeModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return scgPacuTimeModel;
        }


        public async Task<ApiExecutionResult<SurgicalConciergeStageViewModel>> StartSurgicalConciergeStage(long ScgStageProfessionalProcedureId, long patientProfileId, string patientProcedureDetailId)
        {
            ApiExecutionResult<SurgicalConciergeStageViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergeStageViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergeOperatingRoom/StartStage/" + ScgStageProfessionalProcedureId + "?patientProfileId=" + patientProfileId + "&patientProcedureDetailId=" + patientProcedureDetailId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergeStageViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult<SurgicalConciergeStageViewModel>> StopSurgicalConciergeStage(long ScgStageProfessionalProcedureId, long patientProfileId, string patientProcedureDetailId)
        {
            ApiExecutionResult<SurgicalConciergeStageViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergeStageViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergeOperatingRoom/StopStage/" + ScgStageProfessionalProcedureId + "?patientProfileId=" + patientProfileId + "&patientProcedureDetailId=" + patientProcedureDetailId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergeStageViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult<SurgicalConciergeStageViewModel>> PostSurgicalConciergeStageComment(long ScgStageProfessionalProcedureId, long patientProfileId, string patientProcedureDetailId, string additionalComment)
        {
            ApiExecutionResult<SurgicalConciergeStageViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergeStageViewModel>();
            try
            {
                var values = new Dictionary<string, string>
                {
                   { "ScgStageProfessionalProcedureId", ScgStageProfessionalProcedureId+"" },
                   { "PatientProfileId", patientProfileId+"" },
                   { "PatientProcedureDetailId", patientProcedureDetailId },
                   {"ScgStageAdditionalComment",additionalComment }
                };
                var content = new FormUrlEncodedContent(values);

                var restUrl = "api/SurgicalConciergeOperatingRoom/StageComment/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergeStageViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<SurgicalConciergeStageCommentViewModel> GetSurgicalConciergeStageComment(long ScgStageProfessionalProcedureId, long patientProfileId, string patientProcedureDetailId)
        {
            SurgicalConciergeStageCommentViewModel surgicalConciergeStageCommentViewModel = new SurgicalConciergeStageCommentViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergeOperatingRoom/StageComment/" + ScgStageProfessionalProcedureId + "?patientProfileId=" + patientProfileId + "&patientProcedureDetailId=" + patientProcedureDetailId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeStageCommentViewModel = JsonConvert.DeserializeObject<SurgicalConciergeStageCommentViewModel>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeStageCommentViewModel;
        }

        #endregion

        #region Patient Attendee

        public async Task<ApiExecutionResult<PatientAttendeeProfileViewModel>> AddOrEditPatientAttendee(
            long attendeeProfileId, long patientProfileId, string emailAddress, string phoneNumber, string phoneCode,
            bool emailAllowed, bool smsAllowed)
        {
            return await AddOrEditPatientAttendee(attendeeProfileId, patientProfileId, emailAddress, phoneNumber, phoneCode,
                emailAllowed, smsAllowed, 0);
        }

        public async Task<ApiExecutionResult<PatientAttendeeProfileViewModel>> AddOrEditPatientAttendee(long atteneeProfileId, long patientProfileId, string emailAddress, string phoneNumber, string phoneCode, bool emailAllowed, bool smsAllowed, int attendeeProfileTypeId)
        {
            ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = new ApiExecutionResult<PatientAttendeeProfileViewModel>();
            try
            {
                var values = new Dictionary<string, string>
                {

                   { "AttendeeProfileId", atteneeProfileId+"" },
                   { "EmailAddress", emailAddress+"" },
                   { "PrimaryPhone", phoneNumber },
                   { "PrimaryPhoneCode", phoneCode },
                   { "EmailAllowed", emailAllowed+"" },
                   { "SmsAllowed", smsAllowed+"" },
                   { "PatientProfileId", patientProfileId+"" },
                   { "AttendeeProfileTypeId", attendeeProfileTypeId+"" }
                };
                var content = new FormUrlEncodedContent(values);

                var restUrl = "api/PatientAttendeeProfile/AddOrEditPatientAttendee/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<PatientAttendeeProfileViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult<PatientAttendeeProfileViewModel>> AddOrEditPatientAttendee(PatientAttendeeProfileViewModel model)
        {
            ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = new ApiExecutionResult<PatientAttendeeProfileViewModel>();
            try
            {
                var values = new Dictionary<string, string>
                {

                   { "AttendeeProfileId", model.AttendeeProfileId +"" },
                   { "AttendeeProfileTypeId", model.AttendeeProfileTypeId +"" },
                   { "EmailAddress", model.EmailAddress+"" },
                   { "PrimaryPhone", model.PrimaryPhone },
                   { "PrimaryPhoneCode", model.PrimaryPhoneCode },
                    { "EmailAllowed", model.EmailAllowed +"" },
                    { "SmsAllowed", model.SmsAllowed +"" },
                    { "PatientProfileId", model.PatientProfileId +"" }
                };
                var content = new FormUrlEncodedContent(values);

                var restUrl = "api/PatientAttendeeProfile/AddOrEditPatientAttendee/";
                var uri = restUrl.ToAbsoluteUri();

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<PatientAttendeeProfileViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult<PatientAttendeeProfileViewModel>> DeleteAttendee(PatientAttendeeProfileViewModel patientAttendee)
        {
            ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = new ApiExecutionResult<PatientAttendeeProfileViewModel>();
            try
            {
                var restUrl = "api/PatientAttendeeProfile/DeleteAttendee/" + patientAttendee.AttendeeProfileId;
                var uri = restUrl.ToAbsoluteUri();

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<PatientAttendeeProfileViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<List<PatientAttendeeProfileViewModel>> GetPatientAttendee(long patientProfileId)
        {
            List<PatientAttendeeProfileViewModel> apiExecutionResult = new List<PatientAttendeeProfileViewModel>();
            try
            {
                var restUrl = "api/PatientAttendeeProfile/GetPatientAttendee/" + patientProfileId;
                var uri = restUrl.ToAbsoluteUri();

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<List<PatientAttendeeProfileViewModel>>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }
        
        public async Task<ProfessionalProcedureDropDownViewModel> GetPracticeProcedureDropdown(long practiceProfileId)
        {
            ProfessionalProcedureDropDownViewModel professionalProcedureDropDownViewModel = new ProfessionalProcedureDropDownViewModel();
            try
            {
                var restUrl = "api/SurgicalConcierge/GetPracticeProcedureDropdown/" + practiceProfileId;
                var uri = restUrl.ToAbsoluteUri();

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (result != null)
                            professionalProcedureDropDownViewModel = JsonConvert.DeserializeObject<ProfessionalProcedureDropDownViewModel>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalProcedureDropDownViewModel;
        }

        public async Task<ProfessionalProcedureDropDownViewModel> GetProfessionalProcedureDropdown(long practiceProfileId,long professionalProfileId)
        {
            ProfessionalProcedureDropDownViewModel professionalProcedureDropDownViewModel = new ProfessionalProcedureDropDownViewModel();
            try
            {
                var restUrl = "api/SurgicalConcierge/GetProfessionalProcedureDropdown/" + practiceProfileId+"/"+ professionalProfileId;
                var uri = restUrl.ToAbsoluteUri();

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (result != null)
                            professionalProcedureDropDownViewModel = JsonConvert.DeserializeObject<ProfessionalProcedureDropDownViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalProcedureDropDownViewModel;
        }

        public async Task<ProfessionalProfileDropDownViewModel> GetPracticeProfessionalProfileDropdown(long practiceProfileId)
        {
            ProfessionalProfileDropDownViewModel professionalProfileDropDownViewModel = new ProfessionalProfileDropDownViewModel();
            try
            {
                var restUrl = "api/SurgicalConcierge/GetPracticeProfessionalProfileDropdown/" + practiceProfileId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (result != null)
                            professionalProfileDropDownViewModel = JsonConvert.DeserializeObject<ProfessionalProfileDropDownViewModel>(result);
                    }
                }              
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return professionalProfileDropDownViewModel;
        }

        public async Task<List<PracticeScgDivisionView>> GetSurgicalConciergeDepartment(long practiceProfileId)
        {
            var practiceDivisionViews = new List<PracticeScgDivisionView>();
            try
            {
                var restUrl = $"api/SurgicalConcierge/GetPracticeDivisionList/{practiceProfileId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (result != null)
                            practiceDivisionViews= JsonConvert.DeserializeObject<List<PracticeScgDivisionView>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return practiceDivisionViews;
        }

        public async Task<SurgicalConciergePatientViewModel> GetSurgicalConciergePatient()
        {
            SurgicalConciergePatientViewModel surgicalConciergePatientViewModel = new SurgicalConciergePatientViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/SurgicalConciergePatient/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (result != null)
                            surgicalConciergePatientViewModel = JsonConvert.DeserializeObject<SurgicalConciergePatientViewModel>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergePatientViewModel;
        }
        public async Task<PatientShortProfileView> GetPatientShortProfile(long patientProfileId)
        {
            PatientShortProfileView surgicalConciergePatientViewModel = new PatientShortProfileView();
            try
            {
                var restUrl = $"api/PatientProfile/PatientShortProfile/{patientProfileId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (result != null)
                            surgicalConciergePatientViewModel = JsonConvert.DeserializeObject<PatientShortProfileView>(result);
                    }
                }              
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergePatientViewModel;
        }
        public async Task<ApiExecutionResult<SurgicalConciergePatientViewModel>> PostSurgicalConciergePatient(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            ApiExecutionResult<SurgicalConciergePatientViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/SurgicalConciergePatient/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(surgicalConciergePatientViewModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergePatientViewModel>>(result);
                    }
                }     
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult<SurgicalConciergePatientViewModel>> PostSurgicalConciergePatientNew(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            ApiExecutionResult<SurgicalConciergePatientViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/SurgicalConciergePatientNew/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(surgicalConciergePatientViewModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult<SurgicalConciergePatientViewModel>> PostSurgicalConciergePatientAddOrEditNew(SurgicalConciergePatientViewModel surgicalConciergePatientViewModel)
        {
            ApiExecutionResult<SurgicalConciergePatientViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergePatientViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/SurgicalConciergePatientAddOrEditNew/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(surgicalConciergePatientViewModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConciergePatientViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }


        public async Task<ApiExecutionResult<ProfessionalProfileDropDownViewModel>> CreateSurgicalProfessional(ProfessionalProfileViewModel professionalProfileViewModel)
        {
            ApiExecutionResult<ProfessionalProfileDropDownViewModel> apiExecutionResult = new ApiExecutionResult<ProfessionalProfileDropDownViewModel>();
            try
            {
                var restUrl = "api/SurgicalConcierge/CreateSurgicalProfessional/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(professionalProfileViewModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<ProfessionalProfileDropDownViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<List<SurgicalConceirgePracticeDivision>> GetSurgicalConceirgePracticeDivision()
        {
            List<SurgicalConceirgePracticeDivision> practiceDivisionList = new List<SurgicalConceirgePracticeDivision>();
            try
            {
                var restUrl = "api/SurgicalConcierge/GetPracticeDivision/";
                var uri = restUrl.ToAbsoluteUri();

                using(var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        practiceDivisionList = JsonConvert.DeserializeObject<List<SurgicalConceirgePracticeDivision>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return practiceDivisionList;
        }

        public async Task<List<PatientAttendeeProfileTypeViewModel>> GetSurgicalConceirgePatientAttendeeProfileType()
        {
            List<PatientAttendeeProfileTypeViewModel> patientAttendeeProfileTypeViewModelList = new List<PatientAttendeeProfileTypeViewModel>();
            try
            {
                var restUrl = "api/PatientAttendeeProfile/GetPatientAttendeeProfileType/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientAttendeeProfileTypeViewModelList = JsonConvert.DeserializeObject<List<PatientAttendeeProfileTypeViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return patientAttendeeProfileTypeViewModelList;
        }

        public async Task<List<SurgicalConceirgePracticeDivisionUnit>> GetSurgicalConceirgePracticeDivisionUnit(long? practiceDivisionId)
        {
            List<SurgicalConceirgePracticeDivisionUnit> practiceDivisionUnitList = new List<SurgicalConceirgePracticeDivisionUnit>();
            try
            {
                var restUrl = $"api/SurgicalConcierge/SurgicalConceirgePracticeDivisionUnit?PracticeDivisionId={practiceDivisionId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        practiceDivisionUnitList = JsonConvert.DeserializeObject<List<SurgicalConceirgePracticeDivisionUnit>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return practiceDivisionUnitList;
        }

        public async Task<ApiExecutionResult<SurgicalConceirgePatientEmailAddModel>> AddOrEditPatientEmail(SurgicalConceirgePatientEmailAddModel scgPatientEmailAddModel)
        {
            ApiExecutionResult<SurgicalConceirgePatientEmailAddModel> apiExecutionResult = new ApiExecutionResult<SurgicalConceirgePatientEmailAddModel>();

            try
            {
                var restUrl = "api/PatientProfile/AddSurgicalConceirgePatientEmail/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(scgPatientEmailAddModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<SurgicalConceirgePatientEmailAddModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }

            return apiExecutionResult;
        }

        #endregion

        #region FLOOR

        public async Task<SurgicalConciergeFloorViewModel> GetSurgicalConciergeFloor(long patientProfileId, string patientProcedureDetailId)
        {
            SurgicalConciergeFloorViewModel surgicalConciergeFloorViewModel = new SurgicalConciergeFloorViewModel();
            try
            {
                var restUrl = "api/SurgicalConcierge/ScgFloorComments/" + patientProfileId + "/" + patientProcedureDetailId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeFloorViewModel = JsonConvert.DeserializeObject<SurgicalConciergeFloorViewModel>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeFloorViewModel;
        }

        public async Task<SurgicalConciergeFloorOnlyViewModel> GetSurgicalConciergeFloorOnly()
        {
            SurgicalConciergeFloorOnlyViewModel surgicalConciergeFloorOnlyViewModel = new SurgicalConciergeFloorOnlyViewModel();
            try
            {
                var restUrl = "api/SurgicalConcierge/ScgFloorCommentsOnly/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeFloorOnlyViewModel = JsonConvert.DeserializeObject<SurgicalConciergeFloorOnlyViewModel>(result);
                    }
                }              
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeFloorOnlyViewModel;
        }

        public async Task<SurgicalConciergeFloorOnlyViewModel> GetSurgicalConciergeFloorOnlyByProcedureId(long procedureId)
        {
            SurgicalConciergeFloorOnlyViewModel surgicalConciergeFloorOnlyViewModel = new SurgicalConciergeFloorOnlyViewModel();
            try
            {
                var restUrl = "api/SurgicalConcierge/ScgFloorCommentsOnlyByProcedureId/" + procedureId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeFloorOnlyViewModel = JsonConvert.DeserializeObject<SurgicalConciergeFloorOnlyViewModel>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeFloorOnlyViewModel;
        }

        public async Task<ApiExecutionResult> ScgFloorCommentSend(ScgFloorCommentViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConcierge/ScgFloorCommentSend/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        #endregion

        #region PACU

        public async Task<SurgicalConciergePacuViewModel> GetSurgicalConciergePacu(long patientProfileId, string patientProcedureDetailId)
        {
            SurgicalConciergePacuViewModel surgicalConciergePacuViewModel = new SurgicalConciergePacuViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergePacu/ScgPacuComments/" + patientProfileId + "/" + patientProcedureDetailId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergePacuViewModel = JsonConvert.DeserializeObject<SurgicalConciergePacuViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergePacuViewModel;
        }

        public async Task<List<ScgPacuCommentViewModel>> GetSurgicalConciergePacuOnly()
        {
            List<ScgPacuCommentViewModel> surgicalConciergePacuCommentViewModelList = new List<ScgPacuCommentViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergePacu/ScgPacuCommentsOnly";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergePacuCommentViewModelList = JsonConvert.DeserializeObject<List<ScgPacuCommentViewModel>>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergePacuCommentViewModelList;
        }

        public async Task<ApiExecutionResult> ScgPacuCommentSend(SurgicalConciergePacuCommentParamViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConciergePacu/ScgPacuCommentSend/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult> ScgPacuAdditionalCommentSend(SurgicalConciergePacuCommentParamViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConciergePacu/SendPacuAdditionalSend/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult> ScgPacuCommentSendAjax(SurgicalConciergePacuQuestionViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConciergePacu/ScgPacuCommentSendAjax";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        #endregion

        #region Discharge

        public async Task<ProgramResourceViewModel> GetSurgicalConciergeDischargeForProgram()
        {
            ProgramResourceViewModel programResourceViewModel = new ProgramResourceViewModel();
            try
            {
                var restUrl = "api/ProfessionalProgram/SurgicalConciergeDischarge";
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

        public async Task<SurgicalConciergeDischargeViewModel> GetSurgicalConciergeDischarge(long patientProfileId, string patientProcedureDetailId)
        {
            SurgicalConciergeDischargeViewModel surgicalConciergeDischargeViewModel = new SurgicalConciergeDischargeViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergeDischarge/ScgDischargeComments/" + patientProfileId + "/" + patientProcedureDetailId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeDischargeViewModel = JsonConvert.DeserializeObject<SurgicalConciergeDischargeViewModel>(result);
                    }
                }              
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeDischargeViewModel;
        }

        public async Task<SurgicalConciergeDischargeOnlyViewModel> GetSurgicalConciergeDischargeOnly()
        {
            SurgicalConciergeDischargeOnlyViewModel surgicalConciergeDischargeOnlyViewModel = new SurgicalConciergeDischargeOnlyViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergeDischarge/ScgDischargeCommentsOnly/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeDischargeOnlyViewModel = JsonConvert.DeserializeObject<SurgicalConciergeDischargeOnlyViewModel>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeDischargeOnlyViewModel;
        }

        public async Task<SurgicalConciergeDischargeOnlyViewModel> GetSurgicalConciergeDischargeOnlyByProcedureId(long procedureId)
        {
            SurgicalConciergeDischargeOnlyViewModel surgicalConciergeDischargeOnlyViewModel = new SurgicalConciergeDischargeOnlyViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergeDischarge/ScgDischargeCommentsOnlyByProcedureId/" + procedureId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeDischargeOnlyViewModel = JsonConvert.DeserializeObject<SurgicalConciergeDischargeOnlyViewModel>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeDischargeOnlyViewModel;
        }

        public async Task<ApiExecutionResult> ScgDischargeCommentSend(ScgDischargeCommentViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConciergeDischarge/ScgDischargeCommentSendNew/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        #endregion

        #region PreSurgerySummary

        public async Task<List<CancerLocationViewModel>> GetPreSurgerySummaryGetCancerLocations(int cancerLocationTemplateType, Guid? preSurgerySummaryId = null)
        {
            List<CancerLocationViewModel> cancerLocationViewModel = new List<CancerLocationViewModel>();
            try
            {
                var restUrl = $"api/SurgicalConciergePreCancerSummary/GetCancerLocations?cancerLocationTemplateType={cancerLocationTemplateType}&preSurgerySummaryId={preSurgerySummaryId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        cancerLocationViewModel = JsonConvert.DeserializeObject<List<CancerLocationViewModel>>(result);
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return cancerLocationViewModel;
        }

        public async Task<PatientPreSurgerySummaryViewModel> GetPreSurgerySummaryInitialData(string id, DateTime? pastDay, long? practiceDivisionUnitId)
        {
            PatientPreSurgerySummaryViewModel patientPreSurgerySummaryViewModel = new PatientPreSurgerySummaryViewModel();
            try
            {
                var restUrl = $"api/SurgicalConciergePreCancerSummary/Detail?id={id}&pastDay={pastDay}&practiceDivisionUnitId={practiceDivisionUnitId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientPreSurgerySummaryViewModel = JsonConvert.DeserializeObject<PatientPreSurgerySummaryViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return patientPreSurgerySummaryViewModel;
        }

        public async Task<ApiExecutionResult> SavePreSurgerySummary(PatientPreSurgerySummaryViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConciergePreCancerSummary/Detail/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        apiExecutionResult.Success = true;
                    }
                    else
                    {
                        apiExecutionResult.Success = false;
                    }
                }               
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<ApiExecutionResult> UpdatePreSurgerySummary(PatientPreSurgerySummaryViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConciergePreCancerSummary/Edit/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        apiExecutionResult.Success = true;
                    }
                    else
                    {
                        apiExecutionResult.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }
        #endregion

        #region NursingRounds
        public async Task<SurgicalConciergeNursingRoundViewModel> GetSurgicalConciergeNursingRounds()
        {
            SurgicalConciergeNursingRoundViewModel patientPreSurgerySummaryViewModel = new SurgicalConciergeNursingRoundViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergeNursingRound/GetScgNursingRound/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientPreSurgerySummaryViewModel = JsonConvert.DeserializeObject<SurgicalConciergeNursingRoundViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return patientPreSurgerySummaryViewModel;
        }

        public async Task<ApiExecutionResult> SurgicalConciergeNursingRoundSendNotification(SurgicalConciergeNursingRoundViewModel model)
        {
            ApiExecutionResult apiExecutionResult = new ApiExecutionResult();
            try
            {
                var restUrl = "api/SurgicalConciergeNursingRound/SendScgNursingRoundNotification/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        apiExecutionResult.Message = AppConstant.NotificationSuccessMessage;
                        apiExecutionResult.Success = true;
                    }
                    else
                    {
                        apiExecutionResult.Message = AppConstant.NotificationFailedMessage;
                        apiExecutionResult.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }
        #endregion

        #region Admin
        public async Task<List<CountryViewModel>> GetCountryList()
        {
            List<CountryViewModel> countryList = new List<CountryViewModel>();
            try
            {
                var restUrl = "api/SurgicalConcierge/Country/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        countryList = JsonConvert.DeserializeObject<List<CountryViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return countryList;
        }

        public async Task<string> GetCountryListAsJsonString()
        {
            string result = "";
            try
            {
                var restUrl = "api/AdministrationCountry/Country";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<string>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return result;
        }

        public async Task<string> GetCountryPhoneCodesAsString()
        {
            string result = "";
            try
            {
                var restUrl = "api/AdministrationCountry/GetCountryPhoneCodesAsString/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<string>(result);
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return result;
        }

        public async Task<PatientScheduleHomePageViewModel> GetNotificationListDetail(string patientProcedureDetailId)
        {
            PatientScheduleHomePageViewModel notificationList = new PatientScheduleHomePageViewModel();
            try
            {
                var uri = $"api/PatientProfile/Schedule/{patientProcedureDetailId}".ToAbsoluteUri();
                var httpClient = new HttpClient(HttpClientHelper.InitializeHttpClientHandler()) { BaseAddress = new Uri(AppConstant.BaseAddress) };
                //httpClient.MaxResponseContentBufferSize = 556000;
                httpClient.MaxResponseContentBufferSize = 9999999;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    notificationList = JsonConvert.DeserializeObject<PatientScheduleHomePageViewModel>(result);
                }

            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return notificationList;
        }
        #endregion


        public async Task<SurgicalConciergeSidebarSearchViewModel> GetSurgicalConciergeSidebarSearchViewModels()
        {
            SurgicalConciergeSidebarSearchViewModel surgicalConciergeSidebarSearchViewModel = new SurgicalConciergeSidebarSearchViewModel();
            try
            {
                var restUrl = "api/SurgicalConciergePatient/GetSurgicalConciergeSidebarSearch/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeSidebarSearchViewModel = JsonConvert.DeserializeObject<SurgicalConciergeSidebarSearchViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeSidebarSearchViewModel;
        }

        public async Task<SurgicalConciergeSidebarSearchViewModel> GetOutcomeSidebarSearchViewModels()
        {
            SurgicalConciergeSidebarSearchViewModel surgicalConciergeSidebarSearchViewModel = new SurgicalConciergeSidebarSearchViewModel();
            try
            {
                var restUrl = "api/PatientReportedOutcome/GetPatientReportedOutcomeSidebarSearch/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surgicalConciergeSidebarSearchViewModel = JsonConvert.DeserializeObject<SurgicalConciergeSidebarSearchViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surgicalConciergeSidebarSearchViewModel;
        }

        public async Task<MobileSurveyReportViewModel> GetMobileSurveyReportViewModel()
        {
            await Task.Yield();
            MobileSurveyReportViewModel reportViewModel = new MobileSurveyReportViewModel();
            try
            {
                //var restUrl = "api/MobileSurveyReport/?patientProfileId=11902&patientProcedureDetailId=f0f5b86f-b740-41a1-97fe-b937577b48df&reportType=1&outcomeReportType=1";
                //var uri = restUrl.ToAbsoluteUri();
                //using (var httpClient = InitializeHttpClient())
                //{
                //    var response = await httpClient.GetAsync(uri);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var result = await response.Content.ReadAsStringAsync();
                //        reportViewModel = JsonConvert.DeserializeObject<MobileSurveyReportViewModel>(result);
                //    }
                //}
                reportViewModel.ProfessionalSurveyReportMobileMenus = new List<ProfessionalSurveyReportMobileMenu>
                {
                    new ProfessionalSurveyReportMobileMenu{MenuTitle="Post Op"},
                    new ProfessionalSurveyReportMobileMenu{MenuTitle="IIEF"},
                    new ProfessionalSurveyReportMobileMenu{MenuTitle="IPSS"},
                    new ProfessionalSurveyReportMobileMenu{MenuTitle="Pads"},
                    new ProfessionalSurveyReportMobileMenu{MenuTitle="UQOL"},
                };
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return reportViewModel;
        }

        public async Task<List<SurveyQuestionDetail>> GetSurveyQuestionDetails(long surveyQuestionId)
        {
            List<SurveyQuestionDetail> surveyQuestionDetails = new List<SurveyQuestionDetail>();
            try
            {
                var restUrl = "api/SurgicalConciergeOperatingRoom/GetSurveyQuestionDetails"
                    + "?surveyQuestionId=" + surveyQuestionId;
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        surveyQuestionDetails = JsonConvert.DeserializeObject<List<SurveyQuestionDetail>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return surveyQuestionDetails;
        }

        public async Task<ApiExecutionResult<NerveSparingSelectionViewModel>> SubmitNerveSparingSelection(NerveSparingSelectionViewModel nerveSparingSelectionViewModel)
        {
            ApiExecutionResult<NerveSparingSelectionViewModel> apiExecutionResult = new ApiExecutionResult<NerveSparingSelectionViewModel>();
            try
            {
                var restUrl = "api/SurgicalConciergeOperatingRoom/SubmitNerveSparingSelection/";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var json = JsonConvert.SerializeObject(nerveSparingSelectionViewModel);
                    var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        apiExecutionResult = JsonConvert.DeserializeObject<ApiExecutionResult<NerveSparingSelectionViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return apiExecutionResult;
        }

        public async Task<List<PatientPreSurgerySummaryXrayViewModel>> GetPreSurgerySummaryXraysData(string preSurgerySummaryId)
        {
            List<PatientPreSurgerySummaryXrayViewModel> patientPreSurgerySummaryXrayViewModelList = new List<PatientPreSurgerySummaryXrayViewModel>();
            try
            {
                var restUrl = $"api/SurgicalConciergePreCancerSummary/GetXrays?preSurgerySummaryId={preSurgerySummaryId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientPreSurgerySummaryXrayViewModelList = JsonConvert.DeserializeObject<List<PatientPreSurgerySummaryXrayViewModel>>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return patientPreSurgerySummaryXrayViewModelList;
        }

        public async Task<PatientPreSurgerySummaryAssessmentPlanViewModel> GetPreSurgerySummaryAssessmentPlanData(string preSurgerySummaryId)
        {
            PatientPreSurgerySummaryAssessmentPlanViewModel patientPreSurgerySummaryAssessmentPlanViewModel = new PatientPreSurgerySummaryAssessmentPlanViewModel();
            try
            {
                var restUrl = $"api/SurgicalConciergePreCancerSummary/GetAssessmentPlan?preSurgerySummaryId={preSurgerySummaryId}";
                var uri = restUrl.ToAbsoluteUri();
                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientPreSurgerySummaryAssessmentPlanViewModel = JsonConvert.DeserializeObject<PatientPreSurgerySummaryAssessmentPlanViewModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Error: ", ex.ToString());
            }
            return patientPreSurgerySummaryAssessmentPlanViewModel;
        }

    }
}
