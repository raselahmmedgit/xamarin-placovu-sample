using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergeViewModel
    {
        public SurgicalConciergeViewModel()
        {
            this.SurgicalConciergeDetailViewModels = new HashSet<SurgicalConciergeStageViewModel>();
        }
        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string PracticeProfileName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferredName { get; set; }


        public DateTime? DateOfBirth { get; set; }

        public string PrimaryPhone { get; set; }

        public string OtherPhone { get; set; }
        public string EmailAddress { get; set; }
        public bool IsDeleted { get; set; }

        public string ProfilePictureId { get; set; }

        public string BirthMonth { get; set; }

        public string BirthDay { get; set; }

        public string BirthYear { get; set; }
        public string DateOfBirthString => BirthDay + "-" + BirthMonth + "-" + BirthYear + " 00:00:00";
        public string DateOfBirthStr { get; set; }

        public int PatientActiveProcedureCount { get; set; }

        public string ProfessionalName { get; set; }

        public string ProcedureName { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public string SurgeryTime { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public virtual ICollection<SurgicalConciergeStageViewModel> SurgicalConciergeDetailViewModels { get; set; }
        public virtual IEnumerable<PatientAttendeeProfileViewModel> PatientAttendeeProfileViewModels { get; set; }
        public virtual IEnumerable<LeonardoSpeechText> LeonardSpeechTextViewModels { get; set; }


    }
}
