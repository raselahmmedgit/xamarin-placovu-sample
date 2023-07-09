using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class CompliancePatientProfileModel
    {
        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferredName { get; set; }

        public string DateOfBirthStr { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
