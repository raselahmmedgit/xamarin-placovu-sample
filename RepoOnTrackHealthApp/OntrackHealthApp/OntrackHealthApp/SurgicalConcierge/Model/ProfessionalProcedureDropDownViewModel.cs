using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class ProfessionalProcedureDropDownViewModel
    {
        public long ProcedureId { get; set; }
        public IEnumerable<SelectListItem> SelectOptions { get; set; }
    }
}
