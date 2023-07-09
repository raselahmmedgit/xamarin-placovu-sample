using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.ViewModel
{
    public class SurgicalConciergePatientProfilePageViewModel : ViewModelBase
    {
        public ObservableCollection<PatientProfileWithProfessionalProcedureView> PatientProfileWithProfessionalProcedures { get; set; }
        public Command LoadPatientProfileWithProfessionalProceduresCommand { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergePathologyClient _SurgicalConciergePathologyClient;

        public SurgicalConciergePatientProfilePageViewModel()
        {
            _iTokenContainer = new TokenContainer();
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
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _SurgicalConciergePathologyClient = new SurgicalConciergePathologyClient(apiClient);
            PatientProfileWithProfessionalProcedures = new ObservableCollection<PatientProfileWithProfessionalProcedureView>();
            LoadPatientProfileWithProfessionalProceduresCommand = new Command(async () => await ExecuteLoadPatientProfileWithProfessionalProceduresCommand());

        }

        async Task ExecuteLoadPatientProfileWithProfessionalProceduresCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                PatientProfileWithProfessionalProcedures.Clear();
                var items = await _SurgicalConciergePathologyClient.GetPatientProfilesWithProfessionalProcedureView(0, 20, "");
                var data = items.DataList;
                foreach (var item in items.DataList)
                {
                    PatientProfileWithProfessionalProcedures.Add(item);
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
