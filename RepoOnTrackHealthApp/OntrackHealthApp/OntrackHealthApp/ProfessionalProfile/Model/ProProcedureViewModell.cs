using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class ProProcedureViewModel
    {
        public long ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
