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
    public class SurgicalConciergeFloorPageViewModel : ViewModelBase
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeRestApiService restApiService;

        public long PatientProfileId { get; set; }
        public string PatientProcedureDetailId { get; set; }

        public SurgicalConciergeFloorViewModel SurgicalConciergeFloorViewModel { get; set; }
        public SurgicalConciergeFloorOnlyViewModel SurgicalConciergeFloorOnlyViewModel { get; set; }
        public ObservableCollection<ScgFloorCommentViewModel> SurgicalConciergeFloorCommentViewModels { get; set; }
        public ObservableCollection<ScgProstatectomyViewModel> ScgProstatectomyViewModels { get; set; }

        public Command LoadSurgicalConciergeFloorCommand { get; set; }

        public SurgicalConciergeFloorPageViewModel()
        {

            _iTokenContainer = new TokenContainer();
            restApiService = new SurgicalConciergeRestApiService();
            SurgicalConciergeFloorOnlyViewModel = new SurgicalConciergeFloorOnlyViewModel();
            restApiService = new SurgicalConciergeRestApiService();

            LoadSurgicalConciergeFloorCommand = new Command(async () => await ExecuteLoadSurgicalConciergeFloorAsync());
        }

        public async Task<bool> ScgFloorCommentSendAsync(ScgFloorCommentViewModel viewModel)
        {

            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return false;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                var result = await restApiService.ScgFloorCommentSend(viewModel);
                return result.Success;
            }
        }

        public async Task ExecuteLoadSurgicalConciergeFloorAsync()
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
                    SurgicalConciergeFloorCommentViewModels = new ObservableCollection<ScgFloorCommentViewModel>();
                    ScgProstatectomyViewModels = new ObservableCollection<ScgProstatectomyViewModel>();

                    SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();

                    //SurgicalConciergeFloorOnlyViewModel = await restApiService.GetSurgicalConciergeFloorOnly();
                    SurgicalConciergeFloorViewModel = await restApiService.GetSurgicalConciergeFloor(PatientProfileId, PatientProcedureDetailId);

                    //var items = SurgicalConciergeFloorOnlyViewModel.ScgFloorCommentViewModels;

                    var items = SurgicalConciergeFloorViewModel.ScgFloorCommentViewModels;

                    foreach (var item in items)
                    {
                        SurgicalConciergeFloorCommentViewModels.Add(item);
                    }

                    //var itemsTwo = SurgicalConciergeFloorOnlyViewModel.ScgProstatectomyViewModels;

                    var itemsTwo = SurgicalConciergeFloorViewModel.ScgProstatectomyViewModels;

                    foreach (var item in itemsTwo)
                    {
                        ScgProstatectomyViewModels.Add(item);
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
