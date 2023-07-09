using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class PracticeProfile
    {
        public long PracticeProfileId { get; set; }

        public string PracticeProfileName { get; set; }
    }

    public class SurgicalConciergePatientAddViewModel
    {
        public string PracticeProfileName { get; set; }
        public IEnumerable<PracticeProfile> PracticeProfiles { set; get; }
        public IEnumerable<SelectListItem> PracticeProcedures { set; get; }
        public IEnumerable<SelectListItem> ProfessionalProfiles { set; get; }
    }

}
