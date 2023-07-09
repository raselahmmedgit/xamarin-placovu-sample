using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
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
    public class SurgicalConciergeRecipientPageViewModel : ViewModelBase
    {
        private readonly ITokenContainer _iTokenContainer;
        SurgicalConciergeRestApiService restService;

        public ObservableCollection<PatientAttendeeProfileViewModel> PatientAttendeeProfileViewModels { get; set; }
        public Command LoadPatientAttendeeProfilesCommand { get; set; }

        public bool ShowHideContinueButton {
            get {
                return PatientAttendeeProfileViewModels.Count > 0;
            }
        }

        public long PatientProfileId;
        public SurgicalConciergeRecipientPageViewModel() {
            _iTokenContainer = new TokenContainer();
            restService = new SurgicalConciergeRestApiService();
            PatientAttendeeProfileViewModels = new ObservableCollection<PatientAttendeeProfileViewModel>();
            LoadPatientAttendeeProfilesCommand = new Command(async () => await ExecuteLoadPatientAttendeeProfilesCommand());
        }

        public async Task ExecuteLoadPatientAttendeeProfilesCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var patientAttendyProfiles = await restService.GetPatientAttendee(PatientProfileId);

                PatientAttendeeProfileViewModels.Clear();
                foreach (var item in patientAttendyProfiles)
                {
                    PatientAttendeeProfileViewModels.Add(item);
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
