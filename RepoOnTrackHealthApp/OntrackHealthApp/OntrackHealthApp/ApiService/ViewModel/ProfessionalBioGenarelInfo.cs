using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ProfessionalBioGenarelInfo
    {
        public Guid ProfessionalBioDetailId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public Guid? ProfessionalBioSectionId { get; set; }

        public int? YearBoardCertified { get; set; }

        public int? YearJoinedCurrentPractice { get; set; }

        public string CurrentPracticeName { get; set; }

        public string CurrentPracticeLocation { get; set; }

        public string CareerSummary { get; set; }
    }
}
