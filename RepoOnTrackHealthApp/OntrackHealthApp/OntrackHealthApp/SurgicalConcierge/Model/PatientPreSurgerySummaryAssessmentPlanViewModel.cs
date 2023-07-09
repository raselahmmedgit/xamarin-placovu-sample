using System;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public partial class PatientPreSurgerySummaryAssessmentPlanViewModel
    {
        public Guid AssessmentPlanId { get; set; }
        public Guid PreSurgerySummaryId { get; set; }
        public string AssessmentPlanHtml { get; set; }
        public string AthenaPatientId { get; set; }
        public int PageId { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
