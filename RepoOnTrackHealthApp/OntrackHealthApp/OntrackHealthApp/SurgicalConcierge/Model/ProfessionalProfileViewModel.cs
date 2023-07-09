using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public partial class ProfessionalProfileViewModel
    {

        public long ProfessionalProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string PracticeName { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string DoctorPreferredName { get; set; }

        public string ProfileName => string.IsNullOrEmpty(DoctorPreferredName) ? (DoctorFirstName + " " + DoctorLastName) : DoctorPreferredName;

        public string MobilePhoneCode { get; set; }
        public string MobilePhone { get; set; }

        public int? MobilePhoneCountryId { get; set; }


        public string OfficePhoneCode { get; set; }
        public string OfficePhone { get; set; }

        public int? OfficePhoneCountryId { get; set; }

        public string OfficePhoneCountryIso { get; set; }

        public string DoctorEmail { get; set; }

        public string DoctorEmailOld { get; set; }
        public string AssistantName { get; set; }
         public string AssistantEmail { get; set; }

        public string AssistantPhone { get; set; }

        public string AppUserId { get; set; }

        public string ProfilePictureId { get; set; }
        public bool IsScgProfessional { get; set; }

    }
}
