using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class UserPracticeViewModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public long? PracticeProfileId { get; set; }

        public string PracticeName { get; set; }

        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleNameDisplay { get; set; }

        public bool IsSystemAdmin { get; set; }

        public Guid? PracticeLocationId { get; set; }

        public string LocationName { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsLocationUser
        {
            get
            {
                if (PracticeLocationId != null)
                    return true;
                return false;
            }
        }

    }
}
