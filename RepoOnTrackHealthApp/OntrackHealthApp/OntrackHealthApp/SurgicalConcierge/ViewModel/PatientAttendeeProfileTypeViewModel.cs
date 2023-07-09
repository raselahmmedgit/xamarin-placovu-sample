using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.ViewModel
{
    public class PatientAttendeeProfileTypeViewModel
    {
        public int AttendeeProfileTypeId { get; set; }

        public string AttendeeProfileTypeName { get; set; }

        public int DisplayOrder { get; set; }
    }


    public class PatientAttendeeProfileTypeSelectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }
}
