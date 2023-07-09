using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class PatientProfile
    {
        public PatientProfile()
        {
            this.PracticeProfileDropDownViewModel = new PracticeProfileDropDownViewModel();
        }

        public long PatientProfileId { get; set; }

        
        public long PracticeProfileId { get; set; }

     
        public string PracticeProfileName { get; set; }

        public string FirstName { get; set; }

       
        public string LastName { get; set; }
        
       
        public DateTime? DateOfBirth { get; set; }

        
        public string PrimaryPhoneCode { get; set; }

        
        public string PrimaryPhone { get; set; }

        public int? PrimaryPhoneCountryId { get; set; }

        public string PrimaryPhoneCountryIso { get; set; }

      

      
        public string EmailAddress { get; set; }

        
        
      
        public PracticeProfileDropDownViewModel PracticeProfileDropDownViewModel { get; set; }
    }
}
