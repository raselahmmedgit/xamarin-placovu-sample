using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public partial class ProfessionalProfileDropDownViewModel
    {
        public ProfessionalProfileDropDownViewModel()
        {
            //
        }
        public long ProfessionalProfileId { get; set; }

        public IEnumerable<SelectListItem> SelectOptions { get; set; }
    }
}
