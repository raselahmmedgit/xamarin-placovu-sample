using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConceirgePatientEmailAddModel
    {
        public SurgicalConceirgePatientEmailAddModel()
        {
            this.CountryViewModels = new List<CountryViewModel>();
        }
        public long PatientProfileId { get; set; }

        public string EmailAddress { get; set; }

        public string PrimaryPhoneCode { get; set; }

        public string PrimaryPhone { get; set; }

        public int? PrimaryPhoneCountryId { get; set; }

        public string PrimaryPhoneCountryIso { get; set; }

        public bool EnableSmsNotification { get; set; }

        public bool EnablePathologySmsNotification { get; set; }

        public virtual IEnumerable<CountryViewModel> CountryViewModels { get; set; }
    }
}
