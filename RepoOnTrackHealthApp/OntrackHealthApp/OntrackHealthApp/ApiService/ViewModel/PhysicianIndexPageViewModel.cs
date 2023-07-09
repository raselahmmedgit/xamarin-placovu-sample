using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PhysicianIndexPageViewModel : ViewModelBase
    {
        public long ProfessionalProfileId { get; set; }

        public string ProfessionalProfileName { get; set; }

        public string ProfessionalProfileImage { get; set; }

        public string ProcedureName { get; set; }

        public string PatientProcedureDetailId { get; set; }        

        public string PageDescription { get; set; }

        public string ProfilePictureId { get; set; }

        public string YearBoardCertifiedSince {

            get {
                if (ProfessionalBioGenarelInfo != null && ProfessionalBioGenarelInfo.YearBoardCertified.HasValue){
                    return "Board Certified since " + ProfessionalBioGenarelInfo?.YearBoardCertified.ToString();
                }
                return "";
            }
        }
        public bool ShowYearBoardCertifiedSince
        {
            get
            {
                if (!string.IsNullOrEmpty(YearBoardCertifiedSince))
                {
                    return true;
                }
                return false;
            }
        }
        public string CareerSummary => ProfessionalBioGenarelInfo?.CareerSummary.ToString().Replace("&nbsp;", " ");

        public string CareerSummaryHtml {
            get {
                if(string.IsNullOrEmpty(CareerSummary))
                {
                    return "";
                }
                var str = "<html>"
                    + " <head><style type=\"text/css\">"
                    + " @font-face {"
                    + " font-family: georgia;"
                    + " src: url('Fonts/georgia.ttf');"
                    + " } body{font-family: georgia; font-size:18px; padding:0; margin:0; line-height:26px}"                    
                    + " p {font-family: georgia; font-size:18px; margin:10px; padding:0px;}"
                    + " h2{margin:0px; padding:0px; font-size:18px;margin-bottom:15px;}"
                    + " h3{margin:0px; padding:0px; font-size:17px;margin-bottom:15px;}"
                    + " h4{margin:0px; padding:0px; font-size:16px;margin-bottom:15px;}"
                    + " .row {margin-right:0px; margin-left:0px;} .row:after{clear: both;} .col-md-12{float: left; width: 100%; position: relative; min-height: 1px;padding-right: 15px;padding-left: 15px}"
                    + " table{border-collapse: collapse;}"
                    + " td{border-bottom:1px solid #777;vertical-align: middle;padding-top:6px; padding-bottom:6px;}"
                    + " td:first-child{padding-right:4px} td:last-child{padding-left:4px}"
                    + " .img-responsive{display: block; max-width: 100%; height: auto; vertical-align: middle; border: 0;}"
                    + " </style></head><body>"
                    + CareerSummary
                    + " </body></html>";
                return str.Replace("&nbsp;", " ");
            }
        }

        public string YearJoinedCurrentPractice
        {
            get
            {
                if (ProfessionalBioGenarelInfo != null && ProfessionalBioGenarelInfo.YearJoinedCurrentPractice.HasValue)
                {
                    return "Joined: " + ProfessionalBioGenarelInfo?.CurrentPracticeName.ToString() + " in " + ProfessionalBioGenarelInfo?.YearJoinedCurrentPractice.ToString();
                }
                return "";
            }
        }
        public bool ShowYearJoinedCurrentPractice
        {
            get
            {
                if (!string.IsNullOrEmpty(YearJoinedCurrentPractice))
                {
                    return true;
                }
                return false;
            }
        }

        public string CurrentPracticeLocation {
            get {
                if (ProfessionalBioGenarelInfo != null) {
                    return ProfessionalBioGenarelInfo.CurrentPracticeLocation.ToString();
                }                
                return "";
            }
        }
        public bool ShowCurrentPracticeLocation
        {
            get
            {
                if (!string.IsNullOrEmpty(CurrentPracticeLocation))
                {
                    return true;
                }
                return false;
            }
        }
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

        public bool ShowCustomSection {
            get {
                if(ProfessionalBioCustomSections != null && ProfessionalBioCustomSections.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool ShowEdicationSection
        {
            get
            {
                if (ProfessionalBioEducations != null && ProfessionalBioEducations.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool ShowBioAssociations
        {
            get
            {
                if (ProfessionalBioAssociations != null && ProfessionalBioAssociations.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool ShowBioInterests
        {
            get
            {
                if (ProfessionalBioInterests != null && ProfessionalBioInterests.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool ShowBioLicensureViews
        {
            get
            {
                if (ProfessionalBioLicensureViews != null && ProfessionalBioLicensureViews.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool ShowBioSections
        {
            get
            {
                if (ProfessionalBioSections != null && ProfessionalBioSections.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool ShowCareerSummary
        {
            get
            {
                if (!string.IsNullOrEmpty(CareerSummary))
                {
                    return true;
                }
                return false;
            }
        }
        
        public ProfessionalBioGenarelInfo ProfessionalBioGenarelInfo { get; set; }

        public List<ProfessionalBioSection> ProfessionalBioSections { get; set; }

        public List<ProfessionalBioEducation> ProfessionalBioEducations { get; set; }

        public List<ProfessionalBioAssociation> ProfessionalBioAssociations { get; set; }

        public List<ProfessionalBioInterest> ProfessionalBioInterests { get; set; }

        public List<ProfessionalBioLicensureView> ProfessionalBioLicensureViews { get; set; }

        public List<ProfessionalBioCustomSection> ProfessionalBioCustomSections { get; set; }

    }
}
