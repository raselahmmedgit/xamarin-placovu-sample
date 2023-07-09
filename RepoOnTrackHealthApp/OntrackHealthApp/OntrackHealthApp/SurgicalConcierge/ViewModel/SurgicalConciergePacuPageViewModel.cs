using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.SurgicalConcierge.ViewModel
{
    public class SurgicalConciergePacuPageViewModel : ViewModelBase
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService;  

        public long PatientProfileId { get; set; }
        public string PatientProcedureDetailId { get; set; }       
        //public SurgicalConciergePacuViewModel SurgicalConciergePacuViewModel { get; set; }
        public ObservableCollection<ScgPacuCommentViewModel> ScgPacuCommentViewModels { get; set; }

        public Command LoadSurgicalConciergePacuCommand { get; set; }

        public SurgicalConciergePacuPageViewModel() {
            
            _iTokenContainer = new TokenContainer();
            restApiService = new SurgicalConciergeRestApiService();
            //SurgicalConciergePacuViewModel = new SurgicalConciergePacuViewModel();
            restApiService = new SurgicalConciergeRestApiService();
            //PageTitle = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                PageTitle = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                PageTitle = _iTokenContainer.ApiPracticeName;
            }
            LoadSurgicalConciergePacuCommand = new Command(async () => await ExecuteLoadSurgicalConciergePacuAsync());
        }

        public async Task<bool> ScgPacuCommentSendAsync(SurgicalConciergePacuCommentParamViewModel viewModel)
        {

            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return false;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                var result = await restApiService.ScgPacuCommentSend(viewModel);
                return result.Success;
            }
        }

        public async Task<bool> ScgPacuAdditionalCommentSendAsync(SurgicalConciergePacuCommentParamViewModel viewModel)
        {

            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return false;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                var result = await restApiService.ScgPacuAdditionalCommentSend(viewModel);
                return result.Success;
            }
        }

        public async Task ExecuteLoadSurgicalConciergePacuAsync()
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                if (IsBusy)
                    return;
                IsBusy = true;

                try
                {
                    ScgPacuCommentViewModels = new ObservableCollection<ScgPacuCommentViewModel>();
                    SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                    var data = await restApiService.GetSurgicalConciergePacuOnly();                    
                    foreach (var item in data) {
                        ScgPacuCommentViewModels.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

    }
}
