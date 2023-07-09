using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergeFloorViewModel
    {
        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string PracticeProfileName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferredName { get; set; }

        public string PatientFullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PrimaryPhoneCode { get; set; }

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

        public virtual IEnumerable<ScgFloorCommentViewModel> ScgFloorCommentViewModels { get; set; }

        public virtual IEnumerable<ScgProstatectomyViewModel> ScgProstatectomyViewModels { get; set; }
    }

    public class SurgicalConciergeFloorOnlyViewModel
    {
        public virtual IEnumerable<ScgFloorCommentViewModel> ScgFloorCommentViewModels { get; set; }

        public virtual IEnumerable<ScgProstatectomyViewModel> ScgProstatectomyViewModels { get; set; }
    }

    public class ScgFloorCommentViewModel
    {
        public Guid ScgFloorCommentId { get; set; }

        public string ScgFloorCommentText { get; set; }

        public bool IsSelected { set; get; }

        public string ScgFloorAdditionalComment { get; set; }

        public long PatientProfileId { get; set; }

        public string PatientProcedureDetailId { get; set; }

        public string[] ProstatectomyIds { get; set; }

        public string VerificationCode { get; set; }
    }

    public class ScgProstatectomyViewModel
    {
        public long ProstatectomyId { get; set; }
        public string ProstatectomyName { get; set; }
        public bool IsActive { get; set; }
        public string ResourceIconUrl { get; set; }
        public string ResourceIconName { get; set; }
    }
}
