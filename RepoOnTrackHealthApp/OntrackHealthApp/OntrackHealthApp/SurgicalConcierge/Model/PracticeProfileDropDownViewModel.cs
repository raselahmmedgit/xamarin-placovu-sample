using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class PracticeProfileDropDownViewModel
    {
        public long PracticeProfileId { get; set; }
        public IEnumerable<SelectListItem> SelectOptions { get; set; }

        public IEnumerable<PracticeProfileViewModel> PracticeProfileViewModels { get; set; }

    }
}
