using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class PracticeProcedureLocationDropDownViewModel
    {
        public Guid? PracticeLocationId { get; set; }
        public IEnumerable<SelectListItem> SelectOptions { get; set; }
    }
}
