using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class ProfessionaNotesToNurse
    {
        public long NoteId { get; set; }
        public string NoteHeader { get; set; }
        public string NoteDetail { get; set; }
        public bool? IsActive { get; set; }

    }
}
