using OntrackHealthApp.ProfessionalProfile.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProgramProcedureViewModel
    {
        public ProgramProcedureViewModel()
        {
            ProfessionalProcedureViewModels = new List<ProfessionalProcedureViewModel>();
            Medications = new List<ProProcedureViewModel>();
        }

        public List<ProfessionalProcedureViewModel> ProfessionalProcedureViewModels { get; set; }

        public List<ProProcedureViewModel> Medications { get; set; }

        public List<ProProcedureViewModel> SurgicalConceirgeProcedures { get; set; }

    }
}
