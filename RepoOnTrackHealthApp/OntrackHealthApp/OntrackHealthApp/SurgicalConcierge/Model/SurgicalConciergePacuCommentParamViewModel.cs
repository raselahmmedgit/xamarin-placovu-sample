using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergePacuCommentParamViewModel
    {
        public long ScgPacuCommentId { get; set; }

        public long PatientProfileId { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public string ScgPacuCommentText { get; set; }

        public string ScgPacuCommentTextRoomNo { get; set; }

        public string ScgPacuAdditionalComment { get; set; }

        public long ProcedureId { get; set; }

        public bool HasFreeTextBox { get; set; }

        public string FreeTextBoxLabel { get; set; }

        public bool IsAdditionalComment { get; set; }

        public int DisplayOrder { get; set; }

    }
}
