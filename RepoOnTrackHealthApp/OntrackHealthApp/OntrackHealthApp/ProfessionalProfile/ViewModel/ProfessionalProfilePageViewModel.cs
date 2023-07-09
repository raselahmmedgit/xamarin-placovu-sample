using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalProfilePageViewModel : OntrackHealthApp.ViewModel.BaseViewModel
    {

        public long ProfessionalProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string PracticeName { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public string DoctorPreferredName { get; set; }

        public string ProfessionalProfileName => string.IsNullOrEmpty(DoctorPreferredName) ? (DoctorFirstName + " " + DoctorLastName) : DoctorPreferredName;

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

        public virtual PracticeProfileViewModel PracticeProfile { get; set; }

        public int ProfessionalTotalPatient { get; set; }

        public virtual IEnumerable<ProfessionalDashboardDivisionViewModel> ProfessionalDashboardDivisionViewModels { get; set; }

        public string ProfessionalProfileImage { get; set; }

        public ImageSource ProfessionalProfilePicture
        {
            get
            {
                if (!string.IsNullOrEmpty(ProfessionalProfileImage))
                {
                    byte[] base64Stream = Convert.FromBase64String(ProfessionalProfileImage);
                    return ImageSource.FromStream(() => new MemoryStream(base64Stream));
                }
                return null;
            }
        }

        public string ProfessionalPracticeName
        {
            get
            {
                if (PracticeProfile != null)
                {
                    return PracticeProfile.PracticeName.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        public string ProfessionalTotalPatientCount
        {
            get
            {
                if (ProfessionalTotalPatient > 0)
                {
                    return ProfessionalTotalPatient.ToString() + " Patients";
                }
                else {
                    return "0 Patients";
                }
            }
        }

        public string ProfessionalDivisionOutcome
        {
            get
            {
                return ((int)Enums.ProfessionPracticeDivision.Outcomes).ToString();
            }
        }

        public string ProfessionalDivisionHospital
        {
            get
            {
                return ((int)Enums.ProfessionPracticeDivision.Hospital).ToString();
            }
        }

        public string ProfessionalDivisionNotesOrPathology
        {
            get
            {
                return ((int)Enums.ProfessionPracticeDivision.NotesOrPathology).ToString();
            }
        }

        public string ProfessionalDivisionOthersDivision
        {
            get
            {
                return ((int)Enums.ProfessionPracticeDivision.OthersDivisionId).ToString();
            }
        }
    }
}
