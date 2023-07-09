using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ProfessionalBioSection
    {
        public Guid ProfessionalBioSectionId { get; set; }

        public string ProfessionalBioSectionTitle { get; set; }

        public bool SectionCanDelete { get; set; }

        public int SectionDisplayOrder { get; set; }

    }
}
