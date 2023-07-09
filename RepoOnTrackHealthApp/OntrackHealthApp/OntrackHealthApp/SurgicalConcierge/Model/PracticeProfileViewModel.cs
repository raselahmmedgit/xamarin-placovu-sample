using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public partial class PracticeProfileViewModel : BaseViewModel
    {
        public PracticeProfileViewModel()
        {
            this.CountryViewModels = new List<CountryViewModel>();
        }

        public long PracticeProfileId { get; set; }

        public Nullable<long> OrganizationId { get; set; }
        public string PracticeProfileLogoId { get; set; }

        public string PracticeName { get; set; }

        public string Hours { get; set; }

        // public string AppUserId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }

        public Nullable<int> StateId { get; set; }
        public string Zip { get; set; }
        public string EmailAddress { get; set; }

        public string WebsiteAddress { get; set; }
        public string AdminEmailAddress { get; set; }

        public string AdminName { get; set; }

        public string AppUserId { get; set; }

        public string OfficePhoneCode { get; set; }

        public string OfficePhone { get; set; }

        public int? OfficePhoneCountryId { get; set; }

        public string OfficePhoneCountryIso { get; set; }
        public virtual IEnumerable<CountryViewModel> CountryViewModels { get; set; }
    }
}
