using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.ViewModel
{
    public class SurgicalConciergeSidebarSearchViewModel
    {
        public virtual IEnumerable<PracticeProfileCheckBoxListView> PracticeProfileCheckBoxListViews { get; set; }
        public virtual IEnumerable<ProfessionalProfileCheckBoxListView> ProfessionalProfileCheckBoxListViews { get; set; }
        public virtual IEnumerable<ProcedureCheckBoxListView> ProcedureCheckBoxListViews { get; set; }
        public virtual IEnumerable<PracticeLocationCheckBoxListView> PracticeLocationCheckBoxListViews { get; set; }
    }

    public partial class PracticeProfileCheckBoxListView
    {
        public long PracticeProfileId { get; set; }

        public string PracticeName { get; set; }

        public bool IsChecked { get; set; }
    }

    public partial class ProfessionalProfileCheckBoxListView
    {
        public long ProfessionalProfileId { get; set; }

        public string ProfessionalProfileName { get; set; }

        public bool IsChecked { get; set; }
    }

    public partial class ProcedureCheckBoxListView
    {
        public long ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public string ProcedureTypeName { get; set; }

        public int ProcedureTypeId { get; set; }

        public bool IsChecked { get; set; }
    }

    public class PracticeLocationCheckBoxListView
    {
        public Guid PracticeLocationId { get; set; }

        public string LocationName { get; set; }

        //public string PracticeLocationTypeName { get; set; }

        //public int PracticeLocationTypeId { get; set; }

        public bool IsChecked { get; set; }
    }
}
