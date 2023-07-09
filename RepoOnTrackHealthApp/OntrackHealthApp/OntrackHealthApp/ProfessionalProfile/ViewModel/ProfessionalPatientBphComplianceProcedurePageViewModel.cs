using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalPatientBphComplianceProcedurePageViewModel
    {
        public ProfessionalPatientBphComplianceProcedurePageViewModel()
        {
            ProfessionalPatientBphComplianceProcedureTypeViewModels = new List<ProfessionalPatientBphComplianceProcedureTypeViewModel>();
        }
        public List<ProfessionalPatientBphComplianceProcedureTypeViewModel> ProfessionalPatientBphComplianceProcedureTypeViewModels { get; set; }
    }
}
