using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class PatientAggregateSurveyReportForBphViewModel
    {
        public PatientProfileViewModelForAggregateSurvey PatientProfileViewModel { get; set; }
        public string PatientAge { get; set; }
        public List<PatientProcedureDetailViewModelForBphAggregateSurvey> PatientProcedureDetailViewModels { set; get; }
    }

    public class PatientProcedureDetailViewModelForBphAggregateSurvey
    {
        public int BphProcedureTypeId { set; get; }

        public int ProcedureTypeId { get; set; }

        public long PatientProfileId { get; set; }

        public string ProcedureTypeName { get; set; }

        public string ProcedureName { get; set; }
    }

}
