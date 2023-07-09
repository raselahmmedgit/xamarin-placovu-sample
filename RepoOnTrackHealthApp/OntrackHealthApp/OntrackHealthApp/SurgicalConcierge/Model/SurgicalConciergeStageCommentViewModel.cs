using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergeStageCommentViewModel
    {
        public long ScgStageProfessionalProcedureId { get; set; }
        public long PatientProfileId { get; set; }
        public Guid PatientProcedureDetailId { get; set; }
        public string ScgStageAdditionalComment { get; set; }

        public IEnumerable<ScgStageCommentViewModel> ScgStageCommentList { get; set; }
    }
}
