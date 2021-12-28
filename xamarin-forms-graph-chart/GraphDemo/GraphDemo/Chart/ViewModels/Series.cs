using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDemo.Chart.ViewModels
{
    public class Series
    {
        public string LabelName { get; set; }
        public List<double> DataList { get; set; }
        public GraphProperty GraphProperty { get; set; }
        public object Type { get; set; }
        public object YAxisId { get; set; }
    }
}
