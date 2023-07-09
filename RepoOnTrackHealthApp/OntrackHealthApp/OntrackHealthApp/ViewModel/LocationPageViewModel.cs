using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OntrackHealthApp.ViewModel
{
    public class LocationPageViewModel : ViewModelBase
    {
        private readonly ITokenContainer ITokenContainer;
        private readonly ILocationClient ILocationClient;

        public ObservableCollection<PatientProcedureLocationViewModel> PatientProcedureLocationViewModelList { get; set; }
        public PatientProcedureLocationViewModel PatientProcedureLocationViewModel { get; set; }


        public LocationPageViewModel() {
            ITokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, ITokenContainer);
            ILocationClient = new LocationClient(apiClient);
           
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var response = await ILocationClient.LocationPage();
                if (response.StatusIsSuccessful)
                {
                    var data = response.Data;

                    if (data != null)
                    {
                        #region PatientProcedureLocationViewModel
                        LocationIndexPageViewModel locationIndexPageViewModel = data;
                        var patientProcedureLocationViewModels = locationIndexPageViewModel.PatientProcedureLocationViewModels.ToList().Select(item =>
                                    new PatientProcedureLocationViewModel
                                    {
                                        PatientProfileId = item.PatientProfileId,
                                        ProcedureId = item.ProcedureId,
                                        ProcedureName = item.ProcedureName,
                                        LocationName = item.LocationName,
                                        StreetAddress = item.StreetAddress,
                                        CityName = item.CityName,
                                        StateName = item.StateName,
                                        ZipCode = item.ZipCode,
                                        Address = (item.StreetAddress + ", " + item.CityName + ", " + item.StateName + "-" + item.ZipCode)
                                    });


                        PatientProcedureLocationViewModel = patientProcedureLocationViewModels.FirstOrDefault();

                        PatientProcedureLocationViewModelList = new ObservableCollection<PatientProcedureLocationViewModel>(patientProcedureLocationViewModels);
                       
                        #endregion
                    }

                }
                else
                {
                    ErrorMessage = AppConstant.DisplayAlertErrorMessage;
                }

            }
            catch (Exception)
            {
                ErrorMessage = AppConstant.DisplayAlertErrorMessage;
            }
        }
    }
}
