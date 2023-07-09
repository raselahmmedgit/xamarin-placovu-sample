using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public partial class ProfessionalProcedureViewModel
    {
        public long ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public int ProcedureTypeId { get; set; }

        public string ProcedureTypeName { get; set; }

        public bool IsShowInScg { set; get; }

        public bool IsDeleted { get; set; }
    }
}
