using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Model
{
    public class UserIdentityModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public string UserNameDisplay { get; set; }

        public string UserEmail { get; set; }

        public long? PracticeProfileId { get; set; }

        public long? PatientProfileId { get; set; }
        public string PracticeLocationName { get; set; }

        public bool IsSystemAdmin { get; set; }

        public bool IsAdmin { get; set; }

        public string PracticeName { get; set; }

        public string ProcedureName { get; set; }

        public long? ProfessionalProfileId { get; set; }

        public string ProfessionalProfileName { get; set; }
    }
}
