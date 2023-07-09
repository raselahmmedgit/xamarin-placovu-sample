using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProgramResourceDetailViewModel
    {
        public long ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public PatientProcedureResourceViewModel PatientProcedureResourceViewModel { get; set; }
        public string HtmlDocument { get; set; }
    }
}
