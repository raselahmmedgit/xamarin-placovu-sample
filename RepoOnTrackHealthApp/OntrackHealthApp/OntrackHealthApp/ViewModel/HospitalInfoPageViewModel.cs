using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.AppCore;
using System;
using System.Threading.Tasks;

namespace OntrackHealthApp.ViewModel
{
    public class HospitalInfoPageViewModel : ViewModelBase
    {
        private readonly ITokenContainer ITokenContainer;
        private readonly IHospitalInfoClient IHospitalInfoClient;

        public SurgicalConciergeDocumentViewModel SurgicalConciergeDocumentViewModel { get; set; }

        public HospitalInfoPageViewModel()
        {
            ITokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, ITokenContainer);
            IHospitalInfoClient = new HospitalInfoClient(apiClient);

            SurgicalConciergeDocumentViewModel = new SurgicalConciergeDocumentViewModel();

        }

        public async Task LoadDataAsync()
        {
            try
            {
                var response = await IHospitalInfoClient.HospitalInfoPage();
                if (response.StatusIsSuccessful)
                {
                    var data = response.Data;

                    if (data != null)
                    {
                        #region SurgicalConciergeDocumentViewModel

                        SurgicalConciergeDocumentViewModel = data;
                        
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
