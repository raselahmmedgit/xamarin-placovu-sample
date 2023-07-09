using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class NerveSparingSelectionViewModel
    {
        public long SurveyQuestionDetailId { get; set; }
        public long PatientProfileId { get; set; }
        public long PracticeProfileId { get; set; }
        public long ProfessionalProfileId { get; set; }
        public string SurveyQuestionAnswareText { get; set; }
        public Guid PatientProcedureDetailId { get; set; }
    }
}
