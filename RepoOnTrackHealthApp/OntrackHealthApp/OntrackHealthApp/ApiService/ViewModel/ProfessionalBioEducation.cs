using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ProfessionalBioEducation
    {
        public Guid ProfessionalBioDetailId { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public Guid? ProfessionalBioSectionId { get; set; }

        public string ProgramName { get; set; }

        public string SchoolName { get; set; }

        public string DepartmentName { get; set; }

        public string SchoolLocation { get; set; }

        public int? EducationDisplayOrder { get; set; }

        public string SchoolLogo { get; set; }

        public ImageSource EducationSchoolLogo
        {
            get
            {
                if (!string.IsNullOrEmpty(SchoolLogo))
                {
                    var imgData = SchoolLogo.Split(',');
                    byte[] base64Stream = Convert.FromBase64String(imgData[1]);
                    return ImageSource.FromStream(() => new MemoryStream(base64Stream));
                }
                return null;
            }
        }

        public string BioEducationDepartmentName
        {
            get
            {
                if (!string.IsNullOrEmpty(DepartmentName))
                {
                    return "Department: " + DepartmentName;
                }
                return "";
            }
        }

        public string BioEducationProgramName
        {
            get
            {
                if (!string.IsNullOrEmpty(ProgramName))
                {
                    return "Program: " + ProgramName;
                }
                return "";
            }
        }
    }
}
