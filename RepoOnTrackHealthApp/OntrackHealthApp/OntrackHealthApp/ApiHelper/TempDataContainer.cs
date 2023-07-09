using Newtonsoft.Json;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.ApiHelper
{
    public sealed class TempDataContainer
    {

        public static string CountryViewModelJson
        {
            get
            {
                string result = "";
                if (Application.Current.Properties.ContainsKey("CountryViewModel") == true)
                {
                    result = (string)Application.Current.Properties["CountryViewModel"];
                }
                return result;
            }
            set
            {
                if (Application.Current.Properties != null)
                {

                    Application.Current.Properties["CountryViewModel"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }
        public static async Task<List<string>> GetCountryViewModelFromJsonAsync()
        {
            var data = CountryViewModelJson;
            List<string> returnData = new List<string>();
            if (string.IsNullOrEmpty(data))
            {
                var restApiService = new SurgicalConciergeRestApiService();
                var _countryViewModelList = await restApiService.GetCountryPhoneCodesAsString();
                CountryViewModelJson = _countryViewModelList;
            }
            data = CountryViewModelJson;
            var dataList = data.Split(';').ToList<string>();
            foreach (var countryViewModel in dataList)
            {
                returnData.Add("+" + countryViewModel);
            }
            return returnData;
        }



        public static string PracticeDivision
        {
            get
            {
                string result = "";
                if (Application.Current.Properties.ContainsKey("PracticeDivision") == true)
                {
                    result = (string)Application.Current.Properties["PracticeDivision"];
                }
                return result;
            }
            set
            {
                if (Application.Current.Properties != null)
                {

                    Application.Current.Properties["PracticeDivision"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }
        public static async Task<List<SurgicalConceirgePracticeDivision>> GetPracticeDivisionFromJsonAsync()
        {
            var data = PracticeDivision;
            List<SurgicalConceirgePracticeDivision> returnData = new List<SurgicalConceirgePracticeDivision>();
            if (string.IsNullOrEmpty(data))
            {
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                List<SurgicalConceirgePracticeDivision> practiceDivisionList = await restApiService.GetSurgicalConceirgePracticeDivision();
                if (practiceDivisionList != null)
                {
                    var surgicalConciergeTrackerPracticeDivisionId = (int)PracticeDivisionId.SurgicalConciergeTracker;
                    var patientReportedOutcomePracticeDivisionId = (int)PracticeDivisionId.PatientReportedOutcome;
                    var surgicalConciergeTrackerRemove = practiceDivisionList.FirstOrDefault(x => x.DivisionId == surgicalConciergeTrackerPracticeDivisionId);
                    var patientReportedOutcomeRemove = practiceDivisionList.FirstOrDefault(x => x.DivisionId == patientReportedOutcomePracticeDivisionId);
                    practiceDivisionList.Remove(surgicalConciergeTrackerRemove);
                    practiceDivisionList.Remove(patientReportedOutcomeRemove);
                    PracticeDivision = JsonConvert.SerializeObject(practiceDivisionList);
                    returnData = practiceDivisionList;
                }
                else
                {
                    PracticeDivision = string.Empty;
                }
            }
            else
            {
                returnData = JsonConvert.DeserializeObject<List<SurgicalConceirgePracticeDivision>>(data);
            }
            return returnData;
        }


        public static string PracticeDivisionUnit
        {
            get
            {
                string result = "";
                if (Application.Current.Properties.ContainsKey("PracticeDivisionUnit") == true)
                {
                    result = (string)Application.Current.Properties["PracticeDivisionUnit"];
                }
                return result;
            }
            set
            {
                if (Application.Current.Properties != null)
                {

                    Application.Current.Properties["PracticeDivisionUnit"] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }
        public static async Task<List<SurgicalConceirgePracticeDivisionUnit>> GetPracticeDivisionUnitFromJsonAsync(long practiceDivisionId)
        {

            SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
            var data = PracticeDivisionUnit;
            List<SurgicalConceirgePracticeDivisionUnit> returnData = new List<SurgicalConceirgePracticeDivisionUnit>();
            if (!string.IsNullOrEmpty(data))
            {
                var currdivision = JsonConvert.DeserializeObject<List<SurgicalConceirgePracticeDivisionUnit>>(data);
                if(currdivision.FirstOrDefault().SurgicalConceirgeDivisionId != practiceDivisionId)
                {
                    data = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(data))
            {
                returnData = JsonConvert.DeserializeObject<List<SurgicalConceirgePracticeDivisionUnit>>(data);
                var dataTmp = returnData.Where(x => x.SurgicalConceirgeDivisionId == practiceDivisionId).ToList();
                if (dataTmp == null)
                {
                    List<SurgicalConceirgePracticeDivisionUnit> practiceDivisionUnitList = await restApiService.GetSurgicalConceirgePracticeDivisionUnit(practiceDivisionId);
                    returnData.AddRange(practiceDivisionUnitList);
                }
                PracticeDivisionUnit = JsonConvert.SerializeObject(returnData);
                returnData = returnData.Where(x => x.SurgicalConceirgeDivisionId == practiceDivisionId).ToList();
            }
            else
            {
                List<SurgicalConceirgePracticeDivisionUnit> practiceDivisionUnitList = await restApiService.GetSurgicalConceirgePracticeDivisionUnit(practiceDivisionId);
                PracticeDivisionUnit = JsonConvert.SerializeObject(practiceDivisionUnitList);
                returnData = practiceDivisionUnitList;
            }
            return returnData;
        }
    }
}
