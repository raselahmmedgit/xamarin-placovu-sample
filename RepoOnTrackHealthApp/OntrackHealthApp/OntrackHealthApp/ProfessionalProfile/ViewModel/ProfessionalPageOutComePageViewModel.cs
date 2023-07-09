using OntrackHealthApp.ProfessionalProfile.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalPageOutComePageViewModel
    {
        public ProfessionalPageOutComePageViewModel()
        {
            //ctr
        }

        public string ProfessionalPracticeName { get; set; }

        public string ProfessionalDivisionName { get; set; }

        public long ProfessionalDivisionId { get; set; }

        public int ProfessionalTotalPatient { get; set; }

        public string ProfessionalProfileName { get; set; }

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

        public string ProfessionalTotalPatientCount
        {
            get
            {
                if (ProfessionalTotalPatient > 0)
                {
                    return ProfessionalTotalPatient.ToString() + " Patients";
                }
                else
                {
                    return "0 Patients";
                }
            }
        }

        public string GetProfessionalDivisionTitle()
        {
            if (ProfessionalDivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.Hospital)
            {
                return string.Empty;
            }
            if (ProfessionalDivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.Outcomes)
            {
                return "Outcome Data";
            }
            if (ProfessionalDivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.NotesOrPathology)
            {
                return "Notes/Pathology";
            }
            if (ProfessionalDivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.OthersDivisionId)
            {
                return "Other Links Available";
            }
            return "";
        }

        public virtual List<ProfessionalDashboardDivisionUnit> ProfessionalDashboardDivisionUnits { get; set; }
    }
}
