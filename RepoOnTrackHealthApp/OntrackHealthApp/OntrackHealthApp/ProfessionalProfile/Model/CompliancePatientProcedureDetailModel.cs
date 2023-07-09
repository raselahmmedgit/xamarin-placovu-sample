using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class CompliancePatientProcedureDetailModel
    {
        public long ProcedureId { get; set; }

        public long PracticeProfileId { get; set; }

        public string ProcedureName { get; set; }

        public string SurgeryTime { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public DateTime SurgeryDateTime { get; set; }

        public string LocationName { get; set; }
    }
}
