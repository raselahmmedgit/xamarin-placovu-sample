using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ProfessionalBioCustomSection
    {
        public Guid ProfessionalBioSectionId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public string ProfessionalBioSectionTitle { get; set; }

        public string SectionContent { get; set; }

        public bool SectionCanDelete { get; set; }

        public int? SectionDisplayOrder { get; set; }

        public string SectionContentHtml
        {
            get
            {
                if (string.IsNullOrEmpty(SectionContent))
                {
                    return "";
                }
                var str = "<html>"
                    + " <head><style type=\"text/css\">"
                    + " @font-face {"
                    + " font-family: georgia;"
                    + " src: url('Fonts/georgia.ttf');"
                    + " } body{font-family: georgia; font-size:17px; padding:0; margin:0;}"
                    + " p { margin:10px; padding:0px; width:100%}"
                    + " li { margin:5px; padding:0px; width:100%}"
                    + " .img-responsive{display: block; max-width: 100%; height: auto; vertical-align: middle; border: 0;}"
                    + " </style></head><body>"
                    + SectionContent
                    + " </body></html>";
                return str.Replace("&nbsp;", " ");
            }
        }

    }
}
