using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientReportedOutcomePageViewModel
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeBgColor { get; set; }
        public string TypeDescription { get; set; }
        public string TypeIcon { get; set; }

        public long PracticeId { get; set; }
        public long DivisionId { get; set; }
        public long DivisionUnitId { get; set; }

        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }

        public long PatientProfileId { get; set; }
        public string PatientProcedureDetailId { get; set; }

        public List<string> SelectedProcedure { get; set; }
    }
}
