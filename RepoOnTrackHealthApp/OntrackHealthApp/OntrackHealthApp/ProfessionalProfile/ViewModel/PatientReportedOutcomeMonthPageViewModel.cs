using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientReportedOutcomeMonthPageViewModel
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public long PracticeId { get; set; }
        public long DivisionId { get; set; }
        public long DivisionUnitId { get; set; }

        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }

        public List<string> SelectedProcedure { get; set; }
    }
}
