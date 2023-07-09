using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalPatientBphComplianceProcedureTypeViewModel
    {
        public int ProcedureTypeId { get; set; }

        public string ProcedureTypeName { get; set; }

        public List<ProfessionalPatientBphComplianceProcedureViewModel> ProfessionalPatientBphComplianceProcedureViewModels { get; set; }
    }

    public class ProfessionalPatientBphComplianceProcedureViewModel
    {
        public int BphProcedureTypeId { set; get; }

        public int ProcedureTypeId { get; set; }

        public long PatientProfileId { get; set; }

        public string ProcedureTypeName { get; set; }

        public string ProcedureName { get; set; }

        public long ProcedureId { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public string IdFormated
        {
            get
            {
                if (PatientProcedureDetailId != null)
                { return (ProcedureId + "_" + PatientProfileId + "_" + PatientProcedureDetailId); }
                return string.Empty;
            }
        }
    }

}
