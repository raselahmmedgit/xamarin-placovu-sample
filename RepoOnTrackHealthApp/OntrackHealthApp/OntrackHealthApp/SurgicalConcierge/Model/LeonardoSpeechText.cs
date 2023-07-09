using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class LeonardoSpeechText
    {
        public long SpeechTextId { get; set; }
        public string SpeechText { get; set; }
        public bool IsEnabled { get; set; }
    }
}
