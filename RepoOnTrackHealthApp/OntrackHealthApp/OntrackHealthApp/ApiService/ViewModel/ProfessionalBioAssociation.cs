using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ProfessionalBioAssociation
    {
        public Guid ProfessionalBioDetailId { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public Guid? ProfessionalBioSectionId { get; set; }

        public string AssociationName { get; set; }

    }
}
