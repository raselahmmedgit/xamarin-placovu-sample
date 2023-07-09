using OntrackHealthApp.ProfessionalProfile.Model;
using System.Collections.Generic;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientReportedOutcomeSearchPageViewModel
    {
        public PatientReportedOutcomeSearchPageViewModel()
        {
            ProfessionalPatientReportedOutcomeSearchViewModels = new List<ProfessionalPatientReportedOutcomeSearchViewModel>();
        }
        public List<ProfessionalPatientReportedOutcomeSearchViewModel> ProfessionalPatientReportedOutcomeSearchViewModels { get; set; }
    }
}
