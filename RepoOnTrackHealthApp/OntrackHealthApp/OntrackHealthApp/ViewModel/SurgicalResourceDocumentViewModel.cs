using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class SurgicalResource
    {
        public string ResourceTitle { get; set; }
        public string ResourceText { set; get; }
    }

    public class SurgicalResourceDocumentViewModel
    {
        public SurgicalResourceDocumentViewModel()
        {
            SurgicalResourceList = new List<SurgicalResource>();
        }
        public string SurgicalResourceTitle { set; get; }
        public string SurgicalResourceShortName { get; set; }
        public string ScgResourceDocumentIcon { get; set; }
        public string ScgResourceDocumentIconName { get; set; }
        public IEnumerable<SurgicalResource> SurgicalResourceList { set; get; }
    }

    public class SurgicalResourceViewModel
    {
        public int ResourceId { get; set; }
        public string ResourceTitle { get; set; }
        public string ResourceText { set; get; }
        public string ResourceTextFormated
        {
            get {
                string tmp = "<html><body style=\"font-size:16px;height:100%;background-color:#FFFFFF;color:#000000\">";
                if(ResourceText != "")
                {
                    tmp += ResourceText;
                    tmp += "</body></html>";
                }
                return tmp;
            }
        }
    }

    public class ProfessionalProgramResourceViewModel
    {

        public int ResourceId { get; set; }
        //public int ResourceHeight
        //{
        //    get
        //    {
        //        if (ResourceText != null)
        //        {
        //            return ResourceText.Length / 4;
        //        }
        //        return 0;
        //    }
        //}
        public string ResourceTitle { get; set; }
        public string ResourceText { set; get; }
        public string ResourceTextFormated
        {
            get
            {
                string tmp = $"<html><body style=\"font-size:32px;padding-top:40px;background-color:#FFFFFF;color:#000000\">";
                if (ResourceText != "")
                {
                    tmp += ResourceText;
                    tmp += "</body></html>";
                }
                return tmp;
            }
        }
    }
}
