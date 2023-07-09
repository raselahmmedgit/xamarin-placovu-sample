using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ProfessionalBioLicensureView
    {
        public Guid ProfessionalBioDetailId { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public Guid? ProfessionalBioSectionId { get; set; }

        public string LicensureName { get; set; }

        public int? StateId { get; set; }

        public string StateShortName { get; set; }

        public string StateName { get; set; }

        public string StateImage { get; set; }

        public ImageSource StateImageImageSource
        {
            get
            {
                if (!string.IsNullOrEmpty(StateImage))
                {
                    byte[] base64Stream = Convert.FromBase64String(StateImage);
                    return ImageSource.FromStream(() => new MemoryStream(base64Stream));
                }
                return null;
            }
        }
    }
}
