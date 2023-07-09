using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class PatientPreviousMedicalHistoryView
    {
        public Guid PreviousMedicalHistoryId { get; set; }
        public long PracticeProfileId { get; set; }
        public long PatientProfileId { get; set; }
        public int? TreatmentPlanId { get; set; }
        public long? ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public int ProcedureTypeId { get; set; }
        public string ProcedureTypeName { get; set; }
        public string CommentText { get; set; }
        public int? Ipsscore { get; set; }
        public int? IiefScore { get; set; }
        public int PreviousMedicalHistoryTypeId { get; set; }
        public string PreviousMedicalHistoryTypeName { get; set; }
    }
}
