using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using OntrackHealthApp.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.AppCore;
using System.Threading.Tasks;

namespace OntrackHealthApp.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private ITokenContainer _iTokenContainer;
        private IProcedureClient _iProcedureClient;
        public PatientProcedureDetailModel PatientProcedureDetailModel { get; set; }

        public ObservableCollection<PatientProcedureDetailModel> PatientProcedureDetailModels { get; set; }

        public MainPageViewModel()
        {
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);
        }

        public async Task ExecuteLoadCommandAsync() {

            try
            {
                IsBusy = true;

                var responseCurrentActiveProcedure = await _iProcedureClient.CurrentActiveProcedure();
                if (responseCurrentActiveProcedure.StatusIsSuccessful)
                {
                    var data = responseCurrentActiveProcedure.Data;
                    if (data != null)
                    {
                        PatientProcedureDetailModel = data;
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }

                var responseActiveProcedures = await _iProcedureClient.ActiveProcedures();
                if (responseActiveProcedures.StatusIsSuccessful)
                {
                    var dataActiveProcedures = responseActiveProcedures.DataList;
                    if (dataActiveProcedures != null || dataActiveProcedures.Count > 0)
                    {
                        PatientProcedureDetailModels = new ObservableCollection<PatientProcedureDetailModel>(dataActiveProcedures);
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine(ex);
                IsSuccess = false;
                ErrorMessage = AppConstant.ErrorCommon;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadCommandAsync(string currentPatientProcedureDetailId)
        {

            try
            {
                IsBusy = true;

                var responseCurrentActiveProcedure = await _iProcedureClient.CurrentActiveProcedure();
                if (responseCurrentActiveProcedure.StatusIsSuccessful)
                {
                    var data = responseCurrentActiveProcedure.Data;
                    if (data != null)
                    {
                        PatientProcedureDetailModel = data;
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }

                var responseActiveProcedures = await _iProcedureClient.ActiveProcedures();
                if (responseActiveProcedures.StatusIsSuccessful)
                {
                    var dataActiveProcedures = responseActiveProcedures.DataList;
                    if (dataActiveProcedures != null)
                    {
                        PatientProcedureDetailModels = new ObservableCollection<PatientProcedureDetailModel>(dataActiveProcedures);
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine(ex);
                IsSuccess = false;
                ErrorMessage = AppConstant.ErrorCommon;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
