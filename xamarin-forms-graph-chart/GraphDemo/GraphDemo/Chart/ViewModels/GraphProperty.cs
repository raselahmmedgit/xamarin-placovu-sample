using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDemo.Chart.ViewModels
{
    public class GraphProperty
    {
        public string BackgroundColor { get; set; }
        public bool Fill { get; set; }
        public int PointRadius { get; set; }
        public string PointBorderColor { get; set; }
        public string PointBackgroundColor { get; set; }
        public int PointBorderWidth { get; set; }
        public int LineTension { get; set; }
        public string BorderColor { get; set; }
        public int GraphPropertyFor { get; set; }
    }
}
