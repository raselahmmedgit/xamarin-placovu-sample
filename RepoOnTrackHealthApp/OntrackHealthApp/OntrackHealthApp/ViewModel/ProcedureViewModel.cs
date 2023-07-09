using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class ProcedureViewModel
    {
        public long ProcedureId { get; set; }
        public long? PracticeProfileId { get; set; }
        public string ProcedureName { get; set; }
        public string PatientPortalBanner { get; set; }
        public bool HasCathRemovalProcess { get; set; }
        public string ScgPublicPageTitle { get; set; }
        public bool IsScgProcedure { get; set; }
        public bool IsShowInScg { get; set; }
        public int ProcedureGenderTypeId { get; set; }
        public bool IsDummyProcedure { get; set; }
        public bool HasPregnancyDate { get; set; }
        public bool HasDeliveryDate { get; set; }
        public bool IsGenderDependent { get; set; }
        public bool IsBphProcedure { get; set; }
        public int? BphProcedureTypeId { get; set; }
        public int ProcedureTypeId { get; set; }
        public int? TreatmentOrganId { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}
