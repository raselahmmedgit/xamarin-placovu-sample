using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class ReportTabularData<T> where T : class
    {
        public ReportTableHeader ReportTableHeader { get; set; }
        public List<T> DataList { set; get; }
    }
}
