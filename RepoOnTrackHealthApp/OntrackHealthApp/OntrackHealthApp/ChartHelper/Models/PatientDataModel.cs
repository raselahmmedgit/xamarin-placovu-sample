using System.Collections.Generic;

namespace OntrackHealthApp.ChartHelper.Models
{
    public class PatientDataModel
    {
        public PatientDataModel()
        {
            Labels = new List<string>();
            LabelNames = new List<string>();
        }
        public List<string> Labels { set; get; }
        public List<string> LabelNames { set; get; }
    }
}
