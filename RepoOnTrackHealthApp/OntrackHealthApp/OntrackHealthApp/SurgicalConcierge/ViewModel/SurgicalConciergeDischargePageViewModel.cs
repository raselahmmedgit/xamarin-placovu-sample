using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.SurgicalConcierge.ViewModel
{
    public class SurgicalConciergeDischargePageViewModel : ViewModelBase
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService;

        public long PatientProfileId { get; set; }
        public string PatientProcedureDetailId { get; set; }
        public event EventHandler AfterLoadSurgicalConciergeDischargeData;
        public void OnAfterLoadSurgicalConciergeDischargeData()
        {
            AfterLoadSurgicalConciergeDischargeData(this, EventArgs.Empty);
        }
        public SurgicalConciergeDischargeViewModel SurgicalConciergeDischargeViewModel { get; set; }
        public SurgicalConciergeDischargeOnlyViewModel SurgicalConciergeDischargeOnlyViewModel { get; set; }
        public ObservableCollection<ScgDischargeCommentViewModel> SurgicalConciergeDischargeCommentViewModels { get; set; }
        public ObservableCollection<ScgProstatectomyViewModel> ScgProstatectomyViewModels { get; set; }
        public IEnumerable<long> ScgProfessionalProcedureProstectomyIdList { set; get; }

        public Command LoadSurgicalConciergeDischargeCommand { get; set; }

        public Command LoadSurgicalConciergeDischargeCommandForProgram { get; set; }

        public SurgicalConciergeDischargePageViewModel()
        {

            _iTokenContainer = new TokenContainer();
            restApiService = new SurgicalConciergeRestApiService();
            SurgicalConciergeDischargeViewModel = new SurgicalConciergeDischargeViewModel();
            SurgicalConciergeDischargeOnlyViewModel = new SurgicalConciergeDischargeOnlyViewModel();
            restApiService = new SurgicalConciergeRestApiService();
            LoadSurgicalConciergeDischargeCommand = new Command(async () => await ExecuteLoadSurgicalConciergeDischargeAsync());
        }

        public SurgicalConciergeDischargePageViewModel(bool isProgram)
        {

            _iTokenContainer = new TokenContainer();
            restApiService = new SurgicalConciergeRestApiService();
            SurgicalConciergeDischargeViewModel = new SurgicalConciergeDischargeViewModel();
            SurgicalConciergeDischargeOnlyViewModel = new SurgicalConciergeDischargeOnlyViewModel();
            restApiService = new SurgicalConciergeRestApiService();
            LoadSurgicalConciergeDischargeCommandForProgram = new Command(async () => await ExecuteLoadSurgicalConciergeDischargeForProgramAsync());
        }

        public async Task<bool> ScgDischargeCommentSendAsync(ScgDischargeCommentViewModel viewModel)
        {
            if(SurgicalConciergeDischargeViewModel != null)
            {
                viewModel.ScgDischargeCommentId = SurgicalConciergeDischargeViewModel.SurgicalConciergeDischargeCommentViewModel.ScgDischargeCommentId;
                viewModel.ScgDischargeCommentText = SurgicalConciergeDischargeViewModel.SurgicalConciergeDischargeCommentViewModel.ScgDischargeCommentText;
            }
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return false;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                var result = await restApiService.ScgDischargeCommentSend(viewModel);
                return result.Success;
            }
        }

        public async Task ExecuteLoadSurgicalConciergeDischargeAsync()
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                SurgicalConciergeDischargeCommentViewModels = new ObservableCollection<ScgDischargeCommentViewModel>();
                ScgProstatectomyViewModels = new ObservableCollection<ScgProstatectomyViewModel>();

                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                SurgicalConciergeDischargeViewModel = await restApiService.GetSurgicalConciergeDischarge(PatientProfileId, PatientProcedureDetailId);
                var items = SurgicalConciergeDischargeViewModel.ScgDischargeCommentViewModels;
                foreach (var item in items)
                {
                    SurgicalConciergeDischargeCommentViewModels.Add(item);
                }
                var itemsTwo = SurgicalConciergeDischargeViewModel.ScgProstatectomyViewModels;
                foreach (var item in itemsTwo)
                {
                    ScgProstatectomyViewModels.Add(item);
                }
                ScgProfessionalProcedureProstectomyIdList = SurgicalConciergeDischargeViewModel.ScgProfessionalProcedureProstectomyIdList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                OnAfterLoadSurgicalConciergeDischargeData();
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadSurgicalConciergeDischargeForProgramAsync()
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                SurgicalConciergeDischargeCommentViewModels = new ObservableCollection<ScgDischargeCommentViewModel>();
                ScgProstatectomyViewModels = new ObservableCollection<ScgProstatectomyViewModel>();

                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();

                ProgramResourceViewModel programResourceViewModel = new ProgramResourceViewModel();

                programResourceViewModel = await restApiService.GetSurgicalConciergeDischargeForProgram();

                if (programResourceViewModel != null) {
                    SurgicalConciergeDischargeViewModel = programResourceViewModel.SurgicalConciergeDischargeViewModel;
                    
                    var scgProstatectomyList = SurgicalConciergeDischargeViewModel.ScgProstatectomys;
                    foreach (var item in scgProstatectomyList)
                    {
                        ScgProstatectomyViewModel scgProstatectomyViewModel = new ScgProstatectomyViewModel() {
                            ProstatectomyId = item.ProstatectomyId,
                            ProstatectomyName = item.ProstatectomyName,
                            IsActive = item.IsActive,
                            ResourceIconUrl = item.ResourceIconUrl,
                            ResourceIconName = item.ResourceIconName
                        };

                        ScgProstatectomyViewModels.Add(scgProstatectomyViewModel);
                    }
                    ScgProfessionalProcedureProstectomyIdList = SurgicalConciergeDischargeViewModel.ScgProfessionalProcedureProstectomyIdList;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                OnAfterLoadSurgicalConciergeDischargeData();
                IsBusy = false;
            }
        }
    }
}
