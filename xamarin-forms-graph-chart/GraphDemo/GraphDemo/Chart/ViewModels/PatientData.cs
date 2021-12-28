using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDemo.Chart.ViewModels
{
    public class PatientData
    {
        public PatientData()
        {
            Labels = new List<string>();
            LabelNames = new List<string>();
        }
        public List<string> Labels { set; get; }
        public List<string> LabelNames { set; get; }
    }
}
