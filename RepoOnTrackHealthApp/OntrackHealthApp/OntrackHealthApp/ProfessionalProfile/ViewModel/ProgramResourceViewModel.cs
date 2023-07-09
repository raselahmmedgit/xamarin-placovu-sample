using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProgramResourceViewModel
    {
        public long ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public IEnumerable<PatientProcedureResourceViewModel> PatientProcedureResourceViewModels { get; set; }
        public IEnumerable<ScgStageViewModel> SurgicalConciergeStageViewModels { set; get; }
        public SurgicalConciergeDocumentViewModel SurgicalConciergeDocumentViewModel { set; get; }
        public SurgicalConciergeNursingRoundViewModel NursingRoundViewModel { set; get; }
        public SurgicalConciergeDischargeViewModel SurgicalConciergeDischargeViewModel { set; get; }
        public IEnumerable<ScgPacuCommentViewModel> ScgPacuCommentViewModels { set; get; }
    }
}
