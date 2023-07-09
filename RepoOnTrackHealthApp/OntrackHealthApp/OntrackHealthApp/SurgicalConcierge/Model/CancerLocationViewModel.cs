using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class CancerLocationViewModel
    {
        public Guid LocationId { get; set; }
        public string LocationDispalyText { get; set; }
        public string LocationValue { get; set; }
        public bool IsChecked { get; set; }
        public bool HasInvolvedValue
        {
            get { return Involved != null; }
        }
        public decimal? Involved { get; set; }
    }
}
