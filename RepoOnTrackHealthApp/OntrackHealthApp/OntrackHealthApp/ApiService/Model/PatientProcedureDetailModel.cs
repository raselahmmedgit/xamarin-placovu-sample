using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Model
{
    public class PatientProcedureDetailModel 
    {
        public Guid PatientProcedureDetailId { get; set; }

        public long PracticeProfileId { get; set; }

        public long PatientProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public long ProcedureId { get; set; }

        public Guid PracticeLocationId { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public string SurgeryTime { get; set; }

        public DateTime? CathRemovalDate { get; set; }

        public string ProcedureName { get; set; }

        public string ProfessionalName { get; set; }

        public string LocationName { get; set; }

        public int PatientProcedureStatusId { get; set; }

        public string PatientProcedureStatusName { get; set; }

        public bool HasCathRemovalProcess { get; set; }

        public bool IsSurgeryCompleted { get; set; }
    }
}
