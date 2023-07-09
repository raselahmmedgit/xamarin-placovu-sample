using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConceirgePracticeDivision
    {
        public long DivisionId { get; set; }
        public string DivisionName { get; set; }
        public long PracticeId { get; set; }
        public bool IsActive { get; set; }
        public string DivisionBgColor { get; set; }
        public int DisplayOrder { get; set; }
        public string DivisionDescription { get; set; }

        public string MobileImageBgColor { get; set; } //like as: #275791
        public string MobileImageSource { get; set; } //like as: division_unit_operation.png
    }
}
