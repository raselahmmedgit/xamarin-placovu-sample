using System;
using System.IO;
using Xamarin.Forms;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public partial class PatientPreSurgerySummaryXrayViewModel
    {
        public Guid XrayId { get; set; }
        public Guid PreSurgerySummaryId { get; set; }
        public string XrayImage { get; set; }
        public string AthenaPatientId { get; set; }
        public int LabresultId { get; set; }
        public int PageId { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public ImageSource XrayImageSource
        {
            get
            {
                if (!string.IsNullOrEmpty(XrayImage))
                {
                    //byte[] base64Stream = Convert.FromBase64String(XrayImage);
                    //return ImageSource.FromStream(() => new MemoryStream(base64Stream));
                    var imgData = XrayImage.Split(',');
                    byte[] base64Stream = Convert.FromBase64String(imgData[1]);
                    return ImageSource.FromStream(() => new MemoryStream(base64Stream));
                }
                return null;
            }
        }
    }
}
