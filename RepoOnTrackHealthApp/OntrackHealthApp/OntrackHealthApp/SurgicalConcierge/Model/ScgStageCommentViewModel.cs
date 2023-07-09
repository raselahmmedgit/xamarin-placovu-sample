using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class ScgStageCommentViewModel
    {
        public long ScgStageCommentId { get; set; }
        public string ScgStageCommentText { get; set; }
        public bool IsSelected { set; get; }
    }
}
